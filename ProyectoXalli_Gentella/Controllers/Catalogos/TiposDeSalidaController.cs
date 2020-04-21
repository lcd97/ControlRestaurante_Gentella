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
    public class TiposDeSalidaController : Controller
    {
        private DBControl db = new DBControl();
        private bool completado = false;
        private string mensaje = "";

        // GET: TiposDeSalida
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
            var tiposDeSalida = await db.TiposDeSalida.Where(c => c.EstadoTipoSalida == true).ToListAsync();

            return Json(new { data = tiposDeSalida }, JsonRequestBehavior.AllowGet);
        }

        // GET: TiposDeSalida/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TipoDeSalida tiposDeSalida = await db.TiposDeSalida.FindAsync(id);
            if (tiposDeSalida == null)
            {
                return HttpNotFound();
            }
            return View(tiposDeSalida);
        }

        // GET: TiposDeSalida/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: TiposDeSalida/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "Id,CodigoTipoSalida,DescripcionTipoSalida,EstadoTipoSalida")] TipoDeSalida TipoDeSalida)
        {
            TipoDeSalida bod = db.TiposDeSalida.DefaultIfEmpty(null).FirstOrDefault(b => b.CodigoTipoSalida.Trim() == TipoDeSalida.CodigoTipoSalida.Trim());

            if (bod != null)
            {
                ModelState.AddModelError("CodigoTipoSalida", "Código ya utilizado");
                mensaje = "Código de Tipo de salidas ya existente";
            }

            //ESTADO DE TIPO DE SALIDAS CUANDO SE CREA SIEMPRE ES TRUE
            TipoDeSalida.EstadoTipoSalida = true;
            if (ModelState.IsValid)
            {
                db.TiposDeSalida.Add(TipoDeSalida);
                completado = await db.SaveChangesAsync() > 0 ? true : false;
                mensaje = completado ? "Almacenado correctamente" : "Error al guardar";
            }

            return Json(new { success = completado, message = mensaje }, JsonRequestBehavior.AllowGet);
        }

        // GET: TiposDeSalida/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TipoDeSalida TipoDeSalida = await db.TiposDeSalida.FindAsync(id);
            if (TipoDeSalida == null)
            {
                return HttpNotFound();
            }
            return View(TipoDeSalida);
        }

        // POST: TiposDeSalida/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "Id,CodigoTipoSalida,DescripcionTipoSalida,EstadoTipoSalida")] TipoDeSalida TipoDeSalida)
        {
            if (ModelState.IsValid)
            {
                db.Entry(TipoDeSalida).State = EntityState.Modified;
                completado = await db.SaveChangesAsync() > 0 ? true : false;
                mensaje = completado ? "Almacenado correctamente" : "Error al guardar";
            }
            return Json(new { success = completado, message = mensaje }, JsonRequestBehavior.AllowGet);
        }

        // POST: TiposDeSalida/Delete/5
        [HttpPost, ActionName("Delete")]
        //[ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            var TipoDeSalida = db.TiposDeSalida.Find(id);
            //BUSCANDO QUE TIPO DE SALIDA NO TENGA SALIDAS NI SALIDAS REGISTRADAS CON SU ID
            Salida oSalida = db.Salidas.DefaultIfEmpty(null).FirstOrDefault(p => p.TipoSalidaId == TipoDeSalida.Id);

            if (oSalida == null)
            {
                db.TiposDeSalida.Remove(TipoDeSalida);
                completado = await db.SaveChangesAsync() > 0 ? true : false;
                mensaje = completado ? "Eliminado correctamente" : "Se encontraron salidas en este tipo de salida";
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