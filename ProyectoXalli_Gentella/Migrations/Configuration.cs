namespace ProyectoXalli_Gentella.Migrations
{
    using ProyectoXalli_Gentella.Models;
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<ProyectoXalli_Gentella.Models.DBControl>
    {
        public Configuration() {
            AutomaticMigrationsEnabled = true;
            AutomaticMigrationDataLossAllowed = true;
            ContextKey = "ProyectoXalli_Gentella.Models.DBControl";
        }

        protected override void Seed(ProyectoXalli_Gentella.Models.DBControl context)
        {
            //  This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method 
            //  to avoid creating duplicate seed data.
            context.Bodegas.AddOrUpdate(u => u.CodigoBodega,
                new Bodega { CodigoBodega = "001", DescripcionBodega = "Bar", EstadoBodega = true },
                new Bodega { CodigoBodega = "002", DescripcionBodega = "Cocina", EstadoBodega = true });
            context.SaveChanges();

            context.CategoriasProducto.AddOrUpdate(u => u.CodigoCategoria,
                new CategoriaProducto { CodigoCategoria = "001", DescripcionCategoria = "Bebidas gaseosas", EstadoCategoria = true },
                new CategoriaProducto { CodigoCategoria = "002", DescripcionCategoria = "Bebidas alcohólicas", EstadoCategoria = true },
                new CategoriaProducto { CodigoCategoria = "003", DescripcionCategoria = "Condimientos", EstadoCategoria = true },
                new CategoriaProducto { CodigoCategoria = "004", DescripcionCategoria = "Frutas", EstadoCategoria = true },
                new CategoriaProducto { CodigoCategoria = "005", DescripcionCategoria = "Verduras", EstadoCategoria = true },
                new CategoriaProducto { CodigoCategoria = "006", DescripcionCategoria = "Carnes", EstadoCategoria = true },
                new CategoriaProducto { CodigoCategoria = "007", DescripcionCategoria = "Pescados/Mariscos", EstadoCategoria = true },
                new CategoriaProducto { CodigoCategoria = "008", DescripcionCategoria = "Lácteos", EstadoCategoria = true });
            context.SaveChanges();

            context.TiposDeEntrada.AddOrUpdate(u => u.CodigoTipoEntrada,
                new TipoDeEntrada { CodigoTipoEntrada = "001", DescripcionTipoEntrada = "Compras", EstadoTipoEntrada = true },
                new TipoDeEntrada { CodigoTipoEntrada = "002", DescripcionTipoEntrada = "Otros", EstadoTipoEntrada = true });
            context.SaveChanges();

            context.TiposDeSalida.AddOrUpdate(u => u.CodigoTipoSalida,
                new TipoDeSalida { CodigoTipoSalida = "001", DescripcionTipoSalida = "Venta", EstadoTipoSalida = true },
                new TipoDeSalida { CodigoTipoSalida = "002", DescripcionTipoSalida = "Mal estado", EstadoTipoSalida = true },
                new TipoDeSalida { CodigoTipoSalida = "003", DescripcionTipoSalida = "Otros", EstadoTipoSalida = true });
            context.SaveChanges();

            context.TiposDeSalida.AddOrUpdate(u => u.CodigoTipoSalida,
                new TipoDeSalida { CodigoTipoSalida = "001", DescripcionTipoSalida = "Venta", EstadoTipoSalida = true },
                new TipoDeSalida { CodigoTipoSalida = "002", DescripcionTipoSalida = "Mal estado", EstadoTipoSalida = true },
                new TipoDeSalida { CodigoTipoSalida = "003", DescripcionTipoSalida = "Otros", EstadoTipoSalida = true });
            context.SaveChanges();

            context.UnidadesDeMedida.AddOrUpdate(u => u.CodigoUnidadMedida,
                new UnidadDeMedida { CodigoUnidadMedida = "001", DescripcionUnidadMedida = "Unidad", AbreviaturaUM = "Ud.", EstadoUnidadMedida = true },
                new UnidadDeMedida { CodigoUnidadMedida = "002", DescripcionUnidadMedida = "Libra", AbreviaturaUM = "Lb.", EstadoUnidadMedida = true },
                new UnidadDeMedida { CodigoUnidadMedida = "003", DescripcionUnidadMedida = "Docena", AbreviaturaUM = "Doc.", EstadoUnidadMedida = true },
                new UnidadDeMedida { CodigoUnidadMedida = "004", DescripcionUnidadMedida = "Cajilla 24", AbreviaturaUM = "Caj.", EstadoUnidadMedida = true },
                new UnidadDeMedida { CodigoUnidadMedida = "005", DescripcionUnidadMedida = "Galón", AbreviaturaUM = "Gal.", EstadoUnidadMedida = true },
                new UnidadDeMedida { CodigoUnidadMedida = "006", DescripcionUnidadMedida = "Quintal", AbreviaturaUM = "Q.", EstadoUnidadMedida = true },
                new UnidadDeMedida { CodigoUnidadMedida = "007", DescripcionUnidadMedida = "Lata", AbreviaturaUM = "Lata.", EstadoUnidadMedida = true });
            context.SaveChanges();

            context.Datos.AddOrUpdate(d => d.DNI,
                new Dato { DNI = "xxx-xxxxxx-xxxxx", PNombre = "default", PApellido = "default", RUC = "xxx-xxx-xx" });
            context.SaveChanges();

            context.CategoriasMenu.AddOrUpdate(d => d.CodigoCategoriaMenu,
                new CategoriaMenu { CodigoCategoriaMenu = "001", DescripcionCategoriaMenu = "Postres", EstadoCategoriaMenu = true },
                new CategoriaMenu { CodigoCategoriaMenu = "002", DescripcionCategoriaMenu = "Desayunos", EstadoCategoriaMenu = true },
                new CategoriaMenu { CodigoCategoriaMenu = "003", DescripcionCategoriaMenu = "Cena", EstadoCategoriaMenu = true },
                new CategoriaMenu { CodigoCategoriaMenu = "004", DescripcionCategoriaMenu = "Bebidas Alcholicas", EstadoCategoriaMenu = true },
                new CategoriaMenu { CodigoCategoriaMenu = "005", DescripcionCategoriaMenu = "Vinos", EstadoCategoriaMenu = true });
            context.SaveChanges();
        }
    }
}
