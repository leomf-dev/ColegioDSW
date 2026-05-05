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
        private readonly SeccionBC _seccionBC;
        public MatriculaController(MatriculaBC matricula, AlumnoBC alumnoBC, SeccionBC seccionBC)
        {
            _matriculaBC = matricula;
            _alumnoBC = alumnoBC;
            _seccionBC = seccionBC;
        }

        public IActionResult Index()
        {
            var lista = _matriculaBC.Listar();
            return View(lista);
        }

        public IActionResult Create()
        {
            ViewBag.Alumnos = new SelectList(_alumnoBC.ListarAlumnos(), "IdAlumno", "Nombre");
            ViewBag.Secciones = new SelectList(_seccionBC.Listar(), "IdSeccion", "CodigoSeccion");
            return View(new Matricula { SeccionesSeleccionadas = new List<int>() });
        }

        [HttpPost]
        public IActionResult Create(Matricula matricula, string[] SeccionesSeleccionadas)
        {
            Console.WriteLine($"=== DEBUG MATRICULA ===");
            Console.WriteLine($"Parámetro SeccionesSeleccionadas array: {SeccionesSeleccionadas?.Length ?? 0}");
            if (SeccionesSeleccionadas != null)
            {
                foreach (var seccion in SeccionesSeleccionadas)
                {
                    Console.WriteLine($"Seccion string: '{seccion}'");
                }
            }

            // Inicializar lista si viene null
            if (matricula.SeccionesSeleccionadas == null)
            {
                matricula.SeccionesSeleccionadas = new List<int>();
            }

            // Convertir array de strings a lista de ints
            if (SeccionesSeleccionadas != null && SeccionesSeleccionadas.Length > 0)
            {
                matricula.SeccionesSeleccionadas = SeccionesSeleccionadas
                    .Where(s => !string.IsNullOrEmpty(s))
                    .Select(s => int.Parse(s))
                    .ToList();
                Console.WriteLine($"Convertidos a int: {matricula.SeccionesSeleccionadas.Count} secciones");
            }

            Console.WriteLine($"IdAlumno: {matricula.IdAlumno}");
            Console.WriteLine($"FechaMatricula: {matricula.FechaMatricula}");
            Console.WriteLine($"Periodo: '{matricula.Periodo}'");
            Console.WriteLine($"Estado: '{matricula.Estado}'");
            Console.WriteLine($"SeccionesSeleccionadas Count: {matricula.SeccionesSeleccionadas?.Count ?? 0}");
            if (matricula.SeccionesSeleccionadas != null)
            {
                foreach (var seccion in matricula.SeccionesSeleccionadas)
                {
                    Console.WriteLine($"Seccion ID: {seccion}");
                }
            }

            if (!ModelState.IsValid)
            {
                Console.WriteLine("ModelState NO es válido");
                foreach (var error in ModelState.Values.SelectMany(v => v.Errors))
                {
                    Console.WriteLine($"Error: {error.ErrorMessage}");
                }

                // Reconstruccion
                ViewBag.Alumnos = new SelectList(_alumnoBC.ListarAlumnos(), "IdAlumno", "Nombre");
                ViewBag.Secciones = new SelectList(_seccionBC.Listar(), "IdSeccion", "CodigoSeccion");

                return View(matricula);
            }

            try
            {
                Console.WriteLine("Intentando crear matrícula...");
                _matriculaBC.Crear(matricula);
                Console.WriteLine("Matrícula creada exitosamente");
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al crear matrícula: {ex.Message}");
                Console.WriteLine($"Stack trace: {ex.StackTrace}");
                ModelState.AddModelError("", ex.Message);

                // Reconstruccion
                ViewBag.Alumnos = new SelectList(_alumnoBC.ListarAlumnos(), "IdAlumno", "Nombre");
                ViewBag.Secciones = new SelectList(_seccionBC.Listar(), "IdSeccion", "CodigoSeccion");

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
        public IActionResult RetirarSeccion(int idMatricula, int idSeccion)
        {
            try
            {
                _matriculaBC.RetirarSeccion(idMatricula, idSeccion);
                TempData["Success"] = "Sección retirada correctamente";
            }
            catch (Exception ex)
            {
                TempData["Error"] = $"Error al retirar sección: {ex.Message}";
            }
            return RedirectToAction(nameof(Details), new { id = idMatricula });
        }
    }
}
