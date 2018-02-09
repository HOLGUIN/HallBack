using Hallearn.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hallearn.Model.Model
{
    class chatModels
    {
    }

    public class chat
    {
        public int hlnchatid { get; set; }
        public string mensaje { get; set; }
        public DateTime fecha { get; set; }
        public int hlnclaseid { get; set; }
        public int hlnusuarioid { get; set; }
        public string username { get; set; }
    }

    public class chatProcesos
    {
        db_HallearnEntities context = new db_HallearnEntities();

        public void savechat(chat msg)
        {
            hlnchat modelo = new hlnchat()
            {
                fecha = DateTime.Now,
                mensaje = msg.mensaje,
                hlnclaseid = msg.hlnclaseid,
                hlnusuarioid = msg.hlnusuarioid,
                username = msg.username
            };

            context.hlnchat.Add(modelo);
            context.SaveChanges();
        }

        public List<chat> getchat(int hlnclaseid)
        {
            var chats = context.hlnchat.Where(x => x.hlnclaseid == hlnclaseid).Select(x => new chat {
                hlnchatid = x.hlnchatid,
                hlnclaseid = x.hlnclaseid,
                hlnusuarioid = x.hlnusuarioid,
                fecha = x.fecha,
                mensaje = x.mensaje,
                username = x.username
            }).ToList();

            return chats;
        }


    }



}
