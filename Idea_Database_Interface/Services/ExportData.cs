using Idea_Database_Interface.Data.UnitOfWork;
using Idea_Database_Interface.Models;
using IronXL;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Identity.Client;
using Syncfusion.Pdf.Parsing;
using Syncfusion.XlsIO;

namespace Idea_Database_Interface.Services
{
    public class ExportData
    {
        private readonly IQueryable<Bonos> _bonos;
        public ExportData(IQueryable<Bonos> bonos) { _bonos = bonos; }
        public IActionResult DownloadExcelDb()
        {
            using (ExcelEngine engine = new ExcelEngine())
            {
                IApplication application = engine.Excel;
                application.DefaultVersion = ExcelVersion.Xlsx;
                IWorkbook workBook = application.Workbooks.Create(1);
                IWorksheet worksheet = workBook.Worksheets[0];
                worksheet.Range["B1"].Text = "Nombre";
                worksheet.Range["D1"].Text = "Móvil";
                worksheet.Range["E1"].Text = "Email";
                worksheet.Range["F1"].Text = "Código Postal";
                worksheet.Range["I1"].Text = "Fecha";
                worksheet.Range["J1"].Text = "Hora";
                worksheet.Range["Q1"].Text = "Estado";
                worksheet.Range["R1"].Text = "DNI";
                worksheet.Range["S1"].Text = "Localidad";
                worksheet.Range["T1"].Text = "Localizador";
                worksheet.Range["U1"].Text = "Apellido 1";
                worksheet.Range["V1"].Text = "Apellido 2";
                worksheet.Range["W1"].Text = "Dirección";
                int i = 2;
                foreach (var b in _bonos)
                {
                    worksheet.Range["B" + i].Text = b.Nombre;
                    worksheet.Range["D" + i].Text = b.Teléfono;
                    worksheet.Range["E" + i].Text = b.Correo;
                    worksheet.Range["F" + i].Text = b.CódigoPostal;
                    worksheet.Range["I" + i].DateTime = b.Date.Date;
                    worksheet.Range["J" + i].TimeSpan = b.Date.TimeOfDay;
                    worksheet.Range["Q" + i].Text = "Confirmada";
                    worksheet.Range["R" + i].Text = b.DNI;
                    worksheet.Range["S" + i].Text = b.Localidad;
                    worksheet.Range["T" + i].Text = b.Localizador;
                    worksheet.Range["U" + i].Text = b.PrimerApellido;
                    worksheet.Range["V" + i].Text = b.SegunodApellido;
                    worksheet.Range["W" + i].Text = b.Direcction;
                    i++;
                }
                MemoryStream stream = new MemoryStream();
                workBook.SaveAs(stream);
                stream.Position = 0;
                FileStreamResult fileStreamResult = new FileStreamResult(stream, "application/excel");
                fileStreamResult.FileDownloadName = "BaseDeDatos.xlsx";
                return fileStreamResult;
            }
        }
        public async Task<IActionResult> PrintPdf(int id)
        {
            Bonos bono = _bonos.Where(x=>x.Id == id).FirstOrDefault();
            FileStream fileStream = new FileStream("Data/PDFOutput.pdf",FileMode.Open,FileAccess.Read);
            PdfLoadedDocument loadedDocument = new PdfLoadedDocument(fileStream);
            PdfLoadedForm form = loadedDocument.Form;
            if(bono != null)
            {
                (form.Fields["Hora"] as PdfLoadedTextBoxField).Text = bono.Date.ToString("HH");
                (form.Fields["Minuto"] as PdfLoadedTextBoxField).Text = bono.Date.ToString("mm");
                (form.Fields["Nombre"] as PdfLoadedTextBoxField).Text = bono.Nombre;
                (form.Fields["Apellido"] as PdfLoadedTextBoxField).Text = bono.PrimerApellido + " " + bono.SegunodApellido;
                (form.Fields["Dia"] as PdfLoadedTextBoxField).Text = bono.Date.Day.ToString();
                (form.Fields["Mesa"] as PdfLoadedTextBoxField).Text = bono.Date.Month.ToString();
                (form.Fields["Año"] as PdfLoadedTextBoxField).Text = bono.Date.Year.ToString();
                (form.Fields["Teléfono"] as PdfLoadedTextBoxField).Text = bono.Teléfono ?? "";
                (form.Fields["NúmeroBono"] as PdfLoadedTextBoxField).Text = bono.NumeroDeBonos.ToString();
                (form.Fields["TarjetaNúmero"] as PdfLoadedTextBoxField).Text = bono.TarjetaNum ?? "";
                (form.Fields["Localizador"] as PdfLoadedTextBoxField).Text = bono.Localizador ?? "";
                (form.Fields["Correo"] as PdfLoadedTextBoxField).Text = bono.Correo;
                (form.Fields["DNI"] as PdfLoadedTextBoxField).Text = bono.DNI;
                (form.Fields["númeroId1"] as PdfLoadedTextBoxField).Text = bono.NúmeroId ?? "";
                (form.Fields["númeroId2"] as PdfLoadedTextBoxField).Text = bono.NúmeroId2 ?? "";
                (form.Fields["númeroId2"] as PdfLoadedTextBoxField).Text = bono.NúmeroId2 ?? "";
                (form.Fields["dirección"] as PdfLoadedTextBoxField).Text = bono.Direcction ?? "";                
                (form.Fields["Código postal"] as PdfLoadedTextBoxField).Text = bono.CódigoPostal ?? "";                
                (form.Fields["localidad"] as PdfLoadedTextBoxField).Text = bono.Localidad ?? "";                
            }
            MemoryStream stream = new MemoryStream();
            loadedDocument.Save(stream);
            stream.Position = 0;
            loadedDocument.Close(true);
            string contentType = "application/pdf";
            string fileName = "output.pdf";
            FileStreamResult fileStreamResult = new FileStreamResult(stream,contentType);
            fileStreamResult.FileDownloadName = fileName;
            return fileStreamResult;
        }
    }
}
