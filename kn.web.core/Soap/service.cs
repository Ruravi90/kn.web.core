using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using kn.web.core.Models;
using kn.web.core.Soap;
using BC = BCrypt.Net.BCrypt;

namespace kn.web.core.Soap
{
    public class Service : IService
    {
        public Models.EFContext Db { get; }
        public Service(EFContext db)
        {
            Db = db;
        }

        public List<Models.Event> GetGPSEvents(Filter filter)
        {
            List<Models.Event> _events;
            var user = Db.Users.Where(u => u.UserName == filter.UserName).FirstOrDefault();

            if (!BC.Verify(filter.Password, user.Password))
            {
                _events = new List<Models.Event>();
                _events.Add(new Models.Event
                {
                    antena = "Usuario no autorizado"
                });

                return _events;
            }

            var obj = Db.Events.AsQueryable();

            filter.Fecha_Fin = filter.Fecha_Fin.Replace("?", "");

            DateTime temp;
            if (!DateTime.TryParse(filter.Fecha_Ini, out temp)) {
                throw  new Exception("Fecha_Ini invalida");
            }
            else if (filter.Fecha_Fin != null && filter.Fecha_Fin != "" && !DateTime.TryParse(filter.Fecha_Fin, out temp))
            {
                throw new Exception("Fecha_Fin invalida");
            }

            if (filter.Fecha_Ini != null)
                obj = obj.Where(o=> o.fecha_evento >= DateTime.Parse(filter.Fecha_Ini));
            if(filter.Fecha_Fin != null && filter.Fecha_Fin != "")
                obj = obj.Where(o => o.fecha_evento <= DateTime.Parse(filter.Fecha_Fin));

            _events = obj.ToList();

            //Db.Events.RemoveRange(_events);
            //Db.SaveChanges();

            return _events;
        }
    }
}