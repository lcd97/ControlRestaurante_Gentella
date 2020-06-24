using ProyectoXalli_Gentella.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace ProyectoXalli_Gentella.Controllers.Catalogos
{
    public class TiposDeClienteController : Controller
    {
        private DBControl db = new DBControl();
        private bool completado = false;
        private string mensaje = "";

        // GET: TiposDeCliente
        public ActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// METODO PARA OBTENER EL DATA DE LA VISTA INDEX
        /// </summary>
        /// <returns></returns>
        public JsonResult getData()
        {
            var tipoC = from obj in db.TiposDeCliente.ToList()
                        where obj.EstadoTipoCliente = true
                        select new {
                            Id = obj.Id,
                            DescripcionTipo = obj.DescripcionTipoCliente,
                            CodigoTipo = obj.CodigoTipoCliente
                        };

            return Json(new { data = tipoC }, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// METODO GET DE LA VISTA CREATE
        /// </summary>
        /// <returns></returns>
        public ActionResult Create() 
        {
            return View();
        }

        // POST: TiposDeEntrada/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "Id,CodigoTipoCliente,DescripcionTipoCliente,EstadoTipoCliente")] TipoDeCliente TipoDeCliente)
        {
            TipoDeCliente cliente = db.TiposDeCliente.DefaultIfEmpty(null).FirstOrDefault(b => b.CodigoTipoCliente.Trim() == TipoDeCliente.CodigoTipoCliente.Trim());

            if (cliente != null) {
                ModelState.AddModelError("CodigoTipoEntrada", "Código ya utilizado");
                mensaje = "Código de Tipo de entrada ya existente";
            } else {

                //ESTADO DE TIPO DE ENTRADA CUANDO SE CREA SIEMPRE ES TRUE
                TipoDeCliente.EstadoTipoCliente = true;
                if (ModelState.IsValid) {
                    db.TiposDeCliente.Add(TipoDeCliente);
                    completado = await db.SaveChangesAsync() > 0 ? true : false;
                    mensaje = completado ? "Almacenado correctamente" : "Error al guardar";
                }
            }

            return Json(new { success = completado, message = mensaje }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Edit(int? id) 
        {
            if (id == null) {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TipoDeCliente TipoDeCliente = db.TiposDeCliente.Find(id);
            if (TipoDeCliente == null) {
                return HttpNotFound();
            }
            return View(TipoDeCliente);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,CodigoTipoCliente,DescripcionTipoCliente,EstadoTipoCliente")] TipoDeCliente TipoDeCliente) 
        {
            if (ModelState.IsValid) {
                db.Entry(TipoDeCliente).State = EntityState.Modified;
                completado = db.SaveChanges() > 0 ? true : false;
                mensaje = completado ? "Almacenado correctamente" : "Error al guardar";
            }

            return Json(new { success = completado, message = mensaje }, JsonRequestBehavior.AllowGet);
        }

        // POST: TiposDeCliente/Delete/5
        [HttpPost, ActionName("Delete")]
        //[ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id) {
            var TipoDeCliente = db.TiposDeCliente.Find(id);
            //BUSCANDO QUE TIPO DE ENTRADA NO TENGA SALIDAS NI ENTRADAS REGISTRADAS CON SU ID
            Cliente oCliente = db.Clientes.DefaultIfEmpty(null).FirstOrDefault(p => p.TipoClienteId == TipoDeCliente.Id);

            if (oCliente == null) {
                db.TiposDeCliente.Remove(TipoDeCliente);
                completado = await db.SaveChangesAsync() > 0 ? true : false;
                mensaje = completado ? "Eliminado correctamente" : "Se encontraron entradas en este tipo de entrada";
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