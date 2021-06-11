using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using ENTERPRISE_CWK2.Utility;
using ENTERPRISE_CWK2.ViewControllers;

namespace ENTERPRISE_CWK2
{
    enum Buttons
    {
        Home, Account, Contact, Income, Expense, Report, Prediction, User
    }
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// Acts as a controller hub for all other controllers
    /// </summary>
    public partial class MainWindow : Window
    {
        public ModelDataManager mdm { get; set; }
        private List<Button> buttonsList;
        private Dictionary<Type, UserControl> viewsList = new Dictionary<Type, UserControl>();
        private UserControl currentView;
        private UserControl previousView;
        public CRUDButtonDesigner buttonDesigner { get; set; }
        public UIElementDesigner uiDesigner { get; set; }
        private int previousButton;
        private int currentButton;
        public NotificationWindow nw { get; set; }

        public MainWindow()
        {

            InitializeComponent();
            this.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            mdm = new ModelDataManager();

            populateNavButtonList();
            updateBalanceLabel();
           
            buttonDesigner = new CRUDButtonDesigner();
            uiDesigner = new UIElementDesigner();

            Navigate(typeof(HomePageView));

        }

        /// <summary>
        /// updates the value of balance label
        /// </summary>
        public void updateBalanceLabel()
        {
            balanceLabel.Content = mdm.getTotalBalance();
        }

        /// <summary>
        /// Method used to navigate to individual UserControl views and storing views in a dictionary.
        /// Code referenced from: O'Neil, A. (2010) WPF Navigation Basic Sample. 
        /// Accessed from: https://gallery.technet.microsoft.com/WPF-Navigation-Basic-Sample-11f10c74
        /// </summary>
        /// <param name="viewType"></param>
        private void Navigate(Type viewType)
        {
            //assigns current userControl value to previous before it gets changed to new page.
            //intended to be used when returning to a previous page is required. 
            if (currentView != null)
            {
                if (previousView != currentView)
                    previousView = currentView;
            }

            if (viewsList.ContainsKey(viewType))
            {
                currentView = viewsList[viewType];
            }
            else
            {
                currentView = (UserControl)Activator.CreateInstance(viewType, this);
                viewsList[viewType] = currentView;
            }
            DataContext = currentView;//assignment to display current usercontrol. 
        }
        

        /// <summary>
        /// Method that is used to create a list of navigation buttons
        /// </summary>
        private void populateNavButtonList()
        {
            buttonsList = new List<Button>();
            buttonsList.Add(homeButton);
            buttonsList.Add(accountsButton);
            buttonsList.Add(contactsButton);
            buttonsList.Add(incomeButton);
            buttonsList.Add(expensesButton);
            buttonsList.Add(reportButton);
            buttonsList.Add(predictionButton);
            buttonsList.Add(userProfileButton);
        }


        /// <summary>
        /// Method that acts as a button selector.
        /// Selected buttons are disabled and all the rest enabled.
        /// </summary>
        /// <param name="id">carrying the button id</param>
        private void NavigationButtonSelection(int id)
        {

            if (buttonsList != null)
            {
                //sets up previous and current buttons
                if (previousButton != currentButton)
                {
                    previousButton = currentButton;
                }
                currentButton = id;

                for (int i = 0; i < buttonsList.Count; i++)
                {
                    if (id == i)
                    {
                        buttonsList[id].IsEnabled = false;
                    }
                    else
                    {
                        buttonsList[i].IsEnabled = true;
                    }
                }
            }
        }


        /*********************************************************************
                                   Button Handlers
         *********************************************************************/

        private void Account_Clicked(object sender, RoutedEventArgs e)
        {
            NavigationButtonSelection((int)Buttons.Account);
            Navigate(typeof(AccountsCtrl));
        }

        private void Home_Clicked(object sender, RoutedEventArgs e)
        {
            NavigationButtonSelection((int)Buttons.Home);
            Navigate(typeof(HomePageView));
        }

        private void ContactsButton_Click(object sender, RoutedEventArgs e)
        {
            NavigationButtonSelection((int)Buttons.Contact);
            createViewAndClearIfExists(typeof(ContactsView));
        }

        private void ReportButton_Click(object sender, RoutedEventArgs e)
        {
            NavigationButtonSelection((int)Buttons.Report);
            Navigate(typeof(ReportView));
        }

        private void PredictionButton_Click(object sender, RoutedEventArgs e)
        {
            NavigationButtonSelection((int)Buttons.Prediction);
            Navigate(typeof(PredictionView));
        }

        /// <summary>
        /// Opens entering income - clears form before opening
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void IncomeButton_Click(object sender, RoutedEventArgs e)
        {
            NavigationButtonSelection((int)Buttons.Income);
            createViewAndClearIfExists(typeof(AddIncomeView));
        }

        /// <summary>
        /// Opens entering expenses - clears form before opening
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ExpensesButton_Click(object sender, RoutedEventArgs e)
        {
            NavigationButtonSelection((int)Buttons.Expense);          
            createViewAndClearIfExists(typeof(AddExpenseView));
        }

        private void UserProfileButton_Click(object sender, RoutedEventArgs e)
        {
            NavigationButtonSelection((int)Buttons.User);
            Navigate(typeof(UserProfileView));
        }

        private void LogoutButton_Click(object sender, RoutedEventArgs e)
        {
            createNotificationWindow(NotificationWindow.NOTIFICATION_VIEW, "The logout button is not yet operational\nIt will be with you shortly!");
        }

        /// <summary>
        /// Creates add account view form
        /// </summary>
        public void CreateAddAccount()
        {
            Navigate(typeof(AddAccountView));
        }
        /// <summary>
        /// Creates edit transaction view form
        /// </summary>
        public void CreateEditTransaction()
        {
            Navigate(typeof(EditTransactionView));
        }

        /// <summary>
        /// Returns to previous view
        /// </summary>
        public void returnToPreviousView()
        {
            removeView(currentView.GetType());//clears the element on which back button has been pressed.
            NavigationButtonSelection(currentButton);//assigns the button to the currentButton value.
            Navigate(previousView.GetType());//navigates to the previous view
        }

        /// <summary>
        /// Removes an element from the viewsList
        /// </summary>
        /// <param name="currentView"></param>
        private void removeView(Type currentView)
        {
            viewsList.Remove(currentView);
        }
        /// <summary>
        /// Creates add contact view
        /// </summary>
        public void createAddContact()
        {

            Navigate(typeof(AddContactView));
            //add create add contact here

        }
        /// <summary>
        /// Creates Edit contact view
        /// </summary>
        public void createEditContact()
        {
            Navigate(typeof(EditContactView));
        }

        /// <summary>
        /// creates a contact details view
        /// </summary>
        public void createViewContact()
        {             
            createViewAndClearIfExists(typeof(ContactDetailsView));
            //Navigate(typeof(ContactDetailsView));
        }

        /// <summary>
        /// Creates an edit account view
        /// </summary>
        public void createEditAccount()
        {
            Navigate(typeof(EditAccountView));
        }

        public void createViewAndClearIfExists(Type ucType)
        {
            if (viewsList.ContainsKey(ucType))//checks if this view already exists and if it does - deletes it from the list
            {
                removeView(ucType);
            }

            Navigate(ucType);

        }

        /// <summary>
        /// retrieves a desired element from viewsList
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public UserControl getUserControlFromList(Type type)
        {
            try
            {
              return  viewsList[type];

            }
            catch(Exception)
            {
                return null;
            }      
        }

        /// <summary>
        /// Creates a notification window 
        /// </summary>
        /// <param name="type"></param>
        /// <param name="message"></param>
        public void createNotificationWindow(int type, string message)
        {
            nw = new NotificationWindow(type, message);
        }

        /*********************************************************************
                                   Concurrent Methods
         *********************************************************************/

        /// <summary>
        /// Generic concurrent update method
        /// </summary>
        /// <param name="action"></param>
        public async void dispatchData(Action action)
        {
            await Task.Run(action);
        }

        /// <summary>
        /// Concurrent update with creating a new view when required
        /// </summary>
        /// <param name="action"></param>
        /// <param name="type"></param>
        public async void dispatchDataCreateView(Action action, Type type)
        {
            await Task.Run(action);
            await Task.Run(() => mdm.updateCollections());
            createViewAndClearIfExists(type);
        }
                
        /// <summary>
        /// Concurrent method with ObservableCollections update
        /// </summary>
        /// <param name="action"></param>
        public async void dispatchDataUpdateCollections(Action action)
        {
            await Task.Run(action);
            await Task.Run(() => mdm.updateCollections());

        }

        /// <summary>
        /// Concurrent method with update of custom account buttons'
        /// </summary>
        /// <param name="action"></param>
        public async void dispatchDataButtonsUpdate(Action action)
        {
            await Task.Run(action);
            await Task.Run(() => mdm.updateCollections());

            if (getUserControlFromList(typeof(AccountsCtrl)) != null)
            {
                AccountsCtrl accCtrl = getUserControlFromList(typeof(AccountsCtrl)) as AccountsCtrl;
                accCtrl.fillAccountButtonDetails();
            }            
        }

    }
}