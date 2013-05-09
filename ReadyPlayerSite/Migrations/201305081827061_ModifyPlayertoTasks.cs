namespace ReadyPlayerSite.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ModifyPlayertoTasks : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.PlayerToMilestones", "Player_ID", "dbo.Players");
            DropForeignKey("dbo.PlayerToMilestones", "Task_ID", "dbo.Tasks");
            DropForeignKey("dbo.PlayerToTasks", "Player_ID", "dbo.Players");
            DropForeignKey("dbo.PlayerToTasks", "Task_ID", "dbo.Tasks");
            DropIndex("dbo.PlayerToMilestones", new[] { "Player_ID" });
            DropIndex("dbo.PlayerToMilestones", new[] { "Task_ID" });
            DropIndex("dbo.PlayerToTasks", new[] { "Player_ID" });
            DropIndex("dbo.PlayerToTasks", new[] { "Task_ID" });
            DropTable("dbo.PlayerToMilestones");
            DropTable("dbo.PlayerToTasks");
            CreateTable(
                "dbo.PlayerToTasks",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        playerID = c.Int(nullable: false),
                        taskID = c.Int(nullable: false),
                        when = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.Players", t => t.playerID, cascadeDelete: true)
                .ForeignKey("dbo.Tasks", t => t.taskID, cascadeDelete: true)
                .Index(t => t.playerID)
                .Index(t => t.taskID);
            
           
        }
        
        public override void Down()
        {
            DropIndex("dbo.PlayerToTasks", new[] { "taskID" });
            DropIndex("dbo.PlayerToTasks", new[] { "playerID" });
            DropForeignKey("dbo.PlayerToTasks", "taskID", "dbo.Tasks");
            DropForeignKey("dbo.PlayerToTasks", "playerID", "dbo.Players");
            DropTable("dbo.PlayerToTasks");
            CreateTable(
                "dbo.PlayerToTasks",
                c => new
                    {
                        Player_ID = c.Int(nullable: false),
                        Task_ID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.Player_ID, t.Task_ID });
            
            CreateTable(
                "dbo.PlayerToMilestones",
                c => new
                    {
                        Player_ID = c.Int(nullable: false),
                        Task_ID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.Player_ID, t.Task_ID });
            
            
            CreateIndex("dbo.PlayerToTasks", "Task_ID");
            CreateIndex("dbo.PlayerToTasks", "Player_ID");
            CreateIndex("dbo.PlayerToMilestones", "Task_ID");
            CreateIndex("dbo.PlayerToMilestones", "Player_ID");
            AddForeignKey("dbo.PlayerToTasks", "Task_ID", "dbo.Tasks", "ID", cascadeDelete: true);
            AddForeignKey("dbo.PlayerToTasks", "Player_ID", "dbo.Players", "ID", cascadeDelete: true);
            AddForeignKey("dbo.PlayerToMilestones", "Task_ID", "dbo.Tasks", "ID", cascadeDelete: true);
            AddForeignKey("dbo.PlayerToMilestones", "Player_ID", "dbo.Players", "ID", cascadeDelete: true);
        }
    }
}
