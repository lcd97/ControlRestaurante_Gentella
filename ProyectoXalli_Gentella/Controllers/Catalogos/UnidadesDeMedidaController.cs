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
    public class UnidadesDeMedidaController : Controller
    {
        private DBControl db = new DBControl();
        private bool completado = false;
        private string mensaje = "";

        // GET: UnidadesDeMedida
        public ActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// RECUPERA DATOS PARA LLENAR LA TABLA UNIDADES DE MEDIDA A TRAVES DE JSON
        /// </summary>
        /// <returns></returns>
        public JsonResult GetData()
        {
            var unidades = (from u in db.UnidadesDeMedida.ToList()
                            where u.EstadoUnidadMedida == true
                            select new
                            {
                                Id = u.Id,
                                CodigoUnidadMedida = u.CodigoUnidadMedida,
                                DescripcionUnidadMedida = u.DescripcionUnidadMedida + " - " + u.AbreviaturaUM,
                            });

            return Json(new { data = unidades }, JsonRequestBehavior.AllowGet);
        }

        // GET: UnidadesDeMedida/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            UnidadDeMedida unidad = await db.UnidadesDeMedida.FindAsync(id);
            if (unidad == null)
            {
                return HttpNotFound();
            }
            return View(unidad);
        }

        // GET: UnidadDeMedida/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: UnidadDeMedida/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "Id,CodigoUnidadMedida,DescripcionUnidadMedida,AbreviaturaUM,EstadoUnidadMedida")] UnidadDeMedida UnidadDeMedida)
        {
            UnidadDeMedida bod = db.UnidadesDeMedida.DefaultIfEmpty(null).FirstOrDefault(b => b.CodigoUnidadMedida.Trim() == UnidadDeMedida.CodigoUnidadMedida.Trim());

            if (bod != null)
            {
                ModelState.AddModelError("CodigoUnidadMedida", "Código ya utilizado");
                mensaje = "Código de unidad de medida ya existente";
            }

            //ESTADO DE UNIDADES DE MEDIDA CUANDO SE CREA SIEMPRE ES TRUE
            UnidadDeMedida.EstadoUnidadMedida = true;
            if (ModelState.IsValid)
            {
                db.UnidadesDeMedida.Add(UnidadDeMedida);
                completado = await db.SaveChangesAsync() > 0 ? true : false;
                mensaje = completado ? "Almacenado correctamente" : "Error al guardar";
            }

            return Json(new { success = completado, message = mensaje }, JsonRequestBehavior.AllowGet);
        }

        // GET: UnidadDeMedida/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            UnidadDeMedida UnidadDeMedida = await db.UnidadesDeMedida.FindAsync(id);
            if (UnidadDeMedida == null)
            {
                return HttpNotFound();
            }
            return View(UnidadDeMedida);
        }

        // POST: UnidadDeMedida/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "Id,CodigoUnidadMedida,DescripcionUnidadMedida,AbreviaturaUM,EstadoUnidadMedida")] UnidadDeMedida UnidadDeMedida)
        {
            if (ModelState.IsValid)
            {
                db.Entry(UnidadDeMedida).State = EntityState.Modified;
                completado = await db.SaveChangesAsync() > 0 ? true : false;
                mensaje = completado ? "Almacenado correctamente" : "Error al guardar";
            }
            return Json(new { success = completado, message = mensaje }, JsonRequestBehavior.AllowGet);
        }

        // POST: UnidadDeMedida/Delete/5
        [HttpPost, ActionName("Delete")]
        //[ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            var unidadDeMedida = db.UnidadesDeMedida.Find(id);
            //BUSCANDO QUE UNIDAD DE MEDIDA NO TENGA PRODUCTOS REGISTRADOS CON SU ID
            Producto oProd = db.Productos.DefaultIfEmpty(null).FirstOrDefault(p => p.UnidadMedidaId == unidadDeMedida.Id);

            if (oProd == null)
            {
                db.UnidadesDeMedida.Remove(unidadDeMedida);
                completado = await db.SaveChangesAsync() > 0 ? true : false;
                mensaje = completado ? "Eliminado correctamente" : "Se encontraron productos en esta unidad de medida";
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