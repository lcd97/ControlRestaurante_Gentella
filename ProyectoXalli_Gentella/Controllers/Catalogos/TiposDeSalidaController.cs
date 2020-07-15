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
            //SE BUSCA UN TIPO DE SALIDA QUE TENGA LA MISMA DESCRIPCION
            TipoDeSalida bod = db.TiposDeSalida.DefaultIfEmpty(null).FirstOrDefault(b => b.DescripcionTipoSalida.ToUpper().Trim() == TipoDeSalida.DescripcionTipoSalida.ToUpper().Trim());

            //SI SE ENCUENTRA UN TIPO DE SALIDA CON ESA DESCRIPCION
            if (bod != null) {
                ModelState.AddModelError("DescripcionTipoSalida", "Utilice otro nombre");
                mensaje = "La descripción ya se encuentra registrada";
                return Json(new { success = completado, message = mensaje }, JsonRequestBehavior.AllowGet);
            }

            using (var transact = db.Database.BeginTransaction()) {
                try {
                    //ESTADO DE TIPO DE SALIDAS CUANDO SE CREA SIEMPRE ES TRUE
                    TipoDeSalida.EstadoTipoSalida = true;
                    if (ModelState.IsValid) {
                        db.TiposDeSalida.Add(TipoDeSalida);
                        completado = await db.SaveChangesAsync() > 0 ? true : false;
                        mensaje = completado ? "Almacenado correctamente" : "Error al guardar";
                    }

                    transact.Commit();
                } catch (Exception) {
                    mensaje = "Error al almacenar";
                    transact.Rollback();
                }
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
            //SE BUSCA UN TIPO DE SALIDA QUE TENGA LA MISMA DESCRIPCION
            TipoDeSalida bod = db.TiposDeSalida.DefaultIfEmpty(null).FirstOrDefault(b => b.DescripcionTipoSalida.ToUpper().Trim() == TipoDeSalida.DescripcionTipoSalida.ToUpper().Trim() && b.Id != TipoDeSalida.Id);

            //SI SE ENCUENTRA UN TIPO DE SALIDA CON ESA DESCRIPCION
            if (bod != null) {
                ModelState.AddModelError("DescripcionTipoSalida", "Utilice otro nombre");
                mensaje = "La descripción ya se encuentra registrada";
                return Json(new { success = completado, message = mensaje }, JsonRequestBehavior.AllowGet);
            }

            using (var transact = db.Database.BeginTransaction()) {
                try {
                    if (ModelState.IsValid) {
                        db.Entry(TipoDeSalida).State = EntityState.Modified;
                        completado = await db.SaveChangesAsync() > 0 ? true : false;
                        mensaje = completado ? "Modificado correctamente" : "Error al modificar";
                    }

                    transact.Commit();
                } catch (Exception) {
                    mensaje = "Error al modificar";
                    transact.Rollback();
                }//FIN TRY-CATCH
            }//FIN USING

            return Json(new { success = completado, message = mensaje }, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// RETORNA EL CODIGO AUTOMATICAMENTE A LA VISTA CREATE
        /// </summary>
        /// <returns></returns>
        public ActionResult SearchCode() {
            //BUSCAR EL VALOR MAXIMO DE LAS BODEGAS REGISTRADAS
            var code = db.TiposDeSalida.Max(x => x.CodigoTipoSalida.Trim());
            int valor;
            string num;

            //SI EXISTE ALGUN REGISTRO
            if (code != null) {
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
            } else
                num = "001";//SE COMIENZA CON EL PRIMER CODIGO DEL REGISTRO

            return Json(new { data = num }, JsonRequestBehavior.AllowGet);
        }

        // POST: TiposDeSalida/Delete/5
        [HttpPost, ActionName("Delete")]
        //[ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            var TipoDeSalida = db.TiposDeSalida.Find(id);
            //BUSCANDO QUE TIPO DE SALIDA NO TENGA SALIDAS NI SALIDAS REGISTRADAS CON SU ID
            Salida oSalida = db.Salidas.DefaultIfEmpty(null).FirstOrDefault(p => p.TipoSalidaId == TipoDeSalida.Id);

            using (var transact = db.Database.BeginTransaction()) {
                try {
                    if (oSalida == null) {
                        db.TiposDeSalida.Remove(TipoDeSalida);
                        completado = await db.SaveChangesAsync() > 0 ? true : false;
                        mensaje = completado ? "Eliminado correctamente" : "Error al eliminar";
                    } else {
                        mensaje = "Se encontraron salidas registrados a esta tipo de salidas";
                    }

                    transact.Commit();
                } catch (Exception) {
                    mensaje = "Error al eliminar";
                    transact.Rollback();
                }//FIN TRY-CATCH
            }//FIN USING

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