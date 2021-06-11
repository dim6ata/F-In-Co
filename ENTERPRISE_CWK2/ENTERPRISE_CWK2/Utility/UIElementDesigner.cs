using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace ENTERPRISE_CWK2.Utility
{
    public class UIElementDesigner
    {

        public TextBox getNewTextBox()
        {

            TextBox tb = new TextBox();
            tb.Style = (Style)Application.Current.Resources["formTextBoxStyle"];
            return tb; 

        }

        public ComboBox getNewComboBox()
        {
            ComboBox cb = new ComboBox();
            cb.Style = (Style)Application.Current.Resources["formComboBoxStyle"];       
            return cb;
        }

        public DatePicker getNewDatePicker()
        {
            DatePicker dp = new DatePicker();
            //add a styling:
            dp.Style = (Style)Application.Current.Resources["formDatePicker"];
            return dp;
        }

        public Viewbox getNewViewBox()
        {
            Viewbox vb = new Viewbox();
            vb.Style = (Style)Application.Current.Resources["vBoxStyle"];

            return vb;
        }

        public Label getNewHeaderLabel()
        {
            Label lb = new Label();
            lb.Style = (Style)Application.Current.Resources["formLabelStyle"];
            return lb;
        }

        public ToolTip getNewToolTip()
        {
            ToolTip tt = new ToolTip();
            tt.Style = (Style)Application.Current.Resources["toolTipStyle"];
            return tt;
        }

    }
}
