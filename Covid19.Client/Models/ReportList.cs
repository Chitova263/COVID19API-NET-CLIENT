using System.Collections.Generic;

namespace Covid19.Client.Models
{
    public class ReportList: ResponseBase
    {
        public IEnumerable<Report> Reports { get; set; }

        public ReportList()
        {
            Reports = new List<Report>();
        }
    }
}
