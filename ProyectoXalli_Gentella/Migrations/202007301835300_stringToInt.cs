namespace ProyectoXalli_Gentella.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class stringToInt : DbMigration
    {
        public override void Up()
        {
            AlterColumn("Ord.Ordenes", "CodigoOrden", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("Ord.Ordenes", "CodigoOrden", c => c.String(nullable: false, maxLength: 3));
        }
    }
}
