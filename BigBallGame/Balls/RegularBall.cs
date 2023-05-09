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
                await RepellentInteraction((RepellentBall)b);
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
                this.IncreaseRadiusBy(b.Radius);
                b.Die();
            }
            else
            {
                b.IncreaseRadiusBy(Radius);
                this.Die();
            }

            int p = Radius + b.Radius;


            Color c1 = BColor;
            Color c2 = b.BColor;
            
            int resR = ((c1.R * Radius + c2.R * b.Radius)) / p;
            int resG = ((c1.G * Radius + c2.G * b.Radius)) / p;
            int resB = ((c1.B * Radius + c2.B * b.Radius)) / p;
            Color result = Color.FromRgb((byte)resR,(byte)resG,(byte)resB);

            b.SetBodyColorTo(result);
            this.SetBodyColorTo(result);

            return Task.CompletedTask;
        }
        async Task RepellentInteraction(RepellentBall b)
        {
            if (IntersectingWith.Contains(b))
            {
                return;
            }

            this.StartsIntersecting(b);
            b.StartsIntersecting(this);

            DX *= -1;
            DY *= -1;
            b.SetBodyColorTo(BColor);
            while (DoIntersect(b))
            {
                //Location = new Point(Location.X + b.DX, Location.Y + b.DY);
                await Task.Delay(1);
            }

            this.StopsIntersecting(b);
            b.StopsIntersecting(this);
            //return Task.CompletedTask;
        }
        Task MonsterBallInteraction(MonsterBall b)
        {
            b.IncreaseRadiusBy(this.Radius);
            this.Die();

            return Task.CompletedTask;
        }
    }
}
