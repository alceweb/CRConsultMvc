namespace CRConsultMvc.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class user1 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Gettonis", "Id", c => c.String(maxLength: 128));
            CreateIndex("dbo.Gettonis", "Id");
            AddForeignKey("dbo.Gettonis", "Id", "dbo.AspNetUsers", "Id");
            DropColumn("dbo.Gettonis", "Id");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Gettonis", "Id", c => c.String());
            DropForeignKey("dbo.Gettonis", "Id", "dbo.AspNetUsers");
            DropIndex("dbo.Gettonis", new[] { "Id" });
            DropColumn("dbo.Gettonis", "Id");
        }
    }
}
