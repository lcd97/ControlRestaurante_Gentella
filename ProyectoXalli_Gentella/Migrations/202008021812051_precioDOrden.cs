namespace ProyectoXalli_Gentella.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class precioDOrden : DbMigration
    {
        public override void Up()
        {
            AddColumn("Ord.DetallesDeOrden", "PrecioMenu", c => c.Double(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("Ord.DetallesDeOrden", "PrecioMenu");
        }
    }
}
