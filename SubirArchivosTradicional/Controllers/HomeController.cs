using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SubirArchivosTradicional.Controllers
{
    public class HomeController : Controller
    {
        private HomeController h = null;
        private List<object> res = new List<object>();
        private string type = "application/json";

        // GET: Home
        public ActionResult Index() => View();

        [HttpPost]

        public ActionResult SubirArchivo(string nombreAr, HttpPostedFileBase file)
        {
            this.h = new HomeController();
            this.res.Add(this.h.CargarArchivos(nombreAr, file));            
            return Json(this.res, this.type,JsonRequestBehavior.AllowGet);
        }

        public ActionResult SubirMultiArchivo(HttpPostedFileBase[] files)
        {
            this.h = new HomeController();
            files.ToList().ForEach(f => this.res.Add(h.CargarArchivos(null, f)));
            return Json(this.res, this.type, JsonRequestBehavior.AllowGet);
        }

        private  object CargarArchivos(string nombreAr, HttpPostedFileBase file)
        {
            string path = Path.Combine(HttpRuntime.AppDomainAppPath, "Archivos\\"); 
            string mensaje = string.Empty;
            string estatus = string.Empty;
        
            try
            {
                if (file != null && file.ContentLength > 0 && file.ContentType.Equals("application/pdf"))
                {

                    mensaje = $"El archivo {file.FileName} se cargo correctamente";
                    estatus = "Se cargo con exito";

                    if( nombreAr == null)
                    {
                        file.SaveAs(path +  Guid.NewGuid().ToString() +"-"+ file.FileName);
                    }else
                    {
                        file.SaveAs(path + (nombreAr.Trim().Equals("") ? (Guid.NewGuid().ToString()+ "-"+ file.FileName) : nombreAr+ "-"+file.FileName));
                    }
                    

                }
                else
                {
                    mensaje = "El archivo no es pdf, favor de seleccionar otro";
                    estatus = "No cargado";

                }
            }
            catch (Exception e)
            {
                mensaje = e.Message;
                estatus = "No cargado";

            }
   

            return new { mensaje, estatus };
        }


    }
}