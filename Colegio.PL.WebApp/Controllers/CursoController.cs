using Colegio.BL.BC;
using Colegio.BL.BE;
using Microsoft.AspNetCore.Mvc;

namespace Colegio.PL.WebApp.Controllers
{
    public class CursoController : Controller
    {
        private readonly CursoBC _cursoBC;
        public CursoController(CursoBC cursoBC)
        {
            _cursoBC = cursoBC;
        }

        public IActionResult Index()
        {
            var lista = _cursoBC.Listar();
            return View(lista);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(Curso curso)
        {
            if (!ModelState.IsValid)
                return View(curso);

            try
            {
                _cursoBC.Crear(curso);
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return View(curso);
            }
        }

        public IActionResult Edit(int id)
        {
            var c = _cursoBC.Obtener(id);
            if (c == null) return NotFound();
            return View(c);
        }

        [HttpPost]
        public IActionResult Edit(Curso curso)
        {
            if (!ModelState.IsValid) return View(curso);
            _cursoBC.Actualizar(curso);
            return RedirectToAction(nameof(Index));
        }

        public IActionResult Details(int id)
        {
            var c = _cursoBC.Obtener(id);
            if (c == null) return NotFound();
            return View(c);
        }

        public IActionResult Delete(int id)
        {
            var c = _cursoBC.Obtener(id);
            if (c == null) return NotFound();
            return View(c);
        }

        [HttpPost/*, ActionName("DeleteConfirmed")*/]
        public IActionResult DeleteConfirmed(int id)
        {
            _cursoBC.Eliminar(id);
            return RedirectToAction(nameof(Index));
        }
    }
}
