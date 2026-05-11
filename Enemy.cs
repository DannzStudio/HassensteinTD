using HassensteinTD;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


public class Enemy
{
    public int id;

    public int speed;
    public int health;
    public int reward;
    public Color color;

    public Rectangle rectangle;

    int levelGridWidth = 19;
    int levelGridHeight = 13;
    Form1.Tile[,] logicalMap;
    int levelStartX;
    int levelStartY;
    public Enemy(Form1 form, int i, int x, int y, int spd, int hth, int rew, Color clr)
    {
        // Initializing enemy values

        rectangle = new Rectangle(x, y, 50, 50);  
        id = i;

        speed = spd;
        health = hth;
        reward = rew;
        color = clr;

        logicalMap = form.logicalMap;
        levelStartX = form.currentLevelData.startPosX;
        levelStartY = form.currentLevelData.startPosY;

    }

    Queue<int> moveSet; // 1-→ | 2-↓ | 3-← | 4-↑

    void generateMoveSet()
    {
        moveSet = new Queue<int>();
        int currentX;
        int currentY;

        for(int y = levelStartY; y < logicalMap.GetLength(1); y++)
        {
            for(int x = levelStartX; x < logicalMap.GetLength(0); x++)
            {
                moveSet.Enqueue(1);
            }
        }

        bool a = logicalMap[levelStartX, levelStartY].isBuildable;
    }

    public void move(Form1 form)
    {
        rectangle.Offset(speed, 0); // Move right by speed pixels
    }
}

