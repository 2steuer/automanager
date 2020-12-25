using System.Collections.Generic;
using automanager.Models.Database;

namespace automanager.Models
{
    public class FuelStatistics
    {
        public GasFillingEntry BaseEntry { get; set; }

        public IEnumerable<GasFillingEntry> IncludedEntries { get; set; }

        public long DistanceKilometers { get; set; }

        public decimal Price { get; set; }

        public decimal Liters { get; set; }

        public decimal LiterPerKm => Liters / DistanceKilometers;

        public decimal LiterPer100Km => LiterPerKm * 100;

        public decimal PricePerLiter => Price / Liters;

        public decimal PricePerKm => Price / DistanceKilometers;

        public FuelStatistics(long lastKilometers, GasFillingEntry baseEntry, IEnumerable<GasFillingEntry> includedEntries)
        {
            BaseEntry = baseEntry;
            IncludedEntries = includedEntries;

            DistanceKilometers = baseEntry.Kilometers - lastKilometers;
            Price = baseEntry.Price;
            Liters = baseEntry.Liters;

            foreach (var e in includedEntries)
            {
                Price += e.Price;
                Liters += e.Liters;
            }
        }
    }
}