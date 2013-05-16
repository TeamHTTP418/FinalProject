namespace ReadyPlayerSite.Migrations
{
    using System;
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;
    using ReadyPlayerSite.Models;
    using System.Web.Security;
    using WebMatrix.WebData;
    using System.IO;

    internal sealed class Configuration : DbMigrationsConfiguration<ReadyPlayerSite.Models.PlayerDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(ReadyPlayerSite.Models.PlayerDbContext context)
        {
            Random rand = new Random();

            WebSecurity.InitializeDatabaseConnection(
                "PlayerDBContext",
                "Users",
                "ID",
                "username", autoCreateTables: true);

            if (!Roles.RoleExists("Administrator"))
            {
                Roles.CreateRole("Administrator");
            }

            if (!WebSecurity.UserExists("admin"))
            {
                WebSecurity.CreateUserAndAccount(
                    "admin",
                    "admin",
                        new { admin = true });
            }
            if (!Roles.GetRolesForUser("admin").Contains("Administrator"))
            {
                Roles.AddUserToRole("admin", "Administrator");
            }
            for (int i = 0; i < 500; i++)
            {
                if (!WebSecurity.UserExists("player" + i))
                {
                    WebSecurity.CreateUserAndAccount(
                        "player" + i,
                        "testpassword");
                }
            }

            context.SaveChanges();
            List<User> users = context.Users.ToList();
            foreach (User u in users)
            {
                Player player = createPlayer(u, rand);
                context.Players.Add(player);
            }
            context.SaveChanges();

            DirectoryInfo iconDirectory = new DirectoryInfo(@"ReadyPlayerSite/Content/icons");
            
            var icons = from FileInfo f in iconDirectory.GetFiles()
                        select Path.GetFileName(f.Name);
            for (int i = 1; i < 20; i++)
            {
                Task task = createTask(rand, i, icons.ToArray());
                context.Tasks.Add(task);
            }
            context.SaveChanges();

            var alltasks = context.Tasks.ToArray();
            foreach (Player player in context.Players.Take(50))
            {
                giveRandomTasks(player, alltasks, rand);
            }
            context.SaveChanges();
        }

        private static void giveRandomTasks(Player player, Task[] tasks, Random rand)
        {
            int count = rand.Next(0, tasks.Length);
            player.tasksCompleted = new List<PlayerToTask>();
            player.puzzleScore = 0;
            player.storyScore = 0;
            player.attendanceScore = 0;
            player.cooperationScore = 0;
            player.crossCurricularScore = 0;
            List<Task> added = new List<Task>();
            for (int i = 0; i < count; i++)
            {
                Task toAdd = tasks[rand.Next(0, tasks.Length)];
                if (!added.Contains(toAdd))
                {
                    player.addTaskPoints(toAdd);
                    player.tasksCompleted.Add(PlayerToTask.GetPTT(player, toAdd));
                    added.Add(toAdd);
                }
                
            }
        }

        private static Player createPlayer(User user, Random rand)
        {
            
            Player player = new Player
            {
                user = user/*,
                attendanceScore = rand.Next(0, 11),
                puzzleScore = rand.Next(0, 11),
                cooperationScore = rand.Next(0, 11),
                crossCurricularScore = rand.Next(0, 11),
                storyScore = rand.Next(0, 11)*/
            };
            return player;
        }

        private Task createTask(Random rand, int i = 0, string[] iconList = null)
        {
            Task t = new Task { ID = i, value = rand.Next(1, 10), name = i == 0 ? "New Task" : "New Task " + i };

            switch (rand.Next(0, 4))
            {
                case 0:
                    t.start = DateTime.Now.AddDays(rand.NextDouble() * 28 - 14);
                    break;
                case 1:
                    t.end = DateTime.Now.AddDays(rand.NextDouble() * 28 - 14);
                    break;
                case 2:
                    t.start = DateTime.Now.AddDays(rand.NextDouble() * 28 - 14);
                    t.end = t.start.Value.AddDays(rand.NextDouble() * 28 - 14);
                    break;
                default:
                    break;
            }
            TaskType[] types = Enum.GetValues(typeof(TaskType)) as TaskType[];
            t.type = types[rand.Next(0, types.Length)];
            string[] descriptions = { "Test Description", "Placeholder", "Auto Generated Description" };
            t.description = descriptions[rand.Next(0, descriptions.Length)];
            string[] icons = iconList; //{ "gate.png", "key.png", "quarter.png" };
            t.token = Guid.NewGuid().ToString();
            string[] keywords = { "Solution" };
            t.solution = keywords[rand.Next(0, keywords.Length)];
            if (rand.Next(0, 3) == 0)
            {
                t.isMilestone = true;
                t.maxCompletedBonus = rand.Next(5, 15);
                t.numberCompleted = 0;
                t.bonusPoints = rand.Next(5, 10);
                t.iconName = icons[rand.Next(0, icons.Length)];
            }
            return t;
        }
    }

}