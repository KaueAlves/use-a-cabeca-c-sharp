using System;
using System.Collections.Generic;
using System.Diagnostics;
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
using System.Windows.Threading;

namespace matchgame
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        TextBlock lastTBlockClicked;
        bool findingMatch = false;

        Stopwatch stopwatch = new Stopwatch();
        double tenthsOfSeconsElapsed;
        double restante;
        int matchesFound;

        public MainWindow()
        {
            InitializeComponent();
            SetUpGame();
        }

        private void GameLoop(object sender, EventArgs e)
        {
            restante = tenthsOfSeconsElapsed - stopwatch.Elapsed.TotalSeconds;
            if (restante <= 0)
            {
                restante = 0;
                CompositionTarget.Rendering -= GameLoop;
            }
            timeTextBlock.Text = restante.ToString("0,0s");
            if (restante <= 0 || matchesFound == 8)
            {
                stopwatch.Stop();
                timeTextBlock.Text += " - Play Again";
            }
        }

        private void SetUpGame()
        {
            List<string> animaisEmPares = new List<string>() {
                "🐩","🐩",
                "🙊","🙊",
                "🐶","🐶",
                "🦍","🦍",
                "🐮","🐮",
                "🐑","🐑",
                "🐍","🐍",
                "🐷","🐷"
            };

            Random randomAnimais = new Random();

            foreach (TextBlock textBlock in mainGrid.Children.OfType<TextBlock>())
            {
                if (animaisEmPares.ToList().Count() == 0) break;
                textBlock.Visibility = Visibility.Visible;
                int index = randomAnimais.Next(animaisEmPares.Count);
                string nextAnimal = animaisEmPares.ToList()[index];
                textBlock.Text = nextAnimal;
                animaisEmPares.Remove(nextAnimal);
            }

            stopwatch.Restart();
            tenthsOfSeconsElapsed = 5;
            matchesFound = 0;
            CompositionTarget.Rendering += GameLoop;

        }

        private void dataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void TextBlock_MouseDown(object sender, MouseButtonEventArgs e)
        {
            TextBlock textBlock = sender as TextBlock;
            if (restante <= 0)
            {
                return;
            }

            if (findingMatch == false)
            {
                textBlock.Visibility = Visibility.Hidden;
                lastTBlockClicked = textBlock;
                findingMatch = true;
            }
            else if (textBlock.Text == lastTBlockClicked.Text)
            {
                matchesFound++;
                textBlock.Visibility = Visibility.Hidden;
                findingMatch = false;
            }
            else
            {
                lastTBlockClicked.Visibility = Visibility.Visible;
                findingMatch = false;
            }

        }

        private void TimeTextBlock_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (matchesFound == 8 || restante == 0)
            {
                SetUpGame();
            }
        }
    }
}
