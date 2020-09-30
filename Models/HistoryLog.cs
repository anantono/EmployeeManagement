using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EmployeeManagement.Models
{
    public class HistoryLog
    {
        public int Id { get; set; }

        [Display(Name = "Event Description")]
        public string EventDescription { get; set; }

        [Display(Name = "Event Date & Time")]
        [DataType(DataType.DateTime)]
        public DateTime EventDateTime { get; set; }
    }
}
