using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;

namespace BigBallGame.Balls
{
    public class RepellentBall : Ball
    {
        public RepellentBall() : base()
        {

        }
        public override async Task InteractWith(Ball b)
        {
            if (b.GetType() == typeof(RegularBall))
            {
                await b.InteractWith(this);
            }
            else if (b.GetType() == typeof(RepellentBall))
            {
                await RepellentInteraction((RepellentBall)b);
            }
            else if (b.GetType() == typeof(MonsterBall))
            {
                await MonsterBallInteraction((MonsterBall)b);
            }
        }

        Task MonsterBallInteraction(MonsterBall b)
        {
            Radius /= 2;
            if (Radius < 1)
            {
                IsAlive = false;
            }
            return Task.CompletedTask;
        }

        Task RepellentInteraction(RepellentBall b)
        {
            (BColor, b.BColor) = (b.BColor, BColor);
            return Task.CompletedTask;
        }
    }
}
