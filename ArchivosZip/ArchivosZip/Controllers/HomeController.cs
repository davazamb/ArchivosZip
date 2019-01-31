using ArchivosZip.Models;
using Ionic.Zip;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ArchivosZip.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            string[] filePaths = Directory.GetFiles(Server.MapPath("~/Files/"));
            List<ArchivoModel> files = new List<ArchivoModel>();
            foreach (string filePath in filePaths)
            {
                files.Add(new ArchivoModel()
                {
                    FileName = Path.GetFileName(filePath),
                    FilePath = filePath
                });
            }

            return View(files);
        }

        [HttpPost]
        public ActionResult Index(List<ArchivoModel> files)
        {
            using (ZipFile zip = new ZipFile())
            {
                zip.AlternateEncodingUsage = ZipOption.AsNecessary;
                zip.AddDirectoryByName("Files");
                foreach (ArchivoModel file in files)
                {
                    if (file.IsSelected)
                    {
                        zip.AddFile(file.FilePath, "Files");
                    }
                }
                string zipName = String.Format("Comprimido_{0}.zip", DateTime.Now.ToString("yyyyMMdd-HHmm"));
                using (MemoryStream memoryStream = new MemoryStream())
                {
                    zip.Save(memoryStream);
                    return File(memoryStream.ToArray(), "application/zip", zipName);
                }
            }


        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}