﻿using ProyectoXalli_Gentella.Models;
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
            //BUSCAR SI YA SE ENCUENTRA REGISTRADO UNA BODEGA CON LA DESCRIPCION INGRESADA
            Bodega bod = db.Bodegas.DefaultIfEmpty(null).FirstOrDefault(b => b.DescripcionBodega.ToUpper().Trim() == bodega.DescripcionBodega.ToUpper());

            //SI ENCUENTRA UNA BODEGA CON ESA DESCRIPCION
            if (bod != null) {
                ModelState.AddModelError("DescripcionBodega", "Utilice otro nombre");
                mensaje = "La descripción ya se encuentra registrada";
            } else {
                bodega.EstadoBodega = true;
                if (ModelState.IsValid) {
                    db.Bodegas.Add(bodega);
                    completado = await db.SaveChangesAsync() > 0 ? true : false;
                    mensaje = completado ? "Almacenado correctamente" : "Error al guardar";
                } else {
                    //ESTO ES PARA VER EL ERROR QUE DEVUELVE EL MODELO
                    string cad = "";
                    foreach (ModelState modelState in ViewData.ModelState.Values) {
                        foreach (ModelError error in modelState.Errors) {
                            cad += (error);
                        }
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
            //COMPROBAR QUE NO EXISTA LA MISMA DESCRIPCION DE BODEGAS
            var comprobar = db.Bodegas.DefaultIfEmpty(null).FirstOrDefault(c=> c.DescripcionBodega.ToUpper().Trim() == bodega.DescripcionBodega.ToUpper().Trim() && c.Id != bodega.Id);

            //SI YA EXISTE LA BODEGA MANDAR ERROR
            if (comprobar != null) {
                ModelState.AddModelError("DescripcionBodega", "Utilice otro nombre");
                mensaje = "La descripción ya se encuentra registrada";
            } else
            if (ModelState.IsValid)
            {
                db.Entry(bodega).State = EntityState.Modified;
                completado = await db.SaveChangesAsync() > 0 ? true : false;
                mensaje = completado ? "Modificado correctamente" : "Error al modificar";
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

        /// <summary>
        /// RETORNA EL CODIGO AUTOMATICAMENTE A LA VISTA CREATE
        /// </summary>
        /// <returns></returns>
        public ActionResult SearchCode() {
            //BUSCAR EL VALOR MAXIMO DE LAS BODEGAS REGISTRADAS
            var code = db.Bodegas.Max(x => x.CodigoBodega.Trim());
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

        // POST: Bodegas/Delete/5
        [HttpPost, ActionName("Delete")]
        //[ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            var bodega = db.Bodegas.Find(id);
            //BUSCANDO QUE BODEGA NO TENGA SALIDAS NI ENTRADAS REGISTRADAS CON SU ID
            var oEnt = db.Entradas.DefaultIfEmpty(null).FirstOrDefault(e => e.BodegaId == bodega.Id);
            var oSal = db.Salidas.DefaultIfEmpty(null).FirstOrDefault(s => s.BodegaId == bodega.Id);

            //SI NO SE ENCONTRARON SALIDAS O ENTRADAS AL ALMACEN
            if (oEnt == null || oSal == null) {
                db.Bodegas.Remove(bodega);
                completado = await db.SaveChangesAsync() > 0 ? true : false;
                mensaje = completado ? "Eliminado correctamente" : "Error al eliminar";
            } else
                mensaje = "Se encontraron productos registrados a este almacen";

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