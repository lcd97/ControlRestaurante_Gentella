using ProyectoXalli_Gentella.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity;
using System.Data;
using System.Data.Entity.Validation;

namespace ProyectoXalli_Gentella.Controllers.Catalogos {
    public class MenusController : Controller {
        private DBControl db = new DBControl();
        private bool completado = false;
        private string mensaje = "";

        // GET: Platillos
        public ActionResult Index() {
            return View();
        }

        /// <summary>
        /// RECUPERA DATOS PARA LLENAR LA TABLA MENU A TRAVES DE JSON
        /// </summary>
        /// <returns></returns>
        public JsonResult GetData() {
            var menu = (from obj in db.Menus.ToList()
                        join i in db.Imagenes.ToList() on obj.ImagenId equals i.Id
                        where obj.EstadoMenu == true
                        orderby obj.Id descending
                        select new {
                            Id = obj.Id,
                            DescripcionPlatillo = obj.DescripcionMenu,
                            Precio = obj.PrecioMenu,
                            Imagen = i.Ruta
                        });

            return Json(new { data = menu }, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// COMPRUEBA SI EL PLATILLO A INGRESAR EXISTE
        /// </summary>
        /// <param name="Codigo"></param>
        /// <returns></returns>
        public ActionResult Comprobar(string Codigo) {
            var menu = db.Menus.DefaultIfEmpty(null).FirstOrDefault(m => m.CodigoMenu.Trim() == Codigo.Trim());

            if (menu != null) {
                completado = false;
            } else
                completado = true;

            return Json(new { completado }, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// MUESTRA LA VISTA DEL CREATE
        /// </summary>
        /// <returns></returns>
        public ActionResult Create() {
            ViewBag.CategoriaId = new SelectList(db.CategoriasMenu, "Id", "DescripcionCategoriaMenu");

            return View();
        }

        // POST: Menus/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        //[ValidateAntiForgeryToken]
        public ActionResult Create(HttpPostedFileBase urlImage, string codigoMenu, string descripcionMenu, double precio, int categoriaId, string tiempo, string ingredientes) {
            int EnviarId = 0;
            //PRIMERO SE ALMACENA LA IMAGEN Y SU DIRECCION EN LA BD

            string path = Server.MapPath("~/images/Menu");

            //CREA EL DIRECTORIO DONDE SE ALMACENARN LAS FOTOS, EN CASO NO EXISTA
            if (!Directory.Exists(path)) {
                Directory.CreateDirectory(path);
            }

            //QUE NO SE REPITA EL PLATILLO
            var platillo = db.Menus.DefaultIfEmpty(null).FirstOrDefault(p => p.DescripcionMenu.ToUpper().Trim() == descripcionMenu.ToUpper().Trim());

            if (platillo != null) {

                mensaje = "Ya se encuentra registrado un platillo con ese nombre. Intentelo de nuevo";
                completado = false;
                return Json(new { data = completado, message = mensaje, Id = EnviarId }, JsonRequestBehavior.AllowGet);
            }


            using (var transact = db.Database.BeginTransaction()) {
                try {
                    //SI LLEGA AL MENOS UN ARCHIVO
                    if (urlImage != null || Request.Files[0].ContentLength > 0) {
                        var file = Request.Files[0];//OBTENEMOS EL ARCHIVO DE LA VISTA
                        var extension = "." + Path.GetFileName(file.FileName).Split('.').Last();//SE OBTIENE LA EXTENSION DE LA IMAGEN

                        //ASIGNAR NOMBRE DEL PLATILLO A LA IMAGEN PARA GUARDAR - QUITANDO ESPACIOS ENTRE PALABRAS Y AGREGANDO SU EXTENSION
                        string filename = string.Concat(descripcionMenu.Where(c => !char.IsWhiteSpace(c))).ToLower() + extension;

                        //BUSCAR EL NOMBRE DEL ARCHIVO EN LA BASE DE DATOS CON EL MISMO NOMBRE
                        var img = db.Imagenes.DefaultIfEmpty(null).FirstOrDefault(c => c.Ruta.Trim() == ("/images/Menu" + @"\" + filename).Trim());

                        //SI LA IMAGEN NO EXISTE SE PROCEDE A GUARDAR
                        if (img == null) {
                            path = Path.Combine(Server.MapPath("~/images/Menu"), filename);//SE CREA LA DIRECCION DONDE SE ALMACENARA LA IMAGEN--OJO
                            file.SaveAs(path);//SE MANDA A GUARDAR EL ARCHIVO

                            string url = Path.Combine("/images/Menu", filename);//SE CREA LA DIRECCION PARA ALMACENAR LA IMAGEN
                            Imagen obj = new Imagen();//SE CREA EL OBJETO IMAGEN

                            //OJO AQUI, SOLO SE ESTA TRABAJANDO LOCAL. CUANDO YA SE SUBA A ALGUN SERVIDOR SE DEBE MODIFICAR//
                            obj.Ruta = url;//SE ASIGNA LA RUTA

                            db.Imagenes.Add(obj);//SE AGREGA EL OBJETO

                            //SI SE GUARDO CORRECTAMENTE SE ALMACENA LOS DEMAS CAMPOS
                            if (db.SaveChanges() > 0) {
                                //SE CREA UNA INSTANCIA PARA ALMACENAR EL PLATILLO
                                Menu item = new Menu();

                                item.CodigoMenu = codigoMenu;
                                item.DescripcionMenu = descripcionMenu;
                                item.PrecioMenu = precio;
                                item.CategoriaMenuId = categoriaId;
                                item.ImagenId = obj.Id;//SE ASIGNA EL ID RECIEN ALMACENADO
                                item.EstadoMenu = true;

                                db.Menus.Add(item);
                                //SI SE ALMACENO CORRECTAMENTE EL PLATILLO
                                if (db.SaveChanges() > 0) {
                                    //GUARDAR RECETA
                                    Receta receta = new Receta();

                                    receta.TiempoEstimado = tiempo;
                                    receta.Ingredientes = ingredientes;
                                    receta.MenuId = item.Id;

                                    //PARA BUSCAR Y AGREGAR EL ITEM EN EL INDEX
                                    EnviarId = item.Id;

                                    //

                                    try {
                                        db.Recetas.Add(receta);
                                        completado = db.SaveChanges() > 0 ? true : false;
                                        mensaje = completado ? "Almacenado correctamente" : "Vuelva a intentarlo";
                                    } catch (DbEntityValidationException dbEx) {
                                        foreach (var validationErrors in dbEx.EntityValidationErrors) {
                                            foreach (var validationError in validationErrors.ValidationErrors) {
                                                System.Console.WriteLine("Property: {0} Error: {1}", validationError.PropertyName, validationError.ErrorMessage);
                                            }
                                        }
                                    }

                                    //

                                } else {
                                    completado = false;
                                    mensaje = "No se guardó el platillo";
                                }
                            } else {
                                completado = false;
                                mensaje = "No se guardó bien la imagen";
                            }
                        } else {
                            //DECIRLE QUE NO LLEGO NINGUNA IMAGEN
                            completado = false;
                            mensaje = "Imagen ya existe. Revise el tipo de platillo";
                        }
                    }

                    transact.Commit();
                } catch (Exception) {
                    mensaje = "Error al almacenar";
                    transact.Rollback();
                }//FIN TRY-CATCH
            }//FIN USING

            return Json(new { data = completado, message = mensaje, Id = EnviarId }, JsonRequestBehavior.AllowGet);
        }//FIN POST CREATE

        /// <summary>
        /// METODO GET PARA MOSTRAR LA VISTA EDIT
        /// </summary>
        /// <returns></returns>
        public async Task<ActionResult> Edit(int? id) {
            if (id == null) {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Menu menu = await db.Menus.FindAsync(id);
            if (menu == null) {
                return HttpNotFound();
            }

            ViewBag.CategoriaId = new SelectList(db.CategoriasMenu, "Id", "DescripcionCategoriaMenu", menu.CategoriaMenuId);

            return View(menu);
        }

        /// <summary>
        /// RECUPERA EL OBJETO PLATILLO CONFORME UN ID
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public ActionResult getMenuItem(int Id) {
            var menu = (from obj in db.Menus.ToList()
                        join u in db.Imagenes.ToList() on obj.ImagenId equals u.Id
                        join c in db.CategoriasMenu.ToList() on obj.CategoriaMenuId equals c.Id
                        join r in db.Recetas.ToList() on obj.Id equals r.MenuId
                        where obj.Id == Id
                        select new {
                            PlatilloId = obj.Id,
                            Codigo = obj.CodigoMenu,
                            Platillo = obj.DescripcionMenu,
                            Precio = obj.PrecioMenu,
                            Categoria = c.DescripcionCategoriaMenu,
                            CategoriaId = c.Id,
                            Tiempo = r.TiempoEstimado,
                            Ingredientes = r.Ingredientes,
                            Ruta = u.Ruta,
                            Estado = obj.EstadoMenu
                        });

            return Json(menu, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// METODO GET PARA MOSTRAR LA VISTA DETAIL
        /// </summary>
        /// <returns></returns>
        public ActionResult Detail(int? id) {
            if (id == null) {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Menu menu = db.Menus.Find(id);
            if (menu == null) {
                return HttpNotFound();
            }

            return View(menu);
        }

        /// <summary>
        /// RECUPERA DATOS PARA EL DETALLE
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public ActionResult getDetail(int Id) {
            //BUSCAR EL MENU
            var menu = (from obj in db.Menus
                        join u in db.Imagenes on obj.ImagenId equals u.Id
                        join o in db.Recetas on obj.Id equals o.MenuId
                        join c in db.CategoriasMenu on obj.CategoriaMenuId equals c.Id
                        where obj.Id == Id
                        select new {
                            //CAMPOS DEL PLATILLO  
                            Id = obj.Id,
                            Ruta = u.Ruta,
                            Tiempo = o.TiempoEstimado,
                            Categoria = c.DescripcionCategoriaMenu,
                            Precio = obj.PrecioMenu,
                            Platillo = obj.DescripcionMenu,
                            Ingredientes = o.Ingredientes
                        }).FirstOrDefault();

            return Json(new { data = menu }, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// METODO POST PARA EL EDIT
        /// </summary>
        /// <param name="urlImage"></param>
        /// <param name="codigoMenu"></param>
        /// <param name="descripcionMenu"></param>
        /// <param name="precio"></param>
        /// <param name="categoriaId"></param>
        /// <param name="tiempo"></param>
        /// <param name="ingredientes"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Edit(HttpPostedFileBase urlImage, string codigoMenu, string descripcionMenu, double precio, int categoriaId, string tiempo, string ingredientes, bool estado) {
            //BUSCAR EL OBJETO A MODIFICAR
            var menu = db.Menus.DefaultIfEmpty(null).FirstOrDefault(m => m.CodigoMenu.Trim() == codigoMenu.Trim());

            //ALMACENAR EL ID DEL PLATILLO (PARA ACTUALIZAR EN INDEX)
            int Id = menu.Id;

            using (var transact = db.Database.BeginTransaction()) {
                try {
                    //BUSCAR EL PLATILLO
                    if (menu != null) {

                        //SI NO LLEGA CAMBIO DE IMAGEN
                        if (urlImage == null || !(Request.Files[0].ContentLength > 0)) {

                            //SI EL NOMBRE DEL PLATILLO CAMBIA
                            if (menu.DescripcionMenu.Trim() != descripcionMenu.Trim()) {
                                //BUSCO LA IMAGEN
                                var img = db.Imagenes.Find(menu.ImagenId);
                                //BUSCAR LA EXTENSION DE LA IMAGEN
                                var extension = "." + (img.Ruta).Split('.').Last();

                                var newNameFullPath = "/" + string.Concat(descripcionMenu.Where(c => !char.IsWhiteSpace(c))).ToLower() + extension;

                                //SI EXISTE UN PLATILLO CON ESE NOMBRE
                                if (System.IO.File.Exists(Server.MapPath("~/images/Menu") + newNameFullPath)) {
                                    completado = false;
                                    mensaje = "Error. Ese platillo ya existe";

                                    return Json(new { data = completado, message = mensaje }, JsonRequestBehavior.AllowGet);
                                } else {

                                    //SE CAMBIA EL NOMBRE DE LA IMAGEN ALMACENADA - SE PASA EN NOMBRE ACTUAL Y EL NUEVO NOMBRE
                                    System.IO.File.Move(Server.MapPath(img.Ruta), (Server.MapPath("~/images/Menu") + newNameFullPath));

                                    //SE CAMBIA EL NOMBRE EN LA BASE DE DATO
                                    img.Ruta = "/images/Menu" + newNameFullPath;

                                    db.Entry(img).State = EntityState.Modified;
                                    completado = db.SaveChanges() > 0 ? true : false;
                                }
                            }//FIN CAMBIO DE NOMBRE

                            //SE MODIFICA DE UN SOLO EL OBJETO MENU
                            menu.DescripcionMenu = descripcionMenu;
                            menu.PrecioMenu = precio;
                            menu.CategoriaMenuId = categoriaId;
                            menu.EstadoMenu = estado;

                            db.Entry(menu).State = EntityState.Modified;//SE MODIFICA EL OBJETO

                            //SI SE ALMACENO CORRECTAMENTE EL PLATILLO
                            if (db.SaveChanges() > 0) {
                                //GUARDAR RECETA
                                var receta = db.Recetas.FirstOrDefault(r => r.MenuId == menu.Id);

                                receta.TiempoEstimado = tiempo;
                                receta.Ingredientes = ingredientes;

                                db.Entry(receta).State = EntityState.Modified;//SE MODIFICA EL OBJETO

                                try {
                                    db.Entry(receta).State = EntityState.Modified;
                                    completado = db.SaveChanges() > 0 ? true : false;
                                    mensaje = completado ? "Almacenado correctamente" : "Vuelva a intentarlo";
                                } catch (DbEntityValidationException dbEx) {
                                    foreach (var validationErrors in dbEx.EntityValidationErrors) {
                                        foreach (var validationError in validationErrors.ValidationErrors) {
                                            System.Console.WriteLine("Property: {0} Error: {1}", validationError.PropertyName, validationError.ErrorMessage);
                                        }
                                    }
                                }
                            }

                        } //LLEGA UNA IMAGEN PARA CAMBIAR
                        else {
                            //ELIMINAR EL ARCHIVO DE LA CARPETA - SE BUSCA OBJETO IMAGEN
                            var imagen = db.Imagenes.Find(menu.ImagenId);

                            //SE BUSCA LA DIRECCION DONDE SE ENCUENTRA LA IMAGEN
                            var path = Server.MapPath(imagen.Ruta);

                            //SE ELIMINA EL ARCHIVO DE LA CARPETA
                            System.IO.File.Delete(path);

                            //CONFIRMACION DE IMAGEN ELIMINADA
                            if (!System.IO.File.Exists(path)) {

                                var file = Request.Files[0];//OBTENEMOS EL ARCHIVO DE LA VISTA
                                var extension = "." + Path.GetFileName(file.FileName).Split('.').Last();//SE OBTIENE LA EXTENSION DE LA IMAGEN

                                //ASIGNAR NOMBRE DEL PLATILLO A LA IMAGEN PARA GUARDAR - QUITANDO ESPACIOS ENTRE PALABRAS Y AGREGANDO SU EXTENSION
                                string filename = string.Concat(descripcionMenu.Where(c => !char.IsWhiteSpace(c))).ToLower() + extension;


                                path = Path.Combine(Server.MapPath("~/images/Menu"), filename);//SE CREA LA DIRECCION DONDE SE ALMACENARA LA IMAGEN--OJO
                                file.SaveAs(path);//SE MANDA A GUARDAR EL ARCHIVO

                                string url = Path.Combine("/images/Menu", filename);//SE CREA LA DIRECCION PARA ALMACENAR LA IMAGEN

                                //OJO AQUI, SOLO SE ESTA TRABAJANDO LOCAL. CUANDO YA SE SUBA A ALGUN SERVIDOR SE DEBE MODIFICAR//
                                imagen.Ruta = url;//SE ASIGNA LA RUTA

                                db.Entry(imagen).State = EntityState.Modified;//SE MODIFICA EL OBJETO

                                //SI SE MODIFICO CORRECTAMENTE SE ALMACENA LOS DEMAS CAMPOS
                                if (db.SaveChanges() > 0) {

                                    menu.DescripcionMenu = descripcionMenu;
                                    menu.PrecioMenu = precio;
                                    menu.CategoriaMenuId = categoriaId;
                                    menu.EstadoMenu = estado;

                                    db.Entry(menu).State = EntityState.Modified;//SE MODIFICA EL OBJETO
                                                                                //SI SE ALMACENO CORRECTAMENTE EL PLATILLO
                                    if (db.SaveChanges() > 0) {
                                        //GUARDAR RECETA
                                        var receta = db.Recetas.FirstOrDefault(r => r.MenuId == menu.Id);

                                        receta.TiempoEstimado = tiempo;
                                        receta.Ingredientes = ingredientes;

                                        db.Entry(receta).State = EntityState.Modified;//SE MODIFICA EL OBJETO

                                        if (db.SaveChanges() > 0) {
                                            completado = true;
                                            mensaje = "Modificado con éxito";
                                        }//FIN SAVE_CHANGES RECETA
                                        else {
                                            completado = false;
                                            mensaje = "No se modificó la receta ";
                                        }
                                    }//FIN DE SAVE_CHANGES MENU
                                    else {
                                        completado = false;
                                        mensaje = "No se modificó el platillo";
                                    }
                                }//FIN DE SAVE_CHANGES IMAGEN
                                else {
                                    completado = false;
                                    mensaje = "No se modificó bien la imagen";
                                }
                            } else {
                                //DECIRLE QUE NO LLEGO NINGUNA IMAGEN
                                completado = false;
                                mensaje = "Ya existe una imagen asociada a este platillo. Revise";
                            }
                        }
                    }//FIN MENU VACIO

                    transact.Commit();
                } catch (Exception) {
                    mensaje = "Error al modificar";
                    transact.Rollback();
                }//FIN TRY-CATCH
            }//FIN USING
            return Json(new { data = completado, message = mensaje, Id }, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// RETORNA EL CODIGO AUTOMATICAMENTE A LA VISTA CREATE
        /// </summary>
        /// <returns></returns>
        public ActionResult SearchCode() {
            //BUSCAR EL VALOR MAXIMO DE LAS BODEGAS REGISTRADAS
            var code = db.Menus.Max(x => x.CodigoMenu.Trim());
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

        // POST: Proveedor/Delete/5
        [HttpPost, ActionName("Delete")]
        //[ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id) {
            //BUSCAR EL PLATILLO CORRESPONDIENTE AL ID
            var Menu = db.Menus.Find(id);
            //BUSCAR SU RECETA
            var Receta = db.Recetas.FirstOrDefault(r => r.MenuId == Menu.Id);

            //SE BUSCA OBJETO IMAGEN
            var img = db.Imagenes.Find(Menu.ImagenId);

            //SE BUSCA LA DIRECCION DONDE SE ENCUENTRA LA IMAGEN
            var path = Server.MapPath(img.Ruta);

            //SE ELIMINA EL ARCHIVO DE LA CARPETA
            System.IO.File.Delete(path);

            //SE BUSCA QUE NO EXISTA ESE ARCHIVO PARA ELIMINARLO DE LA BD
            if (!System.IO.File.Exists(path)) {

                //SE ELIMINA PRIMERO LAS RECETAS
                db.Recetas.Remove(Receta);
                if (db.SaveChanges() > 0) {
                    //SE BORRA EL MENU
                    db.Menus.Remove(Menu);
                    if (db.SaveChanges() > 0) {
                        //SE BORRA LA IMAGEN DE LA BD
                        db.Imagenes.Remove(img);
                        //SI SE ELIMINO BIEN
                        completado = await db.SaveChangesAsync() > 0 ? true : false;
                        mensaje = completado ? "Actualizado correctamente" : "Error al Eliminar";
                    }
                } else {
                    completado = false;
                    mensaje = "Eliminación incorrecto";
                }
            } else {
                completado = false;
                mensaje = "Archivo Inexistente";
            }

            return Json(new { success = completado, message = mensaje}, JsonRequestBehavior.AllowGet);
        }

        protected override void Dispose(bool disposing) {
            if (disposing) {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}