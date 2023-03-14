using DocumentFormat.OpenXml.Wordprocessing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AdwardSoft.Web.Inside.Helper
{
    public static class DateHelper
    {
        public static DateTime StringToDate(string dateStr, string suffix = "/")
        {         
            string[] dateSpl = dateStr.Split(suffix);
            DateTime date = DateTime.Parse(dateSpl[2] + "-" + dateSpl[1] + "-" + dateSpl[0]);

            return date;
        }

        public static (DateTime dateFrom, DateTime dateTo) StringToDateRange(string dateStr)
        {
            string[] datespl = dateStr.Split(" ");
            string[] datesplForm = datespl[0].Split("/");
            string[] datesplTo = datespl[2].Split("/");

            DateTime dateFrom = DateTime.Parse(datesplForm[2] + "-" + datesplForm[1] + "-" + datesplForm[0]);
            DateTime dateTo = DateTime.Parse(datesplTo[2] + "-" + datesplTo[1] + "-" + datesplTo[0]);

            return (dateFrom, dateTo);
        }

        public static String DateToString(DateTime date)
        {
            return $"{date.Year}-{date.Month}-{date.Day}";
        }
    }
}
