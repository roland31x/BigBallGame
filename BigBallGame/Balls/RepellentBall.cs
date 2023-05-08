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
        public bool IsInGracePeriod { get; set; }
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
            Radius /= 2;
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

            IntersectingWith.Add(b);
            b.IntersectingWith.Add(this);

            (BColor, b.BColor) = (b.BColor, BColor);
            
            while (DoIntersect(b))
            {
                
                await Task.Delay(1);
            }

            IntersectingWith.Remove(b);
            b.IntersectingWith.Remove(this);

        }
    }
}
