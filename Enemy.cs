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
    public Enemy(Form1 form, int i, int x, int y, int spd, int hth, int rew, Color clr)
    {
        // Initializing enemy values

        rectangle = new Rectangle(x, y, 50, 50);  
        id = i;

        speed = spd;
        health = hth;
        reward = rew;
        color = clr;
    }

    public void move(Form1 form)
    {
        rectangle.Offset(speed, 0); // Move right by speed pixels
    }
}

