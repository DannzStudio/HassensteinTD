using HassensteinTD;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


// Places spikes at nearest path, damaging enemies that step on them.
// Has a very high damage, but a very slow attack speed (5000).
// Cost: 1500
public class Trapper
{
    public Rectangle rectangle;
    int id;
    int damage = 20;
    int x;
    int y;
    public Trapper(Form1 form, int i, int x, int y)
    {
        rectangle = new Rectangle(x, y, 50, 50);
        id = i;
        this.x = x;
        this.y = y;
        findNearestPath(form);
    }

    public void trapperAttack(Form1 form)
    {
        mySpike.numOfSpikes++;
    }

    Form1.Spike mySpike; // The spike placed by the trapper

    private void findNearestPath(Form1 form) // Find nearest path to place trap on
    {
        int nearestDist = 9999999;
        int nearestX = 0;
        int nearestY = 0;
        foreach (Form1.Tile tile in form.logicalMap)
        {
            if(!tile.isPath) continue; // Only check path

            int pathX = tile.gridX * 50; // Convert grid coordinates to pixel coordinates
            int pathY = tile.gridY * 50;

            int dx = pathX - x; // Distance x
            int dy = pathY - y; // Distance y
            int distSq = dx * dx + dy * dy; // Squared distance

            if (distSq < nearestDist)
            {
                nearestDist = distSq;
                nearestX = pathX;
                nearestY = pathY;
            }
        }
        mySpike = new Form1.Spike(nearestX, nearestY, damage);
        form.spikes.Add(mySpike); // Place a spike point on the nearest path tile
    }
}

