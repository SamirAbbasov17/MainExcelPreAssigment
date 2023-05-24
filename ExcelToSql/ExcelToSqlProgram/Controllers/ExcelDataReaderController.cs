using ExcelToSqlProgram.Data;
using ExcelToSqlProgram.Models;
using Ganss.Excel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System.Data;
using System.Net;
using Microsoft.Extensions.Configuration;
using SendGrid.Helpers.Mail;
using SendGrid;
using NPOI.SS.Formula.Functions;
using mailinblue;
using System.Text;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Newtonsoft.Json;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace ExcelToSqlProgram.Controllers
{
    public enum Report
    {
        Segment,
        Country,
        Sells,
        Discounts
    }

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


        [Route("api/[controller]/Get")]
        [HttpPost]
        public async Task<IActionResult> Get(Report report,string[] mails , DateTime startDate , DateTime endDate)
        {
            Dictionary<string, double> mailSegmentProductData = new Dictionary<string, double>();
            Dictionary<string, double> mailSegmentSalesData = new Dictionary<string, double>();
            Dictionary<string, double> mailSegmentProfitData = new Dictionary<string, double>();
            Dictionary<string, double> mailSegmentDiscountData = new Dictionary<string, double>();
            Dictionary<string, double> mailCountryProductData = new Dictionary<string, double>();
            Dictionary<string, double> mailCountrySalesData = new Dictionary<string, double>();
            Dictionary<string, double> mailCountryProfitData = new Dictionary<string, double>();
            Dictionary<string, double> mailCountryDiscountData = new Dictionary<string, double>();
            Dictionary<string, double> mailProductProductData = new Dictionary<string, double>();
            Dictionary<string, double> mailProductSalesData = new Dictionary<string, double>();
            Dictionary<string, double> mailProductProfitData = new Dictionary<string, double>();
            Dictionary<string, double> mailProductDiscountData = new Dictionary<string, double>();
            Dictionary<string, double> mailProductDiscountPercentageData = new Dictionary<string, double>();



            foreach (var segment in _db.ExcelData.GroupBy(e => e.Segment))
            {
                var segmentData = segment.Where(m => m.Date > startDate && m.Date < endDate);
                mailSegmentProductData.Add(segment.Key, segmentData.Select(x => x.Product).Count());
                mailSegmentSalesData.Add(segment.Key,Convert.ToDouble( segmentData.Sum(x => x.Sales)));
                mailSegmentProfitData.Add(segment.Key, Convert.ToDouble(segmentData.Sum(x => x.Profit)));
                mailSegmentDiscountData.Add(segment.Key, Convert.ToDouble(segmentData.Average(x => x.Discounts)));
            }

            foreach (var country in _db.ExcelData.GroupBy(e => e.Country))
            {
                var countryData = country.Where(m => m.Date > startDate && m.Date < endDate);
                mailCountryProductData.Add(country.Key, countryData.Select(x => x.Product).Count());
                mailCountrySalesData.Add(country.Key, Convert.ToDouble(countryData.Sum(x => x.Sales)));
                mailCountryProfitData.Add(country.Key, Convert.ToDouble(countryData.Sum(x => x.Profit)));
                mailCountryDiscountData.Add(country.Key, Convert.ToDouble(countryData.Average(x => x.Discounts)));
            }
            foreach (var product in _db.ExcelData.GroupBy(e => e.Product))
            {
                var productData = product.Where(m => m.Date > startDate && m.Date < endDate);
                mailProductProductData.Add(product.Key, productData.Select(x => x.Product).Count());
                mailProductSalesData.Add(product.Key, Convert.ToDouble(productData.Sum(x => x.Sales)));
                mailProductProfitData.Add(product.Key, Convert.ToDouble(productData.Sum(x => x.Profit)));
                mailProductDiscountData.Add(product.Key, Convert.ToDouble(productData.Average(x => x.Discounts)));
            }

            foreach (var product in _db.ExcelData.GroupBy(e => e.Product))
            {
                var productData = product.Where(m => m.Date > startDate && m.Date < endDate);
                double discountPercentage = Convert.ToDouble(productData.Sum(x => (x.Discounts / x .Sales)*100));
                mailProductDiscountPercentageData.Add(product.Key, discountPercentage);
            }

            async Task SendEmail(Dictionary<string,double> firstData , Dictionary<string, double> secondData = null, Dictionary<string, double> thirdData = null, Dictionary<string, double> fourthData = null )
            {
                //API sendinBlue = new mailinblue.API("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");
                string apiKey = "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";
                string url = "https://api.brevo.com/v3/emailCampaigns";
                Dictionary<string, Object> data = new Dictionary<string, Object>();
                Dictionary<string, string> to = new Dictionary<string, string>();
                List<string> from_name = new List<string>();
                foreach (var mail in mails)
                {

                    to.Add(mail, "to this mail " + mail);

                    from_name.Add("abbasovsamir718@hotmail.com");
                    data.Add("to", to);
                    data.Add("from", from_name);
                    data.Add("excel", "Excel Report");
                    if (mail.Substring(mail.LastIndexOf("@")) == "@code.edu.az" && mail.Contains("@") && secondData != null)
                    {
                        StringBuilder table = new StringBuilder();

                        table.Append(@$"
                             <table>
                                  <tr>
                                    <th>{report}</th>
                                    <th>Product</th>
                                    <th>Profit</th>
                                    <th>Sales</th>
                                    <th>Discounts</th>
                                  </tr>
                            ");
                        for (int i = 1; i <= firstData.Count; i++)
                        {
                            table.Append(@$"
                                  <tr>
                                    <td>{firstData.ElementAt(i-1).Key}</td>
                                    <td>{firstData.ElementAt(i-1).Value}</td>
                                    <td>{secondData.ElementAt(i-1).Value}</td>
                                    <td>{thirdData.ElementAt(i-1).Value}</td>
                                    <td>{fourthData.ElementAt(i-1).Value}</td>
                                  </tr>
                            ");
                        }
                        table.Append("</table>");


                        //data.Add("html", table);


                        //data.Add("reports", "Illere uygun hesabat");
                        //Object sendEmail = sendinBlue.send_email(data);
                        data.Add("html", table.ToString());
                        data.Add("reports", "Illere uygun hesabat");

                        using (HttpClient client = new HttpClient())
                        {
                            client.DefaultRequestHeaders.Add("api-key", apiKey);
                            string json = JsonConvert.SerializeObject(data);
                            HttpContent content = new StringContent(json, Encoding.UTF8, "application/json");
                            HttpResponseMessage response = await client.PostAsync(url, content);
                            string responseContent = await response.Content.ReadAsStringAsync();
                        }


                    }
                    else if (mail.Substring(mail.LastIndexOf("@")) == "@code.edu.az" && mail.Contains("@") && secondData == null) 
                    {
                        StringBuilder table = new StringBuilder();

                        table.Append(@$"
                             <table>
                                  <tr>
                                    <th>{report}</th>
                                    <th>Discount</th>
                                  </tr>
                            ");
                        for (int i = 1; i <= firstData.Count; i++)
                        {
                            table.Append($@"
                                  <tr>
                                    <td>{firstData.ElementAt(i-1).Key}</td>
                                    <td>{firstData.ElementAt(i-1).Value}</td>
                                  </tr>
                            ");
                        }
                        table.Append("</table>");


                        //data.Add("html", table);


                        //data.Add("reports", "Illere uygun hesabat");
                        //Object sendEmail = sendinBlue.send_email(data);
                        data.Add("html", table.ToString());
                        data.Add("reports", "Illere uygun hesabat");

                        using (HttpClient client = new HttpClient())
                        {
                            client.DefaultRequestHeaders.Add("api-key", apiKey);
                            string json = JsonConvert.SerializeObject(data);
                            HttpContent content = new StringContent(json, Encoding.UTF8, "application/json");
                            HttpResponseMessage response = await client.PostAsync(url, content);
                            string responseContent = await response.Content.ReadAsStringAsync();
                        }
                    }
                    else
                    {
                        BadRequest("Duzgun deyer secin");
                    }

                }
                
            }

            switch (report)
            {
                case Report.Segment:
                    await SendEmail(mailSegmentProductData,mailSegmentProfitData,mailSegmentSalesData,mailSegmentDiscountData);
                    break;
                case Report.Country:
                    await SendEmail(mailCountryProductData,mailCountryProfitData,mailCountrySalesData,mailCountryDiscountData);
                    break;
                case Report.Sells:
                    await SendEmail(mailProductProductData,mailProductProfitData,mailProductSalesData,mailProductDiscountData);
                    break;
                case Report.Discounts:
                    await SendEmail(mailProductDiscountPercentageData);
                    break;
                default:
                    return BadRequest("Duzgun deyer secin");
            }

            return Ok("Ugur");
        }

        [HttpPost]
        public IActionResult Post(IFormFile file)
        {
            if (file != null && file.Length > 0 && file.Length < 5e+6 && (Path.GetExtension(file.FileName) == ".xls" || Path.GetExtension(file.FileName) == ".xlsx"))
            {
                try
                {
                    string filePath = Path.Combine("C:\\Users\\Lenovo\\Desktop\\Code Acadamy\\ExcelToSql\\ExcelToSqlProgram\\ExcelFile\\", file.FileName);
                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        file.CopyTo(stream);
                    }

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

                    string MainFile = @"C:\Users\Lenovo\Desktop\Code Acadamy\ExcelToSql\ExcelToSqlProgram\ExcelFile\example_data.xlsx";
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
                    return BadRequest("Duzgun Formatda data daxil edin " + ex.Message);
                }
            }
            else
            {
                return BadRequest("Duzgun Fayl Yukleyin");
            }
        }
    }
}
