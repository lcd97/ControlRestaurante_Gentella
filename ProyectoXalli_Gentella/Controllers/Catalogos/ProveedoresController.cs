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
    public class ProveedoresController : Controller
    {
        private DBControl db = new DBControl();
        private bool completado = false;
        private string mensaje = "";
        
        // GET: Proveedor
        public ActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// RECUPERA DATOS PARA LLENAR LA TABLA PROVEEDORES A TRAVES DE JSON
        /// </summary>
        /// <returns></returns>
        public JsonResult GetData()
        {
            var proveedores = (from obj in db.Proveedores.ToList()
                               join u in db.Datos.ToList() on obj.DatoId equals u.Id
                               where obj.EstadoProveedor == true
                               select new
                               {
                                   Id = obj.Id,
                                   //CONDICION PARA ASIGNAR A UN CAMPO UN VALOR ALTERNATIVO EN CASO DE SER NULO (CASE-WHEN)
                                   NombreComercial = obj.NombreComercial != null ? obj.NombreComercial : u.PNombre + " " + u.PApellido,
                                   Telefono = obj.Telefono,
                                   RUC = u.RUC,
                                   Local = obj.Local
                               });

            return Json(new { data = proveedores }, JsonRequestBehavior.AllowGet);
        }

        // GET: Productos/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Productos/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        //[ValidateAntiForgeryToken]
        public ActionResult Create(string NombreComercial, string Telefono, string RUC, bool EstadoProveedor, bool Local, bool RetenedorIR, string NombreProveedor, string ApellidoProveedor, string CedulaProveedor)
        {
            //SE CREAR UNA INSTANCIA PARA ALMACENAR AL PROVEEDOR
            Proveedor proveedor = new Proveedor();
            int datoId = 1;//SE CREA UNA VARIABLE ENTERA PARA ALMACENAR EL ID DE DATO Y SE INICIALIZA EN 1 (EL ID 1 CORRESPONDE A LA PLANTILLA)

            if (NombreComercial == "" || CedulaProveedor == "")
            {
                completado = false;
                mensaje = "Completar campos";
            }
            else
            {
                //DEPENDE DEL TIPO DE PROVEEDOR SE ALMACENA LOS DATOS
                if (Local)
                {
                    //INSTANCIA DE LA TABLA DATO PARA GUARDAR DATOS DEL PROVEEDOR LOCAL
                    Dato dato = new Dato();
                    Proveedor validando = new Proveedor();

                    //VALIDACION DEL CAMPO CEDULA PROVEEDOR EN LA TABLA DATOS
                    Dato Validacion = db.Datos.DefaultIfEmpty(null).FirstOrDefault(d => d.DNI.Trim() == CedulaProveedor.Trim());

                    if (Validacion != null)
                    {
                        //VALIDANDO QUE EL PROVEEDOR NO ESTE REGISTRADO EN LA TABLA Y QUE SEA DIFERENTE DEL ID 2 QUE ES MI PLANTILLA
                        validando = db.Proveedores.DefaultIfEmpty(null).FirstOrDefault(d => d.DatoId == Validacion.Id && d.DatoId != 2);
                    }

                    //SI EXISTE EL OBJETO DATO
                    if (Validacion == null)
                    {
                        //SE GUARDAN DATOS DEL PROVEEDOR LOCAL
                        dato.DNI = CedulaProveedor;
                        dato.PNombre = NombreProveedor;
                        dato.PApellido = ApellidoProveedor;
                        dato.RUC = RUC;

                        db.Datos.Add(dato);

                        //SI SE GUARDO SE ALMACENAN LOS OTROS CAMPOS
                        if (db.SaveChanges() > 0)
                        {
                            //GUARDAR A PROVEEDOR
                            proveedor.Telefono = Telefono;
                            proveedor.EstadoProveedor = EstadoProveedor;
                            proveedor.Local = Local;
                            proveedor.RetenedorIR = RetenedorIR;
                            proveedor.DatoId = dato.Id;//GUARDAR EL ID DEL CAMPO ALMACENADO

                            //GUARDA CAMBIOS EN LA DB
                            db.Proveedores.Add(proveedor);
                            completado = db.SaveChanges() > 0 ? true : false;
                            mensaje = completado ? "Almacenado correctamente" : "Error al guardar";
                        }//FIN SAVECHANGES
                    }//FIN VALIDACION
                    else
                    {
                        if (validando == null)
                        {
                            //SI EXISTE LOS DATOS DEL TIPO PROVEEDOR LOCAL GUARDAR TODO LOS DATOS DEL PROVEEDOR
                            proveedor.Telefono = Telefono;
                            proveedor.EstadoProveedor = EstadoProveedor;
                            proveedor.Local = Local;
                            proveedor.RetenedorIR = RetenedorIR;
                            proveedor.DatoId = Validacion.Id;//GUARDAR EL ID DEL CAMPO ALMACENADO

                            //GUARDA CAMBIOS EN LA DB
                            db.Proveedores.Add(proveedor);
                            completado = db.SaveChanges() > 0 ? true : false;
                            mensaje = completado ? "Almacenado correctamente" : "Error al guardar";
                        }
                        else
                        {
                            completado = false;
                            mensaje = "El proveedor local ya se encuentra registrado";
                        }

                    }//FIN ELSE VALIDACION      

                }//FIN LOCAL
                else if (!Local)
                {
                    //VALIDACION DEL CAMPO CEDULA PROVEEDOR EN LA TABLA DATOS
                    Proveedor proValidacion = db.Proveedores.DefaultIfEmpty(null).FirstOrDefault(d => d.NombreComercial.Trim() == NombreComercial.Trim());

                    if (proValidacion == null)
                    {
                        //GUARDA EL PROVEEDOR
                        proveedor.Telefono = Telefono;
                        proveedor.EstadoProveedor = EstadoProveedor;
                        proveedor.Local = Local;
                        proveedor.RetenedorIR = RetenedorIR;
                        proveedor.NombreComercial = NombreComercial;
                        proveedor.DatoId = datoId;//GUARDA EL ID DE LA PLANTILLA ALMACENADO

                        //GUARDA CAMBIOS EN LA DB
                        db.Proveedores.Add(proveedor);
                        completado = db.SaveChanges() > 0 ? true : false;
                        mensaje = completado ? "Almacenado correctamente" : "Error al guardar";
                    }
                    else
                    {
                        completado = false;
                        mensaje = "El proveedor ingresado ya se encuentra registrado";
                    }


                }//FIN ELSE NO LOCAL
            }

            return Json(new { data = completado, message = mensaje }, JsonRequestBehavior.AllowGet);
        }//FIN POST CREATE

        // GET: Categorias/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Proveedor Proveedor = await db.Proveedores.FindAsync(id);
            if (Proveedor == null)
            {
                return HttpNotFound();
            }            

            return View(Proveedor);
        }

        /// <summary>
        /// RECUPERA LOS DATOS DEL PROVEEDOR PARA MOSTRAR
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult getProveedor(int id)
        {
            var provider = (from obj in db.Proveedores.ToList()
                            join u in db.Datos.ToList() on obj.DatoId equals u.Id
                            where obj.Id == id
                            select new
                            {
                                //CAMPOS DEL PROVEEDOR
                                NombreComercial = obj.NombreComercial,
                                Telefono = obj.Telefono,
                                local = obj.Local,
                                RUC = u.RUC,
                                IR = obj.RetenedorIR,
                                Estado = obj.EstadoProveedor,
                                Nombre = u.PNombre,
                                Apellido = u.PApellido,
                                Cedula = u.DNI
                            });

            return Json(provider, JsonRequestBehavior.AllowGet);
        }


        [HttpPost]
        //[ValidateAntiForgeryToken]
        public ActionResult UpdateProveedor(int Id, string NombreComercial, string Telefono, string RUC, bool EstadoProveedor, bool Local, bool RetenedorIR, string NombreProveedor, string ApellidoProveedor, string CedulaProveedor)
        {
            //BUSCAR AL PROVEEDOR POR MEDIO DEL ID
            Proveedor proveedor = db.Proveedores.Find(Id);

            //DEPENDE DEL TIPO DE PROVEEDOR ALMACENAMOS LOS DATOS
            if (Local)
            {
                //BUSCAR LOS DATOS A MODIFICAR DEL PROVEEDOR LOCAL POR MEDIO DE LA CEDULA
                Dato dato = db.Datos.FirstOrDefault(d => d.DNI.Trim() == CedulaProveedor.Trim());

                //ASIGNAMOS VALORES A DATOS DE PROVEEDOR LOCAL
                dato.PNombre = NombreProveedor;
                dato.PApellido = ApellidoProveedor;
                dato.RUC = RUC;

                //GUARDAR CAMBIOS
                db.Entry(dato).State = EntityState.Modified;
                //CONFIRMACION DE CAMBIOS GUARDADOS
                if (db.SaveChanges() > 0)
                {
                    //ASIGNAMOS VALORES DE PROVEEDOR
                    proveedor.NombreComercial = NombreComercial;
                    proveedor.Telefono = Telefono;
                    proveedor.RetenedorIR = RetenedorIR;
                    proveedor.EstadoProveedor = EstadoProveedor;
                    //GUARDAR CAMBIOS DEL PROVEEDOR
                    db.Entry(proveedor).State = EntityState.Modified;
                    completado = db.SaveChanges() > 0 ? true : false;
                    mensaje = completado ? "Actualizado correctamente" : "Error al actualizar";
                }
                else {
                    //REVERTIR CAMBIOS EN DATOS
                }
            }
            else {
                //ASIGNAMOS VALORES DE PROVEEDOR
                proveedor.NombreComercial = NombreComercial;
                proveedor.Telefono = Telefono;
                proveedor.RetenedorIR = RetenedorIR;
                proveedor.EstadoProveedor = EstadoProveedor;
                //GUARDAR CAMBIOS DEL PROVEEDOR
                db.Entry(proveedor).State = EntityState.Modified;
                completado = db.SaveChanges() > 0 ? true : false;
                mensaje = completado ? "Actualizado correctamente" : "Error al actualizar";
            }        

            return Json(new { success = completado, message = mensaje }, JsonRequestBehavior.AllowGet);
        }

        // POST: Proveedor/Delete/5
        [HttpPost, ActionName("Delete")]
        //[ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            var Proveedor = db.Proveedores.Find(id);
            //BUSCANDO QUE PRODUCTO NO TENGA SALIDAS NI ENTRADAS REGISTRADAS CON SU ID
            Entrada oEnt = db.Entradas.DefaultIfEmpty(null).FirstOrDefault(e => e.ProveedorId == Proveedor.Id);

            if (oEnt == null)
            {
                db.Proveedores.Remove(Proveedor);
                completado = await db.SaveChangesAsync() > 0 ? true : false;
                mensaje = completado ? "Eliminado correctamente" : "Se encontraron registros del proveedor en entradas";
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