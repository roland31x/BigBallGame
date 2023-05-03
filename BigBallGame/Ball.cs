using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace BigBallGame
{
    public abstract class Ball
    {
        public static List<Ball> Balls = new List<Ball>();
        static readonly Random rng = new Random();
        public double DX { get; set; }
        public double DY { get; set; }
        public int Speed { get; set; }
        public int Radius { get; set; }
        public bool IsAlive { get; set; }
        public Ellipse Body { get; set; }
        protected Canvas parent { get; set; }
        public Point Location { get; set; }
        
        static int AliveRegularBalls { get { return Balls.Where(x => (x.GetType() == typeof(RegularBall)) && (x.IsAlive)).Count(); } }
        public abstract void InteractWith(Ball other);
        public void Spawn(Canvas canvas)
        {
            double x = rng.Next(Radius, (int)canvas.Width - Radius);
            double y = rng.Next(Radius, (int)canvas.Height - Radius);
            Location = new Point(x, y);
            canvas.Children.Add(Body);
            Canvas.SetLeft(Body, Location.X - Radius);
            Canvas.SetTop(Body, Location.Y - Radius);
            parent = canvas;

        }
        public static async Task PlayGame()
        {
            foreach (Ball b in Balls)
            {
                if(b.parent == null)
                {
                    throw new Exception("Ball wasn't spwaned");
                }
                b.Move();
            }
            while(AliveRegularBalls > 0)
            {
                await Task.Delay(100);
            }
        }
        async void Move()
        {
            double x = Location.X;
            double y = Location.Y;
            Stopwatch sw = new Stopwatch();
            sw.Start();
            while(IsAlive)
            {
                x = Location.X + DX;
                y = Location.Y + DY;
                if (x >= parent.Width - Radius || x <= Radius)
                {
                    DX *= -1;
                    if(x <= Radius)
                    {
                        x = Radius + 1;
                    }
                    else
                    {
                        x = parent.Width - Radius;
                    }
                }
                if (y >= parent.Height - Radius || y <= Radius)
                {
                    DY *= -1;
                    if (y <= Radius)
                    {
                        y = Radius + 1;
                    }
                    else
                    {
                        y = parent.Height - Radius;
                    }
                }
                Location = new Point(x, y);
                await IntersectionCheck();
                Canvas.SetLeft(Body, Location.X - Radius);
                Canvas.SetTop(Body, Location.Y - Radius);

                await Task.Delay(10);
            }
            sw.Stop();
                       
        }
        protected Ball()
        {
            IsAlive = true;
            Balls.Add(this);
            Radius = rng.Next(5, 20);
            Speed = rng.Next(1, 10);
            DX = rng.Next(-Speed, Speed);
            DY = rng.Next(-Speed, Speed);            
            Body = new Ellipse()
            {
                Height = Radius * 2,
                Width = Radius * 2,
                Fill = new SolidColorBrush(Color.FromRgb((byte)rng.Next(10, 255), (byte)rng.Next(10, 255), (byte)rng.Next(10, 255))),
            };
                     
        }
        async Task IntersectionCheck()
        {
            foreach(Ball b in Balls)
            {
                if(b == this || !b.IsAlive)
                {
                    continue;
                }
                if (this.DoIntersect(b))
                {
                    this.InteractWith(b);
                }               
            }
        }
        protected bool DoIntersect(Ball b)
        {
            double radiusdist = Radius + b.Radius;
            if (radiusdist > GetDist(b))
            {
                return true;
            }
            return false;
        }
        protected double GetDist(Ball other)
        {
            return Math.Sqrt((Location.X - other.Location.X) * (Location.X - other.Location.X) + (Location.Y - other.Location.Y) * (Location.Y - other.Location.Y));
        }
    }
    public class RegularBall : Ball
    {
        public RegularBall() : base()
        {

        }
        public override void InteractWith(Ball b)
        {
            if(b.GetType() == typeof(RegularBall))
            {
                RegularInteraction((RegularBall)b);
            }
            else if(b.GetType() == typeof(RepellentBall))
            {
                RepellentInteraction((RepellentBall)b);
            }
            else if(b.GetType() == typeof(MonsterBall))
            {
                MonsterBallInteraction((MonsterBall)b);
            }
        }
        void RegularInteraction(RegularBall b)
        {
            if (Radius > b.Radius)
            {
                Radius += b.Radius;
                b.IsAlive = false;
                Body.Height = Radius * 2;
                Body.Width = Radius * 2;
                parent.Children.Remove(b.Body);
            }
            else
            {
                b.Radius += Radius;
                IsAlive = false;
                b.Body.Height = b.Radius * 2;
                b.Body.Width = b.Radius * 2;
                parent.Children.Remove(Body);
            }
            //Color c1 = (b.Body.Fill as SolidColorBrush).Color;
            //Color c2 = (Body.Fill as SolidColorBrush).Color;
            //Color result = Color.Add(c1, c2);
            //b.Body.Fill = new SolidColorBrush(result);
            //Body.Fill = new SolidColorBrush(result);
        }
        void RepellentInteraction(RepellentBall b)
        {
            DX *= -1;
            DY *= -1;
            b.Body.Fill = Body.Fill;
            while (DoIntersect(b))
            {
                Location = new Point(Location.X + b.DX, Location.Y + b.DY);
            }                        
        }
        void MonsterBallInteraction(MonsterBall b)
        {
            b.Radius += Radius;
            b.Body.Height = b.Radius * 2;
            b.Body.Width = b.Radius * 2;
            IsAlive = false;
            parent.Children.Remove(this.Body);
        }
    }
    public class RepellentBall : Ball
    {
        public RepellentBall() : base()
        {

        }
        public override void InteractWith(Ball b)
        {
            if (b.GetType() == typeof(RegularBall))
            {
                return;
            }
            else if (b.GetType() == typeof(RepellentBall))
            {
                RepellentInteraction((RepellentBall)b);
            }
            else if (b.GetType() == typeof(MonsterBall))
            {
                MonsterBallInteraction((MonsterBall)b);
            }
        }

        private void MonsterBallInteraction(MonsterBall b)
        {
            Radius /= 2;
            if(Radius < 1)
            {
                IsAlive = false;
            }
            Body.Height = Radius * 2;
            Body.Width = Radius * 2;
        }

        void RepellentInteraction(RepellentBall b)
        {
            (Body.Fill, b.Body.Fill) = (b.Body.Fill, Body.Fill);
        }
    }
    public class MonsterBall : Ball
    {
        public MonsterBall() : base()
        {
            Body.Fill = Brushes.Black;
            Speed = 0;
            DX = 0;
            DY = 0;
        }
        public override void InteractWith(Ball other)
        {

        }
    }
}
