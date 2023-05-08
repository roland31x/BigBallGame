using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows;

namespace BigBallGame.Balls
{
    public class RegularBall : Ball
    {
        public RegularBall() : base()
        {

        }
        public override async Task InteractWith(Ball b)
        {           
            if (b.GetType() == typeof(RegularBall))
            {
                await RegularInteraction((RegularBall)b);
            }
            else if (b.GetType() == typeof(RepellentBall))
            {
                await RepellentInteraction((RepellentBall)b); // discard supresses the warning this method needs to run in the background while it keeps interacting with a repellent
            }
            else if (b.GetType() == typeof(MonsterBall))
            {
                await MonsterBallInteraction((MonsterBall)b);
            }
        }
        Task RegularInteraction(RegularBall b)
        {
            if (Radius > b.Radius)
            {
                Radius += b.Radius;
                b.IsAlive = false;
            }
            else
            {
                b.Radius += Radius;
                IsAlive = false;
            }

            int p = Radius + b.Radius;


            Color c1 = BColor;
            Color c2 = b.BColor;
            
            int resR = ((c1.R * Radius + c2.R * b.Radius)) / p;
            int resG = ((c1.G * Radius + c2.G * b.Radius)) / p;
            int resB = ((c1.B * Radius + c2.B * b.Radius)) / p;
            Color result = Color.FromRgb((byte)resR,(byte)resG,(byte)resB);

            b.BColor = result;
            BColor = result;

            return Task.CompletedTask;
        }
        async Task RepellentInteraction(RepellentBall b)
        {
            if (IntersectingWith.Contains(b))
            {
                return;
            }

            IntersectingWith.Add(b);
            b.IntersectingWith.Add(this);

            DX *= -1;
            DY *= -1;
            b.BColor = BColor;
            while (DoIntersect(b))
            {
                //Location = new Point(Location.X + b.DX, Location.Y + b.DY);
                await Task.Delay(1);
            }

            IntersectingWith.Remove(b);
            b.IntersectingWith.Remove(this);
            //return Task.CompletedTask;
        }
        Task MonsterBallInteraction(MonsterBall b)
        {
            b.Radius += Radius;
            IsAlive = false;

            return Task.CompletedTask;
        }
    }
}
