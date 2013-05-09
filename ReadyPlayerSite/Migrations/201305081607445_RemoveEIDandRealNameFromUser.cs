namespace ReadyPlayerSite.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RemoveEIDandRealNameFromUser : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.Users", "realName");
            DropColumn("dbo.Players", "eid");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Players", "eid", c => c.String());
            AddColumn("dbo.Users", "realName", c => c.String(nullable: false, maxLength: 100));
        }
    }
}
