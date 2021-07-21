using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;
using coreLogic;

namespace coreERP
{
    [DataObject]
    public class IdTypeDataSource
    {
        [DataObjectMethod(DataObjectMethodType.Select)]
        public IEnumerable<idNoType> Get()
        {
            using (var le = new coreLoansEntities())
            {
                return le.idNoTypes.ToList();
            }
        }
    }
}