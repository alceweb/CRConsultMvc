namespace CRConsultMvc.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class user : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Interventis", "Id", c => c.String(maxLength: 128));
            CreateIndex("dbo.Interventis", "Id");
            AddForeignKey("dbo.Interventis", "Id", "dbo.AspNetUsers", "Id");
            DropColumn("dbo.Interventis", "Id");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Interventis", "Id", c => c.String());
            DropForeignKey("dbo.Interventis", "Id", "dbo.AspNetUsers");
            DropIndex("dbo.Interventis", new[] { "Id" });
            DropColumn("dbo.Interventis", "Id");
        }
    }
}
