using System;
using System.Collections.Generic;
using System.Text;

namespace AdwardSoft.Web.Inside.Models
{
    public class TimeSheetViewModel
    {
        public long UserId { get; set; }
        public DateTime Date { get; set; }
        public short Type { get; set; }
        public string Reason { get; set; }
        public string StartTime { get; set; }
        public string EndTime { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}
