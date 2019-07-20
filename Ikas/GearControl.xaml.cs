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

using IkasLib;

namespace Ikas
{
    public delegate void MouseEnterGearEventHandler(object sender, MouseEventArgs e);
    public delegate void MouseLeaveGearEventHandler(object sender, MouseEventArgs e);
    /// <summary>
    /// GearControl.xaml 的交互逻辑
    /// </summary>
    public partial class GearControl : UserControl
    {
        public volatile Gear Gear;

        public event MouseEnterGearEventHandler MouseEnterGear;
        public event MouseLeaveGearEventHandler MouseLeaveGear;

        public GearControl()
        {
            // Initialize component
            InitializeComponent();
            // Set properties for controls
            RenderOptions.SetBitmapScalingMode(bdGear, BitmapScalingMode.HighQuality);
            RenderOptions.SetBitmapScalingMode(bdMain, BitmapScalingMode.HighQuality);
            RenderOptions.SetBitmapScalingMode(bdSub1, BitmapScalingMode.HighQuality);
            RenderOptions.SetBitmapScalingMode(bdSub2, BitmapScalingMode.HighQuality);
            RenderOptions.SetBitmapScalingMode(bdSub3, BitmapScalingMode.HighQuality);
        }

        #region Control Event

        #endregion

        public void SetGear(Gear gear)
        {
            // Remove previous Downloader's handlers, let lambda decide
            //DownloadHelper.RemoveDownloaders(Downloader.SourceType.Gear);
            Gear = gear;
            // Fade out labels and images
            ((Storyboard)FindResource("fade_out")).Begin(bdGear);
            ((Storyboard)FindResource("fade_out")).Begin(bdMain);
            ((Storyboard)FindResource("fade_out")).Begin(bdSub1);
            ((Storyboard)FindResource("fade_out")).Begin(bdSub2);
            ((Storyboard)FindResource("fade_out")).Begin(bdSub3);
            if (Gear != null)
            {
                // Gear
                string image = FileFolderUrl.ApplicationData + Gear.Image;
                try
                {
                    ImageBrush brush = new ImageBrush(new BitmapImage(new Uri(image)));
                    brush.Stretch = Stretch.Uniform;
                    bdGear.Background = brush;
                    ((Storyboard)FindResource("fade_in")).Begin(bdGear);
                }
                catch
                {
                    // Download the image
                    Downloader downloader = new Downloader(FileFolderUrl.SplatNet + Gear.Image, image, Downloader.SourceType.Gear, Depot.Proxy);
                    DownloadHelper.AddDownloader(downloader, new DownloadCompletedEventHandler(() =>
                    {
                        if (System.IO.Path.GetFileName(image) == System.IO.Path.GetFileName(Gear.Image))
                        {
                            ImageBrush brush = new ImageBrush(new BitmapImage(new Uri(image)));
                            brush.Stretch = Stretch.Uniform;
                            bdGear.Background = brush;
                            ((Storyboard)FindResource("fade_in")).Begin(bdGear);
                        }
                    }));
                }
                // Main skill
                string image2 = FileFolderUrl.ApplicationData + Gear.MainSkill.Image;
                try
                {
                    ImageBrush brush = new ImageBrush(new BitmapImage(new Uri(image2)));
                    brush.Stretch = Stretch.Uniform;
                    bdMain.Background = brush;
                    ((Storyboard)FindResource("fade_in")).Begin(bdMain);
                }
                catch
                {
                    // Download the image
                    Downloader downloader = new Downloader(FileFolderUrl.SplatNet + Gear.MainSkill.Image, image2, Downloader.SourceType.Gear, Depot.Proxy);
                    DownloadHelper.AddDownloader(downloader, new DownloadCompletedEventHandler(() =>
                    {
                        if (System.IO.Path.GetFileName(image2) == System.IO.Path.GetFileName(Gear.MainSkill.Image))
                        {
                            ImageBrush brush = new ImageBrush(new BitmapImage(new Uri(image2)));
                            brush.Stretch = Stretch.Uniform;
                            bdMain.Background = brush;
                            ((Storyboard)FindResource("fade_in")).Begin(bdMain);
                        }
                    }));
                }
                // Sub skill
                string image3 = FileFolderUrl.ApplicationData + Gear.SubSkills[0].Image;
                try
                {
                    ImageBrush brush = new ImageBrush(new BitmapImage(new Uri(image3)));
                    brush.Stretch = Stretch.Uniform;
                    bdSub1.Background = brush;
                    ((Storyboard)FindResource("fade_in")).Begin(bdSub1);
                }
                catch
                {
                    // Download the image
                    Downloader downloader = new Downloader(FileFolderUrl.SplatNet + Gear.SubSkills[0].Image, image3, Downloader.SourceType.Gear, Depot.Proxy);
                    DownloadHelper.AddDownloader(downloader, new DownloadCompletedEventHandler(() =>
                    {
                        if (System.IO.Path.GetFileName(image3) == System.IO.Path.GetFileName(Gear.SubSkills[0].Image))
                        {
                            ImageBrush brush = new ImageBrush(new BitmapImage(new Uri(image3)));
                            brush.Stretch = Stretch.Uniform;
                            bdSub1.Background = brush;
                            ((Storyboard)FindResource("fade_in")).Begin(bdSub1);
                        }
                    }));
                }
                if (Gear.SubSkills.Count > 1)
                {
                    // Sub skill 2
                    string image4 = FileFolderUrl.ApplicationData + Gear.SubSkills[1].Image;
                    try
                    {
                        ImageBrush brush = new ImageBrush(new BitmapImage(new Uri(image4)));
                        brush.Stretch = Stretch.Uniform;
                        bdSub2.Background = brush;
                        ((Storyboard)FindResource("fade_in")).Begin(bdSub2);
                    }
                    catch
                    {
                        // Download the image
                        Downloader downloader = new Downloader(FileFolderUrl.SplatNet + Gear.SubSkills[1].Image, image4, Downloader.SourceType.Gear, Depot.Proxy);
                        DownloadHelper.AddDownloader(downloader, new DownloadCompletedEventHandler(() =>
                        {
                            if (Gear.SubSkills.Count > 1)
                            {
                                if (System.IO.Path.GetFileName(image4) == System.IO.Path.GetFileName(Gear.SubSkills[1].Image))
                                {
                                    ImageBrush brush = new ImageBrush(new BitmapImage(new Uri(image4)));
                                    brush.Stretch = Stretch.Uniform;
                                    bdSub2.Background = brush;
                                    ((Storyboard)FindResource("fade_in")).Begin(bdSub2);
                                }
                            }
                        }));
                    }
                    if (Gear.SubSkills.Count > 2)
                    {
                        // Sub skill 3
                        string image5 = FileFolderUrl.ApplicationData + Gear.SubSkills[2].Image;
                        try
                        {
                            ImageBrush brush = new ImageBrush(new BitmapImage(new Uri(image5)));
                            brush.Stretch = Stretch.Uniform;
                            bdSub3.Background = brush;
                            ((Storyboard)FindResource("fade_in")).Begin(bdSub3);
                        }
                        catch
                        {
                            // Download the image
                            Downloader downloader = new Downloader(FileFolderUrl.SplatNet + Gear.SubSkills[2].Image, image5, Downloader.SourceType.Gear, Depot.Proxy);
                            DownloadHelper.AddDownloader(downloader, new DownloadCompletedEventHandler(() =>
                            {
                                if (Gear.SubSkills.Count > 2)
                                {
                                    if (System.IO.Path.GetFileName(image5) == System.IO.Path.GetFileName(Gear.SubSkills[2].Image))
                                    {
                                        ImageBrush brush = new ImageBrush(new BitmapImage(new Uri(image5)));
                                        brush.Stretch = Stretch.Uniform;
                                        bdSub3.Background = brush;
                                        ((Storyboard)FindResource("fade_in")).Begin(bdSub3);
                                    }
                                }
                            }));
                        }
                    }
                    else
                    {
                        bdSub3.Background = new SolidColorBrush(Colors.Transparent);
                        ((Storyboard)FindResource("fade_in")).Begin(bdSub3);
                    }
                }
                else
                {
                    bdSub2.Background = new SolidColorBrush(Colors.Transparent);
                    ((Storyboard)FindResource("fade_in")).Begin(bdSub2);
                    bdSub3.Background = new SolidColorBrush(Colors.Transparent);
                    ((Storyboard)FindResource("fade_in")).Begin(bdSub3);
                }
            }
        }

        private void BdGear_MouseEnter(object sender, MouseEventArgs e)
        {
            MouseEnterGear?.Invoke(this, e);
        }

        private void BdGear_MouseLeave(object sender, MouseEventArgs e)
        {
            MouseLeaveGear?.Invoke(this, e);
        }
    }
}
