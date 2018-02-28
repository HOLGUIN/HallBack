using System.Web;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System;
using Hallearn.Data;
using Hallearn.Utility;
using System.Net.Http.Headers;
using System.Net.Http;
using System.Net;
using System.Web.Http;
using System.Web.Http.Results;


namespace Hallearn.Model.Model
{
    class archivoModels
    {
    }

    public class archivo
    {
        public int hlnarchivoid { get; set; }
        public string extension { get; set; }
        public string titulo { get; set; }
        public string filename { get; set; }
        public int hlnclaseid { get; set; }
    }


    public class archivoProcesos : ApiController
    {

        db_HallearnEntities context = new db_HallearnEntities();

        public response crearArchivo(System.Web.HttpFileCollection files, int hlnclaseid)
        {
            response response = new response();


            if (files.Count > 0)
            {
                string fpath = System.Web.Hosting.HostingEnvironment.MapPath("~/App_Data/uploads/");
                List<archivo> la = new List<archivo>();
                MD5Hash md5 = new MD5Hash();

                for (int i = 0; i < files.Count; i++)
                {
                    System.Web.HttpPostedFile f = files[i];
                    var filename = new FileInfo(f.FileName);

                    if (f.ContentLength > 0)
                    {

                        string filenameid = md5.CalculateMD5Hash(filename.Name + DateTime.Now);

                        if (!File.Exists(fpath + Path.GetFileName(filenameid)))
                        {
                            f.SaveAs(fpath + Path.GetFileName(filenameid));

                            hlnarchivo modelo = new hlnarchivo()
                            {
                                filename = filenameid,
                                titulo = filename.Name,
                                extension = filename.Extension,
                                hlnclaseid = hlnclaseid
                            };

                            context.hlnarchivo.Add(modelo);
                            context.SaveChanges();
                            archivo modeloa = new archivo()
                            {
                                filename = modelo.filename,
                                titulo = modelo.titulo,
                                extension = modelo.extension,
                                hlnclaseid = modelo.hlnclaseid,
                                hlnarchivoid = modelo.hlnarchivoid
                            };

                            la.Add(modeloa);
                        }
                    }
                }
                response.modelo = la;
                response.valida = true;
            }
            else
            {
                response.valida = false;
            }
            return response;
        }

        public List<archivo> getArchivos(int hlnclaseid)
        {
            var archivos = context.hlnarchivo.Where(x => x.hlnclaseid == hlnclaseid).Select(x => new archivo()
            {
                hlnarchivoid = x.hlnarchivoid,
                extension = x.extension,
                titulo = x.titulo,
                filename = x.filename,
                hlnclaseid = x.hlnclaseid
            }).ToList();

            return archivos;
        }

        public HttpResponseMessage DescargarArchivos(int hlnarchivoid)
        {
            // var request = HttpContext.Current.Request;
            try
            {
                hlnarchivo archivo = context.hlnarchivo.Find(hlnarchivoid);
                if (archivo != null)
                {
                    string filepath = HttpContext.Current.Server.MapPath("~/App_Data/uploads/" + archivo.filename);
                    //using (MemoryStream ms = new MemoryStream())
                    //{
                    //    using (FileStream file = new FileStream(filepath, FileMode.Open, FileAccess.Read))
                    //    {

                    //file.Position = 0;
                    //byte[] bytes = new byte[file.Length];
                    //file.Read(bytes, 0, Convert.ToInt32(file.Length));
                    //ms.Write(bytes, 0, Convert.ToInt32(file.Length));
                    Stream bytes = File.OpenRead(filepath);
                    HttpResponseMessage httpResponseMessage = new HttpResponseMessage(HttpStatusCode.OK);
                    httpResponseMessage.Content = new StreamContent(bytes);
                    //  httpResponseMessage.Content = new 
                    httpResponseMessage.Content.Headers.Add("x-filename", archivo.titulo);
                    httpResponseMessage.Content.Headers.ContentType = new MediaTypeHeaderValue("text/plain");
                    httpResponseMessage.Content.Headers.ContentDisposition = new ContentDispositionHeaderValue("attachment");
                    httpResponseMessage.Content.Headers.ContentDisposition.FileName = archivo.titulo;
                    httpResponseMessage.Content.Headers.ContentLength = bytes.Length;
                    return httpResponseMessage;
                    //    }
                    //}
                }
                else
                {
                    return null;
                }

            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public bool eliminarArchivo(int hlnarchivoid)
        {
            var archivo = context.hlnarchivo.Find(hlnarchivoid);
            if (archivo != null)
            {
                string filepath = HttpContext.Current.Server.MapPath("~/App_Data/uploads/" + archivo.filename);
                context.hlnarchivo.Remove(archivo);
                context.SaveChanges();
                File.Delete(filepath);
                return true;
            }
            return false;
        }

    }
}
