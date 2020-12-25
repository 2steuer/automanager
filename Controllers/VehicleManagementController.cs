using System.Threading.Tasks;
using automanager.Database;
using automanager.Models.Database;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace automanager.Controllers
{
    public class VehicleManagementController : Controller
    {
        private readonly CarManagerDatabase _db;
        private readonly ILogger<VehicleManagementController> _log;

        public VehicleManagementController(ILogger<VehicleManagementController> logger, CarManagerDatabase db)
        {
            _db = db;
            _log = logger;
        }

        [HttpGet]
        public IActionResult Index()
        {
            return View(_db.Vehicles);
        }

        [HttpGet]
        public IActionResult Form(long? id)
        {
            if (id != null)
            {
                return View(_db.Vehicles.Find(id));
            }
            else
            {
                return View(null);
            }            
        }

        [HttpPost]
        public IActionResult Create(Vehicle newVehicle)
        {
            _db.Vehicles.Add(newVehicle);
            _db.SaveChanges();
            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> Update(long id)
        {
            var v = _db.Vehicles.Find(id);
            
            if (await TryUpdateModelAsync<Vehicle>(v))
            {
                await _db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            else
            {
                return RedirectToAction("Form", id);
            }

            
        }

        [HttpPost]
        public IActionResult Delete(long id)
        {
            var v = _db.Vehicles.Find(id);
            _db.Vehicles.Remove(v);
            _db.SaveChanges();

            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public IActionResult DeleteForm(long id)
        {
            return View(_db.Vehicles.Find(id));
        }
    }
}