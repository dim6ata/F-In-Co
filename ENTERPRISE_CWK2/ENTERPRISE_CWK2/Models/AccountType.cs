using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ENTERPRISE_CWK2.Models
{
    public class AccountType
    {

        public int accTypeID { get; set; }
        public string accTypeName { get; set; }

        public override string ToString()
        {
            return accTypeName;
        }
    }
}
