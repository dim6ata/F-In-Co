using ENTERPRISE_CWK2.Models;
using ENTERPRISE_CWK2.Utility;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
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
    /// Interaction logic for AddIncomeView.xaml
    /// </summary>
    public partial class AddIncomeView : UserControl
    {
        public FormTemplate ft { get; set; }
        private MainWindow mw;

        public ComboBox paidByCB { get; set; }
        public TextBox accTB { get; set; }
        public TextBox sCodeTB { get; set; }
        public TextBox amountTB { get; set; }
        public ComboBox paidToCB { get; set; }
        public DatePicker dPicker { get; set; }
        public ComboBox serviceTypeCB { get; set; }
        public TextBox referenceTB { get; set; }
        public string pageTitle { get; set; }

        private int NUM_ROWS = 8;

        private Account selectedAccount;
        private Contact selectedContact;

        public AddIncomeView(MainWindow mw)
        {
            InitializeComponent();
            this.mw = mw;

            initializeUIElements();
            pageTitle = "Add New Income";
            //how to create a view
            ft = new FormTemplate(mw, NUM_ROWS);
            setPageTitle();
            ft.addNewRow("Paid By", paidByCB);
            ft.addNewRow("Account Number", accTB);
            ft.addNewRow("Sort Code", sCodeTB);
            ft.addNewRow("Amount", amountTB);
            ft.addNewRow("Paid To Account", paidToCB);
            ft.addNewRow("Date Added", dPicker);
            ft.addNewRow("Type of Service", serviceTypeCB);
            ft.addNewRow("Reference", referenceTB);

            //Debug.WriteLine("This is what prints the combo box " + paidByTB.ToString());


            //oc = new ObservableCollection<Account>(mw.mdm.currentUser.UserAccountsList);

            paidByCB.ItemsSource = mw.mdm.contactCollection;
            paidByCB.SelectionChanged += PaidByCB_SelectionChanged;

            paidToCB.ItemsSource = mw.mdm.accountCollection;
            paidToCB.SelectionChanged += PaidTo_Selection_Changed;

            serviceTypeCB.ItemsSource = mw.mdm.serviceCollection;
            serviceTypeCB.SelectionChanged += ServiceTypeCB_SelectionChanged;

            //Debug.WriteLine(" 1 the item changed is " + paidToCB.SelectedItem);
            //ft.isInner = false;

            ft.addConfirmButton();
            ft.confirmButton.Click += ConfirmButton_Click;

            addIncomeControl.Content = ft;//displays the form 

        }

        public void ConfirmButton_Click(object sender, RoutedEventArgs e)
        {
 
            if (checkIncomeEntriesValid())
            {
                confirmedAddIncomeSequence();
            }
            else
            {
                mw.createNotificationWindow(NotificationWindow.NOTIFICATION_VIEW, "You need to fill in all the boxes of this form");
            }
            
        }

        private void confirmedAddIncomeSequence()
        {
            //TRANSACTION:
            Transaction newIncome = new Income();

            //CONTACT:            
            newIncome.TransContact = selectedContact;

            //DATE:
            newIncome.TransDate = (DateTime)dPicker.SelectedDate;
            newIncome.setTransactionString();

            //ACCOUNT:            
            newIncome.TransAccID = selectedAccount.AccID;

            //AMOUNT:
            double newAmount = mw.mdm.convertAmountStringToDouble(amountTB.Text);
            newIncome.TransAmountString = mw.mdm.convertAmountDoubleToString(newAmount);//used for updating visual elements in lists
            newIncome.TransAmount = newAmount;
            mw.mdm.addToBalance(newIncome, selectedAccount);

            mw.dispatchData(() => mw.mdm.dbUpdater.updateBalance(selectedAccount));//updates balance of account in database.

            //SERVICE:
            Service serv = (Service)serviceTypeCB.SelectedItem;
            newIncome.TransServiceID = serv.ServiceID;

            //REFERENCE:
            newIncome.TransReference = referenceTB.Text;

            //TYPE:
            newIncome.TransType = "IN";

            //UPDATE LISTS: 
            mw.mdm.addTransactionToList(newIncome, selectedContact.ContTransList);
            mw.mdm.addTransactionToList(newIncome, selectedAccount.incomeList);
            mw.mdm.addTransactionToList(newIncome, selectedAccount.transactionsList);

            mw.updateBalanceLabel();//update balance label
            mw.createViewAndClearIfExists(typeof(AddIncomeView));
            //mw.returnToPreviousView();//returns to previous view in order to avoid multiple attempts at editing the same transaction    
            mw.dispatchDataButtonsUpdate(() => mw.mdm.dbWriter.insertTransaction(newIncome));//sends transaction data to database concurrently

        }

        public bool checkIncomeEntriesValid()
        {
            return 
                mw.mdm.customValidator.validateName(referenceTB.Text) &&
                mw.mdm.customValidator.validateAmount(amountTB.Text) &&
                paidByCB.SelectedIndex>=0 &&
                paidToCB.SelectedIndex>=0 &&
                serviceTypeCB.SelectedIndex>=0 &&
                dPicker.SelectedDate!=null;
        }

        /// <summary>
        /// Displays page title
        /// </summary>
        public void setPageTitle()
        {
            ft.titleLabel.Content = pageTitle;
        }

        private void ServiceTypeCB_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Service service = (Service)serviceTypeCB.SelectedItem;           
        }

        /// <summary>
        /// On Change handler of Paid By Combo Box
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void PaidByCB_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            selectedContact = (Contact)paidByCB.SelectedItem;            
            accTB.Text = selectedContact.ContactAccNum;
            sCodeTB.Text = selectedContact.ContactSortCode;
        }

        /// <summary>
        /// On Change handler of Paid To Combo Box
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void PaidTo_Selection_Changed(object sender, SelectionChangedEventArgs e)
        {
            selectedAccount = (Account)paidToCB.SelectedItem;          
        }

        /// <summary>
        /// Initialises all visual elements
        /// </summary>
        private void initializeUIElements()
        {
            paidByCB = mw.uiDesigner.getNewComboBox();
            accTB = mw.uiDesigner.getNewTextBox();
            accTB.IsReadOnly = true;
            sCodeTB = mw.uiDesigner.getNewTextBox();
            sCodeTB.IsReadOnly = true;
            amountTB = mw.uiDesigner.getNewTextBox();
            amountTB.TextChanged += AmountTB_TextChanged;
            paidToCB = mw.uiDesigner.getNewComboBox();
            dPicker = mw.uiDesigner.getNewDatePicker();
            serviceTypeCB = mw.uiDesigner.getNewComboBox();
            referenceTB = mw.uiDesigner.getNewTextBox();
            referenceTB.TextChanged += ReferenceTB_TextChanged;
        }

        private void ReferenceTB_TextChanged(object sender, TextChangedEventArgs e)
        {
            ft.verifyTextBoxState(mw.mdm.customValidator.validateName(referenceTB.Text),"Enter more than three alphabetical characters");
        }

        private void AmountTB_TextChanged(object sender, TextChangedEventArgs e)
        {
            ft.verifyTextBoxState(mw.mdm.customValidator.validateAmount(amountTB.Text), "Enter a numeric value");
        }
    }

}
