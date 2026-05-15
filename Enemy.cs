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

        levelStartX = form.currentLevelData.startPosX;
        levelStartY = form.currentLevelData.startPosY;

        generateMoveSet(form);
    }

    Queue<int> moveSet; // 1-→ | 2-↓ | 3-← | 4-↑

    void generateMoveSet(Form1 form) // Get surround paths to get next dir, also check for last cords so the enemies dont go backwards
    {
        moveSet = new Queue<int>();

        int currentX = levelStartX;
        int currentY = levelStartY;

        int lastX = -1;
        int lastY = -1;

        int width = form.logicalMap.GetLength(0);
        int height = form.logicalMap.GetLength(1);

        while (true)
        {
            //break if enemy is on end point
            if (form.logicalMap[currentX, currentY].isEnd)
            {
                break;
            }

            int nextX = -1;
            int nextY = -1;
            int direction = -1;

            // Right
            if (currentX + 1 < width && form.logicalMap[currentX + 1, currentY].isPath && (currentX + 1 != lastX || currentY != lastY))
            {
                nextX = currentX + 1;
                nextY = currentY;
                direction = 1;
            }
            // Down
            else if (currentY + 1 < height && form.logicalMap[currentX, currentY + 1].isPath && (currentX != lastX || currentY + 1 != lastY))
            {
                nextX = currentX;
                nextY = currentY + 1;
                direction = 2;
            }
            // Left
            else if (currentX - 1 >= 0 && form.logicalMap[currentX - 1, currentY].isPath && (currentX - 1 != lastX || currentY != lastY))
            {
                nextX = currentX - 1;
                nextY = currentY;
                direction = 3;
            }
            // Up
            else if (currentY - 1 >= 0 && form.logicalMap[currentX, currentY - 1].isPath && (currentX != lastX || currentY - 1 != lastY))
            {
                nextX = currentX;
                nextY = currentY - 1;
                direction = 4;
            }

            // if nothing is find break the loop
            if (direction == -1)
            {
                break;
            }

            moveSet.Enqueue(direction);

            // Move to next path
            lastX = currentX;
            lastY = currentY;
            currentX = nextX;
            currentY = nextY;
        }
    }

    public void move(Form1 form)
    {
        if(moveSet.Count > 0) // Move based on dir in move set queue
        {
            int dir = moveSet.Dequeue();
            switch(dir)
            {
                case 1:
                    rectangle.Offset(speed, 0);
                    break;
                case 2:
                    rectangle.Offset(0, speed);
                    break;
                case 3:
                    rectangle.Offset(-speed, 0);
                    break;
                case 4:
                    rectangle.Offset(0, -speed);
                    break;
            }
            
        }
    }
}

