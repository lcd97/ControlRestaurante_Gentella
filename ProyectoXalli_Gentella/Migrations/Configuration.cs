namespace ProyectoXalli_Gentella.Migrations
{
    using ProyectoXalli_Gentella.Models;
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<ProyectoXalli_Gentella.Models.DBControl>
    {
        public Configuration()
        {
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
                new Bodega { CodigoBodega = "B01", DescripcionBodega = "Bar" },
                new Bodega { CodigoBodega = "C01", DescripcionBodega = "Cocina" });
            context.SaveChanges();

            context.Categorias.AddOrUpdate(u => u.CodigoCategoria,
                new Categoria { CodigoCategoria = "B00", DescripcionCategoria = "Bebidas gaseosas", EstadoCategoria = true },
                new Categoria { CodigoCategoria = "B01", DescripcionCategoria = "Bebidas alcohólicas", EstadoCategoria = true },
                new Categoria { CodigoCategoria = "C00", DescripcionCategoria = "Condimientos", EstadoCategoria = true },
                new Categoria { CodigoCategoria = "F00", DescripcionCategoria = "Frutas", EstadoCategoria = true },
                new Categoria { CodigoCategoria = "V00", DescripcionCategoria = "Verduras", EstadoCategoria = true },
                new Categoria { CodigoCategoria = "C01", DescripcionCategoria = "Carnes", EstadoCategoria = true },
                new Categoria { CodigoCategoria = "P00", DescripcionCategoria = "Pescados/Mariscos", EstadoCategoria = true },
                new Categoria { CodigoCategoria = "L00", DescripcionCategoria = "Lácteos", EstadoCategoria = true });
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
                new UnidadDeMedida { CodigoUnidadMedida = "U00", DescripcionUnidadMedida = "Unidad", EstadoUnidadMedida = true },
                new UnidadDeMedida { CodigoUnidadMedida = "U01", DescripcionUnidadMedida = "Libra", EstadoUnidadMedida = true },
                new UnidadDeMedida { CodigoUnidadMedida = "U02", DescripcionUnidadMedida = "Docena", EstadoUnidadMedida = true },
                new UnidadDeMedida { CodigoUnidadMedida = "U03", DescripcionUnidadMedida = "Cajilla 24", EstadoUnidadMedida = true },
                new UnidadDeMedida { CodigoUnidadMedida = "U04", DescripcionUnidadMedida = "Galón", EstadoUnidadMedida = true },
                new UnidadDeMedida { CodigoUnidadMedida = "U05", DescripcionUnidadMedida = "Quintal", EstadoUnidadMedida = true },
                new UnidadDeMedida { CodigoUnidadMedida = "U06", DescripcionUnidadMedida = "Lata", EstadoUnidadMedida = true });
            context.SaveChanges();
        }
    }
}
