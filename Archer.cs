using HassensteinTD;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


// Shoots arrows at nearest enemies
// has a long range and high damage, but a medium attack speed (1000).
// Cost: 200
public class Archer
{
    public Rectangle rectangle;
    public Rectangle rangeRec; // Debug
    int id;
    int range = 300;
    int arrowSpeed = 10;
    int damage = 5;
    int x;
    int y;
    int centerX;
    int centerY;
    public Archer(Form1 form, int i, int x, int y)
    {      
        rectangle = new Rectangle(x, y, 50, 50);
        rangeRec = new Rectangle(x - range + 25, y - range + 25, range * 2, range * 2); // DEBUG
        range *= range;
        id = i;
        this.x = x;
        this.y = y;
        this.centerX = x + rectangle.Width / 2;
        this.centerY = y + rectangle.Height / 2;
    }

    Enemy nearestEnemy;
    double nearestDistance = 9999999;

    public void archerAttack(Form1 form)
    {
        findNearestEnemy(form);
        if(nearestEnemy != null && nearestDistance < range)
        {
            calculateArrowDirection(form); // Shoot an arrow
        }
        
    }

    private void findNearestEnemy(Form1 form) // Find nearest enemy
    {
        nearestEnemy = null;

        // Using already squared distance to avoid using squared root (c2 = a2 + b2)
        double nearestDistSq = range;

        foreach (Enemy enemy in form.enemies)
        {
            int enemyCenterX = enemy.rectangle.X + enemy.rectangle.Width / 2;
            int enemyCenterY = enemy.rectangle.Y + enemy.rectangle.Height / 2;

            double dx = enemyCenterX - centerX; // Distance x
            double dy = enemyCenterY - centerY; // Distance y

            // Pythagorean theorem to calculate distance squared between archer and enemy
            double distSq = (dx * dx) + (dy * dy);

            if (distSq < nearestDistSq)
            {
                nearestDistSq = distSq;
                nearestEnemy = enemy;
                nearestDistance = distSq;
            }
        }
    }

    private void calculateArrowDirection(Form1 form) // Calculate direction of arrow based on position of nearest enemy
    {
        double angle = Math.Atan2(nearestEnemy.rectangle.Y - rectangle.Y, nearestEnemy.rectangle.X - rectangle.X); // Calculate angle between archer and nearest enemy using inverse tan 2d function
        
        double arrowSpeedX = Math.Cos(angle) * arrowSpeed; // Calculate speed of arrow using cos/sin of angle and arrow speed
        double arrowSpeedY = Math.Sin(angle) * arrowSpeed;

        int arrowSpeedXInt = (int)Math.Round(arrowSpeedX); // Round speed of arrow speed to nearest integer
        int arrowSpeedYInt = (int)Math.Round(arrowSpeedY);

        form.arrows.Add(new Form1.Arrow(x + rectangle.Width / 2 - 5, y + rectangle.Height / 2 - 5, damage, arrowSpeedXInt, arrowSpeedYInt)); // Create a new arrow
    }

}

