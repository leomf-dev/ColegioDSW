using Colegio.BL.BC;
using Colegio.BL.BE;
using Microsoft.AspNetCore.Mvc;

namespace Colegio.PL.WebApp.Controllers
{
    public class AlumnoController : Controller
    {
        private readonly AlumnoBC _alumnoBC;
        public AlumnoController(AlumnoBC alumnoBC)
        {
            _alumnoBC = alumnoBC;
        }

        public IActionResult Index()
        {
            var lista = _alumnoBC.ListarAlumnos();
            return View(lista);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(AlumnoBE alumno)
        {
            if (!ModelState.IsValid)
                return View(alumno);

            try
            {
                _alumnoBC.CrearAlumno(alumno);
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return View(alumno);
            }
        }

        public IActionResult Edit(int id)
        {
            var a = _alumnoBC.Obtener(id);
            if (a == null) return NotFound();
            return View(a);
        }

        [HttpPost]
        public IActionResult Edit(AlumnoBE alumno)
        {
            if (!ModelState.IsValid) return View(alumno);
            _alumnoBC.Actualizar(alumno);
            return RedirectToAction(nameof(Index));
        }

        public IActionResult Details(int id)
        {
            var a = _alumnoBC.Obtener(id);
            if (a == null) return NotFound();
            return View(a);
        }

        public IActionResult Delete(int id)
        {
            var a = _alumnoBC.Obtener(id);
            if(a == null) return NotFound();
            return View(a);
        }

        [HttpPost/*, ActionName("DeleteConfirmed")*/]
        public IActionResult DeleteConfirmed(int id)
        {
            _alumnoBC.Eliminar(id);
            return RedirectToAction(nameof(Index));
        }
    }
}
