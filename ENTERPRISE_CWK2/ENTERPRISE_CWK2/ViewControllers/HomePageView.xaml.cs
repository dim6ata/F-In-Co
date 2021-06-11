using ENTERPRISE_CWK2.Utility;
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

namespace ENTERPRISE_CWK2.ViewControllers
{
    /// <summary>
    /// Interaction logic for HomePageView.xaml
    /// </summary>
    public partial class HomePageView : UserControl
    {
        public HomePageView(MainWindow mw)
        {
            InitializeComponent();
            mainLabel.Content = mw.mdm.currentUser.UserName;
            homePageTextBlock.Text = "This is your personal, financial management tool. \n " +
                "Use the buttons on the left to navigate through the system. \n " +
                "Here you can manage your accounts, contacts and transactions!\n" +
                "Enjoy!";
                            
        }
    }
}
