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

using System.Windows.Media.Animation;

using Ikas.Class;

namespace Ikas
{
    /// <summary>
    /// WeaponControl.xaml 的交互逻辑
    /// </summary>
    public partial class WeaponControl : UserControl
    {
        public volatile Weapon Weapon;

        public WeaponControl()
        {
            // Initialize component
            InitializeComponent();
            // Set properties for controls
            RenderOptions.SetBitmapScalingMode(bdWeapon, BitmapScalingMode.HighQuality);
        }

        public void SetWeapon(Weapon weapon)
        {
            Weapon = weapon;
            ((Storyboard)FindResource("fade_out")).Begin(bdWeapon);
            if (Weapon != null)
            {
                string image = FileFolderUrl.ApplicationData + Weapon.Image;
                try
                {
                    ImageBrush brush = new ImageBrush(new BitmapImage(new Uri(image)));
                    brush.Stretch = Stretch.UniformToFill;
                    bdWeapon.Background = brush;
                    ((Storyboard)FindResource("fade_in")).Begin(bdWeapon);
                }
                catch
                {
                    // Download the image
                    Downloader downloader = new Downloader(FileFolderUrl.SplatNet + Weapon.Image, image, Downloader.SourceType.Battle, Depot.Proxy);
                    DownloadHelper.AddDownloader(downloader, new DownloadCompletedEventHandler(() =>
                    {
                        if (Weapon != null)
                        {
                            if (System.IO.Path.GetFileName(image) == System.IO.Path.GetFileName(Weapon.Image))
                            {
                                ImageBrush brush = new ImageBrush(new BitmapImage(new Uri(image)));
                                brush.Stretch = Stretch.UniformToFill;
                                bdWeapon.Background = brush;
                                ((Storyboard)FindResource("fade_in")).Begin(bdWeapon);
                            }
                        }
                    }));
                }
            }
        }
    }
}
