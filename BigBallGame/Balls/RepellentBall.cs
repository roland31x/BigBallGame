using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using System.Windows.Threading;

namespace BigBallGame.Balls
{
    public class RepellentBall : Ball
    {
        DispatcherTimer GraceTimer;
        bool IsInGracePeriod { get; set; }
        public RepellentBall() : base()
        {
            IsInGracePeriod = false;
            GraceTimer = new DispatcherTimer();
            GraceTimer.Interval = TimeSpan.FromSeconds(2);
            GraceTimer.Tick += Dp_Tick;
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
            if (IsInGracePeriod)
            {
                return Task.CompletedTask;
            }

            IsInGracePeriod = true;
            this.DivideRadiusBy(2);
            if (Radius < 4)
            {
                IsAlive = false;
            }
            GraceTimer.Start();

            return Task.CompletedTask;
        }

        private void Dp_Tick(object? sender, EventArgs e)
        {
            IsInGracePeriod = false;
            (sender as DispatcherTimer)!.Stop();
        }

        async Task RepellentInteraction(RepellentBall b)
        {
            if (IntersectingWith.Contains(b))
            {
                return;
            }

            this.StartsIntersecting(b);
            b.StartsIntersecting(this);

            Color aux = BColor;
            this.SetBodyColorTo(b.BColor);
            b.SetBodyColorTo(aux);
            
            while (DoIntersect(b))
            {
                
                await Task.Delay(1);
            }

            this.StopsIntersecting(b);
            b.StopsIntersecting(this);

        }
    }
}
