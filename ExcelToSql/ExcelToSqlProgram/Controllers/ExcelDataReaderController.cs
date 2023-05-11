using ExcelToSqlProgram.Data;
using ExcelToSqlProgram.Models;
using Ganss.Excel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System.Data;
using System.Net;
using Microsoft.Extensions.Configuration;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace ExcelToSqlProgram.Controllers
{



    [Route("api/[controller]")]
    [ApiController]
    public class ExcelDataReaderController : ControllerBase
    {
        private readonly ApplicationDbContext _db;
        private readonly IConfiguration _configuration;

        public ExcelDataReaderController(ApplicationDbContext db, IConfiguration configuration)
        {
            _db = db;
            _configuration = configuration;
        }



        [HttpGet]
        public IActionResult Get()
        {
            return default;
        }

        [HttpPost]
        public IActionResult Post(IFormFile file)
        {
            if (file != null && file.Length > 0 && file.Length < 5e+6 && (Path.GetExtension(file.FileName) == ".xls" || Path.GetExtension(file.FileName) == ".xlsx"))
            {
                try
                {
                    string filePath = Path.Combine("C:\\Users\\Lenovo\\Desktop\\Code Acadamy\\Other\\ExcelToSql\\ExcelToSqlProgram\\ExcelFile\\", file.FileName);
                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        file.CopyTo(stream);
                    }

                    //string connectionString = _configuration.GetConnectionString("DefaultConnection"); 
                    //using (SqlConnection connection = new SqlConnection(connectionString))
                    //{
                    //    connection.Open();

                    //    string query = "DELETE FROM ExcelData"; // Gerçek sorgunuzu burada tanımlayın

                    //    SqlCommand command = new SqlCommand(query, connection);
                    //    command.CommandType = CommandType.Text;
                    //    command.ExecuteNonQuery();

                    //}
                    string connectionString = _configuration.GetConnectionString("DefaultConnection");

                    using (SqlConnection connection = new SqlConnection(connectionString))
                    {
                        connection.Open();

                        string truncateQuery = "TRUNCATE TABLE ExcelData";
                        string resetIdentityQuery = "DBCC CHECKIDENT (ExcelData, RESEED, 0)";

                        using (SqlCommand truncateCommand = new SqlCommand(truncateQuery, connection))
                        {
                            truncateCommand.ExecuteNonQuery();
                        }

                        using (SqlCommand resetIdentityCommand = new SqlCommand(resetIdentityQuery, connection))
                        {
                            resetIdentityCommand.ExecuteNonQuery();
                        }
                    }

                    string MainFile = @"C:\Users\Lenovo\Desktop\Code Acadamy\Other\ExcelToSql\ExcelToSqlProgram\ExcelFile\example_data.xlsx";
                    var datas = new ExcelMapper(MainFile).Fetch<ExcelData>();
                    foreach (ExcelData data in datas)
                    {
                        _db.Add(data);
                        _db.SaveChanges();
                    }
                    return Ok("Dosya başarıyla yüklendi.");
                }
                catch (Exception ex)
                {
                    return BadRequest("Duzgun Formatda data daxil edin ");
                }
            }
            else
            {
                return BadRequest("Duzgun Fayl Yukleyin");
            }
        }
    }
}
