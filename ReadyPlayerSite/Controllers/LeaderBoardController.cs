using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data;
using System.Data.Entity;
using ReadyPlayerSite.Models;
using ReadyPlayerSite.Repository;
using ReadyPlayerSite.ViewModels;


namespace ReadyPlayerSite.Controllers
{
    public class LeaderBoardController : Controller
    {
        private IGenericRepository<Player> players;
        private PlayerDbContext db;
        private const int pageSize = 100;

        public LeaderBoardController()
        {
            db = new PlayerDbContext();
            PlayerDbContext Context = new PlayerDbContext();
            players = new GenericRepository<Player>(new StorageContext<Player>(Context));
        }

<<<<<<< HEAD
        public ActionResult Index(string scoreType, int page = 0)
        {
=======
        public ActionResult Index(string scoreType)
>>>>>>> ff3e71d75b59e0c1de11713a9a833fb22761393b

            IEnumerable<ScoreboardDetails> list;
            var allPlayers = players.GetAll();
            switch (scoreType)
            {
                case "puzzle":
                case "Puzzle":
                    list = allPlayers.OrderByDescending(s => s.puzzleScore)
                        .ThenByDescending(s => s.user.username)
                        .Skip(page * pageSize)
                        .Take(Math.Min(allPlayers.Count() - page * pageSize, pageSize))
                        .Select(s => new ScoreboardDetails
                        {
                            player = s,
                            value = s.puzzleScore,
                            iconList = s.tasksCompleted.Where(m => m.task.isMilestone)
                                   .Select(i => new IconDetails { name = i.task.name, iconName = i.task.iconName })
                        });
                    ViewBag.boardType = "Puzzle";
                    break;
                case "story":
                case "Story":
                    list = allPlayers.OrderByDescending(s => s.storyScore)
                        .ThenByDescending(s => s.user.username)
                        .Skip(page * pageSize)
                        .Take(Math.Min(allPlayers.Count() - page * pageSize, pageSize))
                        .Select(s => new ScoreboardDetails
                        {
                            player = s,
                            value = s.storyScore,
                            iconList = s.tasksCompleted.Where(m => m.task.isMilestone)
                                   .Select(i => new IconDetails { name = i.task.name, iconName = i.task.iconName })
                        });
                    ViewBag.boardType = "Story";
                    break;
                case "attendance":
                case "Attendance":
                    list = allPlayers.OrderByDescending(s => s.attendanceScore)
                        .ThenByDescending(s => s.user.username)
                        .Skip(page * pageSize)
                        .Take(Math.Min(allPlayers.Count() - page * pageSize, pageSize))
                        .Select(s => new ScoreboardDetails
                        {
                            player = s,
                            value = s.attendanceScore,
                            iconList = s.tasksCompleted.Where(m => m.task.isMilestone)
                                   .Select(i => new IconDetails { name = i.task.name, iconName = i.task.iconName })
                        });
                    ViewBag.boardType = "Attendance";
                    break;
                case "crosscurricular":
                case "Cross Curricular":
                    list = allPlayers.OrderByDescending(s => s.crossCurricularScore)
                        .ThenByDescending(s => s.user.username)
                        .Skip(Math.Min(page * pageSize, allPlayers.Count()))
                        .Take(Math.Min(allPlayers.Count() - page * pageSize, pageSize))
                        .Select(s => new ScoreboardDetails
                        {
                            player = s,
                            value = s.crossCurricularScore,
                            iconList = s.tasksCompleted.Where(m => m.task.isMilestone)
                                   .Select(i => new IconDetails { name = i.task.name, iconName = i.task.iconName })
                        });
                    ViewBag.boardType = "Cross Curricular";
                    break;
                case "cooperation":
                case "Cooperation":
                    list = allPlayers.OrderByDescending(s => s.cooperationScore)
                        .ThenByDescending(s => s.user.username)
                        .Skip(page * pageSize)
                        .Take(Math.Min(allPlayers.Count() - page * pageSize, pageSize))
                        .Select(s => new ScoreboardDetails
                        {
                            player = s,
                            value = s.cooperationScore,
                            iconList = s.tasksCompleted.Where(m => m.task.isMilestone)
                                   .Select(i => new IconDetails { name = i.task.name, iconName = i.task.iconName })
                        });
                    ViewBag.boardType = "Cooperation";
                    break;
                default:
                    list = allPlayers.OrderByDescending(s => s.puzzleScore + s.storyScore + s.cooperationScore + s.crossCurricularScore + s.attendanceScore)
                        .ThenByDescending(s => s.user.username)
                        .Skip(page * pageSize)
                        .Take(Math.Min(allPlayers.Count() - page * pageSize, pageSize))
                        .Select(s => new ScoreboardDetails
                        {
                            player = s,
                            value = s.puzzleScore + s.storyScore + s.cooperationScore + s.crossCurricularScore + s.attendanceScore,
                            iconList = s.tasksCompleted.Where(m => m.task.isMilestone)
                                   .Select(i => new IconDetails { name = i.task.name, iconName = i.task.iconName })
                        });
                    ViewBag.boardType = "Total";
                    break;
            }
            ViewBag.page = page;

<<<<<<< HEAD
            return View(list.ToList());
        }

        [HttpPost]
        public JsonResult getPlayers(string scoreType, int page)
=======
>>>>>>> ff3e71d75b59e0c1de11713a9a833fb22761393b
        {
            IEnumerable<ScoreboardDetails> list;
            var allPlayers = players.GetAll();
            switch (scoreType)
            {
                case "puzzle":
                case "Puzzle":
                    list = allPlayers.OrderByDescending(s => s.puzzleScore)
                        .ThenByDescending(s => s.user.username)
                        .Skip(page * pageSize)
                        .Take(Math.Min(allPlayers.Count() - page * pageSize, pageSize))
                        .Select(s => new ScoreboardDetails
                        {
                            player = s,
                            value = s.puzzleScore,
                            iconList = s.tasksCompleted.Where(m => m.task.isMilestone)
                                   .Select(i => new IconDetails { name = i.task.name, iconName = i.task.iconName })
                        });
                    ViewBag.boardType = "Puzzle";
                    break;
                case "story":
                case "Story":
                    list = allPlayers.OrderByDescending(s => s.storyScore)
                        .ThenByDescending(s => s.user.username)
                        .Skip(page * pageSize)
                        .Take(Math.Min(allPlayers.Count() - page * pageSize, pageSize))
                        .Select(s => new ScoreboardDetails
                        {
                            player = s,
                            value = s.storyScore,
                            iconList = s.tasksCompleted.Where(m => m.task.isMilestone)
                                   .Select(i => new IconDetails { name = i.task.name, iconName = i.task.iconName })
                        });
                    ViewBag.boardType = "Story";
                    break;
                case "attendance":
                case "Attendance":
                    list = allPlayers.OrderByDescending(s => s.attendanceScore)
                        .ThenByDescending(s => s.user.username)
                        .Skip(page * pageSize)
                        .Take(Math.Min(allPlayers.Count() - page * pageSize, pageSize))
                        .Select(s => new ScoreboardDetails
                        {
                            player = s,
                            value = s.attendanceScore,
                            iconList = s.tasksCompleted.Where(m => m.task.isMilestone)
                                   .Select(i => new IconDetails { name = i.task.name, iconName = i.task.iconName })
                        });
                    ViewBag.boardType = "Attendance";
                    break;
                case "crosscurricular":
                case "Cross Curricular":
                    list = allPlayers.OrderByDescending(s => s.crossCurricularScore)
                        .ThenByDescending(s => s.user.username)
                        .Skip(page * pageSize)
                        .Take(Math.Min(allPlayers.Count() - page * pageSize, pageSize))
                        .Select(s => new ScoreboardDetails
                        {
                            player = s,
                            value = s.crossCurricularScore,
                            iconList = s.tasksCompleted.Where(m => m.task.isMilestone)
                                   .Select(i => new IconDetails { name = i.task.name, iconName = i.task.iconName })
                        });
                    ViewBag.boardType = "Cross Curricular";
                    break;
                case "cooperation":
                case "Cooperation":
                    list = allPlayers.OrderByDescending(s => s.cooperationScore)
                        .ThenByDescending(s => s.user.username)
                        .Skip(page * pageSize)
                        .Take(Math.Min(allPlayers.Count() - page * pageSize, pageSize))
                        .Select(s => new ScoreboardDetails
                        {
                            player = s,
                            value = s.cooperationScore,
                            iconList = s.tasksCompleted.Where(m => m.task.isMilestone)
                                   .Select(i => new IconDetails { name = i.task.name, iconName = i.task.iconName })
                        });
                    ViewBag.boardType = "Cooperation";
                    break;
                default:
                    list = allPlayers.OrderByDescending(s => s.puzzleScore + s.storyScore + s.cooperationScore + s.crossCurricularScore + s.attendanceScore)
                        .ThenByDescending(s => s.user.username)
                        .Skip(page * pageSize)
                        .Take(Math.Min(allPlayers.Count() - page * pageSize, pageSize))
                        .Select(s => new ScoreboardDetails
                        {
                            player = s,
                            value = s.puzzleScore + s.storyScore + s.cooperationScore + s.crossCurricularScore + s.attendanceScore,
                            iconList = s.tasksCompleted.Where(m => m.task.isMilestone)
                                   .Select(i => new IconDetails { name = i.task.name, iconName = i.task.iconName })
                        });
                    ViewBag.boardType = "Total";
                    break;
            }
            bool morePlayers = (allPlayers.Count() - (page + 1) * pageSize) > 0;
            List<ScoreboardDetails> test = list.ToList();
            return Json(new { players = list.ToList(), morePlayers = morePlayers }, JsonRequestBehavior.AllowGet);
        }

<<<<<<< HEAD
        [HttpPost]
        public JsonResult findPlayer(string boardType, string userName, int page = 0)
        {
            int onPage = 0;
            IEnumerable<Player> allPlayers = db.Players.AsEnumerable();
            Player player = db.Players.Where(s => s.user.username == userName).FirstOrDefault();
            if (player == null)
            {
                return Json(new { foundPlayer = false, onpage = page, playerlist = new List<ScoreboardDetails>() }, JsonRequestBehavior.AllowGet);
            }
            IEnumerable<ScoreboardDetails> pages = null;
            switch (boardType)
            {
                case "puzzle":
                case "Puzzle":
                    allPlayers = allPlayers.OrderByDescending(s => s.puzzleScore).ThenByDescending(s => s.user.username);
                    onPage = allPlayers.ToList().IndexOf(player) / 100;
                    if (onPage > page)
                    {
                        pages = allPlayers.Skip(Math.Min(page * pageSize, allPlayers.Count()))
                            .Take(Math.Min((onPage - page) * pageSize, allPlayers.Count() - page * pageSize))
                            .Select(s => new ScoreboardDetails { player = s, iconList = s.tasksCompleted.Where(x => x.task.isMilestone).Select(i => new IconDetails { name = i.task.name, iconName = i.task.iconName }) });
                    }
                    break;
                case "attendance":
                case "Attendance":
                    allPlayers = allPlayers.OrderByDescending(s => s.attendanceScore).ThenByDescending(s => s.user.username);
                    onPage = allPlayers.ToList().IndexOf(player) / 100;
                    if (onPage > page)
                    {
                        pages = allPlayers.Skip(Math.Min(page * pageSize, allPlayers.Count()))
                            .Take(Math.Min((onPage - page) * pageSize, allPlayers.Count() - page * pageSize))
                            .Select(s => new ScoreboardDetails { player = s, iconList = s.tasksCompleted.Where(x => x.task.isMilestone).Select(i => new IconDetails { name = i.task.name, iconName = i.task.iconName }) });
                    }
                    break;
                case "cooperation":
                case "Cooperation":
                    allPlayers = allPlayers.OrderByDescending(s => s.cooperationScore).ThenByDescending(s => s.user.username);
                    onPage = allPlayers.ToList().IndexOf(player) / 100;
                    if (onPage > page)
                    {
                        pages = allPlayers.Skip(Math.Min(page * pageSize, allPlayers.Count()))
                            .Take(Math.Min((onPage - page) * pageSize, allPlayers.Count() - page * pageSize))
                            .Select(s => new ScoreboardDetails { player = s, iconList = s.tasksCompleted.Where(x => x.task.isMilestone).Select(i => new IconDetails { name = i.task.name, iconName = i.task.iconName }) });
                    }
                    break;
                case "crosscurricular":
                case "Cross Curricular":
                    allPlayers = allPlayers.OrderByDescending(s => s.crossCurricularScore).ThenByDescending(s => s.user.username);
                    onPage = allPlayers.ToList().IndexOf(player) / 100;
                    if (onPage > page)
                    {
                        pages = allPlayers.Skip(Math.Min(page * pageSize, allPlayers.Count()))
                            .Take(Math.Min((onPage - page) * pageSize, allPlayers.Count() - page * pageSize))
                            .Select(s => new ScoreboardDetails { player = s, iconList = s.tasksCompleted.Where(x => x.task.isMilestone).Select(i => new IconDetails { name = i.task.name, iconName = i.task.iconName }) });
                    }
                    break;
                case "story":
                case "Story":
                    allPlayers = allPlayers.OrderByDescending(s => s.storyScore).ThenByDescending(s => s.user.username);
                    onPage = allPlayers.ToList().IndexOf(player) / 100;
                    if (onPage > page)
                    {
                        pages = allPlayers.Skip(Math.Min(page * pageSize, allPlayers.Count()))
                            .Take(Math.Min((onPage - page) * pageSize, allPlayers.Count() - page * pageSize))
                            .Select(s => new ScoreboardDetails { player = s, iconList = s.tasksCompleted.Where(x => x.task.isMilestone).Select(i => new IconDetails { name = i.task.name, iconName = i.task.iconName }) });
                    }
                    break;
                default:
                    allPlayers = allPlayers.OrderByDescending(s => s.puzzleScore + s.storyScore + s.crossCurricularScore + s.cooperationScore + s.attendanceScore).ThenByDescending(s => s.user.username);
                    onPage = allPlayers.ToList().IndexOf(player) / 100;
                    if (onPage > page)
                    {
                        pages = allPlayers.Skip(Math.Min(page * pageSize, allPlayers.Count()))
                            .Take(Math.Min((onPage - page) * pageSize, allPlayers.Count() - page * pageSize))
                            .Select(s => new ScoreboardDetails
                                {
                                    player = s,
                                    value = 0,
                                    iconList = s.tasksCompleted.Where(x => x.task.isMilestone)
                                    .Select(i => new IconDetails
                                        {
                                            name = i.task.name,
                                            iconName = i.task.iconName
                                        })
                                });
                    }
                    break;
            }

            List<ScoreboardDetails> list = pages.ToList();
            if (pages == null)
            {
                return Json(new { foundPlayer = true, onpage = onPage }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(new { foundPlayer = true, onpage = onPage, playerlist = pages.ToList() }, JsonRequestBehavior.AllowGet);
            }
        }
=======












>>>>>>> ff3e71d75b59e0c1de11713a9a833fb22761393b


    }

}
