namespace ProyectoXalli_Gentella.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddDataTable : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "Inv.Datos",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        DNI = c.String(maxLength: 16),
                        PNombre = c.String(maxLength: 50),
                        PApellido = c.String(maxLength: 50),
                        SApellido = c.String(maxLength: 50),
                    })
                .PrimaryKey(t => t.Id);
            
            AddColumn("Inv.Proveedores", "Dato_Id", c => c.Int());
            CreateIndex("Inv.Proveedores", "Dato_Id");
            AddForeignKey("Inv.Proveedores", "Dato_Id", "Inv.Datos", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("Inv.Proveedores", "Dato_Id", "Inv.Datos");
            DropIndex("Inv.Proveedores", new[] { "Dato_Id" });
            DropColumn("Inv.Proveedores", "Dato_Id");
            DropTable("Inv.Datos");
        }
    }
}
