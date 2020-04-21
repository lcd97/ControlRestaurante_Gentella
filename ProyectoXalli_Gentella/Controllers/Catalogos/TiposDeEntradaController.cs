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
    public class TiposDeEntradaController : Controller
    {
        private DBControl db = new DBControl();
        private bool completado = false;
        private string mensaje = "";

        // GET: TiposDeEntrada
        public ActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// RECUPERA DATOS PARA LLENAR LA TABLA CATEGORIAS A TRAVES DE JSON
        /// </summary>
        /// <returns></returns>
        public async Task<JsonResult> GetData()
        {
            var tiposDeEntrada = await db.TiposDeEntrada.Where(c => c.EstadoTipoEntrada == true).ToListAsync();

            return Json(new { data = tiposDeEntrada }, JsonRequestBehavior.AllowGet);
        }

        // GET: TiposDeEntrada/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TipoDeEntrada tiposDeEntrada = await db.TiposDeEntrada.FindAsync(id);
            if (tiposDeEntrada == null)
            {
                return HttpNotFound();
            }
            return View(tiposDeEntrada);
        }

        // GET: TiposDeEntrada/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: TiposDeEntrada/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "Id,CodigoTipoEntrada,DescripcionTipoEntrada,EstadoTipoEntrada")] TipoDeEntrada TipoDeEntrada)
        {
            TipoDeEntrada bod = db.TiposDeEntrada.DefaultIfEmpty(null).FirstOrDefault(b => b.CodigoTipoEntrada.Trim() == TipoDeEntrada.CodigoTipoEntrada.Trim());

            if (bod != null)
            {
                ModelState.AddModelError("CodigoTipoEntrada", "Código ya utilizado");
                mensaje = "Código de Tipo de entrada ya existente";
            }

            //ESTADO DE TIPO DE ENTRADA CUANDO SE CREA SIEMPRE ES TRUE
            TipoDeEntrada.EstadoTipoEntrada = true;
            if (ModelState.IsValid)
            {
                db.TiposDeEntrada.Add(TipoDeEntrada);
                completado = await db.SaveChangesAsync() > 0 ? true : false;
                mensaje = completado ? "Almacenado correctamente" : "Error al guardar";
            }

            return Json(new { success = completado, message = mensaje }, JsonRequestBehavior.AllowGet);
        }

        // GET: TiposDeEntrada/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TipoDeEntrada TipoDeEntrada = await db.TiposDeEntrada.FindAsync(id);
            if (TipoDeEntrada == null)
            {
                return HttpNotFound();
            }
            return View(TipoDeEntrada);
        }

        // POST: TiposDeEntrada/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "Id,CodigoTipoEntrada,DescripcionTipoEntrada,EstadoTipoEntrada")] TipoDeEntrada TipoDeEntrada)
        {
            if (ModelState.IsValid)
            {
                db.Entry(TipoDeEntrada).State = EntityState.Modified;
                completado = await db.SaveChangesAsync() > 0 ? true : false;
                mensaje = completado ? "Almacenado correctamente" : "Error al guardar";
            }
            return Json(new { success = completado, message = mensaje }, JsonRequestBehavior.AllowGet);
        }

        // POST: TiposDeEntrada/Delete/5
        [HttpPost, ActionName("Delete")]
        //[ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            var TipoDeEntrada = db.TiposDeEntrada.Find(id);
            //BUSCANDO QUE TIPO DE ENTRADA NO TENGA SALIDAS NI ENTRADAS REGISTRADAS CON SU ID
            Entrada oEntrada = db.Entradas.DefaultIfEmpty(null).FirstOrDefault(p => p.TipoEntradaId == TipoDeEntrada.Id);

            if (oEntrada == null)
            {
                db.TiposDeEntrada.Remove(TipoDeEntrada);
                completado = await db.SaveChangesAsync() > 0 ? true : false;
                mensaje = completado ? "Eliminado correctamente" : "Se encontraron entradas en este tipo de entrada";
            }

            return Json(new { success = completado, message = mensaje }, JsonRequestBehavior.AllowGet);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}