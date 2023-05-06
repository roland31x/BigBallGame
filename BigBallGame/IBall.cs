using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows;

namespace BigBallGame
{
    public interface IBall
    {
        public int DX { get; set; }
        public int DY { get; set; }
        public int Speed { get; set; }
        public int Radius { get; set; }
        public Color BColor { get; set; }
        public Point Location { get; set; }
        public bool IsAlive { get; set; }

        public Task InteractWith(Ball other);
        public void Spawn(Canvas canvas);
        //public void Spawn(int XMax, int YMax);
    }
}
