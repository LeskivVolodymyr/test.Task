using LumenWorks.Framework.IO.Csv;
using System;
using System.Data;
using System.IO;
using System.Web;
using System.Web.Mvc;
using TestTask.Services.Interfaces;

namespace TestTask.Controllers
{
    public class MatrixController : Controller
    {
        IMatrixService _matrixService;

        public object postedFile { get; private set; }

        public MatrixController(IMatrixService matrixService)
        {
            _matrixService = matrixService;
        }

        public ActionResult Index()
        {
            return View(_matrixService.GenerateMatrix());
        }

        public ActionResult GenerateMatrix()
        {
            var matrix = _matrixService.GenerateMatrix();
            return PartialView("MatrixPartial", matrix);
        }

        public ActionResult TurnMatrix(short[][] arr)
        {
            var matrix = _matrixService.TurnMatrix(arr);
            return PartialView("MatrixPartial", matrix);
        }

        [HttpPost]
        public ActionResult UploadFile(HttpPostedFileBase file)
        {
            try
            {
                if (!Path.GetExtension(file.FileName).ToLower().Contains("csv"))
                {
                    Response.StatusCode = 400;
                    return Content("Invalid file type");
                }
                using (var ms = new MemoryStream())
                {
                    file.InputStream.CopyTo(ms);
                    byte[] bytes = ms.ToArray();
                    Stream stream = new MemoryStream(bytes);
                    DataTable csvTable = new DataTable();

                    using (CsvReader csvReader = new CsvReader(new StreamReader(stream), true))
                    {
                        csvTable.Load(csvReader);
                        var matrix = _matrixService.GetArrayFromTable(csvTable);
                        return PartialView("MatrixPartial", matrix); ;
                    }
                }
            }
            catch (Exception e)
            {
                ViewBag.Message = "File upload failed!";
                return View();
            }
        }

        public ActionResult GetDocumentLink(short[][] arr)
        {
            var fileName = Guid.NewGuid().ToString() + ".csv";
            var pathStr = "~/Files/";
            string path = Server.MapPath(pathStr);
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
            using (var fileStream = new FileStream(Server.MapPath(pathStr) + fileName, FileMode.CreateNew, FileAccess.ReadWrite))
            {
                using (StreamWriter writer = new StreamWriter(fileStream))
                {
                    int rows = arr.GetUpperBound(0) - arr.GetLowerBound(0) + 1;
                    for (var i = 0; i < rows; i++)
                    {
                        writer.WriteLine(string.Join(";", Array.ConvertAll(arr[i], x => x.ToString())) + ";");
                    }
                }
            }

            return Content(pathStr + fileName); ;
        }
    }
}
