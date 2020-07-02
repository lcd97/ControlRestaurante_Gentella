using ProyectoXalli_Gentella.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data;
using System.Data.Entity;
using System.Net;
using System.Threading.Tasks;

namespace ProyectoXalli_Gentella.Controllers.Catalogos
{
    public class ClientesController : Controller
    {
        private DBControl db = new DBControl();
        private bool completado = false;
        private string mensaje = "";

        // GET: Clientes
        public ActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// OBTIENE LOS DATOS PARA LA VISTA INDEX
        /// </summary>
        /// <returns></returns>
        public JsonResult GetData() {
            var clientes = (from obj in db.Clientes.ToList()
                            join d in db.Datos.ToList() on obj.DatoId equals d.Id
                            where obj.EstadoCliente == true
                            select new {
                                Id = obj.Id,
                                Documento = d.DNI == null ? obj.PasaporteCliente : d.DNI,
                                Cliente = d.PNombre + " " + d.PApellido
                            });

            return Json(new { data = clientes }, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// RETORNA LA VISTA CREATE
        /// </summary>
        /// <returns></returns>
        public ActionResult Create() {
            ViewBag.TipoClienteId = new SelectList(db.TiposDeCliente, "Id", "DescripcionTipoCliente");

            return View();
        }

        /// <summary>
        /// ALMACENA UN REGISTRO DE CLIENTE
        /// </summary>
        /// <param name="Nombre"></param>
        /// <param name="Apellido"></param>
        /// <param name="Documento"></param>
        /// <param name="RUC"></param>
        /// <param name="Email"></param>
        /// <param name="Telefono"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Create(string Nombre, string Apellido, string Documento, string RUC, string Email, string Telefono, int TipoCliente, uint? Tipo) {
            //SE BUSCA DESDE LAS DOS TABLAS
            Cliente cliente = new Cliente();
            Dato dato = new Dato();

            //BUSCAR QUE EL RUC INGRESADO NO EXISTA
            var bruc = db.Datos.DefaultIfEmpty(null).FirstOrDefault(r => r.RUC == RUC.Trim().ToUpper());

            //SI EL NUMERO RUC YA SE ENCUENTRA REGISTRADO
            if (bruc != null) {
                mensaje = "ya existe una ruc registrado";
                return Json(new { mensaje });
            } else {
                //SI ES UN CLIENTE NACIONAL
                if (Tipo == 1) {
                    dato = db.Datos.DefaultIfEmpty(null).FirstOrDefault(t => t.DNI.Trim() == Documento.Trim());
                    if (dato != null) {
                        cliente = db.Clientes.DefaultIfEmpty(null).FirstOrDefault(c => c.DatoId == dato.Id);
                    }
                } else {
                    //CLIENTE EXTRANJERO
                    cliente = db.Clientes.DefaultIfEmpty(null).FirstOrDefault(c => c.PasaporteCliente.Trim() == Documento.Trim());
                    if (cliente != null) {
                        dato = db.Datos.DefaultIfEmpty(null).FirstOrDefault(d => d.Id == cliente.DatoId);
                    }
                }//FIN BUSCAR

                //SI EXISTE ALGUN REGISTRO DE CLIENTE
                if (dato != null && cliente != null) {
                    //SI EXISTE UN REGISTRO CLIENTE
                    if (cliente != null) {
                        mensaje = "Ya se encuentra registrado un cliente con esa identificación";
                    } else {
                        //SI SOLO EXISTE LOS DATOS DE LA PERSONA                    
                        //SI EL NUMERO RUC NO ESTA NULO
                        if (RUC != null) {
                            dato.RUC = RUC;
                            //SE ACTUALIZA EL CAMPO
                            db.Entry(dato).State = EntityState.Modified;
                        }

                        Cliente client = new Cliente();

                        //SE COMPRUEBA EL TIPO DE DOCUMENTO PARA ACTUALIZAR
                        if (Tipo == 2) {
                            client.PasaporteCliente = Documento;
                        }
                        //ALMACENAR LOS CAMPOS DE CLIENTE
                        client.EmailCliente = Email != "" ? Email : null;
                        client.TelefonoCliente = Telefono != "" ? Telefono : null;
                        client.EstadoCliente = true;
                        client.TipoClienteId = TipoCliente;
                        client.DatoId = dato.Id;

                        db.Clientes.Add(client);
                        completado = db.SaveChanges() > 0 ? true : false;
                        mensaje = completado ? "Almacenado correctamente" : "Error al almacenar";
                    }
                } else {
                    //SI NO SE ENCUENTRAN COINCIDENCIAS
                    Dato data = new Dato();

                    //SI EL DOCUMENTO ES CEDULA
                    if (Tipo == 1) {
                        data.DNI = Documento.Trim();
                    }
                    data.PNombre = Nombre;
                    data.PApellido = Apellido;
                    data.RUC = RUC != "" ? RUC : null;

                    db.Datos.Add(data);

                    if (db.SaveChanges() > 0) {
                        //ALMACENAR DATOS DE CLIENTE
                        Cliente customer = new Cliente();

                        if (Tipo == 2) {
                            customer.PasaporteCliente = Documento;
                        }
                        customer.EmailCliente = Email != "" ? Email : null;
                        customer.TelefonoCliente = Telefono != "" ? Telefono : null;
                        customer.EstadoCliente = true;
                        customer.TipoClienteId = TipoCliente;
                        customer.DatoId = data.Id;

                        db.Clientes.Add(customer);

                        completado = db.SaveChanges() > 0 ? true : false;
                        mensaje = completado ? "Almacenado correctamente" : "Error al almacenar";
                    }
                }
            }

            return Json(new { success = completado, message = mensaje }, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// DEVULVE LA VISTA EDITAR DEL CLIENTE
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult Edit(int? id) {
            if (id == null) {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Cliente Cliente = db.Clientes.Find(id);
            if (Cliente == null) {
                return HttpNotFound();
            }

            ViewBag.TipoClienteId = new SelectList(db.TiposDeCliente, "Id", "DescripcionTipoCliente", Cliente.TipoClienteId);

            return View(Cliente);
        }

        /// <summary>
        /// EDITA UN OBJETO CLIENTE
        /// </summary>
        /// <param name="Nombre"></param>
        /// <param name="Apellido"></param>
        /// <param name="Documento"></param>
        /// <param name="RUC"></param>
        /// <param name="Email"></param>
        /// <param name="Telefono"></param>
        /// <param name="TipoCliente"></param>
        /// <param name="TipoDocumento"></param>
        /// <param name="Estado"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Edit(int Id, string Nombre, string Apellido, string Documento, string RUC, string Email, string Telefono, int TipoCliente, int TipoDocumento, bool Estado) {
            //SE BUSCA DESDE LAS DOS TABLAS
            Cliente cliente = db.Clientes.Find(Id);
            Dato dato = db.Datos.Find(cliente.DatoId);

            //BUSCAR QUE EL RUC INGRESADO NO EXISTA
            var bruc = db.Datos.DefaultIfEmpty(null).FirstOrDefault(r => r.RUC.Trim() == RUC.Trim() && r.Id != cliente.DatoId);

            //SI EL NUMERO RUC YA SE ENCUENTRA REGISTRADO
            if (bruc != null) {
                mensaje = "El número RUC ya se encuentra registrado";
                return Json(new { mensaje });
            } else {
                dato.PNombre = Nombre;
                dato.PApellido = Apellido;          
                dato.RUC = RUC != "" ? RUC : null;

                db.Entry(dato).State = EntityState.Modified;

                if (db.SaveChanges() > 0) {
                    cliente.EmailCliente = Email != "" ? Email : null;

                    cliente.TipoClienteId = TipoCliente;
                    cliente.TelefonoCliente = Telefono != "" ? Telefono : null;

                    cliente.EstadoCliente = Estado;

                    db.Entry(cliente).State = EntityState.Modified;

                    completado = db.SaveChanges() > 0 ? true : false;
                    mensaje = completado ? "Modificado correctamente" : "Error al actualizar";
                }
            }
            return Json(new { success = completado, message = mensaje});
        }

        /// <summary>
        /// OBTIENE LOS DATOS DEL CLIENTE
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult getCustomer(int id) {
            var customer = from obj in db.Clientes
                           join d in db.Datos on obj.DatoId equals d.Id
                           where obj.Id == id
                           select new {
                               Nombre = d.PNombre,
                               Apellido = d.PApellido,
                               TipoDocumento = d.DNI.Trim() != null ? 1 : 2,
                               Documento = obj.PasaporteCliente != null ? obj.PasaporteCliente.Trim() : d.DNI,
                               RUC = d.RUC,
                               Email = obj.EmailCliente,
                               Telefono = obj.TelefonoCliente,
                               Estado = obj.EstadoCliente
                           };

            return Json(new { data = customer }, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// RECUPERA LA VISTA DETALLE DE CLIENTE
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult Details(int? id) {
            if (id == null) {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Cliente Cliente = db.Clientes.Find(id);
            if (Cliente == null) {
                return HttpNotFound();
            }

            return View(Cliente);
        }

        // POST: Proveedor/Delete/5
        [HttpPost, ActionName("Delete")]
        //[ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id) {
            var cliente = db.Clientes.Find(id);
            //BUSCANDO QUE CLIENTE NO TENGA ORDENES REGISTRADAS CON SU ID
            Orden orden = db.Ordenes.DefaultIfEmpty(null).FirstOrDefault(p => p.ClienteId == cliente.Id);

            //SI NO EXISTEN ORDENES DEL CLIENTE
            if (orden == null) {
                db.Clientes.Remove(cliente);
                completado = await db.SaveChangesAsync() > 0 ? true : false;
                mensaje = completado ? "Eliminado correctamente" : "Error al eliminar";
            } else {
                mensaje = "Se encontraron ordenes realizadas con el cliente";
            }

            return Json(new { success = completado, message = mensaje }, JsonRequestBehavior.AllowGet);
        }

        protected override void Dispose(bool disposing) {
            if (disposing) {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}