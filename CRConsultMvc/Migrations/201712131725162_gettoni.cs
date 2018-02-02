namespace CRConsultMvc.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class gettoni : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Gettonis",
                c => new
                    {
                        Gettone_id = c.Int(nullable: false, identity: true),
                        Id = c.String(),
                        Data = c.DateTime(nullable: false),
                        NGettoni = c.Int(nullable: false),
                        Totale = c.Single(),
                        Pagato = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Gettone_id);
            
            CreateTable(
                "dbo.Interventis",
                c => new
                    {
                        Intervento_Id = c.Int(nullable: false, identity: true),
                        Id = c.String(),
                        DataIntervento = c.DateTime(nullable: false),
                        NGettoni = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Intervento_Id);
            
            AddColumn("dbo.AspNetUsers", "Nome", c => c.String());
            AddColumn("dbo.AspNetUsers", "Cognome", c => c.String());
            AddColumn("dbo.AspNetUsers", "Indirizzo", c => c.String());
            AddColumn("dbo.AspNetUsers", "Città", c => c.String());
            AddColumn("dbo.AspNetUsers", "CAP", c => c.String());
            AddColumn("dbo.AspNetUsers", "Telefono", c => c.String());
            AddColumn("dbo.AspNetUsers", "PartitaIva", c => c.String());
            AddColumn("dbo.AspNetUsers", "CodiceFiscale", c => c.String());
            AddColumn("dbo.AspNetUsers", "Privato", c => c.Boolean(nullable: false));
            AddColumn("dbo.AspNetUsers", "NGettoni", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.AspNetUsers", "NGettoni");
            DropColumn("dbo.AspNetUsers", "Privato");
            DropColumn("dbo.AspNetUsers", "CodiceFiscale");
            DropColumn("dbo.AspNetUsers", "PartitaIva");
            DropColumn("dbo.AspNetUsers", "Telefono");
            DropColumn("dbo.AspNetUsers", "CAP");
            DropColumn("dbo.AspNetUsers", "Città");
            DropColumn("dbo.AspNetUsers", "Indirizzo");
            DropColumn("dbo.AspNetUsers", "Cognome");
            DropColumn("dbo.AspNetUsers", "Nome");
            DropTable("dbo.Interventis");
            DropTable("dbo.Gettonis");
        }
    }
}
