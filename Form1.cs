using System;
using System.Drawing.Imaging;
using System.IO;
using System.Windows.Forms;

namespace HassensteinTD
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void mainDisplay_Paint(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;

            if (gameState != 2) return;

            renderLevel(g);
            renderSpikes(g);
            renderEnemies(g);
            if (!waveInProgress) renderDragAndDrop(g);
            renderTowers(g);
            renderArrows(g);
            renderBombs(g);
            renderExplosions(g);
        }

        private void mainUpdate_Tick(object sender, EventArgs e)
        {
            checkEnemyStates();
            updateStats();
            mainDisplay.Refresh();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            initMenu();
            //initLevel();

            //loadLevel("Levels/level1.json");
        }

        Font arialFont = new Font("Arial", 14, FontStyle.Bold);



        private void initLevel()
        {
            mainDisplay.Enabled = true;
            mainDisplay.BringToFront();
            mainDisplay.Visible = true;
            int canvasWidth = 950;
            int buttonWidth = 100;
            int buttonCount = 4;
            int yPos = 650;

            // UI tower buttons
            int gap = (canvasWidth - (buttonCount * buttonWidth)) / (buttonCount + 1); // Calculate gap based on canvas width, button count and button width

            backgroundTowerSelRec = new Rectangle(0, yPos, canvasWidth, 100);
            archerUIRec = new Rectangle(gap, yPos, buttonWidth, buttonWidth);
            hedgehogUIRec = new Rectangle(gap * 2 + buttonWidth, yPos, buttonWidth, buttonWidth);
            trapperUIRec = new Rectangle(gap * 3 + buttonWidth * 2, yPos, buttonWidth, buttonWidth);
            bomberUIRec = new Rectangle(gap * 4 + buttonWidth * 3, yPos, buttonWidth, buttonWidth);

            // Level manager
            levelManager = new LevelManager(this);

            menuDisplay.SendToBack();
        }

        //------------------------------- Level Manager ----------------------------

        LevelManager levelManager;
        public LevelManager.LevelData currentLevelData;

        int gridSize = 50;

        bool levelLoaded = false;

        int numberOfWaves = 0;
        int currentWave;
        int gold = 0;
        int lives;
        private void loadLevel(string filePath)
        {
            menuDisplay.Location = new Point(1000, 1000);
            menuDisplay.Enabled = false;
            levelManager.LoadLevel(filePath);
            currentLevelData = levelManager.CurrentLevel;
            numberOfWaves = currentLevelData.Waves.Count;
            gold = currentLevelData.startingGold;
            lives = currentLevelData.lives;
            currentWave = 0;
            if (currentLevelData != null)
            {
                preRenderLevelMap();
                levelLoaded = true;
            }
        }

        public class Tile
        {
            public int gridX;
            public int gridY;
            public bool isPath;
            public bool isBuildable;
            public bool isEnd;

            public Tile(int x, int y, bool path, bool buildable, bool end) // Constructor
            {
                gridX = x;
                gridY = y;
                isPath = path;
                isBuildable = buildable;
                isEnd = end;
            }
        }

        public Tile[,] logicalMap; // 2D array representing the level layout for pathfinding and tower placement logic
        public Tile endPoint;

        Bitmap mapImage;

        private void preRenderLevelMap()
        {
            if (mapImage != null) // Memory leak prevention
            {
                mapImage.Dispose();
                mapImage = null;
            }

            string[] layout = levelManager.CurrentLevel.levelLayout;

            int width = 19;
            int height = 13;

            mapImage = new Bitmap(width * gridSize, height * gridSize);
            logicalMap = new Tile[width, height];

            using (Graphics g = Graphics.FromImage(mapImage)) // Drawing the level map to bitmap
            {
                using (Brush grassBrush = new SolidBrush(Color.LightGreen)) // memory leak prevention
                using (Brush pathBrush = new SolidBrush(Color.SandyBrown))
                using (Brush endBrush = new SolidBrush(Color.DarkGray))
                {
                    for (int y = 0; y < height; y++) // Pre-rendering map image for better performance
                    {
                        for (int x = 0; x < width; x++)
                        {
                            char tile = layout[y][x];
                            Rectangle rect = new Rectangle(x * gridSize, y * gridSize, gridSize, gridSize);

                            // Logical map
                            bool isPath = false;
                            bool isBuildable = false;
                            bool isEnd = false;

                            if (tile == '.')
                            {
                                g.DrawImage(Image.FromFile("Images/Grass.png"), rect); // Grass
                                isBuildable = true;
                            }
                            else if (tile == '#')
                            {
                                g.DrawImage(Image.FromFile("Images/Path.png"), rect); // Path
                                isPath = true;
                            }
                            else if (tile == '*')
                            {
                                g.FillRectangle(endBrush, rect); // End
                                isPath = true;
                                isEnd = true;
                                endPoint = new Tile(x, y, true, false, true);
                            }

                            logicalMap[x, y] = new Tile(x, y, isPath, isBuildable, isEnd);
                        }
                    }

                    Rectangle castleRec = new Rectangle(900, 0, 50, 650);
                    g.DrawImage(Image.FromFile("Images/Castle.png"), castleRec);

                }
            }

            generateEnemyMoveSet();
        }

        public List<int> moveSet; // 1-→ | 2-↓ | 3-← | 4-↑
        private void generateEnemyMoveSet() //Pathfinding algorithm
        {
            moveSet = new List<int>();

            int currentX = currentLevelData.startPosX;
            int currentY = currentLevelData.startPosY;

            int lastX = -1;
            int lastY = -1;

            int wth = logicalMap.GetLength(0);
            int hght = logicalMap.GetLength(1);

            while (true)
            {
                // break if enemy is on end point
                if (logicalMap[currentX, currentY].isEnd)
                {
                    break;
                }

                int nextX = -1;
                int nextY = -1;
                int direction = -1;

                // Right
                if (currentX + 1 < wth && logicalMap[currentX + 1, currentY].isPath && (currentX + 1 != lastX || currentY != lastY))
                {
                    nextX = currentX + 1;
                    nextY = currentY;
                    direction = 1;
                }
                // Down
                else if (currentY + 1 < hght && logicalMap[currentX, currentY + 1].isPath && (currentX != lastX || currentY + 1 != lastY))
                {
                    nextX = currentX;
                    nextY = currentY + 1;
                    direction = 2;
                }
                // Left
                else if (currentX - 1 >= 0 && logicalMap[currentX - 1, currentY].isPath && (currentX - 1 != lastX || currentY != lastY))
                {
                    nextX = currentX - 1;
                    nextY = currentY;
                    direction = 3;
                }
                // Up
                else if (currentY - 1 >= 0 && logicalMap[currentX, currentY - 1].isPath && (currentX != lastX || currentY - 1 != lastY))
                {
                    nextX = currentX;
                    nextY = currentY - 1;
                    direction = 4;
                }

                // if nothing is found
                if (direction == -1)
                {
                    break;
                }

                moveSet.Add(direction);

                // Move to next path
                lastX = currentX;
                lastY = currentY;
                currentX = nextX;
                currentY = nextY;
            }
        }

        Rectangle backgroundTowerSelRec;
        Image backgroundTowerSel = Image.FromFile("Images/TowerSelBackground.png");

        private void renderLevel(Graphics g)
        {
            if (!levelLoaded) return;
            g.DrawImage(mapImage, 0, 0);
            g.DrawImage(backgroundTowerSel, backgroundTowerSelRec);
        }

        //------------------------------- Wave Logic ------------------------------

        Queue<EnemyInQueueData> enemyQueue;
        bool waveInProgress = false;
        bool waveSpawning = false;
        int numberOfEnemiesInCurrentWave = 0;

        class EnemyInQueueData // For transfering enemy data from level manager for spawning;
        {
            public int id;
            public int enemyQueueX;
            public int enemyQueueY;
            public int speed;
            public int health;
            public int damage;
            public int reward;
            public Color color;
            public EnemyInQueueData(int i, int x, int y, int spd, int hth, int dmg, int rew, Color clr)
            {
                id = i;
                enemyQueueX = x;
                enemyQueueY = y;
                speed = spd;
                health = hth;
                damage = dmg;
                reward = rew;
                color = clr;
            }
        }

        private void startWave(int waveIndex)
        {
            // Initializing variables
            int waveReward = currentLevelData.Waves[waveIndex].waveReward;
            int startX = currentLevelData.startPosX;
            int startY = currentLevelData.startPosY;

            numberOfEnemiesInCurrentWave = currentLevelData.Waves[waveIndex].Enemies.Count;

            List<LevelManager.EnemyData> enemyDatas = currentLevelData.Waves[waveIndex].Enemies;
            enemyQueue = new Queue<EnemyInQueueData>();

            waveInProgress = true;

            for (int i = 0; i < enemyDatas.Count; i++) // Filling enemy queue
            {
                for (int j = 0; j < enemyDatas[i].numberOfTheseEnemies; j++)
                {
                    enemyQueue.Enqueue(new EnemyInQueueData(enemyNextID, startX, startY, enemyDatas[i].speed, enemyDatas[i].health, enemyDatas[i].damage, enemyDatas[i].health, enemyDatas[i].color));
                    enemyNextID++;
                }
            }
            waveSpawning = true;
            waveInProgress = true;
            currentWave++;
        }

        //------------------------------- Enemy Logic ------------------------------

        public List<Enemy> enemies = new List<Enemy>();

        public int enemyNextID = 0;


        private void enemyUpdate_Tick(object sender, EventArgs e)
        {
            foreach (Enemy enemy in enemies) // Moving enemies
            {
                enemy.move(this);
            }

            // Spawning enemies from queue
            if (waveSpawning && enemyQueue.Count > 0)
            {
                EnemyInQueueData data = enemyQueue.Dequeue();
                enemies.Add(new Enemy(this, data.id, data.enemyQueueX, data.enemyQueueY, data.speed, data.health, data.damage, data.reward, data.color));
            }
            if (enemyQueue != null) waveSpawning = enemyQueue.Count > 0;

            //Checking for spikes
            spikeUpdate();
        }

        private void checkEnemyStates()
        {
            for (int i = enemies.Count - 1; i >= 0; i--)
            {
                Enemy enemy = enemies[i];

                removeEnemiesBellowHealth(enemy);
                enemyReachedEnd(enemy);
            }
        }

        private void removeEnemiesBellowHealth(Enemy enemy)
        {
            if (enemy.health <= 0)
            {
                gold += enemy.reward;
                totalEnemiesKilled++;
                enemies.Remove(enemy);
            }
        }

        private void enemyReachedEnd(Enemy enemy)
        {
            if (enemy.rectangle.X / 50 == endPoint.gridX && enemy.rectangle.Y / 50 == endPoint.gridY)
            {
                lives -= enemy.damage;
                enemies.Remove(enemy);
            }
        }

        private void renderEnemies(Graphics g)
        {
            foreach (Enemy enemy in enemies)
            {
                g.DrawImage(enemy.sprite, enemy.rectangle);
            }
        }

        public Image TintImage(Image originalImage, Color tintColor)
        {
            Image tintedImage = new Bitmap(originalImage.Width, originalImage.Height); // Create new image

            float r = tintColor.R / 255f;
            float g = tintColor.G / 255f;
            float b = tintColor.B / 255f;

            // Create multiplicative color matrix to apply the tint color to the original image
            ColorMatrix colorMatrix = new ColorMatrix(new float[][]
            {
                new float[] {r, 0, 0, 0, 0}, // R
                new float[] {0, g, 0, 0, 0}, // G
                new float[] {0, 0, b, 0, 0}, // B
                new float[] {0, 0, 0, 1, 0}, // Aplha
                new float[] {0, 0, 0, 0, 1} // Translation
            });

            ImageAttributes attributes = new ImageAttributes();
            attributes.SetColorMatrix(colorMatrix);

            // Generate tinted version of the original image
            using (Graphics gr = Graphics.FromImage(tintedImage))
            {
                gr.DrawImage(originalImage, new Rectangle(0, 0, tintedImage.Width, tintedImage.Height), 0, 0, originalImage.Width, originalImage.Height, GraphicsUnit.Pixel, attributes);
            }

            return tintedImage;
        }

        private void Form1_KeyDown(object sender, KeyEventArgs e) // Debug: Spawn enemy on spacebar press
        {
            if (e.KeyCode == Keys.L) // Debug: Load level on L press
            {
                if (currentWave != numberOfWaves) startWave(currentWave);
            }
        }

        //------------------------------- Drag and Drop Logic ------------------------------

        Rectangle archerUIRec;
        Rectangle hedgehogUIRec;
        Rectangle trapperUIRec;
        Rectangle bomberUIRec;
        Brush towerBrush = new SolidBrush(Color.Blue);
        Brush pickedTowerBrush = new SolidBrush(Color.FromArgb(128, 255, 0, 0));

        Rectangle towerRec;

        List<Archer> archers = new List<Archer>();
        List<Hedgehog> hedgehogs = new List<Hedgehog>();
        List<Trapper> trappers = new List<Trapper>();
        List<Bomber> bombers = new List<Bomber>();

        public int archerCount = 0;
        public int hedgehogCount = 0;
        public int trapperCount = 0;
        public int bomberCount = 0;

        bool isDragging = false;
        int draggingID = 0; // 1-Archer , 2-Hedgehog, 3-Trapper, 4-Bomber

        private void renderDragAndDrop(Graphics g)
        {
            g.FillRectangle(towerBrush, archerUIRec);
            g.DrawString("Archer", arialFont, Brushes.White, archerUIRec.X, archerUIRec.Y + 50);
            g.FillRectangle(towerBrush, hedgehogUIRec);
            g.DrawString("Hedgehog", arialFont, Brushes.White, hedgehogUIRec.X, archerUIRec.Y + 50);
            g.FillRectangle(towerBrush, trapperUIRec);
            g.DrawString("Trapper", arialFont, Brushes.White, trapperUIRec.X, archerUIRec.Y + 50);
            g.FillRectangle(towerBrush, bomberUIRec);
            g.DrawString("Bomber", arialFont, Brushes.White, bomberUIRec.X, archerUIRec.Y + 50);

            // Drawing dragging
            if (isDragging)
                g.FillRectangle(pickedTowerBrush, towerRec);
        }

        private void mainDisplay_MouseDown(object sender, MouseEventArgs e)
        {
            if (waveInProgress || !levelLoaded) return; // Disable tower placement during waves or menu
            // Relative cursor position to mainDisplay
            int cursorX = e.X;
            int cursorY = e.Y;

            // Tower variant selection

            if (e.Button == MouseButtons.Left)
            {
                if (archerUIRec.Contains(cursorX, cursorY)) // Check if tower rectangle is clicked
                {
                    isDragging = true;
                    towerRec = new Rectangle(cursorX - towerRec.Width / 2, cursorY - towerRec.Height / 2, 50, 50); // Set towerRec to the clicked tower's position
                    draggingID = 1; // Set draggingID to Archer
                }
            }
            if (e.Button == MouseButtons.Left)
            {
                if (hedgehogUIRec.Contains(cursorX, cursorY))
                {
                    isDragging = true;
                    towerRec = new Rectangle(cursorX - towerRec.Width / 2, cursorY - towerRec.Height / 2, 50, 50);
                    draggingID = 2; // Set draggingID to Hedgehog
                }
            }
            if (e.Button == MouseButtons.Left)
            {
                if (trapperUIRec.Contains(cursorX, cursorY))
                {
                    isDragging = true;
                    towerRec = new Rectangle(cursorX - towerRec.Width / 2, cursorY - towerRec.Height / 2, 50, 50);
                    draggingID = 3; // Set draggingID to Trapper
                }
            }
            if (e.Button == MouseButtons.Left)
            {
                if (bomberUIRec.Contains(cursorX, cursorY))
                {
                    isDragging = true;
                    towerRec = new Rectangle(cursorX - towerRec.Width / 2, cursorY - towerRec.Height / 2, 50, 50);
                    draggingID = 4; // Set draggingID to Bomber
                }
            }
        }

        private void mainDisplay_MouseMove(object sender, MouseEventArgs e)
        {
            if (waveInProgress || !levelLoaded) return;
            int cursorX = e.X;
            int cursorY = e.Y;

            // Moving dragged tower
            if (isDragging)
            {
                towerRec.X = cursorX - towerRec.Width / 2; // Center the rectangle on the cursor
                towerRec.X = Math.Clamp(towerRec.X, 0, mainDisplay.Width - towerRec.Width); // Prevent moving outside the display
                towerRec.Y = cursorY - towerRec.Height / 2;
                towerRec.Y = Math.Clamp(towerRec.Y, 0, mainDisplay.Height - towerRec.Height);
            }
            mainDisplay.Refresh();
        }

        private void mainDisplay_MouseUp(object sender, MouseEventArgs e)
        {
            if (waveInProgress || !levelLoaded) return;
            isDragging = false;

            int cursorX = e.X;
            int cursorY = e.Y;

            // Snap to closest grid
            towerRec.X = (int)(Math.Round((float)towerRec.X / gridSize) * gridSize);
            towerRec.Y = (int)(Math.Round((float)towerRec.Y / gridSize) * gridSize);

            int gridX = towerRec.X / gridSize; // Calculate grid coordinates
            int gridY = towerRec.Y / gridSize;

            if (towerRec.Y <= 600)
            {
                bool intersects = false;

                // Checking for existing towers and not allowing overplacement (intersection of rectangles)

                intersects = !logicalMap[gridX, gridY].isBuildable;

                // Build tower
                if (!intersects)
                {
                    switch (draggingID)
                    {
                        case 1:
                            archers.Add(new Archer(this, archerCount++, towerRec.X, towerRec.Y));
                            logicalMap[gridX, gridY].isBuildable = false;
                            break;
                        case 2:
                            hedgehogs.Add(new Hedgehog(this, hedgehogCount++, towerRec.X, towerRec.Y));
                            logicalMap[gridX, gridY].isBuildable = false;
                            break;
                        case 3:
                            trappers.Add(new Trapper(this, trapperCount++, towerRec.X, towerRec.Y));
                            logicalMap[gridX, gridY].isBuildable = false;
                            break;
                        case 4:
                            bombers.Add(new Bomber(this, bomberCount++, towerRec.X, towerRec.Y));
                            logicalMap[gridX, gridY].isBuildable = false;
                            break;
                    }
                }
            }

            // Reseting values
            isDragging = false;
            draggingID = 0;
        }

        //------------------------------- Tower Logic ------------------------------

        private void renderTowers(Graphics g)
        {
            // Drawing towers
            foreach (Archer archer in archers)
            {
                using (Image archerSprite = Image.FromFile("Images/Archer.png"))
                {
                    g.DrawImage(archerSprite, archer.rectangle);
                }
            }
            foreach (Hedgehog hedgehog in hedgehogs)
            {
                using (Image hedgehogSprite = Image.FromFile("Images/Hedgehog.png"))
                {
                    g.DrawImage(hedgehogSprite, hedgehog.rectangle);
                }
            }
            foreach (Trapper trapper in trappers)
            {
                using (Image trapperSprite = Image.FromFile("Images/Trapper.png"))
                {
                    g.DrawImage(trapperSprite, trapper.rectangle);
                }
            }
            foreach (Bomber bomber in bombers)
            {
                using (Image bomberSprite = Image.FromFile("Images/Bomber.png"))
                {
                    g.DrawImage(bomberSprite, bomber.rectangle);
                }
            }
        }

        private void renderArrows(Graphics g)
        {
            foreach (Arrow arrow in arrows)
            {
                using (Image arrowSprite = Image.FromFile("Images/Arrow.png"))
                {
                    g.DrawImage(arrowSprite, arrow.rectangle);
                }
            }
        }

        private void renderSpikes(Graphics g)
        {
            foreach (Spike spike in spikes)
            {
                g.DrawString(spike.numOfSpikes.ToString(), arialFont, Brushes.Red, spike.rectangle.X, spike.rectangle.Y); // DEBUG
            }
        }

        private void renderBombs(Graphics g)
        {
            foreach (Bomb bomb in bombs)
            {
                using (Image bombSprite = Image.FromFile("Images/Bomb.png"))
                {
                    g.DrawImage(bombSprite, bomb.rectangle);
                }
            }
        }

        private void renderExplosions(Graphics g)
        {
            foreach (Explosion explosion in explosions)
            {
                using (Image explosionSprite = Image.FromFile("Images/Explosion.png"))
                {
                    g.DrawImage(explosionSprite, explosion.rectangle);
                }
            }
        }

        public class Arrow
        {
            public Rectangle rectangle;
            public int damage;
            public int speedX;
            public int speedY;
            public Arrow(int x, int y, int damage, int speedX, int speedY)
            {
                rectangle = new Rectangle(x, y, 10, 10);
                this.damage = damage;
                this.speedX = speedX;
                this.speedY = speedY;
            }
        }

        public class Spike
        {
            public Rectangle rectangle;
            public int damage;
            public int numOfSpikes;
            public Spike(int x, int y, int damage)
            {
                rectangle = new Rectangle(x, y, 50, 50);
                this.damage = damage;
                numOfSpikes = 0;
            }
        }

        public class Bomb
        {
            public Rectangle rectangle;
            public int damage;
            public int speedX;
            public int speedY;
            public int radius;
            public Bomb(int x, int y, int damage, int speedX, int speedY, int radius)
            {
                rectangle = new Rectangle(x, y, 20, 20);
                this.damage = damage;
                this.speedX = speedX;
                this.speedY = speedY;
                this.radius = radius;
            }
        }

        public class Explosion
        {
            public Rectangle rectangle;
            public int damage;
            public int radius;
            public Explosion(int x, int y, int damage, int radius)
            {
                rectangle = new Rectangle(x, y, radius, radius); // Explosion radius
                this.damage = damage;
            }
        }

        public List<Arrow> arrows = new List<Arrow>();
        public List<Spike> spikes = new List<Spike>();
        public List<Bomb> bombs = new List<Bomb>();
        public List<Explosion> explosions = new List<Explosion>();

        private void arrowUpdate_Tick(object sender, EventArgs e)
        {
            if (!waveInProgress || !levelLoaded) return;
            for (int i = arrows.Count - 1; i >= 0; i--)
            {
                Arrow arrow = arrows[i];
                arrow.rectangle.Offset(arrow.speedX, arrow.speedY);

                // If arrow is out of bounds, remove it
                if (arrow.rectangle.X > 1000 || arrow.rectangle.X < -10 || arrow.rectangle.Y > 800 || arrow.rectangle.Y < -10)
                {
                    arrows.RemoveAt(i);
                }
                for (int j = 0; j < enemies.Count; j++)
                {
                    Enemy enemy = enemies[j];
                    if (arrow.rectangle.IntersectsWith(enemy.rectangle)) // If arrow hits an enemy, damage it and remove the arrow
                    {
                        enemy.health -= arrow.damage;
                        totalDamageDealt += arrow.damage;
                        arrows.RemoveAt(i);
                        break;
                    }
                }
            }
        }

        private void spikeUpdate()
        {
            if (!waveInProgress || !levelLoaded) return;
            for (int i = 0; i < spikes.Count; i++)
            {
                Spike spike = spikes[i];
                if (spike.numOfSpikes > 0) // If there are spikes on the trap, damage enemies and decrease spike count
                {
                    for (int j = 0; j < enemies.Count; j++)
                    {
                        Enemy enemy = enemies[j];
                        if (spike.rectangle.IntersectsWith(enemy.rectangle))
                        {
                            enemy.health -= spike.damage;
                            totalDamageDealt += spike.damage;
                            spike.numOfSpikes--;
                        }
                    }
                }
            }
        }

        private void bombUpdate_Tick(object sender, EventArgs e)
        {
            if (!waveInProgress || !levelLoaded) return;

            for (int i = bombs.Count - 1; i >= 0; i--)
            {
                Bomb bomb = bombs[i];
                bomb.rectangle.Offset(bomb.speedX, bomb.speedY);

                // If bomb is out of bounds, remove it
                if (bomb.rectangle.X > 1000 || bomb.rectangle.X < -20 || bomb.rectangle.Y > 800 || bomb.rectangle.Y < -20)
                {
                    bombs.RemoveAt(i);
                }
                for (int j = 0; j < enemies.Count; j++)
                {
                    Enemy enemy = enemies[j];
                    if (bomb.rectangle.IntersectsWith(enemy.rectangle)) // If bomb hits an enemy, damage it and remove the bomb
                    {
                        bombExplode(enemy.rectangle.X, enemy.rectangle.Y, bomb.damage, bomb.radius);
                        bombs.RemoveAt(i);
                        break;
                    }
                }
            }
        }

        private async void bombExplode(int x, int y, int dmg, int radius)
        {
            int explosionX = x + 25 - radius / 2;
            int explosionY = y + 25 - radius / 2;

            Explosion currentExplosion = new Explosion(explosionX, explosionY, dmg, radius);
            explosions.Add(currentExplosion);

            for (int j = 0; j < enemies.Count; j++)
            {
                Enemy enemy = enemies[j];
                if (currentExplosion.rectangle.IntersectsWith(enemy.rectangle)) // If enemy is within explosion radius, damage it
                {
                    enemy.health -= dmg;
                    totalDamageDealt += dmg;
                }
            }

            await Task.Delay(500); // wait 0,5s and then remove explosion
            explosions.Remove(currentExplosion);

        }

        private void archerUpdate_Tick(object sender, EventArgs e)
        {
            if (!waveInProgress || !levelLoaded) return;
            foreach (Archer archer in archers)
            {
                archer.archerAttack(this);
            }
        }

        private void hedgehogUpdate_Tick(object sender, EventArgs e)
        {
            if (!waveInProgress || !levelLoaded) return;
            foreach (Hedgehog hedgehog in hedgehogs)
            {
                hedgehog.hedgehogAttack(this);
            }
        }

        private void trapperUpdate_Tick(object sender, EventArgs e)
        {
            if (!waveInProgress || !levelLoaded) return;
            foreach (Trapper trapper in trappers)
            {
                trapper.trapperAttack(this);
            }
        }

        private void bomberUpdate_Tick(object sender, EventArgs e)
        {
            if (!waveInProgress || !levelLoaded) return;
            foreach (Bomber bomber in bombers)
            {
                bomber.archerAttack(this);
            }
        }

        //------------------------------- UI Logic --------------------------------

        int gameState = 0; // 0-Menu, 1-Level Selector, 2-Playing

        // Menu
        Rectangle logo;
        Rectangle playButton;

        private void initMenu()
        {
            gameState = 0;
            menuDisplay.Visible = true;
            menuDisplay.Location = new Point(0, 0);
            menuDisplay.BringToFront();

            mainDisplay.Visible = false;

            logo = new Rectangle(0, 0, 970, 817);
            playButton = new Rectangle(320, 430, 331, 124);

            menuDisplay.Refresh();
        }

        // Level selector
        Rectangle level1Button;
        Rectangle level2Button;
        Rectangle level3Button;

        private void initLevelSelector()
        {
            gameState = 1;
            menuDisplay.Visible = true;
            menuDisplay.Location = new Point(0, 0);
            menuDisplay.BringToFront();

            mainDisplay.Visible = false;

            level1Button = new Rectangle(125, 400,200,200);
            level2Button = new Rectangle(375, 400,200,200);
            level3Button = new Rectangle(625, 400,200,200);
        }

        private void renderMenu(Graphics g)
        {
            using(Image menuBackground = Image.FromFile("Images/Logo.png"))
            using(Image playButtonIMG = Image.FromFile("Images/PlayBTN.png"))
            {
                g.DrawImage(menuBackground, logo);
                g.DrawImage(playButtonIMG, playButton);
            } 
        }

        private void renderLevelSelector(Graphics g)
        {
            using (Image menuBackground = Image.FromFile("Images/Logo.png"))
                g.DrawImage(menuBackground, logo);
            g.DrawString("Select Level", new Font("Arial", 50), Brushes.White, 300, 250);
            g.FillRectangle(Brushes.Blue, level1Button);
            g.FillRectangle(Brushes.Blue, level2Button);
            g.FillRectangle(Brushes.Blue, level3Button);
        }

        private void updateStats()
        {
            waveCounterL.Text = $"{currentWave}/{numberOfWaves}";
            goldCounterL.Text = gold.ToString();
            livesCounterL.Text = lives.ToString();
        }

        int totalDamageDealt = 0;
        int totalEnemiesKilled = 0;

        private void gameOver()
        {

        }
        private void renderGameOver(Graphics g)
        {

        }

        private void waveCompleted()
        {

        }
        private void renderWaveCompleted(Graphics g)
        {

        }

        private void victory()
        {

        }
        private void renderVictory(Graphics g)
        {

        }

        private void menuDisplay_Paint(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;

            switch(gameState)
            {
                case 0:
                    renderMenu(g);
                    break;
                case 1:
                    renderLevelSelector(g);
                    break;
            }
        }

        private void menuDisplay_MouseMove(object sender, MouseEventArgs e)
        {
            int cursorX = e.X;
            int cursorY = e.Y;

            // Menu animations
            if (playButton.Contains(cursorX, cursorY))
                playButton = new Rectangle(320, 410, 331, 124);
            else
                playButton = new Rectangle(320, 430, 331, 124);

            // Level selector animations
            if (level1Button.Contains(cursorX, cursorY))
                level1Button = new Rectangle(125, 380, 200, 200);
            else
                level1Button = new Rectangle(125, 400, 200, 200);
            if (level2Button.Contains(cursorX, cursorY))
                level2Button = new Rectangle(375, 380, 200, 200);
            else
                level2Button = new Rectangle(375, 400, 200, 200);
            if (level3Button.Contains(cursorX, cursorY))
                level3Button = new Rectangle(625, 380, 200, 200);
            else
                level3Button = new Rectangle(625, 400, 200, 200);

            menuDisplay.Refresh();
        }

        private void menuDisplay_MouseDown(object sender, MouseEventArgs e)
        {
            int cursorX = e.X;
            int cursorY = e.Y;

            if (gameState == 1)
            {
                if (level1Button.Contains(cursorX, cursorY))
                {
                    initLevel();
                    loadLevel("Levels/level1.json");
                    gameState = 2;
                }
                else if (level2Button.Contains(cursorX, cursorY))
                {
                    //loadLevel(2);
                    gameState = 2;
                }
                else if (level3Button.Contains(cursorX, cursorY))
                {
                    //loadLevel(3);
                    gameState = 2;
                }
            }
            else if (playButton.Contains(cursorX, cursorY) && gameState == 0) // Check if tower rectangle is clicked
            {
                initLevelSelector();
                gameState = 1;
            }

            
            menuDisplay.Refresh();
        }
    }
}
