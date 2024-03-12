using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.ModelView
{
    public class ContractView
    {
        public class ContractViewList
        {
            public ContractViewList()
            {
                
            }
            public required string ProductId { get; set; }
            public required string ProductName { get; set; }
            public int UnitPrice { get; set; }
            public int Quantity { get; set; }
            public int TotalCostOfPoduct { get; set; }
        }
    }
}
