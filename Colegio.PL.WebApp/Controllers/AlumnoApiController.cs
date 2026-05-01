using Colegio.BL.BC;
using Colegio.BL.BE;
using ClosedXML.Excel;
using Microsoft.AspNetCore.Mvc;
using System.IO;

namespace Colegio.PL.WebApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AlumnoApiController : ControllerBase
    {
        private readonly AlumnoBC _alumnoBC;

        public AlumnoApiController(AlumnoBC alumnoBC)
        {
            _alumnoBC = alumnoBC;
        }

        // GET: api/AlumnoApi
        [HttpGet]
        public IActionResult GetAlumnos()
        {
            var alumnos = _alumnoBC.ListarAlumnos();
            return Ok(alumnos);
        }

        // GET: api/AlumnoApi/5
        [HttpGet("{id}")]
        public IActionResult GetAlumno(int id)
        {
            var alumno = _alumnoBC.Obtener(id);
            if (alumno == null)
                return NotFound();
            return Ok(alumno);
        }

        // POST: api/AlumnoApi
        [HttpPost]
        public IActionResult PostAlumno([FromBody] AlumnoBE alumno)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                var id = _alumnoBC.CrearAlumno(alumno);
                alumno.IdAlumno = id;
                return CreatedAtAction(nameof(GetAlumno), new { id = alumno.IdAlumno }, alumno);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // GET: api/AlumnoApi/export/excel
        [HttpGet("export/excel")]
        public IActionResult ExportToExcel()
        {
            var alumnos = _alumnoBC.ListarAlumnos();
            using var workbook = new XLWorkbook();
            var worksheet = workbook.Worksheets.Add("Alumnos");

            worksheet.Cell(1, 1).Value = "IdAlumno";
            worksheet.Cell(1, 2).Value = "Nombre";
            worksheet.Cell(1, 3).Value = "Apellido";
            worksheet.Cell(1, 4).Value = "DNI";
            worksheet.Cell(1, 5).Value = "FechaNacimiento";
            worksheet.Cell(1, 6).Value = "Direccion";
            worksheet.Cell(1, 7).Value = "FechaRegistro";

            for (int i = 0; i < alumnos.Count; i++)
            {
                var row = i + 2;
                worksheet.Cell(row, 1).Value = alumnos[i].IdAlumno;
                worksheet.Cell(row, 2).Value = alumnos[i].Nombre;
                worksheet.Cell(row, 3).Value = alumnos[i].Apellido;
                worksheet.Cell(row, 4).Value = alumnos[i].DNI;
                worksheet.Cell(row, 5).Value = alumnos[i].FechaNacimiento;
                worksheet.Cell(row, 6).Value = alumnos[i].Direccion;
                worksheet.Cell(row, 7).Value = alumnos[i].FechaRegistro;
            }

            worksheet.Columns().AdjustToContents();
            using var stream = new MemoryStream();
            workbook.SaveAs(stream);
            stream.Position = 0;
            return File(stream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "alumnos.xlsx");
        }
    }
}