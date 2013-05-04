namespace ReadyPlayerSite.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddDescriptionToTask : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Tasks", "description", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Tasks", "description");
        }
    }
}
