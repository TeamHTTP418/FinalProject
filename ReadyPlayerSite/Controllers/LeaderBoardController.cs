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
        private const int pageSize = 100;

        public LeaderBoardController()
        {
            PlayerDbContext Context = new PlayerDbContext();
            players = new GenericRepository<Player>(new StorageContext<Player>(Context));
        }


        public ActionResult Index(string scoreType, int page = 0)
        {
            IEnumerable<ScoreboardDetails> list;
            switch (scoreType)
            {
                case "puzzle":
                case "Puzzle":
                    list = getPages("Puzzle", page, page + 1);
                    ViewBag.boardType = "Puzzle";
                    break;
                case "story":
                case "Story":
                    list = getPages("Story", page, page + 1);
                    ViewBag.boardType = "Story";
                    break;
                case "cooperation":
                case "Cooperation":
                    list = getPages("Cooperation", page, page + 1);
                    ViewBag.boardType = "Cooperation";
                    break;
                case "crosscurricular":
                case "Cross Curricular":
                    list = getPages("Cross Curricular", page, page + 1);
                    ViewBag.boardType = "Cross Curricular";
                    break;
                case "attendance":
                case "Attendance":
                    list = getPages("Attendance", page, page + 1);
                    ViewBag.boardType = "Attendance";
                    break;
                default:
                    list = getPages("Total", page, page + 1);
                    ViewBag.boardType = "Total";
                    break;
            }

            ViewBag.page = page;

            return View(list.ToList());
        }

        [HttpPost]
        public JsonResult getPlayers(string scoreType, int page)
        {
            IEnumerable<ScoreboardDetails> list = getPages(scoreType, page, page + 1);

            bool morePlayers = (getAllPlayers().Count() - (page + 1) * pageSize) > 0;
            return Json(new { players = list.ToList(), morePlayers = morePlayers }, JsonRequestBehavior.AllowGet);
        }


        [HttpPost]
        public JsonResult findPlayer(string scoreType, string userName, int page = 0)
        {
            Player target = getAllPlayers().FirstOrDefault(s => s.user.username == userName);
            if (target == null)
            {
                return Json(new { found = false }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                IEnumerable<Player> allPlayers = getAllPlayers();
                int targetIndex = 0;
                switch (scoreType)
                {
                    case "puzzle":
                    case "Puzzle":
                        allPlayers = allPlayers.OrderByDescending(s => s.puzzleScore).ThenByDescending(s => s.user.username);
                        break;
                    case "story":
                    case "Story":
                        allPlayers = allPlayers.OrderByDescending(s => s.storyScore).ThenByDescending(s => s.user.username);
                        break;
                    case "cooperation":
                    case "Cooperation":
                        allPlayers = allPlayers.OrderByDescending(s => s.cooperationScore).ThenByDescending(s => s.user.username);
                        break;
                    case "crosscurricular":
                    case "Cross Curricular":
                        allPlayers = allPlayers.OrderByDescending(s => s.crossCurricularScore).ThenByDescending(s => s.user.username);
                        break;
                    case "attendance":
                    case "Attendance":
                        allPlayers = allPlayers.OrderByDescending(s => s.attendanceScore).ThenByDescending(s => s.user.username);
                        break;
                    default:
                        allPlayers = allPlayers.OrderByDescending(s => s.puzzleScore + s.storyScore + s.cooperationScore + s.crossCurricularScore + s.attendanceScore)
                            .ThenByDescending(s => s.user.username);
                        break;
                }
                targetIndex = allPlayers.ToList().IndexOf(target);
                if (targetIndex <= (page + 1) * pageSize)
                {
                    return Json(new { found = true, onPage = page }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    IEnumerable<ScoreboardDetails> pages = getPages(scoreType, page + 1, targetIndex / pageSize + 1);
                    return Json(new { found = true, onPage = targetIndex / pageSize, playersToLoad = pages.ToList() }, JsonRequestBehavior.AllowGet);
                }

            }

        }

        private IEnumerable<ScoreboardDetails> getPages(string scoreType, int startPage, int endPage)
        {
            IEnumerable<ScoreboardDetails> list;
            
            var allPlayers = getAllPlayers();
            if (allPlayers.Count() < startPage * pageSize)
            {
                return new List<ScoreboardDetails>().AsEnumerable();
            }
            
            switch (scoreType)
            {
                case "puzzle":
                case "Puzzle":
                    list = allPlayers.OrderByDescending(s => s.puzzleScore)
                        .ThenByDescending(s => s.user.username)
                        .Skip(startPage * pageSize)
                        .Take(Math.Min(allPlayers.Count() - startPage * pageSize, (endPage - startPage) * pageSize))
                        .Select(s => new ScoreboardDetails
                        {
                            player = s,
                            value = s.puzzleScore,
                            iconList = s.tasksCompleted.Where(m => m.task.isMilestone)
                                   .Select(i => new IconDetails
                                   {
                                       name = i.task.name,
                                       iconName = i.task.iconName
                                   })
                        });
                    break;
                case "story":
                case "Story":
                    list = allPlayers.OrderByDescending(s => s.storyScore)
                        .ThenByDescending(s => s.user.username)
                        .Skip(startPage * pageSize)
                        .Take(Math.Min(allPlayers.Count() - startPage * pageSize, (endPage - startPage) * pageSize))
                        .Select(s => new ScoreboardDetails
                        {
                            player = s,
                            value = s.storyScore,
                            iconList = s.tasksCompleted.Where(m => m.task.isMilestone)
                                   .Select(i => new IconDetails
                                   {
                                       name = i.task.name,
                                       iconName = i.task.iconName
                                   })
                        });
                    break;
                case "attendance":
                case "Attendance":
                    list = allPlayers.OrderByDescending(s => s.attendanceScore)
                        .ThenByDescending(s => s.user.username)
                        .Skip(startPage * pageSize)
                        .Take(Math.Min(allPlayers.Count() - startPage * pageSize, (endPage - startPage) * pageSize))
                        .Select(s => new ScoreboardDetails
                        {
                            player = s,
                            value = s.attendanceScore,
                            iconList = s.tasksCompleted.Where(m => m.task.isMilestone)
                                   .Select(i => new IconDetails
                                   {
                                       name = i.task.name,
                                       iconName = i.task.iconName
                                   })
                        });
                    break;
                case "crosscurricular":
                case "Cross Curricular":
                    list = allPlayers.OrderByDescending(s => s.crossCurricularScore)
                        .ThenByDescending(s => s.user.username)
                        .Skip(startPage * pageSize)
                        .Take(Math.Min(allPlayers.Count() - startPage * pageSize, (endPage - startPage) * pageSize))
                        .Select(s => new ScoreboardDetails
                        {
                            player = s,
                            value = s.crossCurricularScore,
                            iconList = s.tasksCompleted.Where(m => m.task.isMilestone)
                                   .Select(i => new IconDetails
                                   {
                                       name = i.task.name,
                                       iconName = i.task.iconName
                                   })
                        });
                    break;
                case "cooperation":
                case "Cooperation":
                    list = allPlayers.OrderByDescending(s => s.cooperationScore)
                        .ThenByDescending(s => s.user.username)
                        .Skip(startPage * pageSize)
                        .Take(Math.Min(allPlayers.Count() - startPage * pageSize, (endPage - startPage) * pageSize))
                        .Select(s => new ScoreboardDetails
                        {
                            player = s,
                            value = s.cooperationScore,
                            iconList = s.tasksCompleted.Where(m => m.task.isMilestone)
                                   .Select(i => new IconDetails
                                   {
                                       name = i.task.name,
                                       iconName = i.task.iconName
                                   })
                        });
                    break;
                default:
                    list = allPlayers.OrderByDescending(s => s.puzzleScore + s.storyScore + s.cooperationScore + s.crossCurricularScore + s.attendanceScore)
                        .ThenByDescending(s => s.user.username)
                        .Skip(startPage * pageSize)
                        .Take(Math.Min(allPlayers.Count() - startPage * pageSize, (endPage - startPage) * pageSize))
                        .Select(s => new ScoreboardDetails
                        {
                            player = s,
                            value = s.puzzleScore + s.storyScore + s.cooperationScore + s.crossCurricularScore + s.attendanceScore,
                            iconList = s.tasksCompleted.Where(m => m.task.isMilestone)
                                   .Select(i => new IconDetails
                                   {
                                       name = i.task.name,
                                       iconName = i.task.iconName
                                   })
                        });
                    break;
            }
            return list;
        }

        private IQueryable<Player> getAllPlayers()
        {
            return players.GetAll().Where(s => s.user.admin == false);
        }

    }
}
