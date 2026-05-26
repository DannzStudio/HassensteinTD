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
    public int damage;
    public int reward;
    
    public Color color;
    public Image sprite;

    public Rectangle rectangle;


    Queue<int> moveSet = new Queue<int>();
    public Enemy(Form1 form, int i, int x, int y, int spd, int hth, int dmg, int rew, Color clr )
    {
        // Initializing enemy values

        rectangle = new Rectangle(x, y, 50, 50);  
        id = i;

        speed = spd;
        health = hth;
        damage = dmg;
        reward = rew;
        color = clr;


        getMoveSet(form);
        getSprite(form);
    }

    private void getMoveSet(Form1 form) // Convert move set list in form into queue
    {
        foreach(int dir in form.moveSet)
        {
            moveSet.Enqueue(dir);
        }
    }

    private void getSprite(Form1 form)
    {
        using(Image baseSprite = Image.FromFile("Images/Enemy.png"))
        {
            sprite = form.TintImage(baseSprite, color);
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

