using Colegio.BL.BC;
using Colegio.BL.BE;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Colegio.PL.WebApp.Controllers
{
    public class SeccionController : Controller
    {
        private readonly SeccionBC _seccionBC;
        private readonly CursoBC _cursoBC;
        private readonly DocenteBC _docenteBC;
        public SeccionController(SeccionBC seccionBC, CursoBC cursoBC, DocenteBC docenteBC)
        {
            _seccionBC = seccionBC;
            _cursoBC = cursoBC;
            _docenteBC = docenteBC;
        }

        public IActionResult Index()
        {
            var lista = _seccionBC.Listar();
            return View(lista);
        }

        public IActionResult Create()
        {
            ViewBag.Cursos = new SelectList(_cursoBC.Listar(), "IdCurso", "Nombre");
            ViewBag.Docentes = new SelectList(_docenteBC.Listar(), "IdDocente", "Nombres");
            return View();
        }

        [HttpPost]
        public IActionResult Create(SeccionBE seccion)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Cursos = new SelectList(_cursoBC.Listar(), "IdCurso", "Nombre");
                ViewBag.Docentes = new SelectList(_docenteBC.Listar(), "IdDocente", "Nombres");
                return View(seccion);
            }

            try
            {
                _seccionBC.Crear(seccion);
                TempData["Success"] = "Sección creada exitosamente";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                ViewBag.Cursos = new SelectList(_cursoBC.Listar(), "IdCurso", "Nombre");
                ViewBag.Docentes = new SelectList(_docenteBC.Listar(), "IdDocente", "Nombres");
                return View(seccion);
            }
        }

        public IActionResult Edit(int id)
        {
            var seccion = _seccionBC.Consultar(id);
            if (seccion == null)
                return NotFound();

            ViewBag.Cursos = new SelectList(_cursoBC.Listar(), "IdCurso", "Nombre");
            ViewBag.Docentes = new SelectList(_docenteBC.Listar(), "IdDocente", "Nombres");
            return View(seccion);
        }

        [HttpPost]
        public IActionResult Edit(SeccionBE seccion)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Cursos = new SelectList(_cursoBC.Listar(), "IdCurso", "Nombre");
                ViewBag.Docentes = new SelectList(_docenteBC.Listar(), "IdDocente", "Nombres");
                return View(seccion);
            }

            try
            {
                _seccionBC.Actualizar(seccion);
                TempData["Success"] = "Sección actualizada exitosamente";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                ViewBag.Cursos = new SelectList(_cursoBC.Listar(), "IdCurso", "Nombre");
                ViewBag.Docentes = new SelectList(_docenteBC.Listar(), "IdDocente", "Nombres");
                return View(seccion);
            }
        }

        public IActionResult Delete(int id)
        {
            var seccion = _seccionBC.Consultar(id);
            if (seccion == null)
                return NotFound();
            return View(seccion);
        }

        [HttpPost, ActionName("Delete")]
        public IActionResult DeleteConfirmed(int id)
        {
            try
            {
                _seccionBC.Eliminar(id);
                TempData["Success"] = "Sección eliminada exitosamente";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                TempData["Error"] = $"Error al eliminar sección: {ex.Message}";
                return RedirectToAction(nameof(Index));
            }
        }

        public IActionResult Details(int id)
        {
            var seccion = _seccionBC.Consultar(id);
            if (seccion == null)
                return NotFound();
            return View(seccion);
        }
    }
}