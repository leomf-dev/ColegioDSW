using Colegio.BL.BC;
using Colegio.BL.BE;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace Colegio.PL.WebApp.Controllers
{
    public class CursoController : Controller
    {
        private readonly CursoBC _cursoBC;
        private readonly ILogger<CursoController> _logger;

        public CursoController(CursoBC cursoBC, ILogger<CursoController> logger)
        {
            _cursoBC = cursoBC;
            _logger = logger;
        }

        public IActionResult Index()
        {
            _logger.LogInformation("=== GET /Curso/Index ===");
            try
            {
                var lista = _cursoBC.Listar();
                _logger.LogInformation($"Total cursos listados: {lista.Count}");
                return View(lista);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al listar cursos");
                throw;
            }
        }

        public IActionResult Create()
        {
            _logger.LogInformation("=== GET /Curso/Create ===");
            return View();
        }

        [HttpPost]
        public IActionResult Create(Curso curso)
        {
            _logger.LogInformation("=== POST /Curso/Create ===");
            _logger.LogInformation($"Datos recibidos:");
            _logger.LogInformation($"  - IdCurso: {curso.IdCurso}");
            _logger.LogInformation($"  - Codigo: {curso.Codigo}");
            _logger.LogInformation($"  - Nombre: {curso.Nombre}");
            _logger.LogInformation($"  - Creditos: {curso.Creditos}");
            _logger.LogInformation($"  - HorasSemanales: {curso.HorasSemanales}");
            _logger.LogInformation($"  - Estado: {curso.Estado}");

            if (!ModelState.IsValid)
            {
                _logger.LogWarning("ModelState inválido");
                var errors = ModelState.Values.SelectMany(v => v.Errors);
                foreach (var error in errors)
                {
                    _logger.LogWarning($"  Error: {error.ErrorMessage}");
                }
                return View(curso);
            }

            try
            {
                _logger.LogInformation("Iniciando creación de curso...");
                _cursoBC.Crear(curso);
                _logger.LogInformation("Curso creado exitosamente");
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al crear curso");
                ModelState.AddModelError("", ex.Message);
                return View(curso);
            }
        }

        public IActionResult Edit(int id)
        {
            _logger.LogInformation($"=== GET /Curso/Edit/{id} ===");
            try
            {
                var c = _cursoBC.Obtener(id);
                if (c == null)
                {
                    _logger.LogWarning($"Curso con id {id} no encontrado");
                    return NotFound();
                }
                _logger.LogInformation($"Curso encontrado: {c.Nombre}");
                return View(c);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error al obtener curso {id}");
                throw;
            }
        }

        [HttpPost]
        public IActionResult Edit(Curso curso)
        {
            _logger.LogInformation($"=== POST /Curso/Edit/{curso.IdCurso} ===");
            _logger.LogInformation($"Datos recibidos: Nombre='{curso.Nombre}', Creditos={curso.Creditos}");
            
            if (!ModelState.IsValid)
            {
                _logger.LogWarning("ModelState inválido");
                return View(curso);
            }

            try
            {
                _logger.LogInformation("Actualizando curso...");
                _cursoBC.Actualizar(curso);
                _logger.LogInformation("Curso actualizado exitosamente");
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al actualizar curso");
                throw;
            }
        }

        public IActionResult Details(int id)
        {
            _logger.LogInformation($"=== GET /Curso/Details/{id} ===");
            try
            {
                var c = _cursoBC.Obtener(id);
                if (c == null)
                {
                    _logger.LogWarning($"Curso con id {id} no encontrado");
                    return NotFound();
                }
                _logger.LogInformation($"Mostrando detalles de: {c.Nombre}");
                return View(c);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error al obtener detalles del curso {id}");
                throw;
            }
        }

        public IActionResult Delete(int id)
        {
            _logger.LogInformation($"=== GET /Curso/Delete/{id} ===");
            try
            {
                var c = _cursoBC.Obtener(id);
                if (c == null)
                {
                    _logger.LogWarning($"Curso con id {id} no encontrado");
                    return NotFound();
                }
                _logger.LogInformation($"Preparando eliminación de: {c.Nombre}");
                return View(c);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error al obtener curso {id} para eliminar");
                throw;
            }
        }

        [HttpPost/*, ActionName("DeleteConfirmed")*/]
        public IActionResult DeleteConfirmed(int id)
        {
            _logger.LogInformation($"=== POST /Curso/DeleteConfirmed/{id} ===");
            try
            {
                _logger.LogInformation("Eliminando curso...");
                _cursoBC.Eliminar(id);
                _logger.LogInformation("Curso eliminado exitosamente");
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error al eliminar curso {id}");
                throw;
            }
        }
    }
}
