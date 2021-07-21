using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;

namespace coreERP
{
    [DataObject]
    public class RelationTypeDataSource
    {
        [DataObjectMethod(DataObjectMethodType.Select)]
        public IEnumerable<relationType> Get()
        {
            return new relationType[]
            {
                new relationType{relationTypeName = "Son"},
                new relationType{relationTypeName = "Daughter"},
                new relationType{relationTypeName = "Spouse"},
                new relationType{relationTypeName = "Parent"},
                new relationType{relationTypeName = "Grand-Parent"},
                new relationType{relationTypeName = "Other"},
            };
        }
    }

    public class relationType
    {
        public string relationTypeName { get; set; }
    }
}