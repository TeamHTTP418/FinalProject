namespace ReadyPlayerSite.Migrations
{
    using System;
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;
    using ReadyPlayerSite.Models;

    internal sealed class Configuration : DbMigrationsConfiguration<ReadyPlayerSite.Models.PlayerDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(ReadyPlayerSite.Models.PlayerDbContext context)
        {
            Random rand = new Random();

            List<User> users = new List<User>();
            for (int i = 1; i < 101; i++)
            {
                User user = new User { ID = i, password = "password", realName = "TesterReal" + i, username = "TesterUserName" + i };
                users.Add(user);
                context.Users.Add(user);
            }
            context.SaveChanges();
            List<Player> players = new List<Player>();
            foreach (User u in users)
            {
                int i = 1;
                Player player = createPlayer(u, rand, i++);
                players.Add(player);
                context.Players.Add(player);
            }
            context.SaveChanges();
            List<Task> tasks = new List<Task>();
            for (int i = 1; i < 11; i++)
            {
                Task task = createTask(rand, i);
                tasks.Add(task);
                context.Tasks.Add(task);
            }
            context.SaveChanges();
            var tasks_and_milestones = tasks.GroupBy(t => t.isMilestone).OrderBy(g => g.Key).Select(g => g.ToList()).ToArray();
            Player first = context.Players.First();
            first.tasksCompleted = new List<Task>();
            first.milestonesCompleted = new List<Task>();
            first.puzzleScore = 0;
            first.storyScore = 0;
            first.attendanceScore = 0;
            first.cooperationScore = 0;
            first.crossCurricularScore = 0;
            foreach (Task t in tasks_and_milestones[0])
            {
                first.tasksCompleted.Add(t);
                first.addTaskPoints(t);
            }
            foreach (Task m in tasks_and_milestones[1])
            {
                first.milestonesCompleted.Add(m);
                first.addTaskPoints(m);
            }
            context.SaveChanges();


        }

        private static Player createPlayer(User user, Random rand, int i = 0)
        {
            Player player = new Player
            {
                ID = i,
                eid = rand.Next(100000000, 999999999).ToString(),
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
                    t.end = DateTime.Now.AddDays(rand.NextDouble() * 28 - 14);
                    break;
                default:
                    break;
            }
            switch (rand.Next(0, 5))
            {
                case 0:
                    t.type = TaskType.Attendance;
                    break;
                case 1:
                    t.type = TaskType.Cooperation;
                    break;
                case 2:
                    t.type = TaskType.CrossCurricular;
                    break;
                case 3:
                    t.type = TaskType.Puzzle;
                    break;
                case 4:
                    t.type = TaskType.Story;
                    break;
            }
            if (rand.Next(0, 2) == 0)
            {
                t.isMilestone = true;
                t.maxCompletedBonus = rand.Next(5, 15);
                t.numberCompleted = 0;
                t.bonusPoints = rand.Next(5, 10);
                t.iconName = i == 0 ? "MileStoneIcon" : "MileStoneIcon" + i;
            }
            return t;
        }
    }

}
