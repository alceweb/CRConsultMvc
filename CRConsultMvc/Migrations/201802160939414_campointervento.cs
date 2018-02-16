namespace CRConsultMvc.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class campointervento : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Interventis", "Intervento", c => c.String());
            AlterColumn("dbo.Interventis", "Id", c => c.String(maxLength: 128));
            CreateIndex("dbo.Interventis", "Id");
            AddForeignKey("dbo.Interventis", "Id", "dbo.AspNetUsers", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Interventis", "Id", "dbo.AspNetUsers");
            DropIndex("dbo.Interventis", new[] { "Id" });
            AlterColumn("dbo.Interventis", "Id", c => c.String());
            DropColumn("dbo.Interventis", "Intervento");
        }
    }
}
