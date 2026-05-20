using HassensteinTD;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


// Shoots bombs at nearest enemies
// has a long range and high damage with splash damage, but a slow attack speed (1500).
public class Bomber
{
    public Rectangle rectangle;
    int id;
    public Bomber(Form1 form, int i, int x, int y)
    {
        rectangle = new Rectangle(x, y, 50, 50);
        id = i;
    }
}

