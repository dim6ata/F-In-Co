using ENTERPRISE_CWK2.Models;
using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;

namespace ENTERPRISE_CWK2.ViewControllers
{
    /// <summary>
    /// Interaction logic for AddExpenseView.xaml
    /// </summary>
    public partial class AddExpenseView : UserControl
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

        public AddExpenseView(MainWindow mw)
        {
            InitializeComponent();
            this.mw = mw;
            
            initializeUIElements();
            pageTitle = "Add New Expense";
            //how to create a view
            ft = new FormTemplate(mw, NUM_ROWS);
            setPageTitle();
            ft.addNewRow("Paid By", paidByCB);
            ft.addNewRow("Paid To Account", paidToCB);
            ft.addNewRow("Account Number", accTB);
            ft.addNewRow("Sort Code", sCodeTB);
            ft.addNewRow("Amount", amountTB);            
            ft.addNewRow("Date Added", dPicker);
            ft.addNewRow("Type of Service", serviceTypeCB);
            ft.addNewRow("Reference", referenceTB);

            paidByCB.ItemsSource = mw.mdm.accountCollection;
            paidByCB.SelectionChanged += PaidByCB_SelectionChanged;

            paidToCB.ItemsSource = mw.mdm.contactCollection;
            paidToCB.SelectionChanged += PaidTo_Selection_Changed;

            serviceTypeCB.ItemsSource = mw.mdm.serviceCollection;
            serviceTypeCB.SelectionChanged += ServiceTypeCB_SelectionChanged;
            
            ft.addConfirmButton();
            ft.confirmButton.Click += ConfirmButton_Click;

            addExpenseControl.Content = ft;//displays the form 
        }

        public void ConfirmButton_Click(object sender, RoutedEventArgs e)
        {
           
            if (checkExpenseEntryValid())
            {
                confirmedAddExpenseSequence();
            }
            else
            {
                mw.createNotificationWindow(NotificationWindow.NOTIFICATION_VIEW, "You need to fill in all the boxes of this form");
            }
        }

        /// <summary>
        /// A validation for all form entries
        /// </summary>
        /// <returns></returns>
        public bool checkExpenseEntryValid()
        {
            return
                mw.mdm.customValidator.validateName(referenceTB.Text) &&
                mw.mdm.customValidator.validateAmount(amountTB.Text) &&
                paidByCB.SelectedIndex >= 0 &&
                paidToCB.SelectedIndex >= 0 &&
                serviceTypeCB.SelectedIndex >= 0 &&
                dPicker.SelectedDate != null;
        }

        /// <summary>
        /// Sequence that updates data in working memory and database.
        /// </summary>
        private void confirmedAddExpenseSequence()
        {
            //TRANSACTION:
            Transaction newExpense = new Expense();

            //CONTACT:            
            newExpense.TransContact = selectedContact;

            //DATE:
            newExpense.TransDate = (DateTime)dPicker.SelectedDate;
            newExpense.setTransactionString();

            //ACCOUNT:           
            newExpense.TransAccID = selectedAccount.AccID;

            //AMOUNT:
            double newAmount = mw.mdm.convertAmountStringToDouble(amountTB.Text);
            newExpense.TransAmount = newAmount;
            newExpense.TransAmountString = mw.mdm.convertAmountDoubleToString(newExpense.TransAmount);//used for updating visual elements in lists

            mw.mdm.addToBalance(newExpense, selectedAccount);

            //updates balance of account in database.
            mw.dispatchData(() => mw.mdm.dbUpdater.updateBalance(selectedAccount));

            //SERVICE:
            Service serv = (Service)serviceTypeCB.SelectedItem;
            newExpense.TransServiceID = serv.ServiceID;

            //REFERENCE:
            newExpense.TransReference = referenceTB.Text;

            //TYPE:
            newExpense.TransType = "OUT";

            //UPDATE LISTS: 
            mw.mdm.addTransactionToList(newExpense, selectedContact.ContTransList);
            mw.mdm.addTransactionToList(newExpense, selectedAccount.expenseList);
            mw.mdm.addTransactionToList(newExpense, selectedAccount.transactionsList);

            mw.updateBalanceLabel();//update balance label
            //mw.returnToPreviousView();//returns to previous view in order to avoid multiple attempts at editing the same transaction        
            mw.createViewAndClearIfExists(typeof(AddExpenseView));
            mw.dispatchDataButtonsUpdate(() => mw.mdm.dbWriter.insertTransaction(newExpense));//sends transaction data to database concurrently

        }
        public void setPageTitle()
        {
            ft.titleLabel.Content = pageTitle;
        }

        private void ServiceTypeCB_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Service service = (Service)serviceTypeCB.SelectedItem;            
        }

        private void PaidByCB_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            selectedAccount = (Account)paidByCB.SelectedItem;
        }

        private void PaidTo_Selection_Changed(object sender, SelectionChangedEventArgs e)
        {
            selectedContact = (Contact)paidToCB.SelectedItem;
         
            accTB.Text = selectedContact.ContactAccNum;

            sCodeTB.Text = selectedContact.ContactSortCode;
        }

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
            ft.verifyTextBoxState(mw.mdm.customValidator.validateName(referenceTB.Text), "Enter more than three alphabetical characters");
        }

        private void AmountTB_TextChanged(object sender, TextChangedEventArgs e)
        {
            ft.verifyTextBoxState(mw.mdm.customValidator.validateAmount(amountTB.Text), "Enter a numeric value");
        }

    }

}