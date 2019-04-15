using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using DinkToPdf;
using DinkToPdf.Contracts;
using Microsoft.AspNetCore.Mvc;

namespace PdfDemo.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ValuesController : ControllerBase
    {
        private IConverter _converter;
 
        public ValuesController(IConverter converter)
        {
            _converter = converter;
        }
        
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            if (!Directory.Exists(Path.Combine(Directory.GetCurrentDirectory(), "reports")))
                Directory.CreateDirectory(Path.Combine(Directory.GetCurrentDirectory(), "reports"));

            if(!Directory.Exists(Path.Combine(Directory.GetCurrentDirectory(), "reports", "instanceId")))
                Directory.CreateDirectory(Path.Combine(Directory.GetCurrentDirectory(), "reports", "instanceId"));
            
            var globalSettings = new GlobalSettings
            {
                ColorMode = ColorMode.Color,
                Orientation = Orientation.Portrait,
                PaperSize = PaperKind.A4,
                Margins = new MarginSettings { Top = 10 },
                DocumentTitle = "PDF Report",
                Out = $@"{Path.Combine(Directory.GetCurrentDirectory(), "reports","instanceId")}/Employee_Report.{Guid.NewGuid()}.pdf"
            };
            
            var objectSettings = new ObjectSettings
            {
                PagesCount = true,
                HtmlContent = TemplateGenerator.GetHTMLString(),
                WebSettings = { DefaultEncoding = "utf-8", UserStyleSheet =  Path.Combine(Directory.GetCurrentDirectory(), "assets", "styles.css") },
                HeaderSettings = { FontName = "Arial", FontSize = 9, Right = "Page [page] of [toPage]", Line = true },
                FooterSettings = { FontName = "Arial", FontSize = 9, Line = true, Center = "Report Footer" }
            };
 
            var pdf = new HtmlToPdfDocument()
            {
                GlobalSettings = globalSettings,
                Objects = { objectSettings }
            };
 
            _converter.Convert(pdf);
            
            
            var memory = new MemoryStream();  
            using (var stream = new FileStream(globalSettings.Out, FileMode.Open))  
            {  
                 await stream.CopyToAsync(memory);  
            }  
            memory.Position = 0; 
            return File(memory, GetContentType(globalSettings.Out), Path.GetFileName(globalSettings.Out)); 
        }

        private string GetContentType(string path)  
        {  
            var types = GetMimeTypes();  
            var ext = Path.GetExtension(path).ToLowerInvariant();  
            return types[ext];  
        }

        private Dictionary<string, string> GetMimeTypes()
        {
            return new Dictionary<string, string>
            {
                {".txt", "text/plain"},
                {".pdf", "application/pdf"},
                {".doc", "application/vnd.ms-word"},
                {".docx", "application/vnd.ms-word"},
                {".xls", "application/vnd.ms-excel"},
                {".xlsx", "application/vnd.openxmlformatsofficedocument.spreadsheetml.sheet"},  
                {".png", "image/png"},
                {".jpg", "image/jpeg"},
                {".jpeg", "image/jpeg"},
                {".gif", "image/gif"},
                {".csv", "text/csv"}
            };
        }
    }
}
