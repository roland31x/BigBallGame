using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Threading;
using BigBallGame.Balls;

namespace BigBallGame
{
    public static class BallGame
    {
        static List<Ball> Balls = new List<Ball>();
        static int AliveRegularBalls
        {
            get
            {
                return Balls.Where(x => (x.GetType() == typeof(RegularBall)) && (x.IsAlive)).Count();
            }
        }
        static DispatcherTimer Timer = new DispatcherTimer();
        public static async Task PlayGame()
        {
            Timer = new DispatcherTimer() { Interval = TimeSpan.FromMilliseconds(20) };
            Timer.Tick += Timer_Tick;
            foreach (Ball b in Balls)
            {
                if (b.Parent! == null)
                {
                    throw new Exception("Ball wasn't spwaned");
                }
                b.BeginMove();
            }
            Timer.Start();
            while (AliveRegularBalls > 0)
            {
                await Task.Delay(1000);
            }
            Timer.Stop();
        }

        private static async void Timer_Tick(object? sender, EventArgs e)
        {
            List<Task> tasks = new List<Task>();
            for (int i = 0; i < Balls.Count; i++)
            {
                if (!Balls[i].IsAlive)
                {
                    continue;
                }
                for (int j = i + 1; j < Balls.Count; j++)
                {
                    if (!Balls[j].IsAlive)
                    {
                        continue;
                    }
                    if (Balls[i].DoIntersect(Balls[j]))
                    {
                        Task task = Balls[i].InteractWith(Balls[j]);
                        tasks.Add(task);
                    }

                }
            }
            await Task.WhenAll(tasks);
        }
        public static void Reset()
        {
            foreach(Ball b in Balls)
            {
                b.Die();
            }
            Balls = new List<Ball>();
        }
        public static void SpawnBall<T>(Canvas canvas, int x, int y) where T : Ball, new()
        {
            Ball spawned = new T();
            Balls.Add(spawned);
            spawned.Spawn(canvas);
        }
        public static void SpawnBall<T>(Canvas canvas) where T : Ball, new()
        {
            Ball spawned = new T();
            Balls.Add(spawned);
            spawned.Spawn(canvas);           
        }
    }
}
