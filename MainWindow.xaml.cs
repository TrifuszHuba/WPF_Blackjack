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
                int sum = 0;
                int AceCount = 0;
                for (int i = 0; i < PlayerHumanCards.Children.Count; i++)
                {
                    int temp = GetCardValue((int)((Image)PlayerHumanCards.Children[i]).Tag);
                    if (temp == 11)
                    {
                        AceCount++;
                    }
                    else
                    {
                        sum += temp;
                    }
                }
                for (int i = 0; i < AceCount; i++)
                {
                    if (sum+11+AceCount-i-1 > 21)
                    {
                        sum += AceCount - i;
                        break;
                    }
                    else
                    {
                        sum += 11;
                    }
                }
                return sum;
            }
        }

        public int BotSum
        {
            get
            {
                int sum = 0;
                int AceCount = 0;
                for (int i = 0; i < PlayerBotCards.Children.Count; i++)
                {
                    int temp = GetCardValue((int)((Image)PlayerBotCards.Children[i]).Tag);
                    if (temp == 11)
                    {
                        AceCount++;
                    }
                    else
                    {
                        sum += temp;
                    }
                }
                for (int i = 0; i < AceCount; i++)
                {
                    if (sum + 11 + AceCount - i - 1 > 21)
                    {
                        sum += AceCount - i;
                        break;
                    }
                    else
                    {
                        sum += 11;
                    }
                }
                return sum;
            }
        }

        public int PublicBotSum
        {
            get
            {
                int sum = 0;
                int AceCount = 0;
                int FirstValue = 0;
                bool IsAceFirst = false;
                for (int i = 0; i < PlayerBotCards.Children.Count; i++)
                {
                    int temp = GetCardValue((int)((Image)PlayerBotCards.Children[i]).Tag);
                    if (i == 0)
                    {
                        FirstValue = temp;
                    }
                    if (temp == 11)
                    {
                        if (i == 0)
                        {
                            IsAceFirst = true;
                        }
                        AceCount++;
                    }
                    else
                    {
                        sum += temp;
                    }
                }
                if (AceCount > 0 && IsAceFirst)
                {
                    if (sum + 11 + AceCount - 1 > 21)
                    {
                        return 1;
                    }
                    return 11;
                }
                    
                return FirstValue;
            }
        }
        public MainWindow()
        {
            InitializeComponent();
            StartGame();
        }

        private void StartGame()
        {
            DisplayResult.Visibility = Visibility.Hidden;
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
            MayPull = true;
            Update_CardSums();
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
            DisplayResult.Visibility = Visibility.Visible;
            PlayerBotSum.Content = $"{BotSum}";
            MayPull = false;
            for (int i = 0; i < PlayerBotCards.Children.Count; i++)
            {
                BitmapImage bitmapImg = new();
                bitmapImg.BeginInit();
                int cardTag = (int)((Image)PlayerBotCards.Children[i]).Tag;
                bitmapImg.UriSource = new Uri(System.IO.Path.Combine(Environment.CurrentDirectory, "img", $"{cardTag}.gif"));
                bitmapImg.DecodePixelWidth = 700;
                bitmapImg.EndInit();
                PlayerBotCards.Children.Insert(i, new Image()
                {
                    Source = bitmapImg,
                    Tag = cardTag
                });
                PlayerBotCards.Children.RemoveAt(i + 1);
            }
        }

        private void Button_EndGame(object sender, RoutedEventArgs e)
        {
            if (MayPull)
            {
                Button_EndTurn();
                MayPull = false;
                PlayerHumanSum.Content = $"{HumanSum}";
                EndGameTasks();
                if (HumanSum > BotSum)
                {
                    DisplayResult.Content += "Nyertél!";
                    return;
                }
                if (HumanSum == BotSum)
                {
                    DisplayResult.Content += "Vesztettél! (döntetlen)";
                    return;
                }
                DisplayResult.Content += "Vesztettél!";

            }
        }

        private void Button_RestartGame(object sender, RoutedEventArgs e)
        {
            PlayerBotCards.Children.Clear();
            PlayerHumanCards.Children.Clear();
            StartGame();
        }

        private void Update_CardSums()
        {
            PlayerHumanSum.Content = $"{HumanSum}";

            if (HumanSum > 21)
            {
                EndGameTasks();
                DisplayResult.Content = $"Vesztettél!";
                return;
            }
            if (HumanSum == 21)
            {
                EndGameTasks();
                DisplayResult.Content = $"Nyertél!";
                return;
            }

            PlayerBotSum.Content = $"{PublicBotSum}";

            if (BotSum > 21)
            {
                EndGameTasks();
                DisplayResult.Content = $"Nyertél!";
                return;
            }
            if (BotSum == 21)
            {
                EndGameTasks();
                DisplayResult.Content = $"Vesztettél!";
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
