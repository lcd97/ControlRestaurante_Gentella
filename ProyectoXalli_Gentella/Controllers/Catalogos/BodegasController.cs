using ProyectoXalli_Gentella.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace ProyectoXalli_Gentella.Controllers
{
    public class BodegasController : Controller
    {
        private DBControl db = new DBControl();
        private bool completado = false;
        private string mensaje = "";

        // GET: Bodegas
        public ActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// RECUPERA DATOS PARA LLENAR LA TABLA BODEGA A TRAVES DE JSON
        /// </summary>
        /// <returns></returns>
        public async Task<JsonResult> GetData()
        {
            var bodegas = await db.Bodegas.Where(b => b.EstadoBodega == true).ToListAsync();

            return Json(new { data = bodegas }, JsonRequestBehavior.AllowGet);
        }

        // GET: Bodegas/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Bodega bodega = await db.Bodegas.FindAsync(id);
            if (bodega == null)
            {
                return HttpNotFound();
            }
            return View(bodega);
        }

        // GET: Bodegas/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Bodegas/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "Id,CodigoBodega,DescripcionBodega,EstadoBodega")] Bodega bodega)
        {
            Bodega bod = db.Bodegas.DefaultIfEmpty(null).FirstOrDefault(b => b.CodigoBodega.Trim() == bodega.CodigoBodega.Trim());

            if (bod != null)
            {
                ModelState.AddModelError("CodigoBodega", "Código ya utilizado");
                mensaje = "Código de bodega ya existente";
            }

            bodega.EstadoBodega = true;
            if (ModelState.IsValid)
            {
                db.Bodegas.Add(bodega);
                completado = await db.SaveChangesAsync() > 0 ? true : false;
                mensaje = completado ? "Almacenado correctamente" : "Error al guardar";
            }
            else
            {
                //ESTO ES PARA VER EL ERROR QUE DEVUELVE EL MODELO
                string cad = "";
                foreach (ModelState modelState in ViewData.ModelState.Values)
                {
                    foreach (ModelError error in modelState.Errors)
                    {
                        cad += (error);
                    }
                }
            }

            return Json(new { success = completado, message = mensaje }, JsonRequestBehavior.AllowGet);
        }

        // GET: Bodegas/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Bodega bodega = await db.Bodegas.FindAsync(id);
            if (bodega == null)
            {
                return HttpNotFound();
            }
            return View(bodega);
        }

        // POST: Bodegas/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "Id,CodigoBodega,DescripcionBodega,EstadoBodega")] Bodega bodega)
        {
            if (ModelState.IsValid)
            {
                db.Entry(bodega).State = EntityState.Modified;
                completado = await db.SaveChangesAsync() > 0 ? true : false;
                mensaje = completado ? "Almacenado correctamente" : "Error al guardar";
            }
            return Json(new { success = completado, message = mensaje }, JsonRequestBehavior.AllowGet);
        }

        // GET: Bodegas/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Bodega bodega = await db.Bodegas.FindAsync(id);
            if (bodega == null)
            {
                return HttpNotFound();
            }
            return View(bodega);
        }

        // POST: Bodegas/Delete/5
        [HttpPost, ActionName("Delete")]
        //[ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            var bodega = db.Bodegas.Find(id);
            //BUSCANDO QUE BODEGA NO TENGA SALIDAS NI ENTRADAS REGISTRADAS CON SU ID
            var oEnt = db.Entradas.DefaultIfEmpty(null).FirstOrDefault(e => e.BodegaId == bodega.Id);
            var oSal = db.Salidas.DefaultIfEmpty(null).FirstOrDefault(s => s.BodegaId == bodega.Id);

            if (oEnt == null || oSal == null)
            {
                db.Bodegas.Remove(bodega);
                completado = await db.SaveChangesAsync() > 0 ? true : false;
                mensaje = completado ? "Eliminado correctamente" : "Se encontraron salidas o entradas en esta bodega";
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