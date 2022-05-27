using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Xml;

namespace WebService.Services
{
    public class XMLNoteListToDataTable
    {
        public static DataTable ConvertXmlNodeListToDataTable(XmlNodeList xnl)
        {
            try
            {
                var dt = new DataTable();
                int TempColumn = 0;
                foreach (XmlNode node in xnl.Item(0).ChildNodes)
                {
                    TempColumn++;
                    DataColumn dc = new DataColumn(node.Name, System.Type.GetType("System.String"));
                    if (dt.Columns.Contains(node.Name))
                    {
                        dt.Columns.Add(dc.ColumnName = dc.ColumnName + TempColumn.ToString());
                    }
                    else
                    {
                        dt.Columns.Add(dc);
                    }
                }
                int ColumnsCount = dt.Columns.Count;
                for (int i = 0; i < xnl.Count; i++)
                {
                    DataRow dr = dt.NewRow();
                    for (int j = 0; j < ColumnsCount; j++)
                    {
                        dr[j] = xnl.Item(i).ChildNodes[j].InnerText;
                    }
                    dt.Rows.Add(dr);
                }
                return dt;
            }
            catch (Exception)
            {
                return null;
            }
        }
    }
}