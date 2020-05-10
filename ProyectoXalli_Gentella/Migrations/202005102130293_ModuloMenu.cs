namespace ProyectoXalli_Gentella.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ModuloMenu : DbMigration
    {
        public override void Up()
        {
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
                "Menu.MenusItem",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        CodigoMenu = c.String(nullable: false, maxLength: 3),
                        DescripcionMenu = c.String(nullable: false, maxLength: 100),
                        PrecioMenu = c.Double(nullable: false),
                        EstadoCategoriaMenu = c.Boolean(nullable: false),
                        CategoriaMenuId = c.Int(nullable: false),
                        ImagenId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("Menu.CategoriasMenu", t => t.CategoriaMenuId)
                .ForeignKey("Menu.Imagenes", t => t.ImagenId)
                .Index(t => t.CategoriaMenuId)
                .Index(t => t.ImagenId);
            
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
                .ForeignKey("Menu.MenusItem", t => t.MenuId)
                .Index(t => t.MenuId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("Menu.Recetas", "MenuId", "Menu.MenusItem");
            DropForeignKey("Menu.MenusItem", "ImagenId", "Menu.Imagenes");
            DropForeignKey("Menu.MenusItem", "CategoriaMenuId", "Menu.CategoriasMenu");
            DropIndex("Menu.Recetas", new[] { "MenuId" });
            DropIndex("Menu.MenusItem", new[] { "ImagenId" });
            DropIndex("Menu.MenusItem", new[] { "CategoriaMenuId" });
            DropTable("Menu.Recetas");
            DropTable("Menu.Imagenes");
            DropTable("Menu.MenusItem");
            DropTable("Menu.CategoriasMenu");
        }
    }
}
