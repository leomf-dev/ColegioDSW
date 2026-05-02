using Colegio.BL.BC;
using Colegio.BL.BE;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Colegio.PL.WebApp.Controllers
{
    public class MatriculaController : Controller
    {
        private readonly MatriculaBC _matriculaBC;
        private readonly AlumnoBC _alumnoBC;
        private readonly CursoBC _cursoBC;
        public MatriculaController(MatriculaBC matricula, AlumnoBC alumnoBC, CursoBC cursoBC)
        {
            _matriculaBC = matricula;
            _alumnoBC = alumnoBC;
            _cursoBC = cursoBC;
        }

        public IActionResult Index()
        {
            var lista = _matriculaBC.Listar();
            return View(lista);
        }

        public IActionResult Create()
        {
            ViewBag.Alumnos = new SelectList(_alumnoBC.ListarAlumnos(), "IdAlumno", "Nombre");
            ViewBag.Cursos = new SelectList(_cursoBC.Listar(), "IdCurso", "Nombre");
            return View(new Matricula { CursosSeleccionados = new List<int>() });
        }

        [HttpPost]
        public IActionResult Create(Matricula matricula)
        {
            // Inicializar lista si viene null
            if (matricula.CursosSeleccionados == null)
            {
                matricula.CursosSeleccionados = new List<int>();
            }

            if (!ModelState.IsValid)
            {
                // Reconstruccion
                ViewBag.Alumnos = new SelectList(_alumnoBC.ListarAlumnos(), "IdAlumno", "Nombre");
                ViewBag.Cursos = new SelectList(_cursoBC.Listar(), "IdCurso", "Nombre");

                return View(matricula);
            }

            try
            {
                _matriculaBC.Crear(matricula);
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);

                // Reconstruccion
                ViewBag.Alumnos = new SelectList(_alumnoBC.ListarAlumnos(), "IdAlumno", "Nombre");
                ViewBag.Cursos = new SelectList(_cursoBC.Listar(), "IdCurso", "Nombre");

                return View(matricula);
            }
        }

        public IActionResult Details(int id)
        {
            var matricula = _matriculaBC.Consultar(id);
            if (matricula == null)
                return NotFound();
            return View(matricula);
        }

        [HttpPost]
        public IActionResult Anular(int id, string observacion)
        {
            try
            {
                _matriculaBC.AnularMatricula(id, observacion);
                TempData["Success"] = "Matrícula anulada correctamente";
            }
            catch (Exception ex)
            {
                TempData["Error"] = $"Error al anular matrícula: {ex.Message}";
            }
            return RedirectToAction(nameof(Details), new { id });
        }

        [HttpPost]
        public IActionResult Retirar(int id, string observacion)
        {
            try
            {
                _matriculaBC.RetirarMatricula(id, observacion);
                TempData["Success"] = "Matrícula retirada correctamente";
            }
            catch (Exception ex)
            {
                TempData["Error"] = $"Error al retirar matrícula: {ex.Message}";
            }
            return RedirectToAction(nameof(Details), new { id });
        }

        [HttpPost]
        public IActionResult Cancelar(int id, string observacion)
        {
            try
            {
                _matriculaBC.CancelarMatricula(id, observacion);
                TempData["Success"] = "Matrícula cancelada correctamente";
            }
            catch (Exception ex)
            {
                TempData["Error"] = $"Error al cancelar matrícula: {ex.Message}";
            }
            return RedirectToAction(nameof(Details), new { id });
        }

        [HttpPost]
        public IActionResult RetirarCurso(int idMatricula, int idCurso)
        {
            try
            {
                _matriculaBC.RetirarCurso(idMatricula, idCurso);
                TempData["Success"] = "Curso retirado correctamente";
            }
            catch (Exception ex)
            {
                TempData["Error"] = $"Error al retirar curso: {ex.Message}";
            }
            return RedirectToAction(nameof(Details), new { id = idMatricula });
        }
    }
}
