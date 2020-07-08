﻿using ProyectoXalli_Gentella.Models;
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
    public class CategoriasMenuController : Controller
    {
        private DBControl db = new DBControl();
        private bool completado = false;
        private string mensaje = "";

        // GET: CategoriasMenu
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
            var categorias = await db.CategoriasMenu.Where(c => c.EstadoCategoriaMenu == true).ToListAsync();

            return Json(new { data = categorias }, JsonRequestBehavior.AllowGet);
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
        public async Task<ActionResult> Create([Bind(Include = "Id,CodigoCategoriaMenu,DescripcionCategoriaMenu,EstadoCategoriaMenu")] CategoriaMenu CategoriaMenu)
        {
            //BUSCAR QUE EXISTA UNA CATEGORIA CON ESA DESCRIPCION
            CategoriaProducto bod = db.CategoriasProducto.DefaultIfEmpty(null).FirstOrDefault(b => b.DescripcionCategoria.ToUpper().Trim() == CategoriaMenu.DescripcionCategoriaMenu.ToUpper().Trim());

            //SI EXISTE INGRESADO UNA CATEGORIA CON LA DESCRIPCION
            if (bod != null)
            {
                ModelState.AddModelError("DescripcionCategoriaMenu", "Utilice otro nombre");
                mensaje = "La descripción ya se encuentra registrada";
            }else
            {
                //ESTADO DE LA CATEGORIA CUANDO SE CREA SIEMPRE ES TRUE
                CategoriaMenu.EstadoCategoriaMenu = true;
                if (ModelState.IsValid)
                {
                    db.CategoriasMenu.Add(CategoriaMenu);
                    completado = await db.SaveChangesAsync() > 0 ? true : false;
                    mensaje = completado ? "Almacenado correctamente" : "Error al guardar";
                }
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
            CategoriaMenu CategoriaMenu = await db.CategoriasMenu.FindAsync(id);
            if (CategoriaMenu == null)
            {
                return HttpNotFound();
            }
            return View(CategoriaMenu);
        }

        // POST: Categorias/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "Id,CodigoCategoriaMenu,DescripcionCategoriaMenu,EstadoCategoriaMenu")] CategoriaMenu CategoriaMenu)
        {
            //BUSCAR QUE EXISTA UNA CATEGORIA CON ESA DESCRIPCION
            CategoriaProducto bod = db.CategoriasProducto.DefaultIfEmpty(null).FirstOrDefault(b => b.DescripcionCategoria.ToUpper().Trim() == CategoriaMenu.DescripcionCategoriaMenu.ToUpper().Trim()
            && b.Id != CategoriaMenu.Id);

            //SI EXISTE INGRESADO UNA CATEGORIA CON LA DESCRIPCION
            if (bod != null)
            {
                ModelState.AddModelError("DescripcionCategoriaMenu", "Utilice otro nombre");
                mensaje = "La descripción ya se encuentra registrada";
            }
            else
            {
                if (ModelState.IsValid)
                {
                    db.Entry(CategoriaMenu).State = EntityState.Modified;
                    completado = await db.SaveChangesAsync() > 0 ? true : false;
                    mensaje = completado ? "Modificado correctamente" : "Error al modificar";
                }
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
            var code = db.CategoriasMenu.Max(x => x.CodigoCategoriaMenu.Trim());
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

        // POST: Categorias/Delete/5
        [HttpPost, ActionName("Delete")]
        //[ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            var categoriaMenu = db.CategoriasMenu.Find(id);
            //BUSCANDO QUE Categoria NO TENGA SALIDAS NI ENTRADAS REGISTRADAS CON SU ID
            Menu oMenu = db.Menus.DefaultIfEmpty(null).FirstOrDefault(p => p.CategoriaMenuId == categoriaMenu.Id);

            //SI NO SE ENCUENTRA ASOCIADO NINGUN PLATILLO AL ID
            if (oMenu == null)
            {
                db.CategoriasMenu.Remove(categoriaMenu);
                completado = await db.SaveChangesAsync() > 0 ? true : false;
                mensaje = completado ? "Eliminado correctamente" : "Error al eliminar";
            }
            else {
                mensaje = "Se encontraron platillos asociados a esta cartegoría";
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