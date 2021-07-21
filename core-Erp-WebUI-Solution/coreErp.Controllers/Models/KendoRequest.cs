using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace coreERP
{
    public class KendoRequest
    {
        public int take { get; set; }
        public int skip { get; set; }
        public int page { get; set; }
        public List<KendoSort> sort { get; set; }

        public KendoFilters filter { get; set; }
    }

    public class KendoSort
    {
        public string field { get; set; }
        public string dir { get; set; }
    }

    public class KendoFilter
    {
        public string Operator { get; set; }
        public string Field { get; set; }
        public string Value { get; set; }
    }

    public class KendoFilters
    {
        public List<KendoFilter> filters { get; set; }
        public string logic { get; set; }
    }

    public class KendoResponse
    {

        public Array Data { get; set; }
        public int Count { get; set; }
        public string Errors { get; set; }

        public KendoResponse(Array data, int count)
        {
            this.Data = data;
            this.Count = count;
        }

        public KendoResponse(string errors)
        {
            this.Errors = errors;
        }

        public KendoResponse()
        {

        }
    }
}