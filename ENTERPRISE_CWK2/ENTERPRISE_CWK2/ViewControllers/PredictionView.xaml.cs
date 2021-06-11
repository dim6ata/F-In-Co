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


    enum ROWS
    {
        INCOME, EXPENSE, NET
    }
    /// <summary>
    /// Interaction logic for PredictionView.xaml
    /// </summary>
    public partial class PredictionView : UserControl
    {
        private MainWindow mw;
        public ReportTemplateView rt;
        public DatePicker dPicker { get; set; }
        public ComboBox fieldCB { get; set; }
        public ComboBox filterCB { get; set; }
        public ComboBoxItem grossIncomeCBI { get; set; }
        public ComboBoxItem expensesCBI { get; set; }
        public ComboBoxItem netCBI { get; set; }
        public ComboBoxItem selectAllCBI { get; set; }
        public ComboBoxItem allAccountsCBI { get; set; }
        public CompositeCollection rowsCompositeCollection { get; set; }
        public ComboBoxItem selectedItem { get; set; }
        public int numRows { get; set; }
        public int numColumns { get; set; }
        public const int FIRST_ELEMENT = 1;      
        private string selectAllString = "Select All";
        private string selectedItemString;
        private string[] rowLabelArray = { ReportTemplateView.INCOME, ReportTemplateView.EXPENSE, ReportTemplateView.NET };       
        private Dictionary<PredictionFactory.Location, PredictionFactory> predictionCollection;      

        public PredictionView(MainWindow mw)
        {
            InitializeComponent();
            this.mw = mw;
            rt = new ReportTemplateView(mw);
            predictionCollection = new Dictionary<PredictionFactory.Location, PredictionFactory>();
            
            rt.reportTemplateLabel.Content = "Financial Prediction";
            rt.setupLabels("Select Date", "Select Fields", "Filter By");
            populateUIElements();
            predictionViewControl.Content = rt;

        }

        /// <summary>
        /// Initialises UI elements
        /// </summary>
        private void populateUIElements()
        {

            //initialise rows and columns to count of elements that are selected by default
            numRows = rowLabelArray.Length + 1;
            numColumns = mw.mdm.currentUser.UserAccountsList.Count + 1;

            //BOX 1
            dPicker = mw.uiDesigner.getNewDatePicker();       
            //BOX 2
            fieldCB = mw.uiDesigner.getNewComboBox();

            grossIncomeCBI = new ComboBoxItem();
            expensesCBI = new ComboBoxItem();
            netCBI = new ComboBoxItem();
            selectAllCBI = new ComboBoxItem();

            grossIncomeCBI.Content = rowLabelArray[(int)ROWS.INCOME];
            expensesCBI.Content = rowLabelArray[(int)ROWS.EXPENSE];
            netCBI.Content = rowLabelArray[(int)ROWS.NET];
            selectAllCBI.Content = selectAllString;
            selectAllCBI.IsSelected = true;

            fieldCB.Items.Add(selectAllCBI);
            fieldCB.Items.Add(grossIncomeCBI);
            fieldCB.Items.Add(expensesCBI);
            fieldCB.Items.Add(netCBI);

            fieldCB.SelectionChanged += FieldCB_SelectionChanged;


            //BOX 3
            allAccountsCBI = new ComboBoxItem();
            allAccountsCBI.Content = "All Accounts";
            allAccountsCBI.IsSelected = true;

            filterCB = mw.uiDesigner.getNewComboBox();


            // filterCB.Items.Insert(0, allAccountsCBI);
            rowsCompositeCollection = new CompositeCollection();
            rowsCompositeCollection.Add(allAccountsCBI);


            for (int i = 0; i < mw.mdm.accountCollection.Count; i++)
            {
                rowsCompositeCollection.Add(mw.mdm.accountCollection[i]);
            }

            // filterCB.ItemsSource = mw.mdm.accountCollection;
            filterCB.ItemsSource = rowsCompositeCollection;
            filterCB.SelectedIndex = 0;//makes sure default selection is 'select all'

            filterCB.SelectionChanged += FilterCB_SelectionChanged;

            rt.templateStackPanel1.Children.Insert(1, dPicker);
            rt.templateStackPanel2.Children.Insert(1, fieldCB);
            rt.templateStackPanel3.Children.Insert(1, filterCB);

            rt.confirmButton.Click += ConfirmButton_Click;
        }

        

        /// <summary>
        /// Confirmed button handler
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ConfirmButton_Click(object sender, RoutedEventArgs e)
        {
            //make a check if all the elements are selected.
            if (checkEmpty())
            {
                confirmedPredictionSequence();
            }
            else
            {
                mw.createNotificationWindow(NotificationWindow.NOTIFICATION_VIEW, "You need to select a correct date in the future");
            }
        }

        /// <summary>
        /// Prediction sequence once all boxes have been selected.
        /// </summary>
        private void confirmedPredictionSequence()
        {
            //clears previous grid details if grid is not empty
            if (rt.templateResultsGrid.Children.Count > 0)
            {
                rt.clearGrid();
            }

            if (predictionCollection.Count > 0)//clears the predictionCollection before populating it again
            {
                predictionCollection.Clear();
            }

            //populate columns rows

            rt.createColumnDefinitions(numColumns);
            rt.createRowDefinitions(numRows);

            calculatePrediction();

            rt.templateDateResultLabel.Content = dPicker.SelectedDate;//sets the date in the label to selection of datepicker

            createGridElements();

            //display border containing the whole thing
            rt.borderTemplate.Visibility = Visibility.Visible;
        }

        /// <summary>
        /// Sequence that calculates prediction elements that would be displayed 
        /// </summary>
        private void calculatePrediction()
        {
            //do calculation of elements per row, i.e expense/income/net/all
            if (numRows > 2)//case when all selected
            {
                for (int row = 1; row < numRows; row++)//start from 1 because first row is reserved for column headers
                {
                    retrieveElementPerColumn(row, rowLabelArray[row - 1]);
                }
            }
            else//case when a specific row is selected
            {
                retrieveElementPerColumn(FIRST_ELEMENT, selectedItemString);//only a specific prediction element
            }
        }

        /// <summary>
        /// Retrieves prediction elements per column
        /// </summary>
        /// <param name="rowNumber"></param>
        /// <param name="selectedRowItem"></param>
        private void retrieveElementPerColumn(int rowNumber, string selectedRowItem)
        {
            if (numColumns > 2)//when all elements are selected
            {
                for (int column = 1; column < numColumns; column++)
                {
                    retrievePrediction(rowNumber, column, selectedRowItem, mw.mdm.currentUser.UserAccountsList[column - 1]);
                }
            }
            else//only when one element is selected
            {
                retrievePrediction(rowNumber, FIRST_ELEMENT, selectedRowItem, mw.mdm.selectedAccount);
            }
        }

        /// <summary>
        /// Adds prediction result from PredictionFactory to predictionCollection.
        /// </summary>
        /// <param name="rowNum"></param>
        /// <param name="colNum"></param>
        /// <param name="selectedRowItem"></param>
        /// <param name="selectedAccount"></param>
        private void retrievePrediction(int rowNum, int colNum, string selectedRowItem, Account selectedAccount)
        {
            PredictionFactory pf = new PredictionFactory(rowNum, colNum, selectedRowItem, selectedAccount, (DateTime)dPicker.SelectedDate);
            predictionCollection.Add(pf.location, pf);
        }

        /// <summary>
        /// Creates the elements within a display grid dynamically based on user's choices within combo boxes.
        /// </summary>
        private void createGridElements()
        {
            for (int row = 0; row < numRows; row++)
            {
                for (int col = 1; col < numColumns; col++)//always starts from 1 because column 0 is reserved for row headers
                {
                    //COLUMN HEADERS
                    if (row == 0)//this will populate headers
                    {
                        Label columnHeaderLabel = mw.uiDesigner.getNewHeaderLabel();
                        columnHeaderLabel.Margin = new Thickness(5, 0, 0, 0);
                        if (numColumns == 2)
                        {
                            columnHeaderLabel.Content = mw.mdm.selectedAccount.AccName;
                        }
                        else
                        {
                            columnHeaderLabel.Content = mw.mdm.currentUser.UserAccountsList[col - 1].AccName;//col-1 because list starts from zero
                        }
                        rt.addElementToGrid(columnHeaderLabel, row, col);
                    }
                    else //this will populate the rest of the grid
                    {
                        TextBox textBox = mw.uiDesigner.getNewTextBox();
                        textBox.IsReadOnly = true;
                        PredictionFactory.Location location = new PredictionFactory.Location(row, col);
                        textBox.Text = mw.mdm.convertAmountDoubleToString
                            (predictionCollection[location].prediction.futureAmount);
                        rt.addElementToGrid(textBox, row, col);
                    }
                }

                //ROW HEADERS:
                if (row > 0)//this will populate the row headers
                {
                    Label rowHeaderLabel = mw.uiDesigner.getNewHeaderLabel();
                    if (numRows > 2)//accessing array when select all is chosen
                    {
                        rowHeaderLabel.Content = rowLabelArray[row - 1];//accessing appropriate 
                    }
                    else//using selected string when specific element is chosen.
                    {
                        rowHeaderLabel.Content = selectedItemString;
                    }
                    rt.addElementToGrid(rowHeaderLabel, row, 0);
                }
            }
        }

        /// <summary>
        /// checks if the form elements are all selected before the prediction could be calculated.
        /// </summary>
        /// <returns></returns>
        public bool checkEmpty()
        {
            return (filterCB.SelectedIndex >= 0 &&
                fieldCB.SelectedIndex >= 0 && dPicker.SelectedDate != null
                && mw.mdm.customValidator.validateDate((DateTime)dPicker.SelectedDate));
        }

        /// <summary>
        /// handler of 3rd form that filters prediction by account
        /// setting the number of columns
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void FilterCB_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (filterCB.SelectedItem.ToString().Equals("System.Windows.Controls.ComboBoxItem: All Accounts"))
            {              
                numColumns = mw.mdm.currentUser.UserAccountsList.Count + 1;//add one more for the header
            }
            else
            {
                numColumns = 2; //extra one for the header
                mw.mdm.selectedAccount = filterCB.SelectedItem as Account;
            }

        }

        /// <summary>
        /// handler of 2nd form that filters prediction by 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void FieldCB_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {            
            selectedItem = ((sender as ComboBox).SelectedItem) as ComboBoxItem;

            selectedItemString = selectedItem.ToString().Split(new string[] { ": " }, StringSplitOptions.None).Last();
            if (selectedItemString.Equals("Select All"))
            {
                numRows = 4;//3+1 for the header
            }
            else
            {
                numRows = 2;
            }            
        }
    }
}
