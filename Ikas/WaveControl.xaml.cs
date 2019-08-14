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
    /// WaveControl.xaml 的交互逻辑
    /// </summary>
    public partial class WaveControl : UserControl
    {
        public string SalmonYellowForeground
        {
            get
            {
                return "#FF" + Design.NeonSalmonYellow;
            }
        }

        public volatile Wave Wave;
        public volatile int Number;

        public WaveControl()
        {
            // Initialize component
            InitializeComponent();
        }

        #region Control Event

        private void BdMain_MouseEnter(object sender, MouseEventArgs e)
        {
            ((Storyboard)FindResource("quick_fade_in")).Begin(gridExtra);
            ((Storyboard)FindResource("quick_fade_out")).Begin(gridBasic);
        }

        private void BdMain_MouseLeave(object sender, MouseEventArgs e)
        {
            ((Storyboard)FindResource("quick_fade_in")).Begin(gridBasic);
            ((Storyboard)FindResource("quick_fade_out")).Begin(gridExtra);
        }

        #endregion

        public void SetWave(Wave wave, int number = 0)
        {
            Wave = wave;
            Number = number;
            ((Storyboard)FindResource("fade_out")).Begin(bdTide);
            ((Storyboard)FindResource("fade_out")).Begin(lbWave);
            ((Storyboard)FindResource("fade_out")).Begin(lbGoldenEgg);
            ((Storyboard)FindResource("fade_out")).Begin(lbEvent);
            ((Storyboard)FindResource("fade_out")).Begin(lbResult);
            Storyboard sb = (Storyboard)FindResource("resize_height");
            (sb.Children[0] as DoubleAnimation).To = 0;
            sb.Begin(bdTide);
            ((Storyboard)FindResource("fade_out")).Begin(bdTide);
            if (Wave != null && Number > 0)
            {
                lbWave.Content = string.Format(Translate("wave_{0}", true), Number);
                lbGoldenEgg.Content = string.Format(Translate("{0}/{1}", true), Wave.GoldenEgg, Wave.Quota);
                lbEvent.Content = Translate(Wave.EventType.ToString());
                if (wave.IsClear)
                {
                    lbResult.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FF" + Design.NeonGreen));
                }
                else
                {
                    lbResult.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FF" + Design.NeonOrange));
                }
                lbResult.Content = Translate(Wave.Result.ToString());
                ((Storyboard)FindResource("fade_in")).Begin(lbWave);
                ((Storyboard)FindResource("fade_in")).Begin(lbGoldenEgg);
                ((Storyboard)FindResource("fade_in")).Begin(lbEvent);
                ((Storyboard)FindResource("fade_in")).Begin(lbResult);
                double to;
                switch (wave.WaterLevel)
                {
                    case Wave.WaterLevelType.low:
                        to = 10;
                        break;
                    case Wave.WaterLevelType.normal:
                        to = 40;
                        break;
                    case Wave.WaterLevelType.high:
                        to = 80;
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
                (sb.Children[0] as DoubleAnimation).To = to;
                sb.Begin(bdTide);
                ((Storyboard)FindResource("fade_in")).Begin(bdTide);
            }
        }

        private string Translate(string s, bool isLocal = false)
        {
            try
            {
                if (isLocal)
                {
                    return (string)FindResource("wave_control-" + s);
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
