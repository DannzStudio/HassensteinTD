using HassensteinTD;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


// Shoots arrows at nearest enemies
// has a long range and high damage, but a medium attack speed.
public class Archer
{
    public Rectangle rectangle;
    int id;
    public Archer(Form1 form, int i, int x, int y)
    {
        rectangle = new Rectangle(x, y, 50, 50);
        id = i;
    }
}

