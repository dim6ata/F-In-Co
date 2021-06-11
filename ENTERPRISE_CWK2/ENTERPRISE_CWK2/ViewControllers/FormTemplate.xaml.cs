using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace ENTERPRISE_CWK2.ViewControllers
{
    /// <summary>
    /// Interaction logic for FormTemplate.xaml
    /// </summary>
    public partial class FormTemplate : UserControl
    {
        public ToolTip toolTip { get; set; }
        private Button backButton;
        public Button confirmButton { get; set; }
        private MainWindow mw;
        private int numRows;
        private int currentRow = 0;
        public const int ACCOUNT_NUM_LENGTH = 8;
           

        public const int PHONE_NUM_LENGTH = 11;

        public FormTemplate(MainWindow mw, int numRows)
        {
            InitializeComponent();
            this.mw = mw;
            this.numRows = numRows;           
            createRowDefinitions();
            toolTip = mw.uiDesigner.getNewToolTip();
        }

        
        /// <summary>
        /// creates row definitions based on number of rows provided at constructor
        /// </summary>
        private void createRowDefinitions()
        {
            for (int i = 0; i < numRows; i++)
            {
                this.labelGrid.RowDefinitions.Add(new RowDefinition());
                this.textBoxGrid.RowDefinitions.Add(new RowDefinition());
            }
        }

        /// <summary>
        /// creates and adds a confirmation button
        /// </summary>
        public void addConfirmButton()
        {
            confirmButton = mw.buttonDesigner.confirmButton();
           // confirmButton.Click += ConfirmButton_Click;//put in individual controllers
            confirmButtonStackPanel.Children.Add(confirmButton);
        }


        /// <summary>
        /// creates and adds back button to view
        /// </summary>
        public void addBackButton()
        {
            backButton = mw.buttonDesigner.backButton();
            backButton.Click += BackButton_Click;
            addAccountStackPanel.Children.Add(backButton);
        }

        /// <summary>
        /// back button on click handler.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            if (toolTip.IsOpen)
            {
                closeToolTip();
            }
            mw.returnToPreviousView();
        }

        /// <summary>
        /// adds a new row
        /// </summary>
        /// <param name="rowName">the name used for the label of the row</param>
        /// <param name="obj">the object that would be added next to the row label</param>
        public void addNewRow(string rowName, Object obj)
        {

            if (currentRow < numRows)
            {
                Label setLabel = new Label();
                setLabel.Content = rowName;
                setLabel.Style = (Style)Application.Current.Resources["formLabelStyle"];
                Viewbox vb1 = getViewBox(setLabel);    

                labelGrid.Children.Add(vb1);
                Grid.SetRow(vb1, currentRow);
               
                Viewbox vb2 = getViewBox((UIElement)obj);
                textBoxGrid.Children.Add(vb2);
                Grid.SetRow(vb2, currentRow);

                currentRow++;
            }
        }

        /// <summary>
        /// Creates and adds a UIElement to a Viewbox
        /// </summary>
        /// <param name="e">UIElement which is to be added to a Viewbox object</param>
        /// <returns>returns the Viewbox object to which e has been added to</returns>
        private Viewbox getViewBox(UIElement e)
        {
            Viewbox vb = new Viewbox();
            vb.Stretch = Stretch.Fill;
            vb.Child = e;
            return vb;
        }
  
        /// <summary>
        /// cancels a given click handler for views that use confirm button
        /// </summary>
        /// <param name="clickEvent"></param>
        public void cancelButtonHandler(RoutedEventHandler clickEvent)
        {
            confirmButton.Click -= clickEvent;
        }

        /// <summary>
        /// sets text for tooltip
        /// </summary>
        /// <param name="text"></param>
        public void setToolTipText(string text)
        {
            toolTip.Content = text;
        }

        /// <summary>
        /// opens tool tip
        /// </summary>
        public void openToolTip()
        {
            toolTip.IsOpen = true;
        }

        /// <summary>
        /// closes tool tip
        /// </summary>
        public void closeToolTip()
        {
            toolTip.IsOpen = false;
        }

        /// <summary>
        /// Allows for a tooltip to be opened depending on a condition
        /// </summary>
        /// <param name="condition"></param>
        /// <param name="text"></param>
        public void verifyTextBoxState(bool condition, string text)
        {
            if (!condition)
            {
                setToolTipText(text);
                openToolTip();
            }
            else
            {
                closeToolTip();
            }
        }



    }
}
