namespace CRConsultMvc.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class delChiaveEsternaInterventi : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Interventis", "Id", "dbo.AspNetUsers");
            DropIndex("dbo.Interventis", new[] { "Id" });
            AlterColumn("dbo.Interventis", "Id", c => c.String());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Interventis", "Id", c => c.String(maxLength: 128));
            CreateIndex("dbo.Interventis", "Id");
            AddForeignKey("dbo.Interventis", "Id", "dbo.AspNetUsers", "Id");
        }
    }
}
