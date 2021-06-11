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
    /// Interaction logic for ReportTemplateView.xaml
    /// </summary>
    public partial class ReportTemplateView : UserControl
    {
        public const string INCOME = "Gross Income";
        public const string EXPENSE = "Expenses";
        public const string NET = "Net Total";
        

        private MainWindow mw;
        public Button confirmButton { get; set; }
        //public int numRows { get; set; }
        //public int numColumns { get; set; }
        //public int currentRow { get; set; }
        //public int currentColumn { get; set; }


        public ReportTemplateView(MainWindow mw)
        {
            InitializeComponent();
            this.mw = mw;
            confirmButton = mw.buttonDesigner.confirmButton();
            //confirmButton.Click += ConfirmButton_Click;
            reportTemplateButtonsStackPanel.Children.Add(confirmButton);

        }


        ///// <summary>
        ///// move this to individual classes 
        ///// </summary>
        ///// <param name="sender"></param>
        ///// <param name="e"></param>
        //private void ConfirmButton_Click(object sender, RoutedEventArgs e)
        //{
        //    Debug.WriteLine("Confirm button in template works");
        //}

        public void setupLabels(string l1, string l2, string l3)
        {
            label1.Content = l1;
            label2.Content = l2;
            label3.Content = l3;

        }


        public void createColumnDefinitions(int num)
        {
            for (int i = 0; i < num; i++)
            {
                templateResultsGrid.ColumnDefinitions.Add(new ColumnDefinition());
            }
        }

        public void createRowDefinitions(int num)
        {
            for (int i = 0; i < num; i++)
            {
                templateResultsGrid.RowDefinitions.Add(new RowDefinition());
            }
        }

        /// <summary>
        /// adds a required element to the grid
        /// </summary>
        /// <param name="ue"></param>
        /// <param name="rowNum"></param>
        /// <param name="columnNum"></param>
        public void addElementToGrid(UIElement ue, int rowNum, int columnNum)
        {
            Viewbox vb = populateViewBox(ue);
            
            Grid.SetColumn(vb, columnNum);
            Grid.SetRow(vb, rowNum);
            templateResultsGrid.Children.Add(vb);

        }

        /// <summary>
        /// Clears the grid before it is repopulated again. 
        /// This should be called every time a button is pressed to 
        /// calculate a new result
        /// </summary>
        public void clearGrid()
        {
            
            templateResultsGrid.Children.Clear();
            templateResultsGrid.ColumnDefinitions.Clear();
            templateResultsGrid.RowDefinitions.Clear();

        }

        public void addColumns(string header)
        {

        }

        private Viewbox populateViewBox(UIElement e)
        {
            Viewbox vb = new Viewbox();
            vb.Stretch = Stretch.Fill;
            vb.Child = e;

            return vb;
        }

        //public void addNewColumn(string header, string path, string style)
        //{
        //    templateDataGrid.Columns.Add(new DataGridTextColumn()
        //    {

        //        Header = header,
        //        Binding = new Binding(path),
        //        Width = new DataGridLength(1, DataGridLengthUnitType.Star),
        //        HeaderStyle = Application.Current.Resources[style] as Style
        //    });
        //}

    }
}
