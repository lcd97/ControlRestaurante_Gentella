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
    public class CategoriasController : Controller
    {
        private DBControl db = new DBControl();
        private bool completado = false;
        private string mensaje = "";

        // GET: Categorias
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
            db.Configuration.ProxyCreationEnabled = false;
            var categorias = await db.Categorias.Where(c => c.EstadoCategoria == true).ToListAsync();

            return Json(new { data = categorias }, JsonRequestBehavior.AllowGet);
        }

        // GET: Categorias/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Categoria categorias = await db.Categorias.FindAsync(id);
            if (categorias == null)
            {
                return HttpNotFound();
            }
            return View(categorias);
        }

        // GET: Categorias/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Categorias/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "Id,CodigoCategoria,DescripcionCategoria,EstadoCategoria")] Categoria Categoria)
        {
            Categoria bod = db.Categorias.DefaultIfEmpty(null).FirstOrDefault(b => b.CodigoCategoria.Trim() == Categoria.CodigoCategoria.Trim());

            if (bod != null)
            {
                ModelState.AddModelError("CodigoCategoria", "Código ya utilizado");
                mensaje = "Código de Categoria ya existente";
            }

            //ESTADO DE LA CATEGORIA CUANDO SE CREA SIEMPRE ES TRUE
            Categoria.EstadoCategoria = true;
            if (ModelState.IsValid)
            {
                db.Categorias.Add(Categoria);
                completado = await db.SaveChangesAsync() > 0 ? true : false;
                mensaje = completado ? "Almacenado correctamente" : "Error al guardar";
            }

            return Json(new { success = completado, message = mensaje }, JsonRequestBehavior.AllowGet);
        }

        // GET: Categorias/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Categoria Categoria = await db.Categorias.FindAsync(id);
            if (Categoria == null)
            {
                return HttpNotFound();
            }
            return View(Categoria);
        }

        // POST: Categorias/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "Id,CodigoCategoria,DescripcionCategoria,EstadoCategoria")] Categoria Categoria)
        {
            if (ModelState.IsValid)
            {
                db.Entry(Categoria).State = EntityState.Modified;
                completado = await db.SaveChangesAsync() > 0 ? true : false;
                mensaje = completado ? "Almacenado correctamente" : "Error al guardar";
            }
            return Json(new { success = completado, message = mensaje }, JsonRequestBehavior.AllowGet);
        }

        // POST: Categorias/Delete/5
        [HttpPost, ActionName("Delete")]
        //[ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            var Categoria = db.Categorias.Find(id);
            //BUSCANDO QUE Categoria NO TENGA SALIDAS NI ENTRADAS REGISTRADAS CON SU ID
            Producto oProd = db.Productos.DefaultIfEmpty(null).FirstOrDefault(p => p.CategoriaId == Categoria.Id);

            if (oProd == null)
            {
                db.Categorias.Remove(Categoria);
                completado = await db.SaveChangesAsync() > 0 ? true : false;
                mensaje = completado ? "Eliminado correctamente" : "Se encontraron productos en esta Categoria";
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