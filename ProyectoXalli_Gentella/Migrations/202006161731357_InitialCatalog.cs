namespace ProyectoXalli_Gentella.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialCatalog : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "Inv.Bodegas",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        CodigoBodega = c.String(nullable: false, maxLength: 3),
                        DescripcionBodega = c.String(nullable: false, maxLength: 50),
                        EstadoBodega = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "Inv.Entradas",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        CodigoEntrada = c.String(nullable: false, maxLength: 3),
                        FechaEntrada = c.DateTime(nullable: false),
                        EstadoEntrada = c.Boolean(nullable: false),
                        TipoEntradaId = c.Int(nullable: false),
                        BodegaId = c.Int(nullable: false),
                        ProveedorId = c.Int(nullable: false),
                        TipoDeEntrada_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("Inv.Bodegas", t => t.BodegaId)
                .ForeignKey("Inv.Proveedores", t => t.ProveedorId)
                .ForeignKey("Inv.TiposDeEntrada", t => t.TipoDeEntrada_Id)
                .Index(t => t.BodegaId)
                .Index(t => t.ProveedorId)
                .Index(t => t.TipoDeEntrada_Id);
            
            CreateTable(
                "Inv.DetallesDeEntrada",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        CantidadEntrada = c.Int(nullable: false),
                        PrecioEntrada = c.Double(nullable: false),
                        EntradaId = c.Int(nullable: false),
                        ProductoId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("Inv.Entradas", t => t.EntradaId)
                .ForeignKey("Inv.Productos", t => t.ProductoId)
                .Index(t => t.EntradaId)
                .Index(t => t.ProductoId);
            
            CreateTable(
                "Inv.Productos",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        CodigoProducto = c.String(nullable: false, maxLength: 3),
                        DescripcionProducto = c.String(nullable: false, maxLength: 50),
                        MarcaProducto = c.String(nullable: false, maxLength: 50),
                        CantidadMaxProducto = c.Double(nullable: false),
                        CantidadMinProducto = c.Double(nullable: false),
                        EstadoProducto = c.Boolean(nullable: false),
                        UnidadMedidaId = c.Int(nullable: false),
                        CategoriaId = c.Int(nullable: false),
                        UnidadDeMedida_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("Inv.CategoriasProducto", t => t.CategoriaId)
                .ForeignKey("Inv.UnidadesDeMedida", t => t.UnidadDeMedida_Id)
                .Index(t => t.CategoriaId)
                .Index(t => t.UnidadDeMedida_Id);
            
            CreateTable(
                "Inv.CategoriasProducto",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        CodigoCategoria = c.String(nullable: false, maxLength: 3),
                        DescripcionCategoria = c.String(nullable: false, maxLength: 50),
                        EstadoCategoria = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "Inv.DetallesDeSalida",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        CantidadSalida = c.Int(nullable: false),
                        SalidaId = c.Int(nullable: false),
                        ProductoId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("Inv.Productos", t => t.ProductoId)
                .ForeignKey("Inv.Salidas", t => t.SalidaId)
                .Index(t => t.SalidaId)
                .Index(t => t.ProductoId);
            
            CreateTable(
                "Inv.Salidas",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        CodigoSalida = c.String(nullable: false, maxLength: 3),
                        FechaSalida = c.DateTime(nullable: false),
                        EstadoTipoSalida = c.Boolean(nullable: false),
                        TipoSalidaId = c.Int(nullable: false),
                        BodegaId = c.Int(nullable: false),
                        TipoDeSalida_Id = c.Int(),
                        Orden_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("Inv.Bodegas", t => t.BodegaId)
                .ForeignKey("Inv.TiposDeSalida", t => t.TipoDeSalida_Id)
                .ForeignKey("Ord.Ordenes", t => t.Orden_Id)
                .Index(t => t.BodegaId)
                .Index(t => t.TipoDeSalida_Id)
                .Index(t => t.Orden_Id);
            
            CreateTable(
                "Inv.TiposDeSalida",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        CodigoTipoSalida = c.String(nullable: false, maxLength: 3),
                        DescripcionTipoSalida = c.String(nullable: false, maxLength: 50),
                        EstadoTipoSalida = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "Inv.UnidadesDeMedida",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        CodigoUnidadMedida = c.String(nullable: false, maxLength: 3),
                        DescripcionUnidadMedida = c.String(nullable: false, maxLength: 50),
                        AbreviaturaUM = c.String(nullable: false, maxLength: 10),
                        EstadoUnidadMedida = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "Inv.Proveedores",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        NombreComercial = c.String(maxLength: 50),
                        Telefono = c.String(maxLength: 9),
                        EstadoProveedor = c.Boolean(nullable: false),
                        Local = c.Boolean(nullable: false),
                        RetenedorIR = c.Boolean(nullable: false),
                        DatoId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("Inv.Datos", t => t.DatoId)
                .Index(t => t.DatoId);
            
            CreateTable(
                "Inv.Datos",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        DNI = c.String(maxLength: 16),
                        PNombre = c.String(maxLength: 50),
                        PApellido = c.String(maxLength: 50),
                        RUC = c.String(maxLength: 14),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "Ord.Clientes",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Habitacion = c.String(maxLength: 50),
                        EmailCliente = c.String(maxLength: 50),
                        TelefonoCliente = c.String(maxLength: 9),
                        EstadoCliente = c.Boolean(nullable: false),
                        PasaporteCliente = c.String(maxLength: 10),
                        DatoId = c.Int(nullable: false),
                        TipoClienteId = c.Int(nullable: false),
                        TipoDeCliente_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("Inv.Datos", t => t.DatoId)
                .ForeignKey("Ord.TiposDeCliente", t => t.TipoDeCliente_Id)
                .Index(t => t.DatoId)
                .Index(t => t.TipoDeCliente_Id);
            
            CreateTable(
                "Ord.TiposDeCliente",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        CodigoTipoCliente = c.String(nullable: false, maxLength: 3),
                        DescripcionTipoCliente = c.String(nullable: false, maxLength: 50),
                        EstadoTipoCliente = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "Ord.Meseros",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        INSS = c.String(maxLength: 10),
                        HoraEntrada = c.String(nullable: false, maxLength: 5),
                        HoraSalida = c.String(nullable: false, maxLength: 5),
                        InicioTurno = c.String(nullable: false, maxLength: 5),
                        FinTurno = c.String(nullable: false, maxLength: 5),
                        DatoId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("Inv.Datos", t => t.DatoId)
                .Index(t => t.DatoId);
            
            CreateTable(
                "Ord.Ordenes",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        CodigoOrden = c.String(nullable: false, maxLength: 3),
                        FechaOrden = c.DateTime(nullable: false),
                        EstadoOrden = c.Boolean(nullable: false),
                        MeseroId = c.Int(nullable: false),
                        ClienteId = c.Int(nullable: false),
                        ImagenId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("Ord.Clientes", t => t.ClienteId)
                .ForeignKey("Menu.Imagenes", t => t.ImagenId)
                .ForeignKey("Ord.Meseros", t => t.MeseroId)
                .Index(t => t.MeseroId)
                .Index(t => t.ClienteId)
                .Index(t => t.ImagenId);
            
            CreateTable(
                "Ord.DetallesDeOrden",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        CantidadOrden = c.Int(nullable: false),
                        NotaDetalleOrden = c.String(nullable: false, maxLength: 150),
                        EstadoDetalleOrden = c.Boolean(nullable: false),
                        OrdenId = c.Int(nullable: false),
                        MenuId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("Menu.Menus", t => t.MenuId)
                .ForeignKey("Ord.Ordenes", t => t.OrdenId)
                .Index(t => t.OrdenId)
                .Index(t => t.MenuId);
            
            CreateTable(
                "Menu.Menus",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        CodigoMenu = c.String(nullable: false, maxLength: 3),
                        DescripcionMenu = c.String(nullable: false, maxLength: 100),
                        PrecioMenu = c.Double(nullable: false),
                        EstadoMenu = c.Boolean(nullable: false),
                        CategoriaMenuId = c.Int(nullable: false),
                        ImagenId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("Menu.CategoriasMenu", t => t.CategoriaMenuId)
                .ForeignKey("Menu.Imagenes", t => t.ImagenId)
                .Index(t => t.CategoriaMenuId)
                .Index(t => t.ImagenId);
            
            CreateTable(
                "Menu.CategoriasMenu",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        CodigoCategoriaMenu = c.String(nullable: false, maxLength: 3),
                        DescripcionCategoriaMenu = c.String(nullable: false, maxLength: 50),
                        EstadoCategoriaMenu = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "Menu.Imagenes",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Ruta = c.String(nullable: false, maxLength: 100),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "Menu.Recetas",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        TiempoEstimado = c.String(nullable: false, maxLength: 5),
                        Ingredientes = c.String(nullable: false, maxLength: 100),
                        MenuId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("Menu.Menus", t => t.MenuId)
                .Index(t => t.MenuId);
            
            CreateTable(
                "Inv.TiposDeEntrada",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        CodigoTipoEntrada = c.String(nullable: false, maxLength: 3),
                        DescripcionTipoEntrada = c.String(nullable: false, maxLength: 50),
                        EstadoTipoEntrada = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("Inv.Entradas", "TipoDeEntrada_Id", "Inv.TiposDeEntrada");
            DropForeignKey("Inv.Entradas", "ProveedorId", "Inv.Proveedores");
            DropForeignKey("Inv.Proveedores", "DatoId", "Inv.Datos");
            DropForeignKey("Inv.Salidas", "Orden_Id", "Ord.Ordenes");
            DropForeignKey("Ord.Ordenes", "MeseroId", "Ord.Meseros");
            DropForeignKey("Ord.Ordenes", "ImagenId", "Menu.Imagenes");
            DropForeignKey("Ord.DetallesDeOrden", "OrdenId", "Ord.Ordenes");
            DropForeignKey("Menu.Recetas", "MenuId", "Menu.Menus");
            DropForeignKey("Menu.Menus", "ImagenId", "Menu.Imagenes");
            DropForeignKey("Ord.DetallesDeOrden", "MenuId", "Menu.Menus");
            DropForeignKey("Menu.Menus", "CategoriaMenuId", "Menu.CategoriasMenu");
            DropForeignKey("Ord.Ordenes", "ClienteId", "Ord.Clientes");
            DropForeignKey("Ord.Meseros", "DatoId", "Inv.Datos");
            DropForeignKey("Ord.Clientes", "TipoDeCliente_Id", "Ord.TiposDeCliente");
            DropForeignKey("Ord.Clientes", "DatoId", "Inv.Datos");
            DropForeignKey("Inv.Productos", "UnidadDeMedida_Id", "Inv.UnidadesDeMedida");
            DropForeignKey("Inv.Salidas", "TipoDeSalida_Id", "Inv.TiposDeSalida");
            DropForeignKey("Inv.DetallesDeSalida", "SalidaId", "Inv.Salidas");
            DropForeignKey("Inv.Salidas", "BodegaId", "Inv.Bodegas");
            DropForeignKey("Inv.DetallesDeSalida", "ProductoId", "Inv.Productos");
            DropForeignKey("Inv.DetallesDeEntrada", "ProductoId", "Inv.Productos");
            DropForeignKey("Inv.Productos", "CategoriaId", "Inv.CategoriasProducto");
            DropForeignKey("Inv.DetallesDeEntrada", "EntradaId", "Inv.Entradas");
            DropForeignKey("Inv.Entradas", "BodegaId", "Inv.Bodegas");
            DropIndex("Menu.Recetas", new[] { "MenuId" });
            DropIndex("Menu.Menus", new[] { "ImagenId" });
            DropIndex("Menu.Menus", new[] { "CategoriaMenuId" });
            DropIndex("Ord.DetallesDeOrden", new[] { "MenuId" });
            DropIndex("Ord.DetallesDeOrden", new[] { "OrdenId" });
            DropIndex("Ord.Ordenes", new[] { "ImagenId" });
            DropIndex("Ord.Ordenes", new[] { "ClienteId" });
            DropIndex("Ord.Ordenes", new[] { "MeseroId" });
            DropIndex("Ord.Meseros", new[] { "DatoId" });
            DropIndex("Ord.Clientes", new[] { "TipoDeCliente_Id" });
            DropIndex("Ord.Clientes", new[] { "DatoId" });
            DropIndex("Inv.Proveedores", new[] { "DatoId" });
            DropIndex("Inv.Salidas", new[] { "Orden_Id" });
            DropIndex("Inv.Salidas", new[] { "TipoDeSalida_Id" });
            DropIndex("Inv.Salidas", new[] { "BodegaId" });
            DropIndex("Inv.DetallesDeSalida", new[] { "ProductoId" });
            DropIndex("Inv.DetallesDeSalida", new[] { "SalidaId" });
            DropIndex("Inv.Productos", new[] { "UnidadDeMedida_Id" });
            DropIndex("Inv.Productos", new[] { "CategoriaId" });
            DropIndex("Inv.DetallesDeEntrada", new[] { "ProductoId" });
            DropIndex("Inv.DetallesDeEntrada", new[] { "EntradaId" });
            DropIndex("Inv.Entradas", new[] { "TipoDeEntrada_Id" });
            DropIndex("Inv.Entradas", new[] { "ProveedorId" });
            DropIndex("Inv.Entradas", new[] { "BodegaId" });
            DropTable("Inv.TiposDeEntrada");
            DropTable("Menu.Recetas");
            DropTable("Menu.Imagenes");
            DropTable("Menu.CategoriasMenu");
            DropTable("Menu.Menus");
            DropTable("Ord.DetallesDeOrden");
            DropTable("Ord.Ordenes");
            DropTable("Ord.Meseros");
            DropTable("Ord.TiposDeCliente");
            DropTable("Ord.Clientes");
            DropTable("Inv.Datos");
            DropTable("Inv.Proveedores");
            DropTable("Inv.UnidadesDeMedida");
            DropTable("Inv.TiposDeSalida");
            DropTable("Inv.Salidas");
            DropTable("Inv.DetallesDeSalida");
            DropTable("Inv.CategoriasProducto");
            DropTable("Inv.Productos");
            DropTable("Inv.DetallesDeEntrada");
            DropTable("Inv.Entradas");
            DropTable("Inv.Bodegas");
        }
    }
}
