﻿using System;
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

using System.Text.RegularExpressions;
using System.Windows.Media.Animation;
using System.Windows.Threading;

using Ikas.Class;

namespace Ikas
{
    /// <summary>
    /// SettingsWindow.xaml 的交互逻辑
    /// </summary>
    public partial class SettingsWindow : Window
    {
        private MessageWindow messageWindow;

        private DispatcherTimer tmLoading;
        private int loadingRotationAngle;
        private DispatcherTimer tmSessionToken;

        public string SelectionForeground
        {
            get
            {
                return "#7F" + Design.NeonOrange;
            }
        }

        public string GreenForeground
        {
            get
            {
                return "#FF" + Design.NeonGreen;
            }
        }

        public string GreenBackground
        {
            get
            {
                return "#7F" + Design.NeonGreen;
            }
        }

        public string OrangeForeground
        {
            get
            {
                return "#FF" + Design.NeonOrange;
            }
        }

        public string RedForeground
        {
            get
            {
                return "#FF" + Design.NeonRed;
            }
        }

        private bool isReady = false;

        public SettingsWindow()
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
            // Add handler for global member
            Depot.LanguageChanged += new LanguageChangedEventHandler(LanguageChanged);
            Depot.SessionTokenGet += new AccessGetEventHandler(SessionTokenGet);
            Depot.CookieGet += new AccessGetEventHandler(CookieGet);
            // Prepare windows
            messageWindow = new MessageWindow();
            messageWindow.Opacity = 0;
            messageWindow.Visibility = Visibility.Hidden;
            // Create timers
            loadingRotationAngle = 0;
            tmLoading = new DispatcherTimer();
            tmLoading.Tick += new EventHandler((object source, EventArgs e) =>
            {
                imgLoading.RenderTransform = new RotateTransform(loadingRotationAngle, imgLoading.Source.Width / 2, imgLoading.Source.Height / 2);
                if (loadingRotationAngle >= 359)
                {
                    loadingRotationAngle = 0;
                }
                else
                {
                    loadingRotationAngle++;
                }
            });
            tmLoading.Interval = new TimeSpan(0, 0, 0, 0, 10);
            tmSessionToken = new DispatcherTimer();
            tmSessionToken.Tick += new EventHandler((object source, EventArgs e) =>
            {
                if (Clipboard.ContainsText())
                {
                    string clipboard = Clipboard.GetText();
                    if (clipboard.Contains("session_token_code="))
                    {
                        txtSessionToken.Text = clipboard;
                        LbGetSessionToken_MouseDown(null, null);
                    }
                }
            });
            tmSessionToken.Interval = new TimeSpan(0, 0, 1);
        }

        #region Control Event

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            // Activate animation, maybe WPF bug
            ((Storyboard)FindResource("fore_to_green")).Begin(lbOk);
            ((Storyboard)FindResource("fore_to_orange")).Begin(lbOk);
            ((Storyboard)FindResource("fore_to_red")).Begin(lbOk);
            ((Storyboard)FindResource("fore_to_white")).Begin(lbOk);
            // Load configuration
            txtSessionToken.Text = Depot.SessionToken;
            txtCookie.Text = Depot.Cookie;
            if (Depot.AlwaysOnTop)
            {
                LbAlwaysOnTopTrue_MouseDown(null, null);
                // Animation in former method may not be activated, maybe WPF bug
                lbAlwaysOnTopTrue.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FF" + Design.NeonOrange));
            }
            else
            {
                LbAlwaysOnTopFalse_MouseDown(null, null);
                lbAlwaysOnTopFalse.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FF" + Design.NeonOrange));
            }
            if (Depot.Notification)
            {
                LbNotificationTrue_MouseDown(null, null);
                lbNotificationTrue.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FF" + Design.NeonOrange));
            }
            else
            {
                LbNotificationFalse_MouseDown(null, null);
                lbNotificationFalse.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FF" + Design.NeonOrange));
            }
            if (Depot.UseProxy)
            {
                LbUseProxyTrue_MouseDown(null, null);
                lbUseProxyTrue.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FF" + Design.NeonOrange));
            }
            else
            {
                LbUseProxyFalse_MouseDown(null, null);
                lbUseProxyFalse.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FF" + Design.NeonOrange));
            }
            txtProxyHost.Text = Depot.ProxyHost;
            if (Depot.ProxyPort > 0)
            {
                txtProxyPort.Text = Depot.ProxyPort.ToString();
            }
            else
            {
                txtProxyPort.Text = "";
            }
            switch (Depot.Language)
            {
                case "en-US":
                    LbLanguageEnUs_MouseDown(null, null);
                    lbLanguageEnUs.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FF" + Design.NeonOrange));
                    break;
                case "ja-JP":
                    LbLanguageJaJp_MouseDown(null, null);
                    lbLanguageJaJp.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FF" + Design.NeonOrange));
                    break;
                default:
                    LbLanguageEnUs_MouseDown(null, null);
                    lbLanguageEnUs.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FF" + Design.NeonOrange));
                    break;
            }
            // Finish loading
            isReady = true;
            // Determine log in
            gridUserLogIn.Opacity = 0;
            gridUserLogIn.Visibility = Visibility.Hidden;
            gridUserLoggedIn.Opacity = 0;
            gridUserLoggedIn.Visibility = Visibility.Hidden;
            gridUserManual.Opacity = 0;
            gridUserManual.Visibility = Visibility.Hidden;
            if (txtSessionToken.Text != "")
            {
                gridUserLoggedIn.Opacity = 1;
                gridUserLoggedIn.Visibility = Visibility.Visible;
            }
            else
            {
                gridUserLogIn.Opacity = 1;
                gridUserLogIn.Visibility = Visibility.Visible;
            }
            // Start timers
            tmLoading.Start();
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            e.Cancel = true;
            LbOk_MouseDown(null, null);
        }

        private void Window_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            // DragMove();
        }

        private void LbOk_MouseEnter(object sender, MouseEventArgs e)
        {
            ((Storyboard)FindResource("fore_to_green")).Begin(lbOk);
        }

        private void LbOk_MouseLeave(object sender, MouseEventArgs e)
        {
            ((Storyboard)FindResource("fore_to_white")).Begin(lbOk);
        }

        private void LbOk_MouseDown(object sender, MouseButtonEventArgs e)
        {
            // Stop timers
            tmSessionToken.Stop();
            // Check validity for input
            if (txtCookie.Text.Trim() == "")
            {
                if (MessageBox.Show(Translate("if_you_do_not_enter_a_valid_cookie,_ikas_may_not_work_properly._click_yes_to_close_the_settings,_or_click_no_to_cancel.", true), "Ikas", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.No)
                {
                    return;
                }
            }
            // Restore user-related controls
            if (txtSessionToken.Text != "")
            {
                ((Storyboard)FindResource("grid_fade_out")).Begin(gridUserLogIn);
                ((Storyboard)FindResource("grid_fade_out")).Begin(gridUserManual);
                ((Storyboard)FindResource("grid_fade_in")).Begin(gridUserLoggedIn);
            }
            else
            {
                ((Storyboard)FindResource("grid_fade_out")).Begin(gridUserLoggedIn);
                ((Storyboard)FindResource("grid_fade_out")).Begin(gridUserManual);
                ((Storyboard)FindResource("grid_fade_in")).Begin(gridUserLogIn);
            }
            // Fade out settings
            ((Storyboard)FindResource("window_fade_out")).Begin(this);
        }

        private void BdLogIn_MouseEnter(object sender, MouseEventArgs e)
        {
            ((Storyboard)FindResource("border_to_green")).Begin(bdLogIn);
        }

        private void BdLogIn_MouseLeave(object sender, MouseEventArgs e)
        {
            ((Storyboard)FindResource("border_to_black")).Begin(bdLogIn);
        }

        private void BdLogIn_MouseDown(object sender, MouseButtonEventArgs e)
        {
            LbGetSessionToken_MouseDown(null, null);
        }

        private void BdLogInManually_MouseEnter(object sender, MouseEventArgs e)
        {
            ((Storyboard)FindResource("border_to_green")).Begin(bdLogInManually);
        }

        private void BdLogInManually_MouseLeave(object sender, MouseEventArgs e)
        {
            ((Storyboard)FindResource("border_to_black")).Begin(bdLogInManually);
        }

        private void BdLogInManually_MouseDown(object sender, MouseButtonEventArgs e)
        {
            ((Storyboard)FindResource("grid_fade_out")).Begin(gridUserLogIn);
            ((Storyboard)FindResource("grid_fade_out")).Begin(gridUserLoggedIn);
            ((Storyboard)FindResource("grid_fade_in")).Begin(gridUserManual);
        }

        private void BdUpdateCookie_MouseEnter(object sender, MouseEventArgs e)
        {
            ((Storyboard)FindResource("border_to_green")).Begin(bdUpdateCookie);
        }

        private void BdUpdateCookie_MouseLeave(object sender, MouseEventArgs e)
        {
            ((Storyboard)FindResource("border_to_black")).Begin(bdUpdateCookie);
        }

        private void BdUpdateCookie_MouseDown(object sender, MouseButtonEventArgs e)
        {
            LbUpdateCookie2_MouseDown(null, null);
        }

        private void BdSwitchAccount_MouseEnter(object sender, MouseEventArgs e)
        {
            ((Storyboard)FindResource("border_to_green")).Begin(bdSwitchAccount);
        }

        private void BdSwitchAccount_MouseLeave(object sender, MouseEventArgs e)
        {
            ((Storyboard)FindResource("border_to_black")).Begin(bdSwitchAccount);
        }

        private void BdSwitchAccount_MouseDown(object sender, MouseButtonEventArgs e)
        {
            ((Storyboard)FindResource("grid_fade_out")).Begin(gridUserLoggedIn);
            ((Storyboard)FindResource("grid_fade_out")).Begin(gridUserManual);
            ((Storyboard)FindResource("grid_fade_in")).Begin(gridUserLogIn);
        }

        private void TxtSessionToken_LostFocus(object sender, RoutedEventArgs e)
        {
            if (isReady)
            {
                Depot.SessionToken = txtSessionToken.Text;
            }
        }

        private void LbWhatIsSessionToken_MouseEnter(object sender, MouseEventArgs e)
        {
            ShowMessage(Translate("session_token_is..", true),
                string.Format("{0}\n{1}",
                Translate("a_session_token_is_a_small_piece_of_data_used_for_automatic_cookie_generation.", true),
                Translate("automatic_cookie_generation_sends_minimal_data_including_the_session_token_to_nintendo_and_non-nintendo_servers,_and_get_cookie_back_for_accessing_splatnet.", true)),
                e.GetPosition(this));
        }

        private void LbWhatIsSessionToken_MouseLeave(object sender, MouseEventArgs e)
        {
            ((Storyboard)FindResource("window_fade_out")).Begin(messageWindow);
        }

        private void LbGetSessionToken_MouseEnter(object sender, MouseEventArgs e)
        {
            ((Storyboard)FindResource("fore_to_red")).Begin(lbGetSessionToken);
        }

        private void LbGetSessionToken_MouseLeave(object sender, MouseEventArgs e)
        {
            ((Storyboard)FindResource("fore_to_white")).Begin(lbGetSessionToken);
        }

        private void LbGetSessionToken_MouseDown(object sender, MouseButtonEventArgs e)
        {
            // Stop session token timer
            tmSessionToken.Stop();
            if (!txtSessionToken.Text.Contains("session_token_code="))
            {
                MessageBox.Show(Translate("you_will_be_led_to_a_nintendo_website._log_in,_right_click_on_select_this_person,_copy_the_link_address,_and_then_ikas_will_try_to_get_session_token.", true), "Ikas", MessageBoxButton.OK, MessageBoxImage.Information);
                // Clear the clipboard
                Clipboard.Clear();
                // Authorize
                string url = Depot.LogIn();
                System.Diagnostics.Process.Start(url);
                // Start session token timer
                tmSessionToken.Start();
            }
            else
            {
                MessageBox.Show(Translate("ikas_will_try_to_get_session_token,_which_will_take_seconds_to_minutes_to_finish._please_do_not_close_this_window.", true), "Ikas", MessageBoxButton.OK, MessageBoxImage.Information);
                // Fade in loading
                lbOk.IsEnabled = false;
                bdLoading.IsHitTestVisible = true;
                ((Storyboard)FindResource("fade_in")).Begin(bdLoading);
                // Get session token
                string regex = Regex.Match(txtSessionToken.Text, @"de=(.*)&").Value;
                Depot.GetSessionToken(regex.Substring(3, regex.Length - 4));
            }
        }

        private void TxtCookie_LostFocus(object sender, RoutedEventArgs e)
        {
            if (isReady)
            {
                Depot.Cookie = txtCookie.Text;
            }
        }

        private void LbWhatIsCookie_MouseEnter(object sender, MouseEventArgs e)
        {
            ShowMessage(Translate("cookie_is..", true),
                string.Format("{0}\n{1}\n{2}",
                Translate("a_cookie_is_a_small_piece_of_data_sent_from_a_website_and_stored_on_the_user's_computer.", true),
                Translate("ikas_uses_cookies_to_access_splatnet,_get_schedule_and_battle_data.", true),
                Translate("if_you_don't_know_how_to_get_a_cookie,_you_may_fill_out_the_session_token,_and_the_system_will_get_one_automatically.", true)),
                e.GetPosition(this));
        }

        private void LbWhatIsCookie_MouseLeave(object sender, MouseEventArgs e)
        {
            ((Storyboard)FindResource("window_fade_out")).Begin(messageWindow);
        }

        private void LbUpdateCookie2_MouseEnter(object sender, MouseEventArgs e)
        {
            ((Storyboard)FindResource("fore_to_red")).Begin(lbUpdateCookie2);
        }

        private void LbUpdateCookie2_MouseLeave(object sender, MouseEventArgs e)
        {
            ((Storyboard)FindResource("fore_to_white")).Begin(lbUpdateCookie2);
        }

        private void LbUpdateCookie2_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (txtSessionToken.Text != "")
            {
                MessageBox.Show(Translate("ikas_will_try_to_get_cookie,_which_will_take_seconds_to_minutes_to_finish._please_do_not_close_this_window.", true), "Ikas", MessageBoxButton.OK, MessageBoxImage.Information);
                // DISCLAIMER
                if (MessageBox.Show(Translate("automatic_cookie_generation_will_send_your_session_token_to_nintendo_and_non-nintendo_servers,_which_may_lead_to_privacy_breaches._please_read_the_instructions_in_the_readme_carefully._click_yes_to_continue,_or_click_no_to_view_other_methods.", true), "Ikas", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.Yes)
                {
                    // Fade in loading
                    lbOk.IsEnabled = false;
                    bdLoading.IsHitTestVisible = true;
                    ((Storyboard)FindResource("fade_in")).Begin(bdLoading);
                    // Automatic Cookie Generation
                    _ = Depot.GetCookie(txtSessionToken.Text);
                }
                else
                {
                    // Browse MitM way to get Cookie
                    System.Diagnostics.Process.Start(FileFolderUrl.MitmInstruction);
                }
            }
            else
            {
                MessageBox.Show(Translate("you_may_enter_a_valid_session_token_before_update_cookie.", true), "Ikas", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private void LbAlwaysOnTopTrue_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (isReady)
            {
                Depot.AlwaysOnTop = true;
            }
            ((Storyboard)FindResource("fore_to_orange")).Begin(lbAlwaysOnTopTrue);
            ((Storyboard)FindResource("fore_to_white")).Begin(lbAlwaysOnTopFalse);
        }

        private void LbAlwaysOnTopFalse_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (isReady)
            {
                Depot.AlwaysOnTop = false;
            }
            ((Storyboard)FindResource("fore_to_white")).Begin(lbAlwaysOnTopTrue);
            ((Storyboard)FindResource("fore_to_orange")).Begin(lbAlwaysOnTopFalse);
        }

        private void LbNotificationTrue_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (isReady)
            {
                Depot.Notification = true;
            }
            ((Storyboard)FindResource("fore_to_orange")).Begin(lbNotificationTrue);
            ((Storyboard)FindResource("fore_to_white")).Begin(lbNotificationFalse);
            // Test notification
            if (e != null)
            {
                NotificationHelper.SendTestNotification();
            }
        }

        private void LbNotificationFalse_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (isReady)
            {
                Depot.Notification = false;
            }
            ((Storyboard)FindResource("fore_to_white")).Begin(lbNotificationTrue);
            ((Storyboard)FindResource("fore_to_orange")).Begin(lbNotificationFalse);
        }

        private void LbUseProxyTrue_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (isReady)
            {
                Depot.UseProxy = true;
            }
            ((Storyboard)FindResource("fore_to_orange")).Begin(lbUseProxyTrue);
            ((Storyboard)FindResource("fore_to_white")).Begin(lbUseProxyFalse);
        }

        private void LbUseProxyFalse_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (isReady)
            {
                Depot.UseProxy = false;
            }
            ((Storyboard)FindResource("fore_to_white")).Begin(lbUseProxyTrue);
            ((Storyboard)FindResource("fore_to_orange")).Begin(lbUseProxyFalse);
        }

        private void TxtProxyHost_LostFocus(object sender, RoutedEventArgs e)
        {
            if (isReady)
            {
                Depot.ProxyHost = txtProxyHost.Text;
            }
        }

        private void TxtProxyPort_LostFocus(object sender, RoutedEventArgs e)
        {
            if (isReady)
            {
                if (txtProxyPort.Text != "")
                {
                    int port;
                    if (int.TryParse(txtProxyPort.Text, out port))
                    {
                        if (port < 1 || port > 65535)
                        {
                            MessageBox.Show(Translate("please_enter_a_valid_proxy_port.", true), "Ikas", MessageBoxButton.OK, MessageBoxImage.Warning);
                            // Restore port
                            if (Depot.ProxyPort > 0)
                            {
                                txtProxyPort.Text = Depot.ProxyPort.ToString();
                            }
                            else
                            {
                                txtProxyPort.Text = "";
                            }
                        }
                        else
                        {
                            Depot.ProxyPort = port;
                        }
                    }
                    else
                    {
                        MessageBox.Show(Translate("please_enter_a_valid_proxy_port.", true), "Ikas", MessageBoxButton.OK, MessageBoxImage.Warning);
                        // Restore port
                        if (Depot.ProxyPort > 0)
                        {
                            txtProxyPort.Text = Depot.ProxyPort.ToString();
                        }
                        else
                        {
                            txtProxyPort.Text = "";
                        }
                    }
                }
                else
                {
                    // Restore port
                    if (Depot.ProxyPort > 0)
                    {
                        txtProxyPort.Text = Depot.ProxyPort.ToString();
                    }
                    else
                    {
                        txtProxyPort.Text = "";
                    }
                }
            }
        }

        private void LbLanguageEnUs_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (isReady)
            {
                Depot.Language = "en-US";
            }
            ((Storyboard)FindResource("fore_to_orange")).Begin(lbLanguageEnUs);
            ((Storyboard)FindResource("fore_to_white")).Begin(lbLanguageJaJp);
        }

        private void LbLanguageJaJp_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (isReady)
            {
                Depot.Language = "ja-JP";
            }
            ((Storyboard)FindResource("fore_to_white")).Begin(lbLanguageEnUs);
            ((Storyboard)FindResource("fore_to_orange")).Begin(lbLanguageJaJp);
        }

        private void LbClearCache_MouseEnter(object sender, MouseEventArgs e)
        {
            ((Storyboard)FindResource("fore_to_red")).Begin(lbClearCache);
        }

        private void LbClearCache_MouseLeave(object sender, MouseEventArgs e)
        {
            ((Storyboard)FindResource("fore_to_white")).Begin(lbClearCache);
        }

        private void LbClearCache_MouseDown(object sender, MouseButtonEventArgs e)
        {
            MessageBox.Show(Translate("currently,_ikas_can_not_clear_cache_automatically,_but_ikas_can_guide_you_to_the_application_data_directory.", true), "Ikas", MessageBoxButton.OK, MessageBoxImage.Information);
            System.Diagnostics.Process.Start("explorer.exe", FileFolderUrl.ApplicationData);
        }

        private void LbWhatIsClearCache_MouseEnter(object sender, MouseEventArgs e)
        {
            ShowMessage(Translate("clear_cache_is..", true),
                string.Format("{0}\n{1}\n{2}",
                Translate("when_ikas_gets_schedule_and_battle_data,_it_will_cache_images_including_user_icons,_stages,_weapons_and_gears.", true),
                Translate("sometimes,_due_to_network_or_other_reasons,_ikas_may_not_get_images_properly,_which_may_cause_wrong_cached_images.", true),
                Translate("you_can_help_ikas_get_back_to_normal_operation_by_clearing_the_cached_data.", true)),
                e.GetPosition(this));
        }

        private void LbWhatIsClearCache_MouseLeave(object sender, MouseEventArgs e)
        {
            ((Storyboard)FindResource("window_fade_out")).Begin(messageWindow);
        }

        #endregion

        private void LanguageChanged()
        {
            if (Depot.Language != null && Depot.Language != "")
            {
                ResourceDictionary lang = (ResourceDictionary)Application.LoadComponent(new Uri(@"assets/lang/" + Depot.Language + ".xaml", UriKind.Relative));
                if (Resources.MergedDictionaries.Count > 0)
                {
                    Resources.MergedDictionaries.Clear();
                }
                Resources.MergedDictionaries.Add(lang);
            }
        }

        private void SessionTokenGet(Base.ErrorType error, string sessionToken)
        {
            if (sessionToken == null || sessionToken == "")
            {
                MessageBox.Show(string.Format(Translate("{0},_because_{1}._{2}", true),
                    Translate("ikas_cannot_get_session_token"),
                    Translate(error.ToString()),
                    Translate("after_you_solve_the_problems_above,_if_this_error_message_continues_to_appear,_please_consider_submitting_the_issue.")
                    ), "Ikas", MessageBoxButton.OK, MessageBoxImage.Warning);
                txtSessionToken.Text = "";
                // Fade out loading
                ((Storyboard)FindResource("fade_out")).Begin(bdLoading);
                bdLoading.IsHitTestVisible = false;
                lbOk.IsEnabled = true;
            }
            else
            {
                MessageBox.Show(Translate("get_session_token_successfully.", true), "Ikas", MessageBoxButton.OK, MessageBoxImage.Information);
                txtSessionToken.Text = sessionToken;
                TxtSessionToken_LostFocus(null, null);
                // Update cookie
                if (gridUserLogIn.Visibility == Visibility.Visible)
                {
                    ((Storyboard)FindResource("grid_fade_out")).Begin(gridUserLogIn);
                    ((Storyboard)FindResource("grid_fade_in")).Begin(gridUserLoggedIn);
                    // Fade out loading
                    ((Storyboard)FindResource("fade_out")).Begin(bdLoading);
                    bdLoading.IsHitTestVisible = false;
                    lbOk.IsEnabled = true;
                    BdUpdateCookie_MouseDown(null, null);
                }
                else
                {
                    // Fade out loading
                    ((Storyboard)FindResource("fade_out")).Begin(bdLoading);
                    bdLoading.IsHitTestVisible = false;
                    lbOk.IsEnabled = true;
                }
            }
        }

        private void CookieGet(Base.ErrorType error, string cookie)
        {
            if (cookie == null || cookie == "")
            {
                MessageBox.Show(string.Format(Translate("{0},_because_{1}._{2}", true),
                    Translate("ikas_cannot_update_cookie"),
                    Translate(error.ToString()),
                    Translate("after_you_solve_the_problems_above,_if_this_error_message_continues_to_appear,_please_consider_submitting_the_issue.")
                    ), "Ikas", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
            else
            {
                if (Visibility == Visibility.Visible)
                {
                    MessageBox.Show(Translate("update_cookie_successfully.", true), "Ikas", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                txtCookie.Text = cookie;
                Depot.Cookie = txtCookie.Text;
            }
            // Fade out loading
            ((Storyboard)FindResource("fade_out")).Begin(bdLoading);
            bdLoading.IsHitTestVisible = false;
            lbOk.IsEnabled = true;
        }

        private void ShowMessage(string title, string content, Point point)
        {
            messageWindow.SetContent(title, content);
            messageWindow.Top = point.Y + Top - messageWindow.Height / 2;
            messageWindow.Left = point.X + Left + 10;
            ((Storyboard)FindResource("window_fade_in")).Begin(messageWindow);
        }

        private string Translate(string s, bool isLocal = false)
        {
            try
            {
                if (isLocal)
                {
                    return (string)FindResource("settings_window-" + s);
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
