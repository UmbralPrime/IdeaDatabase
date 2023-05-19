using Idea_Database_Interface.Data.UnitOfWork;
using Idea_Database_Interface.Models;
using Idea_Database_Interface.Viewmodels;
using IronXL;
using System.Data;
using System.Globalization;

namespace Idea_Database_Interface.Services
{
    public class ExcelToDatabase
    {
        private readonly IUnitOfWork _uow;
        public ExcelToDatabase(IUnitOfWork uow) { _uow = uow; }
        public async Task ImportToDatabaseAsync(IFormFile fileUpload, int dateYear)
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
                    DateTime tableTime = DateTime.Parse($"{table.TableName} {dateYear}", new CultureInfo("es-ES"));
                    foreach (DataRow row in table.Rows)
                    {
                        if (row[1].ToString() != "Hora" && row[4].ToString() != "TOTAl" && !string.IsNullOrEmpty(row[3].ToString()))
                        {
                            TimeSpan hour = DateTime.Parse(row[1].ToString()).TimeOfDay;
                            DateTime usedTime = tableTime + hour;
                            int numeroBonos = 0;
                            int.TryParse(row[6].ToString(), out numeroBonos);
                            Bonos bonos = new Bonos()
                            {
                                Date = usedTime,
                                Direcction = row[9].ToString(),
                                DNI = row[5].ToString(),
                                Nombre = row[2].ToString(),
                                PrimerApellido = row[3].ToString(),
                                SegunodApellido = row[4].ToString(),
                                NumeroDeBonos = numeroBonos,
                                Correo = row[8].ToString(),
                                CódigoPostal = row[10].ToString(),
                                Teléfono = row[11].ToString(),
                                TarjetaNum = row[12].ToString(),
                                NúmeroId = row[13].ToString(),
                                NúmeroId2 = row[14].ToString()
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
