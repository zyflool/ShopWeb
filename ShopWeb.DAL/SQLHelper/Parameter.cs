using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShopWeb.DAL
{
    public class Parameter
    {
        public string Name { get; set; }
        public object Value { get; set; }

        public Parameter(string name, object value)
        {
            this.Name = name;
            this.Value = value;
        }
    }
}
