namespace ReadyPlayerSite.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddTokenToTask : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Tasks", "token", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Tasks", "token");
        }
    }
}
