using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace coreLogic.Models.Loans
{
    public class LoanDocumentPageViewModel
    {
        public int loanDocumentTemplatePageId { get; set; }
        public int pageNumber { get; set; }
        public string content { get; set; }
        public IEnumerable<int> placeHolderIds { get; set; }
        public List<string> placeHolders { get; set; }

    }
}
