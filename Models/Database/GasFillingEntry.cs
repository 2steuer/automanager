using System;
using System.ComponentModel.DataAnnotations;

namespace automanager.Models.Database
{
    public class GasFillingEntry
    {
        public long Id { get; set; }
        public DateTime Date { get; set; }
        public long Kilometers { get; set; }
        public decimal Liters { get; set; }
        public decimal Price { get; set; }

        public bool Partial { get; set; } = false;

        public long VehicleId { get; set; }
        public virtual Vehicle Vehicle { get; set; }
    }
}