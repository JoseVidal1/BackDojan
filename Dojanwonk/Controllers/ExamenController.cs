using BLL;
using DAL;
using DAL.Modelos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;

namespace Dojanwonk.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ExamenController : ControllerBase
    {
        private readonly IServiceExamen serviceExamen;
        public ExamenController(IServiceExamen serviceExamen)
        {
            this.serviceExamen = serviceExamen;
        }
        [HttpPost]
        [Authorize(Policy = "AdminOInstructor")]
        public async Task<ActionResult<Examen>> Registrar(Examen examen)
        {
            try
            {
               Examen registrado = await serviceExamen.Registrar(examen);
                return StatusCode(StatusCodes.Status201Created, registrado);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpGet]
        [Authorize(Policy = "AdminOInstructor")]
        public async Task<ActionResult<IEnumerable<Examen>>> Leer()
        {
            return Ok(await serviceExamen.Leer());
        }
        [HttpGet("pdf-examenes")]
        [Authorize(Policy = "AdminOInstructor")]
        public async Task<IActionResult> DescargarPdf()
        {
            var examenes = await serviceExamen.Leer();

            if (examenes == null || !examenes.Any())
                return NotFound("No hay exámenes para generar el PDF.");

            QuestPDF.Settings.License = LicenseType.Community;

            using var stream = new MemoryStream();

            string logoPath = "./Images/logo.png";

            Document.Create(container =>
            {
                container.Page(page =>
                {
                    page.Size(PageSizes.A3);
                    page.Margin(30);
                    page.DefaultTextStyle(x => x.FontSize(12).FontFamily("Arial"));

                    // Encabezado: logo + texto Academia
                    page.Header()
                        .Row(row =>
                        {
                            // Columna logo (altura fija y proporcional)
                            row.ConstantColumn(100).Height(60).Image(logoPath, ImageScaling.FitArea);

                            // Columna texto
                            row.RelativeColumn()
                                .PaddingLeft(10)
                                .AlignMiddle()
                                .Column(column =>
                                {
                                    column.Item().Text("Academia Taekwondo Novalito")
                                        .FontSize(22)
                                        .SemiBold()
                                        .FontColor(Colors.Blue.Darken2);

                                    column.Item().Text("Listado de Exámenes")
                                        .FontSize(18)
                                        .Medium()
                                        .FontColor(Colors.Blue.Medium);
                                });
                        });

                    page.Content()
                        .PaddingTop(20)
                        .Table(table =>
                        {
                            table.ColumnsDefinition(columns =>
                            {
                                columns.RelativeColumn(3); // Estudiante más ancho
                                for (int i = 0; i < 8; i++) columns.ConstantColumn(60);
                            });

                            table.Header(header =>
                            {
                                header.Cell().Element(CellStyle).Text("Estudiante");
                                header.Cell().Element(CellStyle).Text("Calent.");
                                header.Cell().Element(CellStyle).Text("Mano");
                                header.Cell().Element(CellStyle).Text("Patada");
                                header.Cell().Element(CellStyle).Text("Especial");
                                header.Cell().Element(CellStyle).Text("Combate");
                                header.Cell().Element(CellStyle).Text("Rompi.");
                                header.Cell().Element(CellStyle).Text("Teórica");
                                header.Cell().Element(CellStyle).Text("Nota");
                            });

                            foreach (var examen in examenes)
                            {
                                table.Cell().Element(CellStyle).Text($"{examen.Estudiante?.Nombres} {examen.Estudiante?.Apellidos}");
                                table.Cell().Element(CellStyle).Text(examen.Calentamiento.ToString());
                                table.Cell().Element(CellStyle).Text(examen.TecMano.ToString());
                                table.Cell().Element(CellStyle).Text(examen.TecPatada.ToString());
                                table.Cell().Element(CellStyle).Text(examen.TecEspecial.ToString());
                                table.Cell().Element(CellStyle).Text(examen.Combate.ToString());
                                table.Cell().Element(CellStyle).Text(examen.Rompimiento.ToString());
                                table.Cell().Element(CellStyle).Text(examen.Teorica.ToString());
                                table.Cell().Element(CellStyle).Text(examen.NotaFinal?.ToString() ?? "-");
                            }

                            // Estilo de celdas para filas y encabezado
                            static IContainer CellStyle(IContainer container) =>
                                container
                                    .BorderColor(Colors.Grey.Lighten2)
                                    .PaddingVertical(8)
                                    .PaddingHorizontal(5)
                                    .AlignCenter()
                                    .Background(Colors.Grey.Lighten4);
                        });
                });
            }).GeneratePdf(stream);

            stream.Position = 0;
            return File(stream.ToArray(), "application/pdf", "examenes.pdf");
        }


        [HttpPut]
        [Authorize(Policy = "AdminOInstructor")]
        public async Task<ActionResult> Actualizar(Examen examen)
        {
            try
            {
                await serviceExamen.Actualizar(examen);
                return Ok("Actualizado con exito");
            }catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
