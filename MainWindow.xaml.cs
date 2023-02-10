using System;
using System.IO;
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
using System.Reflection.Emit;
using System.Threading;

namespace BlackJack
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public Random generator = new();
        public bool MayPull = false;
        public int HumanSum
        {
            get
            {
                int temp = 0;
                for (int i = 0; i < PlayerHumanCards.Children.Count; i++)
                {
                    temp += GetCardValue((int)((Image)PlayerHumanCards.Children[i]).Tag);
                }
                return temp;
            }
        }

        public int BotSum
        {
            get
            {
                int temp = 0;
                for (int i = 0; i < PlayerBotCards.Children.Count; i++)
                {
                    temp += GetCardValue((int)((Image)PlayerBotCards.Children[i]).Tag);
                }
                return temp;
            }
        }
        public MainWindow()
        {
            InitializeComponent();
            StartGame();
        }

        private void StartGame()
        {
            PlayerBotSum.Visibility = Visibility.Hidden;
            for (int Idx = 0; Idx < 2; Idx++)
            {
                BitmapImage bitmapImg = new();
                bitmapImg.BeginInit();
                var randint = generator.Next(1, 52);
                bitmapImg.UriSource = new Uri(System.IO.Path.Combine(Environment.CurrentDirectory, "img", Idx==1?"69.gif":$"{randint}.gif"));
                bitmapImg.DecodePixelWidth = 700;
                bitmapImg.EndInit();
                PlayerBotCards.Children.Add(new Image
                {
                    Source = bitmapImg,
                    Tag = randint
                });
            }
            for (int Idx = 0; Idx < 2; Idx++)
            {
                BitmapImage bitmapImg = new();
                bitmapImg.BeginInit();
                var randint = generator.Next(1, 52);
                bitmapImg.UriSource = new Uri(System.IO.Path.Combine(Environment.CurrentDirectory, "img", $"{randint}.gif"));
                bitmapImg.DecodePixelWidth = 700;
                bitmapImg.EndInit();

                PlayerHumanCards.Children.Add(new Image
                {
                    Source = bitmapImg,
                    Tag = randint
                });
            }
            {
                BitmapImage bitmapImg = new();
                bitmapImg.BeginInit();
                bitmapImg.UriSource = new Uri(System.IO.Path.Combine(Environment.CurrentDirectory, "img/69.gif"));
                bitmapImg.DecodePixelWidth = 700;
                bitmapImg.EndInit();
                PullCardImage.Source = bitmapImg;
                PullCardImage.Tag = -1;

            }
            Update_CardSums();
            MayPull = true;
        }

        private void Button_PullCard(object sender, RoutedEventArgs e)
        {
            if (MayPull)
            {
                {
                    BitmapImage bitmapImg = new();
                    bitmapImg.BeginInit();
                    var randint = generator.Next(1, 52);
                    bitmapImg.UriSource = new Uri(System.IO.Path.Combine(Environment.CurrentDirectory, "img", $"{randint}.gif"));
                    bitmapImg.DecodePixelWidth = 700;
                    bitmapImg.EndInit();
                    PlayerHumanCards.Children.Add(new Image
                    {
                        Source = bitmapImg,
                        Tag = randint
                    });
                }
                Update_CardSums();
                Button_EndTurn();
            }
        }

        private void Button_EndTurn()
        {
            if (MayPull)
            {
                while (BotSum < 17)
                {
                    BitmapImage bitmapImg = new();
                    bitmapImg.BeginInit();
                    var randint = generator.Next(1, 52);
                    bitmapImg.UriSource = new Uri(System.IO.Path.Combine(Environment.CurrentDirectory, "img", $"69.gif"));
                    bitmapImg.DecodePixelWidth = 700;
                    bitmapImg.EndInit();
                    PlayerBotCards.Children.Add(new Image
                    {
                        Source = bitmapImg,
                        Tag = randint
                    });
                }
                Update_CardSums();
            }
        }

        private void EndGameTasks()
        {
            PlayerBotSum.Visibility = Visibility.Visible;
            for (int i = 0; i < PlayerBotCards.Children.Count; i++)
            {
                BitmapImage bitmapImg = new();
                bitmapImg.BeginInit();
                int cardTag = (int)((Image)PlayerBotCards.Children[i]).Tag;
                bitmapImg.UriSource = new Uri(System.IO.Path.Combine(Environment.CurrentDirectory, "img", $"{cardTag}.gif"));
                bitmapImg.DecodePixelWidth = 700;
                bitmapImg.EndInit();
                PlayerBotCards.Children[i] = new Image()
                {
                    Source = bitmapImg,
                    Tag = cardTag
                };
            }
        }

        private void Button_EndGame(object sender, RoutedEventArgs e)
        {
            if (MayPull)
            {
                Button_EndTurn();
                MayPull = false;
                PlayerHumanSum.Content = $"{HumanSum}";
                PlayerBotSum.Content = $"{BotSum}";
                EndGameTasks();
                if (HumanSum > BotSum)
                {
                    PlayerHumanSum.Content += ": Nyertél!";
                    return;
                }
                if (HumanSum == BotSum)
                {
                    PlayerHumanSum.Content += ": Döntetlen!";
                    PlayerBotSum.Content += ": Döntetlen!";
                    return;
                }
                PlayerBotSum.Content += ": Vesztettél!";

            }
        }

        private void Update_CardSums()
        {
            PlayerHumanSum.Content = $"{HumanSum}";

            if (HumanSum > 21)
            {
                PlayerHumanSum.Content = $"{HumanSum}: Vesztettél!";
                MayPull = false;
                EndGameTasks();
                return;
            }
            if (HumanSum == 21)
            {
                PlayerHumanSum.Content = $"{HumanSum}: Nyertél!";
                MayPull = false;
                EndGameTasks();
                return;
            }
            

            PlayerBotSum.Content = $"{BotSum}";

            if (BotSum > 21)
            {
                PlayerHumanSum.Content = $"{HumanSum}: Nyertél!";
                MayPull = false;
                EndGameTasks();
                return;
            }
            if (BotSum == 21)
            {
                PlayerHumanSum.Content = $"{HumanSum}: Vesztettél!";
                MayPull = false;
                EndGameTasks();
                return;
            }
            
        }

        private int GetCardValue(int card)
        {
            int n = (card-1) % 13;
            if (n == 0)
            {
                return 11;
            }
            if (n > 9)
            {
                return 10;
            }
            return n + 1;
        }
    }
}
