namespace ProyectoXalli_Gentella.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Changes : DbMigration
    {
        public override void Up()
        {
            DropColumn("Inv.Datos", "SApellido");
        }
        
        public override void Down()
        {
            AddColumn("Inv.Datos", "SApellido", c => c.String(maxLength: 50));
        }
    }
}
