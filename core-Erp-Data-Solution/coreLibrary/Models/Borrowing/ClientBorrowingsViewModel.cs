using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace coreLogic.Models.Borrowing
{
    public class ClientBorrowingsViewModel
    {
        public int clientId { get; set; }
        public string clientName { get; set; }
        public IEnumerable<borrowing> brws { get; set; }
        public List<BorrowingAccountViewModel> clientBorrowings { get; set; }        
    }
}
