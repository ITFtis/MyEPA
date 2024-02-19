using System;

namespace MyEPA.Models
{
    public class ContactManualDateModel
    {
        [PrimaryKey]
        public int Id { get; set; }
        public DateTime Date { get; set; }
    }
}