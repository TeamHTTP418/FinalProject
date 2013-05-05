namespace ReadyPlayerSite.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddSolutionToTask : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Tasks", "solution", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Tasks", "solution");
        }
    }
}
