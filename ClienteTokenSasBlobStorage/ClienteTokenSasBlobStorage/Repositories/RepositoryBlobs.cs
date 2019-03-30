using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;

//NOTA: Se agregan los espacios de nombres necesarios:
using Microsoft.WindowsAzure.Storage.Blob; //FUNCIONALIDAD PARA BLOB

namespace ClienteTokenSasBlobStorage.Repositories
{
    public class RepositoryBlobs
    {
        CloudBlobContainer container;

        public RepositoryBlobs()
        {
            String key = this.GetTokenAcceso();
            container = new CloudBlobContainer(new Uri(key));
        }

        public string GetTokenAcceso()
        {
            WebClient cliente = new WebClient(); //NOTA: se debería usar "HttpClient"  porque es más moderno!!!
            //LE INDICAMOS EL TIPO DE CONSUMO QUE VAMOS
            //A REALIZAR
            cliente.Headers["Content-type"] = "application/json";

            String url = "http://localhost:51309/api/Token"; //NOTA: URL de nuestra API

            //DESCARGAMOS LA INFORMACION
            byte[] data = cliente.DownloadData(url);

            //ALMACENAMOS LA INFORMACION RECUPERADA EN UN STREAM
            MemoryStream ms = new MemoryStream();
            ms = new MemoryStream(data);

            //RECUPERAMOS LOS DATOS EN UN OBJETO STRING
            String datos = Encoding.UTF8.GetString(ms.ToArray());
            //SUSTITUIMOS LAS COMILLAS DOBLES PARA PODER RECUPERAR
            //LA URL "LIMPIA"
            //NOTA: Si en la API se ubiese devuelte la URI (con TOKEN) como un propiedad de un OBJETO, no haría falta quitar las comillas de al URI.
            datos = datos.Replace("\"", ""); 
            return datos;
        }

        public String GetContainerName()
        {
            return this.container.Name;
        }

        //NOTA: El metodo "GetBlobsNames()" NO se usa, creado solo para pruebas!!!
        public List<string> GetBlobsNames()
        {
            List<string> blobs = new List<string>();                        
            foreach (ICloudBlob blob in container.ListBlobs())
            {
                blobs.Add(blob.Name);
            }
            return blobs;
        }

        public List<ICloudBlob> GetBlobs()
        {
            List<ICloudBlob> blobs = new List<ICloudBlob>();
            foreach (ICloudBlob blob in container.ListBlobs())
            {
                blobs.Add(blob);
            }
            return blobs;
        }

        //SUBIR BLOBS
        //Fuente: https://www.codeproject.com/Tips/1011040/Upload-and-Delete-Video-File-to-Microsoft-Azure-Bl
        public void SubirBlobs(List<HttpPostedFileBase> archivos)
        {
            foreach (var archivo in archivos)
            {
                CloudBlockBlob blob = container.GetBlockBlobReference(archivo.FileName);
                blob.UploadFromStream(archivo.InputStream);
            }
        }

        public String EliminarBlob(String nombreArchivo)
        {
            String error = "";

            CloudBlockBlob blob = container.GetBlockBlobReference(nombreArchivo);
            try
            {
                blob.DeleteIfExists();
            }
            catch (Exception ex)
            {
                error = ex.Message + " NO TIENE TODOS LOS PERMISOS SAS";
            }
            return error;
        }
    }
}