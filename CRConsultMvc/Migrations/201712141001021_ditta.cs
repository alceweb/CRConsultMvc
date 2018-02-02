namespace CRConsultMvc.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ditta : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AspNetUsers", "Ditta", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.AspNetUsers", "Ditta");
        }
    }
}
