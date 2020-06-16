namespace ProyectoXalli_Gentella.Migrations {
    using ProyectoXalli_Gentella.Models;
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<ProyectoXalli_Gentella.Models.DBControl> {
        public Configuration() {
            AutomaticMigrationsEnabled = true;
            AutomaticMigrationDataLossAllowed = true;
            ContextKey = "ProyectoXalli_Gentella.Models.DBControl";
        }

        protected override void Seed(ProyectoXalli_Gentella.Models.DBControl context) {
            //  This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method 
            //  to avoid creating duplicate seed data.

            context.Bodegas.AddOrUpdate(u => u.CodigoBodega,
                new Bodega { CodigoBodega = "B01", DescripcionBodega = "Bar", EstadoBodega = true },
                new Bodega { CodigoBodega = "C01", DescripcionBodega = "Cocina", EstadoBodega = true });
            context.SaveChanges();

            context.CategoriasProducto.AddOrUpdate(u => u.CodigoCategoria,
                new CategoriaProducto { CodigoCategoria = "B00", DescripcionCategoria = "Bebidas gaseosas", EstadoCategoria = true },
                new CategoriaProducto { CodigoCategoria = "B01", DescripcionCategoria = "Bebidas alcohólicas", EstadoCategoria = true },
                new CategoriaProducto { CodigoCategoria = "C00", DescripcionCategoria = "Condimientos", EstadoCategoria = true },
                new CategoriaProducto { CodigoCategoria = "F00", DescripcionCategoria = "Frutas", EstadoCategoria = true },
                new CategoriaProducto { CodigoCategoria = "V00", DescripcionCategoria = "Verduras", EstadoCategoria = true },
                new CategoriaProducto { CodigoCategoria = "C01", DescripcionCategoria = "Carnes", EstadoCategoria = true },
                new CategoriaProducto { CodigoCategoria = "P00", DescripcionCategoria = "Pescados/Mariscos", EstadoCategoria = true },
                new CategoriaProducto { CodigoCategoria = "L00", DescripcionCategoria = "Lácteos", EstadoCategoria = true });
            context.SaveChanges();

            context.TiposDeEntrada.AddOrUpdate(u => u.CodigoTipoEntrada,
                new TipoDeEntrada { CodigoTipoEntrada = "E00", DescripcionTipoEntrada = "Compras", EstadoTipoEntrada = true },
                new TipoDeEntrada { CodigoTipoEntrada = "E01", DescripcionTipoEntrada = "Otros", EstadoTipoEntrada = true });
            context.SaveChanges();

            context.TiposDeSalida.AddOrUpdate(u => u.CodigoTipoSalida,
                new TipoDeSalida { CodigoTipoSalida = "S00", DescripcionTipoSalida = "Venta", EstadoTipoSalida = true },
                new TipoDeSalida { CodigoTipoSalida = "S01", DescripcionTipoSalida = "Mal estado", EstadoTipoSalida = true },
                new TipoDeSalida { CodigoTipoSalida = "S02", DescripcionTipoSalida = "Otros", EstadoTipoSalida = true });
            context.SaveChanges();

            context.TiposDeSalida.AddOrUpdate(u => u.CodigoTipoSalida,
                new TipoDeSalida { CodigoTipoSalida = "S00", DescripcionTipoSalida = "Venta", EstadoTipoSalida = true },
                new TipoDeSalida { CodigoTipoSalida = "S01", DescripcionTipoSalida = "Mal estado", EstadoTipoSalida = true },
                new TipoDeSalida { CodigoTipoSalida = "S02", DescripcionTipoSalida = "Otros", EstadoTipoSalida = true });
            context.SaveChanges();

            context.UnidadesDeMedida.AddOrUpdate(u => u.CodigoUnidadMedida,
                new UnidadDeMedida { CodigoUnidadMedida = "U00", DescripcionUnidadMedida = "Unidad", AbreviaturaUM = "Ud.", EstadoUnidadMedida = true },
                new UnidadDeMedida { CodigoUnidadMedida = "U01", DescripcionUnidadMedida = "Libra", AbreviaturaUM = "Lb.", EstadoUnidadMedida = true },
                new UnidadDeMedida { CodigoUnidadMedida = "U02", DescripcionUnidadMedida = "Docena", AbreviaturaUM = "Doc.", EstadoUnidadMedida = true },
                new UnidadDeMedida { CodigoUnidadMedida = "U03", DescripcionUnidadMedida = "Cajilla 24", AbreviaturaUM = "Caj.", EstadoUnidadMedida = true },
                new UnidadDeMedida { CodigoUnidadMedida = "U04", DescripcionUnidadMedida = "Galón", AbreviaturaUM = "Gal.", EstadoUnidadMedida = true },
                new UnidadDeMedida { CodigoUnidadMedida = "U05", DescripcionUnidadMedida = "Quintal", AbreviaturaUM = "Q.", EstadoUnidadMedida = true },
                new UnidadDeMedida { CodigoUnidadMedida = "U06", DescripcionUnidadMedida = "Lata", AbreviaturaUM = "Lata.", EstadoUnidadMedida = true });
            context.SaveChanges();

            context.Datos.AddOrUpdate(d => d.DNI,
                new Dato { DNI = "xxx-xxxxxx-xxxxx", PNombre = "default", PApellido = "default"/*, SApellido = "default"*/, RUC = "12345678901234" });
            context.SaveChanges();

            context.CategoriasMenu.AddOrUpdate(d => d.CodigoCategoriaMenu,
                new CategoriaMenu { CodigoCategoriaMenu = "001", DescripcionCategoriaMenu = "Postres", EstadoCategoriaMenu = true },
                new CategoriaMenu { CodigoCategoriaMenu = "002", DescripcionCategoriaMenu = "Desayunos", EstadoCategoriaMenu = true },
                new CategoriaMenu { CodigoCategoriaMenu = "003", DescripcionCategoriaMenu = "Cena", EstadoCategoriaMenu = true },
                new CategoriaMenu { CodigoCategoriaMenu = "004", DescripcionCategoriaMenu = "Bebidas Alcholicas", EstadoCategoriaMenu = true },
                new CategoriaMenu { CodigoCategoriaMenu = "004", DescripcionCategoriaMenu = "Bebidas Alcholicas", EstadoCategoriaMenu = true });
            context.SaveChanges();
        }
    }
}