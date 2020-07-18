namespace ProyectoXalli_Gentella.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class modificandoModelo : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("Inv.Turnos", "MeseroId", "Ord.Meseros");
            DropForeignKey("Ord.Ordenes", "TurnoId", "Inv.Turnos");
            DropIndex("Ord.Ordenes", new[] { "TurnoId" });
            DropIndex("Inv.Turnos", new[] { "MeseroId" });
            DropTable("Inv.Turnos");
        }
        
        public override void Down()
        {
            CreateTable(
                "Inv.Turnos",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        InicioTurno = c.DateTime(nullable: false),
                        FinTurno = c.String(nullable: false),
                        MeseroId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateIndex("Inv.Turnos", "MeseroId");
            CreateIndex("Ord.Ordenes", "TurnoId");
            AddForeignKey("Ord.Ordenes", "TurnoId", "Inv.Turnos", "Id");
            AddForeignKey("Inv.Turnos", "MeseroId", "Ord.Meseros", "Id");
        }
    }
}
