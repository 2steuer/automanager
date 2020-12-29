using System;
using System.Threading.Tasks;
using automanager.Database;
using automanager.Models.Database;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using automanager.Models;
using automanager.Extensions;

namespace automanager.Controllers
{
    public class VehicleController : Controller
    {
        private CarManagerDatabase _db;
        private ILogger<VehicleController> _log;

        public VehicleController(ILogger<VehicleController> logger, CarManagerDatabase db)
        {
            _db = db;
            _log = logger;
        }

        [HttpGet]
        public async Task<IActionResult> FuelEntryForm(long id)
        {
            var v = await _db.Vehicles.FindAsync(id); // fails if vehicle does not exist

            if (v == null)
            {
                return NotFound();
            }

            ViewBag.VehicleId = id;
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> SaveFuelEntry(GasFillingEntry newEntry)
        {
            var v = await _db.Vehicles.Include(v => v.GasFillingEntries).FirstOrDefaultAsync(v => v.Id == newEntry.VehicleId);

            if (v == null)
            {
                return NotFound();
            }

            newEntry.Date = DateTime.Now;
            
            v.GasFillingEntries.Add(newEntry);
            _db.FillingEntries.Add(newEntry);

            await _db.SaveChangesAsync();

            return RedirectToAction(nameof(DisplayVehicle), new {Id = v.Id});
        }

        [HttpGet]
        public async Task<IActionResult> EditFuelEntry(long id)
        {
            var gfe = await _db.FillingEntries.FindAsync(id);
            ViewBag.VehicleId = gfe.VehicleId;
            return View("FuelEntryForm", gfe);
        }

        [HttpPost]
        public async Task<IActionResult> SaveEditFuelEntry(long id)
        {
            var gfe = await _db.FillingEntries.FindAsync(id);

            await TryUpdateModelAsync(gfe);
            await _db.SaveChangesAsync();
            return RedirectToAction(nameof(DisplayVehicle), new {Id = gfe.VehicleId});
        }

        [HttpGet]
        public async Task<IActionResult> DeleteFillingEntryForm(long id)
        {
            return View(await _db.FillingEntries.FindAsync(id));
        }

        [HttpPost]
        public async Task<IActionResult> DeleteFillingEntry(long id)
        {
            var gfe = await _db.FillingEntries.FindAsync(id);
            var vid = gfe.VehicleId;
            _db.Remove(gfe);
            await _db.SaveChangesAsync();

            return RedirectToAction(nameof(DisplayVehicle), new {id = vid});
        }

        [HttpGet]
        public async Task<IActionResult> DisplayFillingEntry(long id)
        {
            var fe = await _db.FillingEntries.Include(f => f.Vehicle).FirstOrDefaultAsync(f => f.Id == id);

            if (fe == null)
            {
                return NotFound();
            }

            if (fe.Partial)
            {
                return RedirectToAction(nameof(DisplayVehicle), new { Id = fe.VehicleId });
            }
            

            return View(fe.GetStatistics());
        }

        public IActionResult DisplayVehicle(long id)
        {
            return View(_db.Vehicles.Include(v => v.GasFillingEntries).First(v => v.Id == id));
        }
    }
}