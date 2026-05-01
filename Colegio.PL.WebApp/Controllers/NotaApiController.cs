using Colegio.BL.BC;
using Colegio.BL.BE;
using ClosedXML.Excel;
using Microsoft.AspNetCore.Mvc;
using System.IO;

namespace Colegio.PL.WebApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NotaApiController : ControllerBase
    {
        private readonly NotaBC _notaBC;

        public NotaApiController(NotaBC notaBC)
        {
            _notaBC = notaBC;
        }

        [HttpGet]
        public IActionResult GetNotas()
        {
            var notas = _notaBC.Listar();
            return Ok(notas);
        }

        [HttpGet("{id}")]
        public IActionResult GetNota(int id)
        {
            var nota = _notaBC.Obtener(id);
            if (nota == null)
                return NotFound();
            return Ok(nota);
        }

        [HttpPost]
        public IActionResult PostNota([FromBody] Nota nota)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var id = _notaBC.Crear(nota);
            nota.IdNota = id;
            return CreatedAtAction(nameof(GetNota), new { id = nota.IdNota }, nota);
        }

        [HttpGet("export/excel")]
        public IActionResult ExportToExcel()
        {
            var notas = _notaBC.Listar();
            using var workbook = new XLWorkbook();
            var worksheet = workbook.Worksheets.Add("Notas");
            worksheet.Cell(1, 1).Value = "IdNota";
            worksheet.Cell(1, 2).Value = "IdMatricula";
            worksheet.Cell(1, 3).Value = "IdCurso";
            worksheet.Cell(1, 4).Value = "CursoNombre";
            worksheet.Cell(1, 5).Value = "IdAlumno";
            worksheet.Cell(1, 6).Value = "Calificacion";

            for (int i = 0; i < notas.Count; i++)
            {
                var row = i + 2;
                worksheet.Cell(row, 1).Value = notas[i].IdNota;
                worksheet.Cell(row, 2).Value = notas[i].IdMatricula;
                worksheet.Cell(row, 3).Value = notas[i].IdCurso;
                worksheet.Cell(row, 4).Value = notas[i].CursoNombre;
                worksheet.Cell(row, 5).Value = notas[i].IdAlumno;
                worksheet.Cell(row, 6).Value = notas[i].Calificacion;
            }

            worksheet.Columns().AdjustToContents();
            using var stream = new MemoryStream();
            workbook.SaveAs(stream);
            stream.Position = 0;
            return File(stream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "notas.xlsx");
        }
    }
}
