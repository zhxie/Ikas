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

namespace Ikas
{
    /// <summary>
    /// TagControl.xaml 的交互逻辑
    /// </summary>
    public partial class TagControl : UserControl
    {

        public new string Content
        {
            get { return (string)GetValue(ContentProperty); }
            set { SetValue(ContentProperty, value); }
        }
        public new static readonly DependencyProperty ContentProperty =
            DependencyProperty.Register("Content", typeof(string), typeof(TagControl), new PropertyMetadata(""));

        public new Brush Background
        {
            get { return (Brush)GetValue(BackgroundProperty); }
            set { SetValue(BackgroundProperty, value); }
        }
        public new static readonly DependencyProperty BackgroundProperty =
            DependencyProperty.Register("Background", typeof(Brush), typeof(TagControl), new PropertyMetadata(new SolidColorBrush(Colors.Transparent)));

        public Thickness TextBlockMargin
        {
            get
            {
                switch (HorizontalAlignment)
                {
                    case HorizontalAlignment.Left:
                        return new Thickness(20, 0, 10, 0);
                    case HorizontalAlignment.Right:
                        return new Thickness(10, 0, 20, 0);
                    default:
                        return new Thickness(15, 0, 15, 0);
                }
            }
        }

        public TagControl()
        {
            InitializeComponent();
        }
    }
}
