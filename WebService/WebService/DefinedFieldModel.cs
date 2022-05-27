using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebService
{
    public class DefinedFieldModel
    {
        public string ERR { get; set; }
        public string SMS { get; set; }
        public string ERRCode { get; set; }
        public List<DefinedFieldList> DataList { get; set; }
    }
    public class DefinedFieldList
    {
        public string LOOKUPID { get; set; }
        public string criteriaValue { get; set; }
        public string DESCRIPTION { get; set; }
        public string OrderBy { get; set; }
        public string Parent { get; set; }
        public string ParentCriteriaValue { get; set; }
    }
}