namespace ProyectoXalli_Gentella.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ModuloFact : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "Fact.DetallesDePago",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        CantidadPagar = c.Double(nullable: false),
                        MontoRecibido = c.Double(nullable: false),
                        MontoEntregado = c.Double(nullable: false),
                        TipoPagoId = c.Int(nullable: false),
                        PagoId = c.Int(nullable: false),
                        TipoDePago_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("Fact.Pagos", t => t.PagoId)
                .ForeignKey("Fact.TiposDePago", t => t.TipoDePago_Id)
                .Index(t => t.PagoId)
                .Index(t => t.TipoDePago_Id);
            
            CreateTable(
                "Fact.OrdenesPago",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        OrdenId = c.Int(nullable: false),
                        PagoId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("Ord.Ordenes", t => t.OrdenId)
                .ForeignKey("Fact.Pagos", t => t.PagoId)
                .Index(t => t.OrdenId)
                .Index(t => t.PagoId);
            
            CreateTable(
                "Fact.Pagos",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        FechaPago = c.DateTime(nullable: false),
                        Descuento = c.Int(nullable: false),
                        IVA = c.Int(nullable: false),
                        Propina = c.Double(nullable: false),
                        OrdenId = c.Int(nullable: false),
                        ImagenId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("Menu.Imagenes", t => t.ImagenId)
                .ForeignKey("Ord.Ordenes", t => t.OrdenId)
                .Index(t => t.OrdenId)
                .Index(t => t.ImagenId);
            
            CreateTable(
                "Fact.TiposDePago",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        CodigoTipoPago = c.String(nullable: false, maxLength: 3),
                        DescripcionTipoPago = c.String(nullable: false, maxLength: 50),
                        EstadoTipoPago = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("Fact.DetallesDePago", "TipoDePago_Id", "Fact.TiposDePago");
            DropForeignKey("Fact.OrdenesPago", "PagoId", "Fact.Pagos");
            DropForeignKey("Fact.Pagos", "OrdenId", "Ord.Ordenes");
            DropForeignKey("Fact.Pagos", "ImagenId", "Menu.Imagenes");
            DropForeignKey("Fact.DetallesDePago", "PagoId", "Fact.Pagos");
            DropForeignKey("Fact.OrdenesPago", "OrdenId", "Ord.Ordenes");
            DropIndex("Fact.Pagos", new[] { "ImagenId" });
            DropIndex("Fact.Pagos", new[] { "OrdenId" });
            DropIndex("Fact.OrdenesPago", new[] { "PagoId" });
            DropIndex("Fact.OrdenesPago", new[] { "OrdenId" });
            DropIndex("Fact.DetallesDePago", new[] { "TipoDePago_Id" });
            DropIndex("Fact.DetallesDePago", new[] { "PagoId" });
            DropTable("Fact.TiposDePago");
            DropTable("Fact.Pagos");
            DropTable("Fact.OrdenesPago");
            DropTable("Fact.DetallesDePago");
        }
    }
}
