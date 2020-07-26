namespace ProyectoXalli_Gentella.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ChangesFields : DbMigration
    {
        public override void Up()
        {
            AlterColumn("Ord.Clientes", "EmailCliente", c => c.String(nullable: false, maxLength: 150));
            AlterColumn("Ord.Meseros", "INSS", c => c.String(nullable: false, maxLength: 9));
            AlterColumn("Inv.DetallesDeEntrada", "CantidadEntrada", c => c.Int(nullable: false));
            AlterColumn("Inv.Productos", "MarcaProducto", c => c.String(maxLength: 50));
        }
        
        public override void Down()
        {
            AlterColumn("Inv.Productos", "MarcaProducto", c => c.String(nullable: false, maxLength: 50));
            AlterColumn("Inv.DetallesDeEntrada", "CantidadEntrada", c => c.Double(nullable: false));
            AlterColumn("Ord.Meseros", "INSS", c => c.String(nullable: false, maxLength: 10));
            AlterColumn("Ord.Clientes", "EmailCliente", c => c.String(maxLength: 150));
        }
    }
}
