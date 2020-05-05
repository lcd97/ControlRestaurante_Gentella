namespace ProyectoXalli_Gentella.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddDataTable1 : DbMigration
    {
        public override void Up()
        {
            DropIndex("Inv.Proveedores", new[] { "Dato_Id" });
            RenameColumn(table: "Inv.Proveedores", name: "Dato_Id", newName: "DatoId");
            AlterColumn("Inv.Proveedores", "DatoId", c => c.Int(nullable: false));
            CreateIndex("Inv.Proveedores", "DatoId");
        }
        
        public override void Down()
        {
            DropIndex("Inv.Proveedores", new[] { "DatoId" });
            AlterColumn("Inv.Proveedores", "DatoId", c => c.Int());
            RenameColumn(table: "Inv.Proveedores", name: "DatoId", newName: "Dato_Id");
            CreateIndex("Inv.Proveedores", "Dato_Id");
        }
    }
}
