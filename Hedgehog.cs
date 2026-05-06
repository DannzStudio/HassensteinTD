using HassensteinTD;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


// Shoots around itself in a circle, damaging all enemies in range at once.
// has a short range and medium damage, but a very fast attack speed.
public class Hedgehog
{
    public Rectangle rectangle;
    int id;
    public Hedgehog(Form1 form, int i, int x, int y)
    {
        rectangle = new Rectangle(x, y, 50, 50);
        id = i;
    }
}

