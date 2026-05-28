using HassensteinTD;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


// Shoots bombs at nearest enemies
// has a long range and high damage with splash damage, but a slow attack speed (1500).
// Cost: 3000
public class Bomber
{
    public Rectangle rectangle;
    int id;
    int range = 300;
    int bombSpeed = 10;
    int explosionRadius = 150;
    int damage = 20;

    int x;
    int y;
    int centerX;
    int centerY;
    public Bomber(Form1 form, int i, int x, int y)
    {
        rectangle = new Rectangle(x, y, 50, 50);
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
        if (nearestEnemy != null && nearestDistance < range)
        {
            calculateBombDirection(form); // Shoot a bomb
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

            // Pythagorean theorem to calculate distance squared between bomber and enemy
            double distSq = (dx * dx) + (dy * dy);

            if (distSq < nearestDistSq)
            {
                nearestDistSq = distSq;
                nearestEnemy = enemy;
                nearestDistance = distSq;
            }
        }
    }

    private void calculateBombDirection(Form1 form) // Calculate direction of bomb based on position of nearest enemy
    {
        double angle = Math.Atan2(nearestEnemy.rectangle.Y - rectangle.Y, nearestEnemy.rectangle.X - rectangle.X); // Calculate angle between bomber and nearest enemy using inverse tan 2d function

        double bombSpeedX = Math.Cos(angle) * bombSpeed; // Calculate speed of bomb using cos/sin of angle and bomb speed
        double bombSpeedY = Math.Sin(angle) * bombSpeed;

        int bombSpeedXInt = (int)Math.Round(bombSpeedX); // Round speed of bomb speed to nearest integer
        int bombSpeedYInt = (int)Math.Round(bombSpeedY);

        form.bombs.Add(new Form1.Bomb(x + rectangle.Width / 2 - 5, y + rectangle.Height / 2 - 5, damage, bombSpeedXInt, bombSpeedYInt, explosionRadius)); // Create a new bomb
    }
}

