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
using System.Windows.Shapes;

using System.Windows.Media.Animation;

using ClassLib;

namespace Ikas
{
    /// <summary>
    /// PlayerWindow.xaml 的交互逻辑
    /// </summary>
    public partial class PlayerWindow : Window
    {
        public Window KeepAliveWindow { get; set; }

        public volatile Player Player;

        public PlayerWindow()
        {
            // Initialize component
            InitializeComponent();
            // Set properties for controls
            RenderOptions.SetBitmapScalingMode(bdIcon, BitmapScalingMode.HighQuality);
        }

        #region Control Event

        private void Window_MouseEnter(object sender, MouseEventArgs e)
        {
            if (KeepAliveWindow != null)
            {
                ((Storyboard)FindResource("window_fade_in")).Begin(KeepAliveWindow);
            }
            // ((Storyboard)FindResource("window_fade_in")).Begin(this);
        }

        private void Window_MouseLeave(object sender, MouseEventArgs e)
        {
            // ((Storyboard)FindResource("window_fade_out")).Begin(this);
        }

        private void Window_LocationChanged(object sender, EventArgs e)
        {
            /*
            if (Top < 0)
            {
                Top = 0;
            }
            if (Top + Height > WpfScreen.GetScreenFrom(this).DeviceBounds.Height)
            {
                Top = WpfScreen.GetScreenFrom(this).DeviceBounds.Height - Height;
            }
            */
        }

        #endregion

        public void SetPlayer(Player player)
        {
            Player = player;
            // Fade out labels and images
            ((Storyboard)FindResource("fade_out")).Begin(tbName);
            ((Storyboard)FindResource("fade_out")).Begin(bdIcon);
            gearHead.SetGear(null);
            gearClothes.SetGear(null);
            gearShoes.SetGear(null);
            // Update player
            tbName.Text = Player.Nickname;
            ((Storyboard)FindResource("fade_in")).Begin(tbName);
            string image = FileFolderUrl.ApplicationData + FileFolderUrl.IconFolder + @"\" + System.IO.Path.GetFileName(Player.Image) + ".jpg";
            try
            {
                ImageBrush brush = new ImageBrush(new BitmapImage(new Uri(image)));
                brush.Stretch = Stretch.UniformToFill;
                bdIcon.Background = brush;
                ((Storyboard)FindResource("fade_in")).Begin(bdIcon);
            }
            catch
            {
                // Download the image
                Downloader downloader = new Downloader(Player.Image, image, Downloader.SourceType.Player, Depot.Proxy);
                Depot.DownloadManager.AddDownloader(downloader, new DownloadCompletedEventHandler(() =>
                {
                    if (System.IO.Path.GetFileName(image) == System.IO.Path.GetFileName(Player.Image) + ".jpg")
                    {
                        ImageBrush brush = new ImageBrush(new BitmapImage(new Uri(image)));
                        brush.Stretch = Stretch.UniformToFill;
                        bdIcon.Background = brush;
                        ((Storyboard)FindResource("fade_in")).Begin(bdIcon);
                    }
                }));
            }
            gearHead.SetGear(Player.HeadGear);
            gearClothes.SetGear(Player.ClothesGear);
            gearShoes.SetGear(Player.ShoesGear);
        }
    }
}
