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
            //ViewBag.Alumnos = _alumnoBC.ListarAlumnos().Select(a => new { a.IdAlumno, NombreCompleto = a.Nombre + " " + a.Apellido }).ToList();
            /*ViewBag.Cursos = _cursoBC.Listar()
                             .Select(c => new SelectListItem
                             {
                                 Value = c.IdCurso.ToString(),
                                 Text = c.Nombre
                             })
                             .ToList();*/
            ViewBag.Cursos = new SelectList(_cursoBC.Listar(), "IdCurso", "Nombre");
            return View();
        }

        [HttpPost]
        public IActionResult Create(Matricula matricula)
        {
            if (!ModelState.IsValid)
            {
                // Reconstruccion
                ViewBag.Alumnos = new SelectList(_alumnoBC.ListarAlumnos(), "IdAlumno", "Nombre");

                ViewBag.Cursos = _cursoBC.Listar()
                                         .Select(c => new SelectListItem
                                         {
                                             Value = c.IdCurso.ToString(),
                                             Text = c.Nombre
                                         })
                                         .ToList();

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

                ViewBag.Cursos = _cursoBC.Listar()
                                         .Select(c => new SelectListItem
                                         {
                                             Value = c.IdCurso.ToString(),
                                             Text = c.Nombre
                                         })
                                         .ToList();

                return View(matricula);
            }
        }
    }
}
