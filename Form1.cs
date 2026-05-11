using System;
using System.IO;

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

            renderLevel(g);
            renderEnemies(g);
            renderDragAndDrop(g);
        }

        private void mainUpdate_Tick(object sender, EventArgs e)
        {
            enemies.RemoveAll(enemy => enemy.health <= 0); // Removes all enemies with health <= 0 from the list (killed enemies)
            waveCounterL.Text = $"{currentWave}/{numberOfWaves}";
            mainDisplay.Refresh();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            init();
            loadLevel("Levels/level1.json");
        }

        Font arialFont = new Font("Arial", 14, FontStyle.Bold);

        private void init()
        {
            int canvasWidth = 950;
            int buttonWidth = 100;
            int buttonCount = 4;
            int yPos = 650;

            // UI tower buttons
            int gap = (canvasWidth - (buttonCount * buttonWidth)) / (buttonCount + 1); // Calculate gap based on canvas width, button count and button width

            archerUIRec = new Rectangle(gap, yPos, buttonWidth, buttonWidth);
            hedgehogUIRec = new Rectangle(gap * 2 + buttonWidth, yPos, buttonWidth, buttonWidth);
            trapperUIRec = new Rectangle(gap * 3 + buttonWidth * 2, yPos, buttonWidth, buttonWidth);
            bomberUIRec = new Rectangle(gap * 4 + buttonWidth * 3, yPos, buttonWidth, buttonWidth);

            // Level manager
            levelManager = new LevelManager(this);
        }

        //------------------------------- Level Manager ----------------------------

        LevelManager levelManager;
        public LevelManager.LevelData currentLevelData;

        int gridSize = 50;

        bool levelLoaded = false;

        int numberOfWaves = 0;
        int currentWave;
        private void loadLevel(string filePath)
        {  
            levelManager.LoadLevel(filePath);
            currentLevelData = levelManager.CurrentLevel;
            numberOfWaves = currentLevelData.Waves.Count;
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
                                g.FillRectangle(grassBrush, rect); // Grass
                                isBuildable = true;
                            }
                            else if (tile == '#')
                            {
                                g.FillRectangle(pathBrush, rect); // Path
                                isPath = true;
                            }
                            else if (tile == '*')
                            {
                                g.FillRectangle(endBrush, rect); // End
                                isPath = true;
                                isEnd = true;
                            }

                            logicalMap[x, y] = new Tile(x, y, isPath, isBuildable, isEnd);
                        }    
                    }
                }
            }
        } 

        private void renderLevel(Graphics g)
        {
            if (!levelLoaded) return;
            g.DrawImage(mapImage, 0, 0);
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
            public int reward;
            public Color color;
            public EnemyInQueueData(int i, int x, int y, int spd, int hth, int rew, Color clr)
            {
                id = i;
                enemyQueueX = x;
                enemyQueueY = y;
                speed = spd;
                health = hth;
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
                for(int j = 0; j < enemyDatas[i].numberOfTheseEnemies; j++)
                {
                    enemyQueue.Enqueue(new EnemyInQueueData(enemyNextID, startX, startY, enemyDatas[i].speed, enemyDatas[i].health, enemyDatas[i].reward, enemyDatas[i].color));
                    enemyNextID++;
                }   
            }
            waveSpawning = true;
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
            if(waveSpawning && enemyQueue.Count > 0)
            {
                EnemyInQueueData data = enemyQueue.Dequeue();
                enemies.Add(new Enemy(this, data.id, data.enemyQueueX, data.enemyQueueY, data.speed, data.health, data.reward, data.color));
            }
            if(enemyQueue != null)waveSpawning = enemyQueue.Count > 0;
        }

        private void renderEnemies(Graphics g)
        {
            foreach (Enemy enemy in enemies)
            {
                using (SolidBrush b = new SolidBrush(enemy.color)) // Memory leak prevention
                {
                    g.FillEllipse(b, enemy.rectangle);
                }

                // Debug
                g.DrawString(enemy.id.ToString(), arialFont, new SolidBrush(Color.White), enemy.rectangle.X, enemy.rectangle.Y);
            }
        }
        private void Form1_KeyDown(object sender, KeyEventArgs e) // Debug: Spawn enemy on spacebar press
        {
            if (e.KeyCode == Keys.Space)
            {
                enemies.Add(new Enemy(this, enemyNextID, 0, 0, 50, 10, 100, Color.Green));
                enemyNextID++;
            }
            else if (e.KeyCode == Keys.L) // Debug: Load level on L press
            {
                if(currentWave != numberOfWaves)startWave(currentWave);
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
            g.DrawString("Archer", arialFont, new SolidBrush(Color.White), archerUIRec.X, archerUIRec.Y + 50);
            g.FillRectangle(towerBrush, hedgehogUIRec);
            g.DrawString("Hedgehog", arialFont, new SolidBrush(Color.White), hedgehogUIRec.X, archerUIRec.Y + 50);
            g.FillRectangle(towerBrush, trapperUIRec);
            g.DrawString("Trapper", arialFont, new SolidBrush(Color.White), trapperUIRec.X, archerUIRec.Y + 50);
            g.FillRectangle(towerBrush, bomberUIRec);
            g.DrawString("Bomber", arialFont, new SolidBrush(Color.White), bomberUIRec.X, archerUIRec.Y + 50);

            
            // Drawing towers
            foreach (Archer archer in archers)
            {
                g.FillRectangle(new SolidBrush(Color.Brown), archer.rectangle);
            }
            foreach (Hedgehog hedgehog in hedgehogs)
            {
                 g.FillRectangle(new SolidBrush(Color.Gray), hedgehog.rectangle);
            }
            foreach (Trapper trapper in trappers)
            {
                 g.FillRectangle(new SolidBrush(Color.Purple), trapper.rectangle);
            }
            foreach (Bomber bomber in bombers)
            {
                 g.FillRectangle(new SolidBrush(Color.Orange), bomber.rectangle);
            }

            // Drawing dragging
            if (isDragging)
                g.FillRectangle(pickedTowerBrush, towerRec);
        }

        private void mainDisplay_MouseDown(object sender, MouseEventArgs e)
        {
            // Relative cursor position to mainDisplay
            int cursorX = e.X;
            int cursorY = e.Y;

            // TOWER VARIANT SELECTION

            if (e.Button == MouseButtons.Left)
            {
                if(archerUIRec.Contains(cursorX, cursorY)) // Check if tower rectangle is clicked
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
            int cursorX = e.X;
            int cursorY = e.Y;
            
            // Moving dragged tower
            if(isDragging)
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
            isDragging = false;

            int cursorX = e.X;
            int cursorY = e.Y;

            int gridX = cursorX / gridSize; // Calculate grid coordinatesy
            int gridY = cursorY / gridSize;

            // Snap to closest grid
            towerRec.X = (int)(Math.Round((float)towerRec.X / gridSize) * gridSize);
            towerRec.Y = (int)(Math.Round((float)towerRec.Y / gridSize) * gridSize);

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
    }
}
