using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace ENTERPRISE_CWK2.Utility
{
    /// <summary>
    /// Class that creates button templates that would be used in the entire project
    /// </summary>
    public class CRUDButtonDesigner
    {

        public Button addButton()
        {
            return createButton("addButtonStyle");
        }

        public Button editButton()
        {
            return createButton("editButtonStyle");
        }

        public Button viewButton()
        {
            return createButton("viewButtonStyle");
        }

        public Button backButton()
        {
            return createButton("backButtonStyle");
        }

        public Button searchButton()
        {
            return createButton("searchButtonStyle");
        }

        public Button confirmButton()
        {
            return createButton("confirmButtonStyle");
        }
        public Button deleteButton()
        {
            return createButton("deleteButtonStyle");
        }

        private Button createButton(string name)
        {
            Button btn = new Button();
            btn.Style = (Style)Application.Current.Resources[name];
            return btn;
        }
    }
}