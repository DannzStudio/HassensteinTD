using HassensteinTD;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


// Shoots around itself in a circle, damaging all enemies in range at once.
// has a short range and medium damage, but a fast attack speed (500).
// Cost: 1000
public class Hedgehog
{
    public Rectangle rectangle;
    public Rectangle rangeRec; // Debug
    int id;
    int range = 200;
    int arrowSpeed = 10;
    int damage = 2;
    int x;
    int y;
    int centerX;
    int centerY;
    public Hedgehog(Form1 form, int i, int x, int y)
    {
        rectangle = new Rectangle(x, y, 50, 50);
        rangeRec = new Rectangle(x - range + 25, y - range + 25, range * 2, range * 2); // DEBUG
        id = i;
        range *= range;
        this.x = x;
        this.y = y;
        this.centerX = x + rectangle.Width / 2;
        this.centerY = y + rectangle.Height / 2;
    }

    public void hedgehogAttack(Form1 form)
    {
        findNearestEnemy(form);
        if(nearestEnemy != null && nearestDistance < range)
        {
            shoot(form);
        }
    }

    Enemy nearestEnemy;
    double nearestDistance = 9999999;

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

    int numOfArrows = 8;

    private void shoot(Form1 form)
    {
        double offsetOfAngle = 2 * Math.PI / numOfArrows;
        for (int i = 0; i < numOfArrows; i++)
        {
            double angle = i * offsetOfAngle;

            double arrowSpeedX = Math.Cos(angle) * arrowSpeed; // Calculate arrow speed using cos/sin of angle and arrow speed
            double arrowSpeedY = Math.Sin(angle) * arrowSpeed;

            int arrowSpeedXInt = (int)Math.Round(arrowSpeedX); // Round arrow speed to nearest int
            int arrowSpeedYInt = (int)Math.Round(arrowSpeedY);

            form.arrows.Add(new Form1.Arrow(x + rectangle.Width / 2 - 5, y + rectangle.Height / 2 - 5, damage, arrowSpeedXInt, arrowSpeedYInt)); // Create a new arrow
        }
    }
}

