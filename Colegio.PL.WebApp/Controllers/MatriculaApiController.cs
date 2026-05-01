using Colegio.BL.BC;
using Colegio.BL.BE;
using ClosedXML.Excel;
using Microsoft.AspNetCore.Mvc;
using System.IO;

namespace Colegio.PL.WebApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MatriculaApiController : ControllerBase
    {
        private readonly MatriculaBC _matriculaBC;

        public MatriculaApiController(MatriculaBC matriculaBC)
        {
            _matriculaBC = matriculaBC;
        }

        [HttpGet]
        public IActionResult GetMatriculas()
        {
            var matriculas = _matriculaBC.Listar();
            return Ok(matriculas);
        }

        [HttpGet("{id}")]
        public IActionResult GetMatricula(int id)
        {
            var matricula = _matriculaBC.Obtener(id);
            if (matricula == null)
                return NotFound();
            return Ok(matricula);
        }

        [HttpPost]
        public IActionResult PostMatricula([FromBody] Matricula matricula)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var id = _matriculaBC.Crear(matricula);
            matricula.IdMatricula = id;
            return CreatedAtAction(nameof(GetMatricula), new { id = matricula.IdMatricula }, matricula);
        }

        [HttpGet("export/excel")]
        public IActionResult ExportToExcel()
        {
            var matriculas = _matriculaBC.Listar();
            using var workbook = new XLWorkbook();
            var worksheet = workbook.Worksheets.Add("Matriculas");
            worksheet.Cell(1, 1).Value = "IdMatricula";
            worksheet.Cell(1, 2).Value = "IdAlumno";
            worksheet.Cell(1, 3).Value = "NombreAlumno";
            worksheet.Cell(1, 4).Value = "ApellidoAlumno";
            worksheet.Cell(1, 5).Value = "DNIAlumno";
            worksheet.Cell(1, 6).Value = "FechaMatricula";
            worksheet.Cell(1, 7).Value = "Periodo";

            for (int i = 0; i < matriculas.Count; i++)
            {
                var row = i + 2;
                worksheet.Cell(row, 1).Value = matriculas[i].IdMatricula;
                worksheet.Cell(row, 2).Value = matriculas[i].IdAlumno;
                worksheet.Cell(row, 3).Value = matriculas[i].NombreAlumno;
                worksheet.Cell(row, 4).Value = matriculas[i].ApellidoAlumno;
                worksheet.Cell(row, 5).Value = matriculas[i].DNIAlumno;
                worksheet.Cell(row, 6).Value = matriculas[i].FechaMatricula;
                worksheet.Cell(row, 7).Value = matriculas[i].Periodo;
            }

            worksheet.Columns().AdjustToContents();
            using var stream = new MemoryStream();
            workbook.SaveAs(stream);
            stream.Position = 0;
            return File(stream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "matriculas.xlsx");
        }
    }
}
