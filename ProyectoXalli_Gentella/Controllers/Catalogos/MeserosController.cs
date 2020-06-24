using ProyectoXalli_Gentella.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Net;
using System.Threading.Tasks;

namespace ProyectoXalli_Gentella.Controllers.Catalogos
{
    public class MeserosController : Controller
    {
        private DBControl db = new DBControl();
        private bool completado = false;
        private string mensaje = "";

        // GET: Meseros
        public ActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// METODO PARA OBTENER LOS DATOS DE LA VISTA INDEX
        /// </summary>
        /// <returns></returns>
        public JsonResult GetData() {
            var meseros = (from obj in db.Meseros.ToList()
                           join d in db.Datos.ToList() on obj.DatoId equals d.Id
                           where obj.EstadoMesero == true
                           select new {
                               Id = obj.Id,
                               NombreMesero = d.PNombre + " " + d.PApellido,
                               HorarioTurno = obj.InicioTurno + " - " + obj.FinTurno,
                               Horario = obj.HoraEntrada + " - " + obj.HoraSalida
                           });

            return Json(new { data = meseros }, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// RETORNA LA VISTA CREATE
        /// </summary>
        /// <returns></returns>
        public ActionResult Create() 
        {
            return View();
        }

        /// <summary>
        /// METODO POST DE CREATE
        /// </summary>
        /// <param name="Nombres"></param>
        /// <param name="Apellido"></param>
        /// <param name="Cedula"></param>
        /// <param name="RUC"></param>
        /// <param name="estado"></param>
        /// <param name="HoraEntrada"></param>
        /// <param name="HoraSalida"></param>
        /// <param name="InicioTurno"></param>
        /// <param name="FinTurno"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Create(string Nombres, string Apellido, string Cedula, string INSS, string RUC, string HoraEntrada, string HoraSalida, string InicioTurno, string FinTurno) {
            //BUSCAR SI LA PERSONA EXISTE
            var persona = db.Datos.DefaultIfEmpty(null).FirstOrDefault(p => p.DNI.Trim() == Cedula.Trim());

            //SI SE ENCUENTRA REGISTRADO
            if (persona != null) {
                //SE BUSCA QUE EL COLABORADOR CON EL DATO ID
                var colaborador = db.Meseros.DefaultIfEmpty(null).FirstOrDefault(c => c.DatoId == persona.Id);

                //SI YA EXISTE EL COLABORADOR MANDAR ERROR
                if (colaborador != null) {
                    mensaje = "El colaborador ya se encuentra registrado";
                    return Json(new { success = completado, message = mensaje }, JsonRequestBehavior.AllowGet);
                } else {
                    //SI NO EXISTE REGISTRADO, AGREGARLO
                    Mesero mesero = new Mesero();

                    mesero.INSS = INSS;
                    mesero.HoraEntrada = HoraEntrada;
                    mesero.HoraSalida = HoraSalida;
                    mesero.InicioTurno = InicioTurno;
                    mesero.FinTurno = FinTurno;
                    mesero.DatoId = persona.Id;
                    mesero.EstadoMesero = true;

                    db.Meseros.Add(mesero);

                    completado = db.SaveChanges() > 0 ? true : false;
                    mensaje = completado ? "Almacenado Correctamente" : "Error al ingresar";
                }
            } else {
                //SI NO SE ENCUENTRA REGISTRADO
                Dato dato = new Dato();

                //LLENAMOS LA TABLA DE DATOS
                dato.DNI = Cedula;
                dato.PNombre = Nombres;
                dato.PApellido = Apellido;
                dato.RUC = RUC;

                db.Datos.Add(dato);

                //SI SE ALMACENO CORRECTAMENTE
                if (db.SaveChanges() > 0) {
                    Mesero waiter = new Mesero();

                    waiter.INSS = INSS;
                    waiter.HoraEntrada = HoraEntrada;
                    waiter.HoraSalida = HoraSalida;
                    waiter.InicioTurno = InicioTurno;
                    waiter.FinTurno = FinTurno;
                    waiter.DatoId = dato.Id;
                    waiter.EstadoMesero = true;

                    db.Meseros.Add(waiter);

                    completado = db.SaveChanges() > 0 ? true : false;
                    mensaje = completado ? "Almacenado Correctamente" : "Error al ingresar";
                }//FIN SAVECHANGES DATO               
            }//FIN ELSE

            return Json(new { success = completado, message = mensaje }, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// OBTIENE LA VISTA EDIT
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult Edit(int? id) {
            if (id == null) {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Mesero Mesero = db.Meseros.Find(id);
            if (Mesero == null) {
                return HttpNotFound();
            }

            return View(Mesero);
        }

        public ActionResult getMeseros(int id) {
            var mesero = from obj in db.Meseros
                         join a in db.Datos on obj.DatoId equals a.Id
                         where obj.Id == id
                         select new {
                             Nombre = a.PNombre,
                             Apellido = a.PApellido,
                             Cedula = a.DNI,
                             Inss = obj.INSS,
                             RUC = a.RUC,
                             EntradaH = obj.HoraEntrada,
                             SalidaH = obj.HoraSalida,
                             TurnoI = obj.InicioTurno,
                             TurnoF = obj.FinTurno,
                             Estado = obj.EstadoMesero
                         };

            return Json(new { data = mesero }, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// ACTUALIZA EL OBJETO MESERO
        /// </summary>
        /// <param name="Name"></param>
        /// <param name="lastName"></param>
        /// <param name="Cedula"></param>
        /// <param name="RUC"></param>
        /// <param name="HoraEntrada"></param>
        /// <param name="HoraSalida"></param>
        /// <param name="InicioTurno"></param>
        /// <param name="FinTurno"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Edit(string Nombres, string Apellido, string Cedula, string RUC, string HoraEntrada, string HoraSalida, string InicioTurno, string FinTurno, bool Estado) {
            var data = db.Datos.DefaultIfEmpty(null).FirstOrDefault(d => d.DNI.Trim() == Cedula.Trim());
            var waiter = db.Meseros.DefaultIfEmpty(null).FirstOrDefault(w => w.DatoId == data.Id);

            //ACTUALIZAR EL REGISTRO DEL MESERO
            if (waiter != null) {
                //EDITAR DATOS PRINCIPALES
                data.PNombre = Nombres;
                data.PApellido = Apellido;
                data.RUC = RUC;

                db.Entry(data).State = EntityState.Modified;

                //GUARDAMOS LOS CAMBIOS
                if (db.SaveChanges() > 0) {
                    //ACTUALIZAR DATOS DE MESERO
                    waiter.HoraEntrada = HoraEntrada;
                    waiter.HoraSalida = HoraSalida;
                    waiter.InicioTurno = InicioTurno;
                    waiter.FinTurno = FinTurno;
                    waiter.EstadoMesero = Estado;

                    db.Entry(waiter).State = EntityState.Modified;
                    completado = db.SaveChanges() > 0 ? true : false;
                    mensaje = completado ? "Almacenado correctamente" : "Error. Hubo error al editar";
                }             
            } else {//NO SE ENCONTRO EL REGISTRO COMO MESERO
                mensaje = "La persona a modificar no se encuentra registrado";
            }

            return Json(new { success = completado, message = mensaje }, JsonRequestBehavior.AllowGet);
        }

        // GET: Meseros/Details/5
        public async Task<ActionResult> Details(int? id) {
            if (id == null) {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Mesero mesero = await db.Meseros.FindAsync(id);
            if (mesero == null) {
                return HttpNotFound();
            }
            return View(mesero);
        }

        // POST: Meseros/Delete/5
        [HttpPost, ActionName("Delete")]
        //[ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id) {
            var mesero = db.Meseros.Find(id);
            //BUSCANDO QUE Categoria NO TENGA SALIDAS NI ENTRADAS REGISTRADAS CON SU ID
            Orden orden = db.Ordenes.DefaultIfEmpty(null).FirstOrDefault(p => p.MeseroId == mesero.Id);

            if (orden == null) {
                db.Meseros.Remove(mesero);
                completado = await db.SaveChangesAsync() > 0 ? true : false;
                mensaje = completado ? "Eliminado correctamente" : "Se encontraron ordenes realizadas con este mesero";
            }

            return Json(new { success = completado, message = mensaje }, JsonRequestBehavior.AllowGet);
        }

        protected override void Dispose(bool disposing) {
            if (disposing) {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}