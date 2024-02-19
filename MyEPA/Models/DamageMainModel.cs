using System;

namespace MyEPA.Models
{
    public class DamageMainModel
    {
        [AutoKey]
        public int Id { get; set; }

        public int DiasterId { get; set; }

        public int CityId { get; set; }

        public bool IsDone { get; set; }

        public DateTime DoneDate { get; set; }

        public DateTime UpdateDate { get; set; }

    }
}