using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using ClienteTokenSasBlobStorage.Repositories;

namespace ClienteTokenSasBlobStorage.Controllers
{
    public class HomeController : Controller
    {
        RepositoryBlobs repo;

        public HomeController()
        {
            this.repo = new RepositoryBlobs();
        }

        public ActionResult Index()
        {
            ViewBag.Error = TempData["ERROR"];

            ViewBag.ContainerName = this.repo.GetContainerName();
            return View(this.repo.GetBlobs());
        }
               
        [HttpPost]
        public ActionResult SubirBlobs(List<HttpPostedFileBase> archivos)
        {
            this.repo.SubirBlobs(archivos);
            return RedirectToAction("Index", "Home");
        }

        public ActionResult EliminarBlob(String nombreArchivo)
        {
            String error = this.repo.EliminarBlob(nombreArchivo);
            TempData["ERROR"] = error;
            return RedirectToAction("Index", "Home");
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

        #region CODIGO DE PRUEBA: BORRAR!!!
        //[HttpPost]
        //public async Task<IActionResult> UploadFiles(IList<IFormFile> files)
        //{
        //    long size = 0;
        //    try
        //    {
        //        foreach (var file in files)
        //        {
        //            var fileName = ContentDispositionHeaderValue
        //                .Parse(file.ContentDisposition)
        //                .FileName
        //                .Trim('"');
        //            CloudBlockBlob blockBlob = cloudBlobContainer.GetBlockBlobReference(fileName);
        //            var stream = file.OpenReadStream();
        //            size = file.Length;
        //            await blockBlob.UploadFromStreamAsync(stream);
        //        }
        //    }
        //    catch (Exception)
        //    {
        //        Response.StatusCode = (int)HttpStatusCode.BadRequest;
        //        return Json("Subida fallida. Por favor, intentelo otra vez.");
        //    }
        //    return View("Index");
        //}

        //https://anexsoft.com/como-subir-un-archivo-con-asp-net-mvc
        //[HttpPost]
        //public ActionResult UploadFiles(HttpPostedFileBase file)
        //{
        //    //OpenFileDialog ofd = new OpenFileDialog();
        //    ////Recuperamos el fichero para subir al contenedor
        //    //if (ofd.ShowDialog() == DialogResult.OK)
        //    //{
        //        ////Recuperamos el nombre del archivo
        //        //String path = ofd.FileName;
        //        //String path = file.FileName;
        //        ////Vamos a recuperar el nombre del Blob del FileName
        //        //int ultimabarra = path.LastIndexOf(@"\") + 1;
        //        //String filename = path.Substring(ultimabarra);
        //        ////Recuperamos una referencia al contenedor
        //        //String key = this.GetTokenAcceso();
        //        //CloudBlobContainer contenedor = new CloudBlobContainer(new Uri(key));
        //        ////Recuperamos una referencia para escribir un BLOB llamado  //como la imagen
        //        //CloudBlockBlob blob = contenedor.GetBlockBlobReference(filename);
        //        CloudBlockBlob blob = container.GetBlockBlobReference(file.FileName);

        //        ////Escribimos el archivo en el Blob
        //        //using (var stream = System.IO.File.OpenRead(path))
        //        //{
        //        //    //SUBIMOS EL ARCHIVO MEDIANTE STREAM
        //        //    blob.UploadFromStream(stream);
        //        //}


        //        blob.UploadFromStream(file.InputStream);

        //    //    MessageBox.Show("Blob subido correctamente");
        //    //}
        //    return View("Index");
        //}


        //public ActionResult Descargar(String nombreArchivo)
        //{
        //    //String sas = this.txttoken.Text;
        //    //CloudBlobContainer contenedor = new CloudBlobContainer(new Uri(sas));
        //    //String nombreblob = this.lstblobs.SelectedItem.ToString();
        //    //CloudBlockBlob blob = contenedor.GetBlockBlobReference(nombreblob);
        //    ////NOTA: la imagen se tiene que cargar en MEMORIA para obtener la imagen porque el acceso es PRIVADO (debido a los permisos SAS). 
        //    ////Si el acceso fuese PRIVADO, para mostrar la imagen bastaría con usar el metodo "Load()". 
        //    //MemoryStream msRead = new MemoryStream();
        //    //msRead.Position = 0;
        //    //blob.DownloadToStream(msRead);
        //    //Console.WriteLine(msRead.Length);
        //    //this.pctblob.Image = Image.FromStream(msRead);


        //    CloudBlockBlob blob = container.GetBlockBlobReference(nombreArchivo);
        //    //NOTA: la imagen se tiene que cargar en MEMORIA para obtener la imagen porque el acceso es PRIVADO (debido a los permisos SAS). 
        //    //Si el acceso fuese PRIVADO, para mostrar la imagen bastaría con usar el metodo "Load()". 
        //    MemoryStream msRead = new MemoryStream();
        //    msRead.Position = 0;
        //    blob.DownloadToStream(msRead);
        //    //Console.WriteLine(msRead.Length);
        //    Image img = new Image(); 
        //    img = Image.FromStream(msRead);

        //    return View();
        //}
        #endregion
    }
}