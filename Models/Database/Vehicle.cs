using System.Collections.Generic;

namespace automanager.Models.Database
{
    public class Vehicle
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public string LicensePlate { get; set; }
        public string FuelName { get; set; }
        public long StartingKilometers { get; set; }

        public bool Visible { get; set; } = true;

        public virtual ICollection<GasFillingEntry> GasFillingEntries { get; set; }
    }
}
