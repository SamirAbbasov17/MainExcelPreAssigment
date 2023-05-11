using ExcelToSqlProgram.Data;
using ExcelToSqlProgram.Models;
using Ganss.Excel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Net;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace ExcelToSqlProgram.Controllers
{



    [Route("api/[controller]")]
    [ApiController]
    public class ExcelDataReaderController : ControllerBase
    {
        private readonly ApplicationDbContext _db;
        public ExcelDataReaderController(ApplicationDbContext db)
        {
            _db = db;
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
                    return BadRequest("Duzgun Formatda data daxil edin");
                }
            }
            else
            {
                return BadRequest("Duzgun Fayl Yukleyin");
            }
        }
    }
}
