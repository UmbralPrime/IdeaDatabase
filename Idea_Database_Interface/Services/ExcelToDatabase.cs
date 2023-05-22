using Idea_Database_Interface.Data.UnitOfWork;
using Idea_Database_Interface.Models;
using Idea_Database_Interface.Viewmodels;
using IronXL;
using Microsoft.AspNetCore.Identity;
using System.Data;
using System.Globalization;

namespace Idea_Database_Interface.Services
{
    public class ExcelToDatabase
    {
        private readonly IUnitOfWork _uow;
        public ExcelToDatabase(IUnitOfWork uow) { _uow = uow; }
        public async Task ImportToDatabaseAsync(IFormFile fileUpload)
        {
            IQueryable<Bonos> allDbBonos = _uow.BonosRepository.GetAll();
            var filePath = Path.Combine(Directory.GetCurrentDirectory(), fileUpload.FileName);
            using (var stream = System.IO.File.Create(filePath, 4096, FileOptions.DeleteOnClose))
            {
                await fileUpload.CopyToAsync(stream);
                WorkBook wb = WorkBook.FromStream(stream);
                DataSet dataSet = wb.ToDataSet();
                foreach (DataTable table in dataSet.Tables)
                {
                    foreach (DataRow row in table.Rows)
                    {
                        if (row[16].ToString() == "Confirmada")
                        {
                            TimeSpan hour = DateTime.Parse(row[9].ToString()).TimeOfDay;
                            DateTime usedTime = DateTime.Parse(row[8].ToString());
                            Bonos bonos = new Bonos()
                            {
                                Date = usedTime + hour,
                                Direcction = row[22].ToString(),
                                DNI = row[17].ToString(),
                                Nombre = row[1].ToString(),
                                PrimerApellido = row[20].ToString(),
                                SegunodApellido = row[21].ToString(),
                                NumeroDeBonos = 0,
                                Correo = row[4].ToString(),
                                CódigoPostal = row[5].ToString(),
                                Localidad = row[18].ToString(),
                                Localizador = row[19].ToString(),
                                Teléfono = row[3].ToString()

                            };
                            if (await bonos.EqualsAsync(allDbBonos) == false)
                            {
                                _uow.BonosRepository.Create(bonos);
                                await _uow.Save();
                            }
                        }
                    }
                }
                stream.Dispose();
            }

        }
    }
}
