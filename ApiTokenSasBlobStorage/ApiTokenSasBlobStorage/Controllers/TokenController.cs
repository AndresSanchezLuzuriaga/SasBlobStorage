using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

//NOTA: Se agregan los espacios de nombres necesarios:
using Microsoft.Azure; //FUNCIONALIDAD PARA LAS KEYS
using Microsoft.WindowsAzure.Storage; //FUNCIONALIDAD PARA LA CUENTA STORAGE
using Microsoft.WindowsAzure.Storage.Blob; //FUNCIONALIDAD PARA BLOB

namespace ApiTokenSasBlobStorage.Controllers
{
    public class TokenController : ApiController
    {
        //METODO PARA DEVOLVER EL CONTENEDOR
        //PARA GENERAR SU CLAVE POSTERIORMENTE
        public CloudBlobContainer RecuperarContenedor()
        {
            String keys = CloudConfigurationManager.GetSetting("cuentablobstorageamsl");
            CloudStorageAccount account = CloudStorageAccount.Parse(keys);
            CloudBlobClient client = account.CreateCloudBlobClient();
            CloudBlobContainer container = client.GetContainerReference("contenedorblobs"); //NOTA: los nombres de los CONTENEDORES siempre tienen que ser en MINUSCULAS.
            //container.CreateIfNotExists();
            return container;
        }


        public String Get()
        {
            CloudBlobContainer container = this.RecuperarContenedor();
            SharedAccessBlobPolicy permisosSas = new SharedAccessBlobPolicy();
            permisosSas.SharedAccessExpiryTime = DateTime.Now.AddMinutes(10);
            //NOTA: El simbolo "|" realiza una suma sustractiva, es decir, permiso 1 + permiso 2, etc
            permisosSas.Permissions = SharedAccessBlobPermissions.Create
                | SharedAccessBlobPermissions.List
                | SharedAccessBlobPermissions.Read
                | SharedAccessBlobPermissions.Write;
                //| SharedAccessBlobPermissions.Delete; //NOTA: Si se quita un PERMISO SAS, la APLICACION CLIENTE NO tendrá acceso a realizar la accion que representa.
            String token = container.GetSharedAccessSignature(permisosSas);
            //PARA DAR ACCESO, EL TOKEN VA DENTRO DEL URI
            //DEL CONTENEDOR
            //NOTA: lo suyo es devolver la URI (con TOKEN) como un OBJETO, es decir, el TOKEN como una propiedad, ahora mismo esta devuelto como un STRING.
            return container.Uri + token; 
        }
    }
}
