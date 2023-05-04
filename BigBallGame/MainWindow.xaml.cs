using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace BigBallGame
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        bool Started = false;
        bool Spawned = false;
        public MainWindow()
        {
            InitializeComponent();
            KeyDown += MainWindow_KeyDown;
        }

        private void MainWindow_KeyDown(object sender, KeyEventArgs e)
        {
            if(e.Key == Key.Escape) 
            {
                this.Close();
            }
        }

        private async void Button_Click(object sender, RoutedEventArgs e)
        {
            if (!Spawned)
            {
                Spawned = true;
                for (int i = 0; i < 10; i++)
                {
                    Ball b = new RegularBall();
                    b.Spawn(MainCanvas);
                }
                for (int i = 0; i < 5; i++)
                {
                    Ball b = new RepellentBall();
                    b.Spawn(MainCanvas);
                }
                //for (int i = 0; i < 1; i++)
                //{
                //    Ball b = new MonsterBall();
                //    b.Spawn(MainCanvas);
                //}
                await Task.Delay(100);
            }           
            else if (!Started)
            {
                (sender as Button).IsEnabled = false;
                await Ball.PlayGame();

                MessageBox.Show("Game finished.");
                (sender as Button).IsEnabled = true;
                MainCanvas.Children.Clear();
                Ball.Balls.Clear();
                Spawned = false;
                Started = false;
            }
        }
    }
}
