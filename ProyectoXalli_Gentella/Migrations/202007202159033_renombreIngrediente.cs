namespace ProyectoXalli_Gentella.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class renombreIngrediente : DbMigration
    {
        public override void Up()
        {
            DropIndex("Menu.Ingredientes", new[] { "Producto_Id" });
            RenameColumn(table: "Menu.Ingredientes", name: "Producto_Id", newName: "ProductoId");
            AlterColumn("Menu.Ingredientes", "ProductoId", c => c.Int(nullable: false));
            CreateIndex("Menu.Ingredientes", "ProductoId");
            DropColumn("Menu.Ingredientes", "IngredienteId");
        }
        
        public override void Down()
        {
            AddColumn("Menu.Ingredientes", "IngredienteId", c => c.Int(nullable: false));
            DropIndex("Menu.Ingredientes", new[] { "ProductoId" });
            AlterColumn("Menu.Ingredientes", "ProductoId", c => c.Int());
            RenameColumn(table: "Menu.Ingredientes", name: "ProductoId", newName: "Producto_Id");
            CreateIndex("Menu.Ingredientes", "Producto_Id");
        }
    }
}
