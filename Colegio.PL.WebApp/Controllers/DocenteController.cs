using Colegio.BL.BC;
using Colegio.BL.BE;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Colegio.PL.WebApp.Controllers
{
    public class DocenteController : Controller
    {
        private readonly DocenteBC _docenteBC;
        public DocenteController(DocenteBC docenteBC)
        {
            _docenteBC = docenteBC;
        }

        public IActionResult Index()
        {
            var lista = _docenteBC.Listar();
            return View(lista);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(DocenteBE docente)
        {
            if (!ModelState.IsValid)
            {
                return View(docente);
            }

            try
            {
                _docenteBC.Crear(docente);
                TempData["Success"] = "Docente creado exitosamente";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return View(docente);
            }
        }

        public IActionResult Edit(int id)
        {
            var docente = _docenteBC.Consultar(id);
            if (docente == null)
                return NotFound();
            return View(docente);
        }

        [HttpPost]
        public IActionResult Edit(DocenteBE docente)
        {
            if (!ModelState.IsValid)
            {
                return View(docente);
            }

            try
            {
                _docenteBC.Actualizar(docente);
                TempData["Success"] = "Docente actualizado exitosamente";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return View(docente);
            }
        }

        public IActionResult Delete(int id)
        {
            var docente = _docenteBC.Consultar(id);
            if (docente == null)
                return NotFound();
            return View(docente);
        }

        [HttpPost, ActionName("Delete")]
        public IActionResult DeleteConfirmed(int id)
        {
            try
            {
                _docenteBC.Eliminar(id);
                TempData["Success"] = "Docente eliminado exitosamente";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                TempData["Error"] = $"Error al eliminar docente: {ex.Message}";
                return RedirectToAction(nameof(Index));
            }
        }

        public IActionResult Details(int id)
        {
            var docente = _docenteBC.Consultar(id);
            if (docente == null)
                return NotFound();
            return View(docente);
        }
    }
}