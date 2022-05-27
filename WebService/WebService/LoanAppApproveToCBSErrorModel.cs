using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml.Serialization;

namespace LoanAppApproveToCBSError
{
    [XmlRoot(ElementName = "Status")]
    public class Status
    {
        [XmlElement(ElementName = "transactionId")]
        public string TransactionId { get; set; }
        [XmlElement(ElementName = "messageId")]
        public string MessageId { get; set; }
        [XmlElement(ElementName = "successIndicator")]
        public string SuccessIndicator { get; set; }
        [XmlElement(ElementName = "application")]
        public string Application { get; set; }
        [XmlElement(ElementName = "messages")]
        public List<string> Messages { get; set; }
    }
    

}