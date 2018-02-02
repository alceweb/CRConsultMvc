namespace CRConsultMvc.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class interventi1 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Interventis", "DataChiamata", c => c.DateTime(nullable: false));
            AddColumn("dbo.Interventis", "Descrizione", c => c.String());
            AddColumn("dbo.Interventis", "Chiuso", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Interventis", "Chiuso");
            DropColumn("dbo.Interventis", "Descrizione");
            DropColumn("dbo.Interventis", "DataChiamata");
        }
    }
}
