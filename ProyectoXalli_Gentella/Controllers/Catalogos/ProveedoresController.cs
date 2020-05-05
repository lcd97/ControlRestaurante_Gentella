using ProyectoXalli_Gentella.Models;
using System;
using System.Collections.Generic;
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
                               where obj.EstadoProveedor == true
                               select new
                               {
                                   Id = obj.Id,
                                   NombreComercial = obj.NombreComercial,
                                   Telefono = obj.Telefono,
                                   RUC = obj.RUC,
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
            int datoId = 2;//SE CREA UNA VARIABLE ENTERA PARA ALMACENAR EL ID DE DATO Y SE INICIALIZA EN 1 (EL ID 1 CORRESPONDE A LA PLANTILLA)

            //DEPENDE DEL TIPO DE PROVEEDOR SE ALMACENA LOS DATOS
            if (Local)
            {
                //INSTANCIA DE LA TABLA DATO PARA GUARDAR DATOS DEL PROVEEDOR LOCAL
                Dato dato = new Dato();

                //VALIDACION DEL CAMPO CEDULA PROVEEDOR EN LA TABLA DATOS
                Dato Validacion = db.Datos.DefaultIfEmpty(null).FirstOrDefault(d => d.DNI.Trim() == CedulaProveedor.Trim());

                //SI EXISTE EL OBJETO DATO
                if (Validacion == null)
                {
                    //SE GUARDAN DATOS DEL PROVEEDOR LOCAL
                    dato.DNI = CedulaProveedor;
                    dato.PNombre = NombreProveedor;
                    dato.PApellido = ApellidoProveedor;

                    db.Datos.Add(dato);

                    //SI SE GUARDO SE ALMACENAN LOS OTROS CAMPOS
                    if (db.SaveChanges() > 0)
                    {
                        //GUARDAR A PROVEEDOR
                        proveedor.Telefono = Telefono;
                        proveedor.RUC = RUC;
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
                else {
                    //SI EXISTE LOS DATOS DEL TIPO PROVEEDOR LOCAL GUARDAR TODO LOS DATOS DEL PROVEEDOR
                    proveedor.Telefono = Telefono;
                    proveedor.RUC = RUC;
                    proveedor.EstadoProveedor = EstadoProveedor;
                    proveedor.Local = Local;
                    proveedor.RetenedorIR = RetenedorIR;
                    proveedor.DatoId = Validacion.Id;//GUARDAR EL ID DEL CAMPO ALMACENADO

                    //GUARDA CAMBIOS EN LA DB
                    db.Proveedores.Add(proveedor);
                    completado = db.SaveChanges() > 0 ? true : false;
                    mensaje = completado ? "Almacenado correctamente" : "Error al guardar";
                }//FIN ELSE VALIDACION              
            }//FIN LOCAL
            else
            {
                //VALIDACION DEL CAMPO CEDULA PROVEEDOR EN LA TABLA DATOS
                Proveedor proValidacion = db.Proveedores.DefaultIfEmpty(null).FirstOrDefault(d => d.NombreComercial.Trim() == NombreComercial.Trim());

                //GUARDA EL PROVEEDOR
                proveedor.Telefono = Telefono;
                proveedor.RUC = RUC;
                proveedor.EstadoProveedor = EstadoProveedor;
                proveedor.Local = Local;
                proveedor.RetenedorIR = RetenedorIR;
                proveedor.NombreComercial = NombreComercial;
                proveedor.DatoId = datoId;//GUARDA EL ID DE LA PLANTILLA ALMACENADO

                //GUARDA CAMBIOS EN LA DB
                db.Proveedores.Add(proveedor);
                completado = db.SaveChanges() > 0 ? true : false;
                mensaje = completado ? "Almacenado correctamente" : "Error al guardar";
            }//FIN ELSE NO LOCAL            

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

            return Json(new { data = Proveedor }, JsonRequestBehavior.AllowGet);
        }
    }
}