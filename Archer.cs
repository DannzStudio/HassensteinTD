using HassensteinTD;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


// Shoots arrows at nearest enemies
// has a long range and high damage, but a medium attack speed (1000).
public class Archer
{
    public Rectangle rectangle;
    int id;
    int range = 200 * 200;
    int arrowSpeed = 10;
    int x;
    int y;
    int centerX;
    int centerY;
    public Archer(Form1 form, int i, int x, int y)
    {
        rectangle = new Rectangle(x, y, 50, 50);
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
            calculateArrowDirection(form);
        }
        
    }

    // Debug
    public int nearestEnemyID;

    private void findNearestEnemy(Form1 form) // Find nearest enemy and shoot at it
    {
        nearestEnemy = null;

        // Using already squared distance to avoid using squared root
        double nearestDistSq = range * range;

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
                nearestEnemyID = enemy.id;
                nearestDistance = distSq;
            }
        }
    }

    private void calculateArrowDirection(Form1 form) // Calculate direction of arrow based on position of nearest enemy and archer
    {
        double angle = Math.Atan2(nearestEnemy.rectangle.Y - rectangle.Y, nearestEnemy.rectangle.X - rectangle.X); // Calculate angle between archer and nearest enemy using inverse tan 2d function
        double arrowSpeedX = Math.Cos(angle) * arrowSpeed; // Calculate x value of arrow speed using cosine of angle and arrow speed
        double arrowSpeedY = Math.Sin(angle) * arrowSpeed; // Calculate y value of arrow speed using sine of angle and arrow speed
        int arrowSpeedXInt = (int)Math.Round(arrowSpeedX); // Round x value of arrow speed to nearest integer
        int arrowSpeedYInt = (int)Math.Round(arrowSpeedY); // Round y value of arrow speed to nearest integer
        form.arrows.Add(new Form1.Arrow(x + rectangle.Width / 2, y + rectangle.Height / 2 - 5, 20, arrowSpeedXInt, arrowSpeedYInt)); // Create a new arrow
    }

}

