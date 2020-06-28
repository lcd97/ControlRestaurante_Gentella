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
            //BUSCAR LA DESCRIPCION DE TIPO DE CLIENTE EN LA BD
            TipoDeCliente cliente = db.TiposDeCliente.DefaultIfEmpty(null).FirstOrDefault(b => b.DescripcionTipoCliente.ToUpper().Trim() == TipoDeCliente.DescripcionTipoCliente.ToUpper().Trim());

            //SI SE ENCONTRO YA UN TIPO DE CLIENTE
            if (cliente != null)
            {
                ModelState.AddModelError("DescripcionTipoCliente", "Utilice otro nombre");
                mensaje = "La descripción ya se encuentra registrada";
            }
            else
            {
                //ESTADO DE TIPO DE ENTRADA CUANDO SE CREA SIEMPRE ES TRUE
                TipoDeCliente.EstadoTipoCliente = true;
                if (ModelState.IsValid)
                {
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
            //BUSCAR LA DESCRIPCION DE TIPO DE CLIENTE EN LA BD
            TipoDeCliente cliente = db.TiposDeCliente.DefaultIfEmpty(null).FirstOrDefault(b => b.DescripcionTipoCliente.ToUpper().Trim() == TipoDeCliente.DescripcionTipoCliente.ToUpper().Trim() && b.Id != TipoDeCliente.Id);

            //SI SE ENCONTRO YA UN TIPO DE CLIENTE
            if (cliente != null)
            {
                ModelState.AddModelError("DescripcionTipoCliente", "Utilice otro nombre");
                mensaje = "La descripción ya se encuentra registrada";
            }
            else
            {
                if (ModelState.IsValid)
                {
                    db.Entry(TipoDeCliente).State = EntityState.Modified;
                    completado = db.SaveChanges() > 0 ? true : false;
                    mensaje = completado ? "Almacenado correctamente" : "Error al guardar";
                }
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

            //SI NO SE ENCONTRARON CLIENTES EN ESTE TIPO DE CLIENTE
            if (oCliente == null)
            {
                db.TiposDeCliente.Remove(TipoDeCliente);
                completado = await db.SaveChangesAsync() > 0 ? true : false;
                mensaje = completado ? "Eliminado correctamente" : "Error al eliminar";
            }
            else {
                mensaje = "Se encontraron clientes asociados a este tipo de cliente";
            }

            return Json(new { success = completado, message = mensaje }, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// RETORNA EL CODIGO AUTOMATICAMENTE A LA VISTA CREATE
        /// </summary>
        /// <returns></returns>
        public ActionResult SearchCode()
        {
            //BUSCAR EL VALOR MAXIMO DE LAS BODEGAS REGISTRADAS
            var code = db.TiposDeCliente.Max(x => x.CodigoTipoCliente.Trim());
            int valor;
            string num;

            //SI EXISTE ALGUN REGISTRO
            if (code != null)
            {
                //CONVERTIR EL CODIGO A ENTERO
                valor = int.Parse(code);

                //SE COMIENZA A AGREGAR UN VALOR SECUENCIAL AL CODIGO ENCONTRADO
                if (valor <= 8)
                    num = "00" + (valor + 1);
                else
                if (valor >= 9 && valor < 100)
                    num = "0" + (valor + 1);
                else
                    num = (valor + 1).ToString();
            }
            else
                num = "001";//SE COMIENZA CON EL PRIMER CODIGO DEL REGISTRO

            return Json(new { data = num }, JsonRequestBehavior.AllowGet);
        }

        protected override void Dispose(bool disposing) {
            if (disposing) {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}