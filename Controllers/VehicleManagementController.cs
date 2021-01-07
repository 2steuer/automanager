using System.Threading.Tasks;
using automanager.Database;
using automanager.Models.Database;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Linq;
using Microsoft.AspNetCore.Authorization;

namespace automanager.Controllers
{
    [Authorize]
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
            return View(_db.Vehicles.OrderBy(v => v.DisplayOrder));
        }

        [HttpGet]
        public async Task<IActionResult> ToggleVisibility(long id)
        {
            var v = await _db.Vehicles.FindAsync(id);
            v.Visible = !v.Visible;
            await _db.SaveChangesAsync();

            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<IActionResult> MoveUp(long id)
        {
            var v1 = await _db.Vehicles.FindAsync(id);

            var minDisplayOrder = _db.Vehicles.Min(v => v.DisplayOrder);

            if (v1.DisplayOrder > minDisplayOrder)
            {
                var v2 = _db.Vehicles.Where(v => v.DisplayOrder < v1.DisplayOrder).OrderByDescending(v => v.DisplayOrder).First();
                var tmpDo = v2.DisplayOrder;
                v2.DisplayOrder = v1.DisplayOrder;
                v1.DisplayOrder = tmpDo;

                await _db.SaveChangesAsync();
            }            

            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<IActionResult> MoveDown(long id)
        {
            var v1 = await _db.Vehicles.FindAsync(id);

            var maxDisplayOrder = _db.Vehicles.Max(v => v.DisplayOrder);

            if (v1.DisplayOrder < maxDisplayOrder)
            {
                var v2 = _db.Vehicles.Where(v => v.DisplayOrder > v1.DisplayOrder).OrderBy(v => v.DisplayOrder).First();
                var tmpDo = v2.DisplayOrder;
                v2.DisplayOrder = v1.DisplayOrder;
                v1.DisplayOrder = tmpDo;

                await _db.SaveChangesAsync();
            }

            return RedirectToAction("Index");
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
            var newOrder = (_db.Vehicles.Count() == 0) ? 0 : (_db.Vehicles.Max(v => v.DisplayOrder) + 1);
            newVehicle.DisplayOrder = newOrder;

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

            var displayOrder = v.DisplayOrder;

            _db.Vehicles.Remove(v);
            foreach (var vehicle in _db.Vehicles.Where(cv => cv.DisplayOrder > displayOrder))
            {
                vehicle.DisplayOrder--;
            }

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