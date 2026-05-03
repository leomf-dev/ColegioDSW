using Colegio.BL.BC;
using Colegio.BL.BE;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Colegio.PL.WebApp.Controllers
{
    public class HorarioController : Controller
    {
        private readonly HorarioBC _horarioBC;
        private readonly SeccionBC _seccionBC;
        public HorarioController(HorarioBC horarioBC, SeccionBC seccionBC)
        {
            _horarioBC = horarioBC;
            _seccionBC = seccionBC;
        }

        public IActionResult Index()
        {
            var lista = _horarioBC.Listar();
            return View(lista);
        }

        public IActionResult Create()
        {
            ViewBag.Secciones = new SelectList(_seccionBC.Listar(), "IdSeccion", "CodigoSeccion");
            ViewBag.Dias = new SelectList(new[]
            {
                new { Value = "Lunes", Text = "Lunes" },
                new { Value = "Martes", Text = "Martes" },
                new { Value = "Miércoles", Text = "Miércoles" },
                new { Value = "Jueves", Text = "Jueves" },
                new { Value = "Viernes", Text = "Viernes" },
                new { Value = "Sábado", Text = "Sábado" },
                new { Value = "Domingo", Text = "Domingo" }
            }, "Value", "Text");
            return View();
        }

        [HttpPost]
        public IActionResult Create(HorarioBE horario)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Secciones = new SelectList(_seccionBC.Listar(), "IdSeccion", "CodigoSeccion");
                ViewBag.Dias = new SelectList(new[]
                {
                    new { Value = "Lunes", Text = "Lunes" },
                    new { Value = "Martes", Text = "Martes" },
                    new { Value = "Miércoles", Text = "Miércoles" },
                    new { Value = "Jueves", Text = "Jueves" },
                    new { Value = "Viernes", Text = "Viernes" },
                    new { Value = "Sábado", Text = "Sábado" },
                    new { Value = "Domingo", Text = "Domingo" }
                }, "Value", "Text");
                return View(horario);
            }

            try
            {
                _horarioBC.Crear(horario);
                TempData["Success"] = "Horario creado exitosamente";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                ViewBag.Secciones = new SelectList(_seccionBC.Listar(), "IdSeccion", "CodigoSeccion");
                ViewBag.Dias = new SelectList(new[]
                {
                    new { Value = "Lunes", Text = "Lunes" },
                    new { Value = "Martes", Text = "Martes" },
                    new { Value = "Miércoles", Text = "Miércoles" },
                    new { Value = "Jueves", Text = "Jueves" },
                    new { Value = "Viernes", Text = "Viernes" },
                    new { Value = "Sábado", Text = "Sábado" },
                    new { Value = "Domingo", Text = "Domingo" }
                }, "Value", "Text");
                return View(horario);
            }
        }

        public IActionResult Edit(int id)
        {
            var horario = _horarioBC.Consultar(id);
            if (horario == null)
                return NotFound();

            ViewBag.Secciones = new SelectList(_seccionBC.Listar(), "IdSeccion", "CodigoSeccion");
            ViewBag.Dias = new SelectList(new[]
            {
                new { Value = "Lunes", Text = "Lunes" },
                new { Value = "Martes", Text = "Martes" },
                new { Value = "Miércoles", Text = "Miércoles" },
                new { Value = "Jueves", Text = "Jueves" },
                new { Value = "Viernes", Text = "Viernes" },
                new { Value = "Sábado", Text = "Sábado" },
                new { Value = "Domingo", Text = "Domingo" }
            }, "Value", "Text");
            return View(horario);
        }

        [HttpPost]
        public IActionResult Edit(HorarioBE horario)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Secciones = new SelectList(_seccionBC.Listar(), "IdSeccion", "CodigoSeccion");
                ViewBag.Dias = new SelectList(new[]
                {
                    new { Value = "Lunes", Text = "Lunes" },
                    new { Value = "Martes", Text = "Martes" },
                    new { Value = "Miércoles", Text = "Miércoles" },
                    new { Value = "Jueves", Text = "Jueves" },
                    new { Value = "Viernes", Text = "Viernes" },
                    new { Value = "Sábado", Text = "Sábado" },
                    new { Value = "Domingo", Text = "Domingo" }
                }, "Value", "Text");
                return View(horario);
            }

            try
            {
                _horarioBC.Actualizar(horario);
                TempData["Success"] = "Horario actualizado exitosamente";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                ViewBag.Secciones = new SelectList(_seccionBC.Listar(), "IdSeccion", "CodigoSeccion");
                ViewBag.Dias = new SelectList(new[]
                {
                    new { Value = "Lunes", Text = "Lunes" },
                    new { Value = "Martes", Text = "Martes" },
                    new { Value = "Miércoles", Text = "Miércoles" },
                    new { Value = "Jueves", Text = "Jueves" },
                    new { Value = "Viernes", Text = "Viernes" },
                    new { Value = "Sábado", Text = "Sábado" },
                    new { Value = "Domingo", Text = "Domingo" }
                }, "Value", "Text");
                return View(horario);
            }
        }

        public IActionResult Delete(int id)
        {
            var horario = _horarioBC.Consultar(id);
            if (horario == null)
                return NotFound();
            return View(horario);
        }

        [HttpPost, ActionName("Delete")]
        public IActionResult DeleteConfirmed(int id)
        {
            try
            {
                _horarioBC.Eliminar(id);
                TempData["Success"] = "Horario eliminado exitosamente";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                TempData["Error"] = $"Error al eliminar horario: {ex.Message}";
                return RedirectToAction(nameof(Index));
            }
        }

        public IActionResult Details(int id)
        {
            var horario = _horarioBC.Consultar(id);
            if (horario == null)
                return NotFound();
            return View(horario);
        }
    }
}