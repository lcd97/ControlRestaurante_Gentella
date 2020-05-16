namespace ProyectoXalli_Gentella.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ChangesCampos : DbMigration
    {
        public override void Up()
        {
            AddColumn("Menu.MenusItem", "EstadoMenu", c => c.Boolean(nullable: false));
            DropColumn("Menu.MenusItem", "EstadoCategoriaMenu");
        }
        
        public override void Down()
        {
            AddColumn("Menu.MenusItem", "EstadoCategoriaMenu", c => c.Boolean(nullable: false));
            DropColumn("Menu.MenusItem", "EstadoMenu");
        }
    }
}
