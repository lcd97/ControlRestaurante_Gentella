namespace ProyectoXalli_Gentella.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class estadoOrden : DbMigration
    {
        public override void Up()
        {
            AlterColumn("Ord.Ordenes", "EstadoOrden", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("Ord.Ordenes", "EstadoOrden", c => c.Boolean(nullable: false));
        }
    }
}
