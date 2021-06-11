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

namespace ENTERPRISE_CWK2.ViewControllers
{
    /// <summary>
    /// Interaction logic for NotificationWindow.xaml
    /// </summary>
    public partial class NotificationWindow : Window
    {
        public bool isConfirmed { get; private set; }
        public const int NOTIFICATION_VIEW = 1;
        public const int CONFIRMATION_VIEW = 2;

        public NotificationWindow(int type,  string message)
        {
            InitializeComponent();
            

            messageTextBlock.Text = message;

            if(type == NOTIFICATION_VIEW)
            {
                this.Title = "Notification";
                cancelNotificationButton.Visibility = Visibility.Collapsed;
            }
            else
            {
                this.Title = "Confirmation";
                cancelNotificationButton.Visibility = Visibility.Visible;
            }

            checkNotificationButton.Click += CheckNotificationButton_Click;
            cancelNotificationButton.Click += CancelNotificationButton_Click;

            this.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            this.Topmost = true;
            this.ShowDialog();

        }

        private void CancelNotificationButton_Click(object sender, RoutedEventArgs e)
        {
            isConfirmed = false;
            closeWindow();
        }

        private void CheckNotificationButton_Click(object sender, RoutedEventArgs e)
        {
            isConfirmed = true;
            closeWindow();
        }

        public void closeWindow()
        {
            this.Close();
        }


    }
}
