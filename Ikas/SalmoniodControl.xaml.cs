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
using System.Windows.Threading;

using Ikas.Class;

namespace Ikas
{
    /// <summary>
    /// SalmoniodControl.xaml 的交互逻辑
    /// </summary>
    public partial class SalmoniodControl : UserControl
    {
        public string OrangeBackground
        {
            get
            {
                return "#3F" + Design.NeonOrange;
            }
        }

        public Salmoniod.Key Salmoniod = Class.Salmoniod.Key.salmoniod_unknown;

        public SalmoniodControl()
        {
            // Load language
            if (Depot.Language != null && Depot.Language != "")
            {
                try
                {
                    ResourceDictionary lang = (ResourceDictionary)Application.LoadComponent(new Uri(@"assets/lang/" + Depot.Language + ".xaml", UriKind.Relative));
                    if (Resources.MergedDictionaries.Count > 0)
                    {
                        Resources.MergedDictionaries.Clear();
                    }
                    Resources.MergedDictionaries.Add(lang);
                }
                catch { }
            }
            // Initialize component
            InitializeComponent();
            // Set properties for controls
            RenderOptions.SetBitmapScalingMode(imgMain, BitmapScalingMode.HighQuality);
            // Add handler for global member
            Depot.LanguageChanged += new LanguageChangedEventHandler(LanguageChanged);
        }

        #region Control Event

        private void BdMain_MouseEnter(object sender, MouseEventArgs e)
        {
            ((Storyboard)FindResource("quick_fade_out")).Begin(lbKill);
            ((Storyboard)FindResource("quick_fade_in")).Begin(lbName);
            ((Storyboard)FindResource("quick_fade_in")).Begin(lbKillShort);
        }

        private void BdMain_MouseLeave(object sender, MouseEventArgs e)
        {
            ((Storyboard)FindResource("quick_fade_in")).Begin(lbKill);
            ((Storyboard)FindResource("quick_fade_out")).Begin(lbName);
            ((Storyboard)FindResource("quick_fade_out")).Begin(lbKillShort);
        }

        #endregion

        private void LanguageChanged()
        {
            ResourceDictionary lang = (ResourceDictionary)Application.LoadComponent(new Uri(@"assets/lang/" + Depot.Language + ".xaml", UriKind.Relative));
            if (Resources.MergedDictionaries.Count > 0)
            {
                Resources.MergedDictionaries.Clear();
            }
            Resources.MergedDictionaries.Add(lang);
            // Force refresh label
            lbName.Content = Translate(Salmoniod.ToString());
        }

        public void SetImage(Salmoniod.Key id)
        {
            Salmoniod = id;
            switch (Salmoniod)
            {
                case Class.Salmoniod.Key.goldie:
                    imgMain.Source = new BitmapImage(new Uri("pack://application:,,,/assets/img/salmoniods-goldie.png"));
                    break;
                case Class.Salmoniod.Key.steelhead:
                    imgMain.Source = new BitmapImage(new Uri("pack://application:,,,/assets/img/salmoniods-steelhead.png"));
                    break;
                case Class.Salmoniod.Key.flyfish:
                    imgMain.Source = new BitmapImage(new Uri("pack://application:,,,/assets/img/salmoniods-flyfish.png"));
                    break;
                case Class.Salmoniod.Key.scrapper:
                    imgMain.Source = new BitmapImage(new Uri("pack://application:,,,/assets/img/salmoniods-scrapper.png"));
                    break;
                case Class.Salmoniod.Key.steel_eel:
                    imgMain.Source = new BitmapImage(new Uri("pack://application:,,,/assets/img/salmoniods-steel-eel.png"));
                    break;
                case Class.Salmoniod.Key.stinger:
                    imgMain.Source = new BitmapImage(new Uri("pack://application:,,,/assets/img/salmoniods-stinger.png"));
                    break;
                case Class.Salmoniod.Key.maws:
                    imgMain.Source = new BitmapImage(new Uri("pack://application:,,,/assets/img/salmoniods-maws.png"));
                    break;
                case Class.Salmoniod.Key.griller2:
                    imgMain.Source = new BitmapImage(new Uri("pack://application:,,,/assets/img/salmoniods-griller.png"));
                    break;
                case Class.Salmoniod.Key.drizzler:
                    imgMain.Source = new BitmapImage(new Uri("pack://application:,,,/assets/img/salmoniods-drizzler.png"));
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        public void SetKill(Salmoniod.Key id, JobPlayer player = null, Job job = null)
        {
            ((Storyboard)FindResource("fade_out")).Begin(gridMain);
            Storyboard sb = (Storyboard)FindResource("resize_width");
            (sb.Children[0] as DoubleAnimation).To = 0;
            sb.Begin(bdRatio);
            ((Storyboard)FindResource("fade_out")).Begin(bdRatio);
            if (player != null && job != null)
            {
                lbName.Content = Translate(Salmoniod.ToString());
                int kill = player.SalmoniodKills.Find(p => p.Salmoniod == id).Count;
                int jobKill = job.GetSalmoniodKill(id);
                int jobCount = job.SalmoniodAppearances.Find(p => p.Salmoniod == id).Count;
                if (jobKill == jobCount && jobKill != 0)
                {
                    lbName.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FF" + Design.NeonYellow));
                    lbKill.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FF" + Design.NeonYellow));
                    lbKillShort.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FF" + Design.NeonYellow));
                }
                else
                {
                    lbName.Foreground = new SolidColorBrush(Colors.White);
                    lbKill.Foreground = new SolidColorBrush(Colors.White);
                    lbKillShort.Foreground = new SolidColorBrush(Colors.White);
                }
                lbKill.Content = string.Format(Translate("x{0}/{1}/{2}", true), kill, jobKill, jobCount);
                lbKillShort.Content = string.Format(Translate("x{0}", true), kill);
                ((Storyboard)FindResource("fade_in")).Begin(gridMain);
                if (jobKill != 0)
                {
                    double to = (kill * 0.9 / jobKill + 0.1) * bdMain.Width;
                    (sb.Children[0] as DoubleAnimation).To = to;
                    sb.Begin(bdRatio);
                    ((Storyboard)FindResource("fade_in")).Begin(bdRatio);
                }
                else
                {
                    double to = 0.1 * bdMain.Width;
                    (sb.Children[0] as DoubleAnimation).To = to;
                    sb.Begin(bdRatio);
                    ((Storyboard)FindResource("fade_in")).Begin(bdRatio);
                }
            }
        }

        private string Translate(string s, bool isLocal = false)
        {
            try
            {
                if (isLocal)
                {
                    return (string)FindResource("salmoniod_control-" + s);
                }
                else
                {
                    return (string)FindResource(s);
                }
            }
            catch
            {
                return s;
            }
        }
    }
}
