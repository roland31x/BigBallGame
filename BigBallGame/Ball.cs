﻿using BigBallGame.Balls;
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
using System.Windows.Threading;

namespace BigBallGame
{
    public abstract class Ball : IBall
    {
        static readonly Random rng = new Random();
        public int DXSign 
        { 
            get 
            { 
                if (DX == 0) 
                    return 0;  
                else 
                    return Math.Abs(DX) / DX; 
            } 
        }
        public int DYSign
        { 
            get
            {
                if (DY == 0)
                    return 0;
                else
                    return Math.Abs(DY) / DY;
            }   
        }
        public int DX { get; protected set; }
        public int DY { get; protected set; }
        public int Speed { get; protected set; }
        int _radius;
        public int Radius 
        { 
            get 
            { 
                return _radius; 
            } 
            protected set 
            { 
                _radius = value;

                // UI stuff
                Body.Width = _radius * 2; 
                Body.Height = _radius * 2; 
            } 
        }
        bool _alive;
        public bool IsAlive 
        { 
            get 
            { 
                return _alive; 
            } 
            protected set 
            { 
                _alive = value;

                // UI stuff
                if (!_alive) 
                { 
                    Parent!.Children.Remove(Body);
                } 
            } 
        }
        // UI stuff for wpf
        public Ellipse Body { get; protected set; }
        public Canvas? Parent { get; protected set; }
        //

        Point _location;
        public Point Location 
        { 
            get 
            { 
                return _location; 
            } 
            protected set 
            { 
                _location = value;

                // UI update movement;
                Canvas.SetTop(Body, _location.Y - _radius); 
                Canvas.SetLeft(Body, _location.X - _radius); 
            } 
        }
        Color _color;
        public Color BColor 
        { 
            get 
            { 
                return _color; 
            }
            protected set
            {
                _color = value;

                // UI stuff
                Body.Fill = new SolidColorBrush(_color);
            }
        }
        protected List<Ball> IntersectingWith { get; set; }
        protected Ball()
        {
            IntersectingWith = new List<Ball>();
            Body = new Ellipse();
            BColor = Color.FromRgb((byte)rng.Next(10, 250), (byte)rng.Next(10, 250), (byte)rng.Next(10, 250));
            IsAlive = true;
            Radius = rng.Next(5, 20);
            Speed = rng.Next(2, 15);
            DX = rng.Next(-Speed, Speed);
            DY = rng.Next(-Speed, Speed);            
        }
        public abstract Task InteractWith(Ball other);
        public void Spawn(Canvas canvas)
        {
            double x = rng.Next(Radius, (int)canvas.Width - Radius);
            double y = rng.Next(Radius, (int)canvas.Height - Radius);
            canvas.Children.Add(Body);
            Location = new Point(x, y);         
            Parent = canvas;
        }
        //public void Spawn(int XMax, int YMax) // for console app if needed
        //{
        //    double x = rng.Next(Radius, XMax - Radius);
        //    double y = rng.Next(Radius, YMax - Radius);
        //    Location = new Point(x, y);

        //    throw new NotImplementedException(); // because properties set stuff on parent canvas which will be null, code needs to be changed slightly in order for this to work, and console logs need to be added.
        //}

        public async void BeginMove()
        {
            while(IsAlive)
            {
                Move();                              
                await Task.Delay(10);
            }                    
        }
        protected void Move()
        {
            double x, y;
            x = Location.X + DX;
            y = Location.Y + DY;
            if (x >= Parent!.Width - Radius || x <= Radius)
            {
                DX *= -1;
                if (x <= Radius)
                {
                    x = Radius + 1;
                }
                else
                {
                    x = Parent!.Width - Radius;
                }
            }
            if (y >= Parent!.Height - Radius || y <= Radius)
            {
                DY *= -1;
                if (y <= Radius)
                {
                    y = Radius + 1;
                }
                else
                {
                    y = Parent!.Height - Radius;
                }
            }
            Location = new Point(x, y);
        }
        public bool DoIntersect(Ball b)
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
        public void Die()
        {
            IsAlive = false;
        }
        public void IncreaseRadiusBy(int radius)
        {
            Radius += radius;
        }
        public void DecreaseRadiusBy(int radius)
        {
            Radius -= radius;
        }
        public void MultiplyRadiusBy(int amount)
        {
            Radius *= amount;
        }
        public void DivideRadiusBy(int amount)
        {
            Radius /= amount;
        }
        public void SetBodyColorTo(Color color)
        {
            BColor = color;
        }
        public void StartsIntersecting(Ball other)
        {
            IntersectingWith.Add(other);
        }
        public void StopsIntersecting(Ball other)
        {
            IntersectingWith.Remove(other);
        }
        public void OverrideLocation(Point p)
        {
            Location = p;
        }
    }
}
