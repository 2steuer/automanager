using System;
using System.Linq;
using automanager.Models;
using automanager.Models.Database;

namespace automanager.Extensions
{
    public static class FillingEntryExtensions
    {
        public static FuelStatistics GetStatistics(this GasFillingEntry entry)
        {
            if (entry.Partial)
            {
                throw new InvalidOperationException("Can only get statistics for non-partial filling entries.");
            }

            var vehicle = entry.Vehicle;

            var lastFullEntry = vehicle.GasFillingEntries
                    .Where(gfe => gfe.Kilometers < entry.Kilometers)
                    .Where(gfe => !gfe.Partial)
                    .OrderByDescending(gfe => gfe.Kilometers)
                    .FirstOrDefault();

            long lastFullKm = vehicle.StartingKilometers;

            if (lastFullEntry != null)
            {
                lastFullKm = lastFullEntry.Kilometers;
            }

            var includedEntries = vehicle.GasFillingEntries
                .Where(gfe => (gfe.Kilometers > lastFullKm) && (gfe.Kilometers < entry.Kilometers))
                .Where(gfe => gfe.Partial)
                .OrderBy(gfe => gfe.Kilometers);

            var stat = new FuelStatistics(lastFullKm, entry, includedEntries);

            return stat;
        }
    }
}