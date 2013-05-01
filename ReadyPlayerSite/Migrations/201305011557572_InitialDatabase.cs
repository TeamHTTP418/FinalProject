namespace ReadyPlayerSite.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialDatabase : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Users",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        username = c.String(nullable: false, maxLength: 100),
                        password = c.String(nullable: false),
                        realName = c.String(nullable: false, maxLength: 100),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.Players",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        userID = c.Int(nullable: false),
                        eid = c.String(),
                        attendanceScore = c.Int(nullable: false),
                        puzzleScore = c.Int(nullable: false),
                        crossCurricularScore = c.Int(nullable: false),
                        cooperationScore = c.Int(nullable: false),
                        storyScore = c.Int(nullable: false),
                        freezeInfoID = c.Int(),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.Users", t => t.userID, cascadeDelete: true)
                .Index(t => t.userID);
            
            CreateTable(
                "dbo.Tasks",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        name = c.String(),
                        start = c.DateTime(),
                        end = c.DateTime(),
                        type = c.Int(nullable: false),
                        value = c.Int(nullable: false),
                        isMilestone = c.Boolean(nullable: false),
                        bonusPoints = c.Int(),
                        numberCompleted = c.Int(),
                        maxCompletedBonus = c.Int(),
                        iconName = c.String(),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.AdminActions",
                c => new
                    {
                        ID = c.Int(nullable: false),
                        type = c.Int(nullable: false),
                        modifyTarget = c.Int(),
                        value = c.Int(),
                        when = c.DateTime(nullable: false),
                        reason = c.String(),
                        playerID = c.Int(nullable: false),
                        userID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.Users", t => t.ID)
                .ForeignKey("dbo.Players", t => t.playerID)
                .Index(t => t.ID)
                .Index(t => t.playerID);
            
            CreateTable(
                "dbo.FreezeInfoes",
                c => new
                    {
                        ID = c.Int(nullable: false),
                        freezeDate = c.DateTime(nullable: false),
                        reason = c.String(),
                        attendanceScore = c.Int(nullable: false),
                        puzzleScore = c.Int(nullable: false),
                        crossCurricularScore = c.Int(nullable: false),
                        cooperationScore = c.Int(nullable: false),
                        storyScore = c.Int(nullable: false),
                        playerID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.Players", t => t.ID)
                .Index(t => t.ID);
            
            CreateTable(
                "dbo.PlayerToMilestones",
                c => new
                    {
                        Player_ID = c.Int(nullable: false),
                        Task_ID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.Player_ID, t.Task_ID })
                .ForeignKey("dbo.Players", t => t.Player_ID, cascadeDelete: true)
                .ForeignKey("dbo.Tasks", t => t.Task_ID, cascadeDelete: true)
                .Index(t => t.Player_ID)
                .Index(t => t.Task_ID);
            
            CreateTable(
                "dbo.PlayerToTasks",
                c => new
                    {
                        Player_ID = c.Int(nullable: false),
                        Task_ID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.Player_ID, t.Task_ID })
                .ForeignKey("dbo.Players", t => t.Player_ID, cascadeDelete: true)
                .ForeignKey("dbo.Tasks", t => t.Task_ID, cascadeDelete: true)
                .Index(t => t.Player_ID)
                .Index(t => t.Task_ID);
            
        }
        
        public override void Down()
        {
            DropIndex("dbo.PlayerToTasks", new[] { "Task_ID" });
            DropIndex("dbo.PlayerToTasks", new[] { "Player_ID" });
            DropIndex("dbo.PlayerToMilestones", new[] { "Task_ID" });
            DropIndex("dbo.PlayerToMilestones", new[] { "Player_ID" });
            DropIndex("dbo.FreezeInfoes", new[] { "ID" });
            DropIndex("dbo.AdminActions", new[] { "playerID" });
            DropIndex("dbo.AdminActions", new[] { "ID" });
            DropIndex("dbo.Players", new[] { "userID" });
            DropForeignKey("dbo.PlayerToTasks", "Task_ID", "dbo.Tasks");
            DropForeignKey("dbo.PlayerToTasks", "Player_ID", "dbo.Players");
            DropForeignKey("dbo.PlayerToMilestones", "Task_ID", "dbo.Tasks");
            DropForeignKey("dbo.PlayerToMilestones", "Player_ID", "dbo.Players");
            DropForeignKey("dbo.FreezeInfoes", "ID", "dbo.Players");
            DropForeignKey("dbo.AdminActions", "playerID", "dbo.Players");
            DropForeignKey("dbo.AdminActions", "ID", "dbo.Users");
            DropForeignKey("dbo.Players", "userID", "dbo.Users");
            DropTable("dbo.PlayerToTasks");
            DropTable("dbo.PlayerToMilestones");
            DropTable("dbo.FreezeInfoes");
            DropTable("dbo.AdminActions");
            DropTable("dbo.Tasks");
            DropTable("dbo.Players");
            DropTable("dbo.Users");
        }
    }
}
