using System;

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

            renderEnemies(g);
            renderDragAndDrop(g);
        }

        private void mainUpdate_Tick(object sender, EventArgs e)
        {
            mainDisplay.Refresh();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            init();
        }

        Font arialFont = new Font("Arial", 14, FontStyle.Bold);

        //------------------------------- Enemy Logic ------------------------------

        public List<Enemy> enemies = new List<Enemy>();

        public int enemyCount = 0; // For enemy ID

        

        private void enemyUpdate_Tick(object sender, EventArgs e)
        {
            foreach (Enemy enemy in enemies) // Moving enemies
            {
                enemy.move(this);
            }
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
                enemies.Add(new Enemy(this, enemyCount, 0, 0, 50, 10, 100, Color.Green));
                enemyCount++;
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
        int gridSize = 50;

        

        private void init()
        {
            int canvasWidth = 950;
            int itemWidth = 100;
            int itemCount = 4;
            int yPos = 650;

            // UI tower buttons
            int gap = (canvasWidth - (itemCount * itemWidth)) / (itemCount + 1); // Calculate gap based on canvas width, item count and item width

            archerUIRec = new Rectangle(gap, yPos, itemWidth, itemWidth);
            hedgehogUIRec = new Rectangle(gap * 2 + itemWidth, yPos, itemWidth, itemWidth);
            trapperUIRec = new Rectangle(gap * 3 + itemWidth * 2, yPos, itemWidth, itemWidth);
            bomberUIRec = new Rectangle(gap * 4 + itemWidth * 3, yPos, itemWidth, itemWidth);
        }

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

            // Snap to closest grid
            towerRec.X = (int)(Math.Round((float)towerRec.X / gridSize) * gridSize);
            towerRec.Y = (int)(Math.Round((float)towerRec.Y / gridSize) * gridSize);

            if (towerRec.Y <= 600)
            {
                bool intersects = false;

                // Checking for existing towers and not allowing overplacement (intersection of rectangles)

                foreach (Archer a in archers)
                {
                    if (towerRec.IntersectsWith(a.rectangle)) { intersects = true; break; }
                }

                if (!intersects)
                {
                    foreach (Hedgehog h in hedgehogs)
                    {
                        if (towerRec.IntersectsWith(h.rectangle)) { intersects = true; break; }
                    }
                }

                if (!intersects)
                {
                    foreach (Trapper t in trappers)
                    {
                        if (towerRec.IntersectsWith(t.rectangle)) { intersects = true; break; }
                    }
                }

                if (!intersects)
                {
                    foreach (Bomber b in bombers)
                    {
                        if (towerRec.IntersectsWith(b.rectangle)) { intersects = true; break; }
                    }
                }

                // Build tower
                if (!intersects)
                {
                    switch (draggingID)
                    {
                        case 1:
                            archers.Add(new Archer(this, archerCount++, towerRec.X, towerRec.Y));
                            break;
                        case 2:
                            hedgehogs.Add(new Hedgehog(this, hedgehogCount++, towerRec.X, towerRec.Y));
                            break;
                        case 3:
                            trappers.Add(new Trapper(this, trapperCount++, towerRec.X, towerRec.Y));
                            break;
                        case 4:
                            bombers.Add(new Bomber(this, bomberCount++, towerRec.X, towerRec.Y));
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
