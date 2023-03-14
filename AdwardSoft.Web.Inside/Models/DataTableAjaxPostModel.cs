using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace AdwardSoft.Web.Inside.Models
{
    public class DataTableAjaxPostModel
    {
        public DataTableAjaxPostModel()
        {
            AdvancedSearch = new AdvancedSearch();
        }

        public int Draw { get; set; }
        public int Start { get; set; }
        public int Length { get; set; }
        public List<Column> columns { get; set; }
        public Search Search { get; set; }
        public List<Order> Order { get; set; }
        public AdvancedSearch AdvancedSearch { get; set; }
    }

    public class Column
    {
        public string Data { get; set; }
        public string Name { get; set; }
        public bool Searchable { get; set; }
        public bool Orderable { get; set; }
        public Search Search { get; set; }
    }

    public class Search
    {
        public string Value { get; set; }
        public string Regex { get; set; }
    }

    public class Order
    {
        public int Column { get; set; }
        public string Dir { get; set; }
    }

    public class AdvancedSearch
    {
        public int day { get; set; }
        public int month { get; set; }
        public int year { get; set; }
        DateTime? _fromdate = null;
        public DateTime? FormDate
        {
            get
            {
                if (!string.IsNullOrEmpty(formDateString) || _fromdate != null)
                    return _fromdate == null ? _fromdate = DateTime.ParseExact(formDateString, "dd/MM/yyyy", CultureInfo.InstalledUICulture) : _fromdate;
                return null;
            }
            set { _fromdate = value; }
        }
        DateTime? _todate = null;
        public DateTime? ToDate
        {
            get
            {
                if (!string.IsNullOrEmpty(toDateString) || _todate != null)
                    return _todate == null ? _todate = DateTime.ParseExact(toDateString, "dd/MM/yyyy", CultureInfo.InstalledUICulture) : _todate;
                return null;
            }
            set { _todate = value; }
        }
        public string formDateString { get; set; }
        public string toDateString { get; set; }
        public string timeCheckIn { get; set; }
        public string timeCheckOut { get; set; }
        public string customerName { get; set; }
        public string staff { get; set; }
        public bool isPrint { get; set; }
    }
}
