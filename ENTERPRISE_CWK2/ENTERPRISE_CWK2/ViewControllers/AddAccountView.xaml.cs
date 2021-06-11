using ENTERPRISE_CWK2.Models;
using ENTERPRISE_CWK2.Utility;
using System;
using System.Collections.Generic;
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
    /// Interaction logic for AddAccountView.xaml
    /// </summary>
    public partial class AddAccountView : UserControl
    {

        private MainWindow mw;
        public FormTemplate ft { get; set; }

        public TextBox accNameTB { get; set; }
        public TextBox accNumTB { get; set; }
        public TextBox sCodeTB { get; set; }
        public ComboBox accTypeCB { get; set; }
        public string pageTitle { get; set; }

        private int NUM_ROWS = 4;
        
        public AddAccountView(MainWindow mw)
        {
            InitializeComponent();
            this.mw = mw;
            
            initializeUIElements();
            pageTitle = "Add Account";

            ft = new FormTemplate(mw, NUM_ROWS);
            setPageTitle();
            ft.addNewRow("Account Holder", accNameTB);
            ft.addNewRow("Account Number", accNumTB);
            ft.addNewRow("Sort Code", sCodeTB);
            ft.addNewRow("Account Type", accTypeCB);

            accTypeCB.ItemsSource = mw.mdm.accTypeCollection;
            accTypeCB.SelectionChanged += AccTypeCB_SelectionChanged;

            ft.addBackButton();
            ft.addConfirmButton();
            ft.confirmButton.Click += ConfirmButton_Click;

            addAccountControl.Content = ft;//displays the form 
        }

        public void ConfirmButton_Click(object sender, RoutedEventArgs e)
        {
            if (checkAccountEntriesValid()) {
                confirmedAddAccountSequence();
            }
            else
            {
                mw.createNotificationWindow(NotificationWindow.NOTIFICATION_VIEW, "You need to fill in all the boxes of this form");
            }
        }

        private void confirmedAddAccountSequence()
        {
            Account acc = new Account();
            acc.AccName = accNameTB.Text;
            acc.AccNum = accNumTB.Text;
            acc.AccSortCode = sCodeTB.Text;
            acc.AccType = mw.mdm.selectedType;
            mw.returnToPreviousView();//check if this would work - page being removed as well as database written to?                        
            mw.dispatchDataButtonsUpdate(() => mw.mdm.dbWriter.insertAccount(acc));
        }
        public bool checkAccountEntriesValid()
        {
            return   mw.mdm.customValidator.validateName(accNameTB.Text) &&
                     mw.mdm.customValidator.validateNumber(accNumTB.Text, FormTemplate.ACCOUNT_NUM_LENGTH) &&
                     mw.mdm.customValidator.validateSortCode(sCodeTB.Text) &&
                     accTypeCB.SelectedIndex >= 0;
        }

        private void AccTypeCB_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            mw.mdm.selectedType = (AccountType)accTypeCB.SelectedItem;
        }

        public void setPageTitle()
        {
            ft.titleLabel.Content = pageTitle;
        }


        private void initializeUIElements()
        {
            this.accNameTB = mw.uiDesigner.getNewTextBox();
            accNameTB.TextChanged += AccNameTB_TextChanged;
            this.accNumTB = mw.uiDesigner.getNewTextBox();
            accNumTB.TextChanged += AccNumTB_TextChanged;
            this.sCodeTB = mw.uiDesigner.getNewTextBox();
            sCodeTB.TextChanged += SCodeTB_TextChanged;
            this.accTypeCB = mw.uiDesigner.getNewComboBox();
        }

        private void SCodeTB_TextChanged(object sender, TextChangedEventArgs e)
        {
            ft.verifyTextBoxState(mw.mdm.customValidator.validateSortCode(sCodeTB.Text), "Enter 6 digits in format: DD-DD-DD");
        }
        
        private void AccNumTB_TextChanged(object sender, TextChangedEventArgs e)
        {
            ft.verifyTextBoxState(mw.mdm.customValidator.validateNumber(accNumTB.Text, FormTemplate.ACCOUNT_NUM_LENGTH), "Enter 8 digits");
        }

        private void AccNameTB_TextChanged(object sender, TextChangedEventArgs e)
        {
            ft.verifyTextBoxState(mw.mdm.customValidator.validateName(accNameTB.Text), 
                "Enter more than 3 characters. Numbers or symbols not allowed.");
        }
    }
}
