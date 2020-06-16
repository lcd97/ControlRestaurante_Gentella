using ProyectoXalli_Gentella.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Data;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace ProyectoXalli_Gentella.Controllers.Catalogos
{
    public class ProductosController : Controller
    {
        private DBControl db = new DBControl();
        private bool completado = false;
        private string mensaje = "";

        // GET: Productos
        public ActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// RECUPERA DATOS PARA LLENAR LA TABLA PRODUCTOS A TRAVES DE JSON
        /// </summary>
        /// <returns></returns>
        public JsonResult GetData()
        {
            //db.Configuration.ProxyCreationEnabled = false;
            //db.Configuration.LazyLoadingEnabled = false;
            var productos = (from obj in db.Productos.ToList()
                             join u in db.CategoriasProducto.ToList() on obj.CategoriaId equals u.Id
                             join c in db.UnidadesDeMedida.ToList() on obj.UnidadMedidaId equals c.Id
                             where obj.EstadoProducto == true
                             select new
                             {
                                 Id = obj.Id,
                                 DescripcionProducto = obj.DescripcionProducto,
                                 CodigoProducto = obj.CodigoProducto,
                                 Marca = obj.MarcaProducto,
                                 UnidadDeMedida = c.DescripcionUnidadMedida,
                                 Categoria = u.DescripcionCategoria
                             });
                
                //await db.Productos.Join(u => u.UnidadDeMedida).join(c => c.Categoria).Where(c => c.EstadoProducto == true).ToListAsync();

            return Json(new { data = productos }, JsonRequestBehavior.AllowGet);
        }

        // GET: Productos/Create
        public ActionResult Create()
        {
            ViewBag.CategoriaId = new SelectList(db.CategoriasProducto, "Id", "DescripcionCategoria");
            ViewBag.UnidadMedidaId = new SelectList(db.UnidadesDeMedida, "Id", "DescripcionUnidadMedida");

            return View();
        }

        // POST: Productos/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "Id,CodigoProducto,DescripcionProducto,MarcaProducto,CantidadMaxProducto,CantidadMinProducto,EstadoProducto,UnidadMedidaId,CategoriaId")] Producto Producto)
        {
            Producto bod = db.Productos.DefaultIfEmpty(null).FirstOrDefault(b => b.CodigoProducto.Trim() == Producto.CodigoProducto.Trim());

            if (bod != null)
            {
                ModelState.AddModelError("CodigoProducto", "Código ya utilizado");
                mensaje = "Código de producto ya existente";
            }

            //ESTADO DE LA CATEGORIA CUANDO SE CREA SIEMPRE ES TRUE
            Producto.EstadoProducto = true;
            if (ModelState.IsValid)
            {
                db.Productos.Add(Producto);
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

            //ESTO ES PARA AGREGARLO EN EL FORMULARIO DE ENTRADAS
            var um = db.UnidadesDeMedida.Find(Producto.UnidadMedidaId);
            var pro = Producto.DescripcionProducto + " " + Producto.MarcaProducto + " " + um.DescripcionUnidadMedida;

            return Json(new { success = completado, message = mensaje, Id = Producto.Id, Producto = pro  }, JsonRequestBehavior.AllowGet);
        }

        // GET: CategoriasProducto/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Producto Producto = await db.Productos.FindAsync(id);
            if (Producto == null)
            {
                return HttpNotFound();
            }

            ViewBag.CategoriaId = new SelectList(db.CategoriasProducto, "Id", "DescripcionCategoria", Producto.CategoriaId);
            ViewBag.UnidadMedidaId = new SelectList(db.UnidadesDeMedida, "Id", "DescripcionUnidadMedida", Producto.UnidadMedidaId);

            return View(Producto);
        }

        // POST: CategoriasProducto/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "Id,CodigoProducto,DescripcionProducto,MarcaProducto,CantidadMaxProducto,CantidadMinProducto,EstadoProducto,UnidadMedidaId,CategoriaId")] Producto Producto)
        {
            if (ModelState.IsValid)
            {
                db.Entry(Producto).State = EntityState.Modified;
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

        // GET: CategoriasProducto/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Producto producto = await db.Productos.FindAsync(id);            
            if (producto == null)
            {
                return HttpNotFound();
            }

            return View(producto);
        }

        public ActionResult getDetails(int id) 
        {
            var producto = from obj in db.Productos.ToList()
                           join c in db.CategoriasProducto.ToList() on obj.CategoriaId equals c.Id
                           join u in db.UnidadesDeMedida.ToList() on obj.UnidadMedidaId equals u.Id
                           where obj.Id == id
                           select new {
                               CodigoProducto = obj.CodigoProducto,
                               DescripcionProducto = obj.DescripcionProducto,
                               MarcaProducto = obj.MarcaProducto,
                               CantidadMaxProducto = obj.CantidadMaxProducto,
                               CantidadMinProducto = obj.CantidadMinProducto,
                               EstadoProducto = obj.EstadoProducto,
                               UnidadMedida = u.DescripcionUnidadMedida,
                               Categoria = c.DescripcionCategoria
                           };

            return Json(new { data = producto }, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// DEVUELVE LA CANTIDAD ACTUAL DEL PRODUCTO Y DONDE SE ALMACENA (SOLO SE LLEVARA EL CONTROL DEL BAR)
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult CantidadActual(int id) {
            var cantActual = (from bod in db.Bodegas
                              join ent in db.Entradas on bod.Id equals ent.BodegaId
                              join det in db.DetallesDeEntrada on ent.Id equals det.EntradaId
                              join pro in db.Productos on det.ProductoId equals pro.Id
                              where pro.Id == id
                              group new { bod, det } by new { bod.Id } into grouped
                              select new {
                                  Destino = grouped.Key.Id,
                                  cantActual = grouped.Sum(b => b.det.CantidadEntrada)
                              });

            return Json(new { data = cantActual }, JsonRequestBehavior.AllowGet);
        }

        // POST: Productos/Delete/5
        [HttpPost, ActionName("Delete")]
        //[ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            var Producto = db.Productos.Find(id);
            //BUSCANDO QUE PRODUCTO NO TENGA SALIDAS NI ENTRADAS REGISTRADAS CON SU ID
            DetalleDeEntrada oEnt = db.DetallesDeEntrada.DefaultIfEmpty(null).FirstOrDefault(e => e.ProductoId == Producto.Id);
            DetalleDeSalida oSal = db.DetallesDeSalida.DefaultIfEmpty(null).FirstOrDefault(s => s.ProductoId == Producto.Id);

            if (oEnt == null || oSal == null)
            {
                db.Productos.Remove(Producto);
                completado = await db.SaveChangesAsync() > 0 ? true : false;
                mensaje = completado ? "Eliminado correctamente" : "Se encontraron movimientos en este producto";
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