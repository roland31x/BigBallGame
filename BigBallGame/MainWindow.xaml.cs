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
        bool SP = false;
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
            if (!SP)
            {
                SP = true;
                for (int i = 0; i < 10; i++)
                {
                    Ball b = new RegularBall();
                    b.Spawn(MainCanvas);
                }
                for (int i = 0; i < 2; i++)
                {
                    RepellentBall b = new RepellentBall();
                    b.Spawn(MainCanvas);
                }
                //MonsterBall ba = new MonsterBall();
                // ba.Spawn(MainCanvas);
                await Task.Delay(100);
            }           
            else
            {
                foreach (Ball b in Ball.Balls)
                {
                    b.Move();
                }                             
            }
        }
    }
}
