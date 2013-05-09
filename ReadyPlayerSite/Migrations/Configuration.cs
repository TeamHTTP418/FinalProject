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
                    "admin");
            }
            if (!Roles.GetRolesForUser("admin").Contains("Administrator"))
            {
                Roles.AddUserToRole("admin", "Administrator");
            }
            for (int i = 0; i < 20; i++)
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

            for (int i = 1; i < 11; i++)
            {
                Task task = createTask(rand, i);
                context.Tasks.Add(task);
            }
            context.SaveChanges();

            var tasks_and_milestones = context.Tasks.ToList().GroupBy(t => t.isMilestone).OrderBy(g => g.Key).Select(g => g.ToList()).ToArray();
            Player first = context.Players.First();
            first.tasksCompleted = new List<PlayerToTask>();
            first.puzzleScore = 0;
            first.storyScore = 0;
            first.attendanceScore = 0;
            first.cooperationScore = 0;
            first.crossCurricularScore = 0;
            foreach (Task t in tasks_and_milestones[0])
            {
                first.tasksCompleted.Add(PlayerToTask.GetPTT(first, t));
                first.addTaskPoints(t);
            }
            foreach (Task m in tasks_and_milestones[1])
            {
                first.tasksCompleted.Add(PlayerToTask.GetPTT(first, m));
                first.addTaskPoints(m);
            }
            context.SaveChanges();


        }

        private static Player createPlayer(User user, Random rand)
        {
            
            Player player = new Player
            {
                user = user,
                attendanceScore = rand.Next(0, 11),
                puzzleScore = rand.Next(0, 11),
                cooperationScore = rand.Next(0, 11),
                crossCurricularScore = rand.Next(0, 11),
                storyScore = rand.Next(0, 11)
            };
            return player;
        }

        private Task createTask(Random rand, int i = 0)
        {
            Task t = new Task { ID = i, value = rand.Next(0, 10), name = i == 0 ? "New Task" : "New Task " + i };

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
            string[] icons = { "gate.png", "key.png", "quarter.png" };
            t.token = Guid.NewGuid().ToString();
            string[] keywords = { "Solution" };
            t.solution = keywords[rand.Next(0, keywords.Length)];
            if (rand.Next(0, 2) == 0)
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