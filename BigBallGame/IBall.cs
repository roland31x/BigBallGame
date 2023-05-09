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
        public Task InteractWith(Ball other);
        public void Spawn(Canvas canvas);
        //public void Spawn(int XMax, int YMax);
    }
}
