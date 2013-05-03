namespace ReadyPlayerSite.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class refactorFreezeInfoFrnKey : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.FreezeInfoes", "playerID");
        }
        
        public override void Down()
        {
            AddColumn("dbo.FreezeInfoes", "playerID", c => c.Int(nullable: false));
        }
    }
}
