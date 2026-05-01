using Colegio.BL.BC;
using Colegio.BL.BE;
using ClosedXML.Excel;
using Microsoft.AspNetCore.Mvc;
using System.IO;
using System.Text;

namespace Colegio.PL.WebApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CursoApiController : ControllerBase
    {
        private readonly CursoBC _cursoBC;

        public CursoApiController(CursoBC cursoBC)
        {
            _cursoBC = cursoBC;
        }

        [HttpGet]
        public IActionResult GetCursos()
        {
            var cursos = _cursoBC.Listar();
            return Ok(cursos);
        }

        [HttpGet("{id}")]
        public IActionResult GetCurso(int id)
        {
            var curso = _cursoBC.Obtener(id);
            if (curso == null)
                return NotFound();
            return Ok(curso);
        }

        [HttpPost]
        public IActionResult PostCurso([FromBody] Curso curso)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var id = _cursoBC.Crear(curso);
            curso.IdCurso = id;
            return CreatedAtAction(nameof(GetCurso), new { id = curso.IdCurso }, curso);
        }

        [HttpGet("export/excel")]
        public IActionResult ExportToExcel()
        {
            var cursos = _cursoBC.Listar();
            using var workbook = new XLWorkbook();
            var worksheet = workbook.Worksheets.Add("Cursos");
            worksheet.Cell(1, 1).Value = "IdCurso";
            worksheet.Cell(1, 2).Value = "Nombre";
            worksheet.Cell(1, 3).Value = "Creditos";

            for (int i = 0; i < cursos.Count; i++)
            {
                var row = i + 2;
                worksheet.Cell(row, 1).Value = cursos[i].IdCurso;
                worksheet.Cell(row, 2).Value = cursos[i].Nombre;
                worksheet.Cell(row, 3).Value = cursos[i].Creditos;
            }

            worksheet.Columns().AdjustToContents();
            using var stream = new MemoryStream();
            workbook.SaveAs(stream);
            stream.Position = 0;
            return File(stream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "cursos.xlsx");
        }
    }
}
