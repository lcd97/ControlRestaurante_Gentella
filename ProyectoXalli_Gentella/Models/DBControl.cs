using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Linq;
using System.Web;

namespace ProyectoXalli_Gentella.Models
{
    public class DBControl : DbContext
    {
        public DBControl() : base("HotelDB")
        {
        }

        //MODULO CONTROL DE INSUMOS
        public DbSet<Bodega> Bodegas { get; set; }
        public DbSet<CategoriaProducto> CategoriasProducto { get; set; }
        public DbSet<Dato> Datos { get; set; }
        public DbSet<DetalleDeEntrada> DetallesDeEntrada { get; set; }
        public DbSet<DetalleDeSalida> DetallesDeSalida { get; set; }
        public DbSet<Entrada> Entradas { get; set; }
        public DbSet<Producto> Productos { get; set; }
        public DbSet<Proveedor> Proveedores { get; set; }
        public DbSet<Salida> Salidas { get; set; }
        public DbSet<TipoDeEntrada> TiposDeEntrada { get; set; }
        public DbSet<TipoDeSalida> TiposDeSalida { get; set; }
        public DbSet<UnidadDeMedida> UnidadesDeMedida { get; set; }

        //MODULO MENU
        public DbSet<Receta> Recetas { get; set; }
        public DbSet<Menu> Menus { get; set; }
        public DbSet<CategoriaMenu> CategoriasMenu { get; set; }
        public DbSet<Imagen> Imagenes { get; set; }

        //MODULO ORDENES
        public DbSet<Mesero> Meseros { get; set; }
        public DbSet<TipoDeCliente> TiposDeCliente { get; set; }
        public DbSet<Cliente> Clientes { get; set; }
        public DbSet<Orden> Ordenes { get; set; }
        public DbSet<DetalleDeOrden> DetallesDeOrden { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Conventions.Remove<PluralizingEntitySetNameConvention>();
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
            modelBuilder.Conventions.Remove<OneToManyCascadeDeleteConvention>();
        }
    }
}