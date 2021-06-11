using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ENTERPRISE_CWK2.Models
{
    public class Service
    {
        public int ServiceID { get; set; }
        public string ServiceName { get; set; }
        public int ServiceAccID { get; set; }

        public override string ToString()
        {
            return ServiceName;
        }
    }



}
