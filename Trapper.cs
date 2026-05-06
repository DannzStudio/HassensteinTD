using HassensteinTD;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


// Places spikes at nearest path, damaging enemies that step on them.
// Has a very high damage, but a very slow attack speed.
public class Trapper
{
    public Rectangle rectangle;
    int id;
    public Trapper(Form1 form, int i, int x, int y)
    {
        rectangle = new Rectangle(x, y, 50, 50);
        id = i;
    }
}

