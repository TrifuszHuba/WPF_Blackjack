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

namespace BlackJack
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public Image[,] cards { get; private protected set; } = new Image[2, 9];
        public MainWindow()
        {
            InitializeComponent();
            for (int playerIdx = 0; playerIdx < 2; playerIdx++)
            {
                for (int Idx = 0; Idx < 2; Idx++)
                {
                    BitmapImage bitmapImg = new();
                    bitmapImg.BeginInit();
                    bitmapImg.UriSource = new Uri("img/69.gif");
                    bitmapImg.DecodePixelWidth = 700;
                    bitmapImg.EndInit();
                    cards[playerIdx, Idx] = new Image
                    {
                        Source = bitmapImg
                    };
                }
            }
                
        }
    }
}
