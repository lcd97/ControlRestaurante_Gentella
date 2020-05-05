namespace ProyectoXalli_Gentella.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialCreate : DbMigration
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
                        EstadoTipoEntrada = c.Boolean(nullable: false),
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
                        CantidadProducto = c.Double(nullable: false),
                        CantidadMaxProducto = c.Double(nullable: false),
                        CantidadMinProducto = c.Double(nullable: false),
                        EstadoProducto = c.Boolean(nullable: false),
                        UnidadMedidaId = c.Int(nullable: false),
                        CategoriaId = c.Int(nullable: false),
                        UnidadDeMedida_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("Inv.Categorias", t => t.CategoriaId)
                .ForeignKey("Inv.UnidadesDeMedida", t => t.UnidadDeMedida_Id)
                .Index(t => t.CategoriaId)
                .Index(t => t.UnidadDeMedida_Id);
            
            CreateTable(
                "Inv.Categorias",
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
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("Inv.Bodegas", t => t.BodegaId)
                .ForeignKey("Inv.TiposDeSalida", t => t.TipoDeSalida_Id)
                .Index(t => t.BodegaId)
                .Index(t => t.TipoDeSalida_Id);
            
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
                        RUC = c.String(maxLength: 14),
                        EstadoProveedor = c.Boolean(nullable: false),
                        Local = c.Boolean(nullable: false),
                        RetenedorIR = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
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
            DropForeignKey("Inv.Productos", "UnidadDeMedida_Id", "Inv.UnidadesDeMedida");
            DropForeignKey("Inv.Salidas", "TipoDeSalida_Id", "Inv.TiposDeSalida");
            DropForeignKey("Inv.DetallesDeSalida", "SalidaId", "Inv.Salidas");
            DropForeignKey("Inv.Salidas", "BodegaId", "Inv.Bodegas");
            DropForeignKey("Inv.DetallesDeSalida", "ProductoId", "Inv.Productos");
            DropForeignKey("Inv.DetallesDeEntrada", "ProductoId", "Inv.Productos");
            DropForeignKey("Inv.Productos", "CategoriaId", "Inv.Categorias");
            DropForeignKey("Inv.DetallesDeEntrada", "EntradaId", "Inv.Entradas");
            DropForeignKey("Inv.Entradas", "BodegaId", "Inv.Bodegas");
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
            DropTable("Inv.Proveedores");
            DropTable("Inv.UnidadesDeMedida");
            DropTable("Inv.TiposDeSalida");
            DropTable("Inv.Salidas");
            DropTable("Inv.DetallesDeSalida");
            DropTable("Inv.Categorias");
            DropTable("Inv.Productos");
            DropTable("Inv.DetallesDeEntrada");
            DropTable("Inv.Entradas");
            DropTable("Inv.Bodegas");
        }
    }
}
