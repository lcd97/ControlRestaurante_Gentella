namespace ProyectoXalli_Gentella.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ModificacionesModelo : DbMigration
    {
        public override void Up()
        {
            AddColumn("Inv.Bodegas", "EstadoBodega", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("Inv.Bodegas", "EstadoBodega");
        }
    }
}
