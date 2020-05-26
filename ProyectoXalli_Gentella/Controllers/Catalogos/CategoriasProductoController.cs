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
    public class CategoriasProductoController : Controller
    {
        private DBControl db = new DBControl();
        private bool completado = false;
        private string mensaje = "";

        // GET: CategoriasProducto
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
            var categorias = await db.CategoriasProducto.Where(c => c.EstadoCategoria == true).ToListAsync();

            return Json(new { data = categorias }, JsonRequestBehavior.AllowGet);
        }

        // GET: CategoriasProducto/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CategoriaProducto categorias = await db.CategoriasProducto.FindAsync(id);
            if (categorias == null)
            {
                return HttpNotFound();
            }
            return View(categorias);
        }

        // GET: CategoriasProducto/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: CategoriasProducto/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "Id,CodigoCategoria,DescripcionCategoria,EstadoCategoria")] CategoriaProducto CategoriaProducto)
        {
            CategoriaProducto bod = db.CategoriasProducto.DefaultIfEmpty(null).FirstOrDefault(b => b.CodigoCategoria.Trim() == CategoriaProducto.CodigoCategoria.Trim());

            if (bod != null)
            {
                ModelState.AddModelError("CodigoCategoria", "Código ya utilizado");
                mensaje = "Código de CategoriaProducto ya existente";
            }

            //ESTADO DE LA CATEGORIA CUANDO SE CREA SIEMPRE ES TRUE
            CategoriaProducto.EstadoCategoria = true;
            if (ModelState.IsValid)
            {
                db.CategoriasProducto.Add(CategoriaProducto);
                completado = await db.SaveChangesAsync() > 0 ? true : false;
                mensaje = completado ? "Almacenado correctamente" : "Error al guardar";
            }

            return Json(new { success = completado, message = mensaje }, JsonRequestBehavior.AllowGet);
        }

        // GET: CategoriasProducto/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CategoriaProducto CategoriaProducto = await db.CategoriasProducto.FindAsync(id);
            if (CategoriaProducto == null)
            {
                return HttpNotFound();
            }
            return View(CategoriaProducto);
        }

        // POST: CategoriasProducto/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "Id,CodigoCategoria,DescripcionCategoria,EstadoCategoria")] CategoriaProducto CategoriaProducto)
        {
            if (ModelState.IsValid)
            {
                db.Entry(CategoriaProducto).State = EntityState.Modified;
                completado = await db.SaveChangesAsync() > 0 ? true : false;
                mensaje = completado ? "Almacenado correctamente" : "Error al guardar";
            }
            return Json(new { success = completado, message = mensaje }, JsonRequestBehavior.AllowGet);
        }

        // POST: CategoriasProducto/Delete/5
        [HttpPost, ActionName("Delete")]
        //[ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            var CategoriaProducto = db.CategoriasProducto.Find(id);
            //BUSCANDO QUE CategoriaProducto NO TENGA SALIDAS NI ENTRADAS REGISTRADAS CON SU ID
            Producto oProd = db.Productos.DefaultIfEmpty(null).FirstOrDefault(p => p.CategoriaId == CategoriaProducto.Id);

            if (oProd == null)
            {
                db.CategoriasProducto.Remove(CategoriaProducto);
                completado = await db.SaveChangesAsync() > 0 ? true : false;
                mensaje = completado ? "Eliminado correctamente" : "Se encontraron productos en esta CategoriaProducto";
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