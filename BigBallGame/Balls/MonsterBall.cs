using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;

namespace BigBallGame.Balls
{
    public class MonsterBall : Ball
    {
        public MonsterBall() : base()
        {
            BColor = Color.FromRgb(0, 0, 0);
            Speed = 0;
            DX = 0;
            DY = 0;
        }
        public override async Task InteractWith(Ball b)
        {
            if (b.GetType() == typeof(RegularBall))
            {
                await b.InteractWith(this); // regularball has interaction logic with monsterball
            }
            else if (b.GetType() == typeof(RepellentBall))
            {
                await b.InteractWith(this); // repellent already has interaction logic with monsterball
            }
            else if (b.GetType() == typeof(MonsterBall))
            {
                throw new ArithmeticException("Monsterballs cannot intersect eachother");
            }
        }
    }
}
