namespace CRConsultMvc.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class interventidatanull : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Interventis", "DataIntervento", c => c.DateTime());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Interventis", "DataIntervento", c => c.DateTime(nullable: false));
        }
    }
}
