using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data;
using System.Data.Entity;
using ReadyPlayerSite.Models;
using ReadyPlayerSite.Repository;
using PagedList;
using ReadyPlayerSite.ViewModels;





namespace ReadyPlayerSite.Controllers
{
    public class LeaderBoardController : Controller
    {
        private IGenericRepository<Player> players;

        public LeaderBoardController()
        {
            PlayerDbContext Context = new PlayerDbContext();
            players = new GenericRepository<Player>(new StorageContext<Player>(Context));
        }





        public ActionResult Index(string scoreType)
        {
            List<ScoreboardDetails> list;
            switch (scoreType)
            {
                case "puzzle":
                    list = players.GetAll().OrderByDescending(s => s.puzzleScore).ThenByDescending(s => s.user.username).Select(s => new ScoreboardDetails { player = s, pointType = "Puzzle",  value = s.puzzleScore}).ToList();
                    break;
                case "story":
                    list = players.GetAll().OrderByDescending(s => s.storyScore).ThenByDescending(s => s.user.username).Select(s => new ScoreboardDetails { player = s, pointType = "Story", value = s.storyScore }).ToList();
                    break;
                case "attendance":
                    list = players.GetAll().OrderByDescending(s => s.attendanceScore).ThenByDescending(s => s.user.username).Select(s => new ScoreboardDetails { player = s, pointType = "Attendance", value = s.attendanceScore }).ToList();
                    break;
                case "crosscurricular":
                    list = players.GetAll().OrderByDescending(s => s.crossCurricularScore).ThenByDescending(s => s.user.username).Select(s => new ScoreboardDetails { player = s, pointType = "Cross Curricular", value = s.crossCurricularScore }).ToList();
                    break;
                case "cooperation":
                    list = players.GetAll().OrderByDescending(s => s.cooperationScore).ThenByDescending(s => s.user.username).Select(s => new ScoreboardDetails { player = s, pointType = "Cooperation", value = s.cooperationScore }).ToList();
                    break;
                default:
                    list = players.GetAll().OrderByDescending(s => s.totalScore()).ThenByDescending(s => s.user.username).Select(s => new ScoreboardDetails { player = s, pointType = scoreType, value = s.totalScore()}).ToList();

                    break;
            }
            return View(list);
        }















        public ActionResult Index(string sortOrder, int? page, string filterString)
        {
            int pageSize = 10;
            int pageNumber = (page ?? 1);

            String currentSort = "";
            String eidSort = "";
            String TotalScoreSort = "";
            String attendanceSort = "";
            String puzzleSort = "";
            String crossCurricularSort = "";
            String cooperationSort = "";
            String storySort = "";
            var playerList = from s in players.GetAll() select s;





            var playerListEnum = playerList.AsEnumerable();


            String[] sorts = sortOrder.Split(';');

            
            int lasteid = -1;
            int lastTotalScore = -1;
            int lastattendance = -1;
            int lastcrossCurricular = -1;
            int lastcooperation = -1;
            int laststory = -1;
            int lastpuzzle = -1;

            bool eidAsc = false;
            bool totalAsc = false;
            bool attendanceAsc = false;
            bool crossCurricularAsc = false;
            bool cooperationAsc = false;
            bool storyAsc = false;
            bool puzzleAsc = false;


            //parsing sorts
            for (int i = 0; i < sorts.Length; i++)
            {
                if (sorts[i].StartsWith("eid"))
                {
                    if (lasteid > 0)
                    {
                        sorts[lasteid] = "";
                    }
                    else
                    {
                        lasteid = i;
                    }
                }
                if (sorts[i].StartsWith("TotalScore"))
                {
                    if (lastTotalScore > 0)
                    {
                        sorts[lastTotalScore] = "";
                    }
                    else
                    {
                        lastTotalScore = i;
                    }
                }
                if (sorts[i].StartsWith("attendance"))
                {
                    if (lastattendance > 0)
                    {
                        sorts[lastattendance] = "";
                    }
                    else
                    {
                        lastattendance = i;
                    }
                }
                if (sorts[i].StartsWith("crossCurricular"))
                {
                    if (lastcrossCurricular > 0)
                    {
                        sorts[lastcrossCurricular] = "";
                    }
                    else
                    {
                        lastcrossCurricular = i;
                    }
                }
                if (sorts[i].StartsWith("cooperation"))
                {
                    if (lastcooperation > 0)
                    {
                        sorts[lastcooperation] = "";
                    }
                    else
                    {
                        lastcooperation = i;
                    }
                }
                if (sorts[i].StartsWith("story"))
                {
                    if (laststory > 0)
                    {
                        sorts[laststory] = "";
                    }
                    else
                    {
                        laststory = i;
                    }
                }
                if (sorts[i].StartsWith("puzzle"))
                {
                    if (lasteid > 0)
                    {
                        sorts[lastpuzzle] = "";
                    }
                    else
                    {
                        lasteid = i;
                    }
                }

                //doing sorts
                foreach (string s in sorts)
                {
                    if (s.Length <= 0)
                    {
                        continue;
                    }
                    currentSort = currentSort + s + ";";
                    if (s.Equals("eid_asc"))
                    {
                        playerListEnum = playerListEnum.OrderBy(x => x.eid);
                        eidSort = eidSort + s + ";";
                        eidAsc = true;
                    }
                    if (s.Equals("eid_desc"))
                    {
                        playerListEnum = playerListEnum.OrderByDescending(x => x.eid);
                        eidSort = eidSort + s + ";";
                        eidAsc = false;
                    }
                    if (s.Equals("TotalScore_asc"))
                    {
                        playerListEnum = playerListEnum.OrderBy(x => x.TotalScore());
                        TotalScoreSort = TotalScoreSort + s + ";";
                        totalAsc = true;
                    }
                    if (s.Equals("TotalScore_desc"))
                    {
                        playerListEnum = playerListEnum.OrderByDescending(x => x.TotalScore());
                        TotalScoreSort = TotalScoreSort + s + ";";
                        totalAsc = false;
                    }
                    if (s.Equals("attendance_asc"))
                    {
                        playerListEnum = playerListEnum.OrderBy(x => x.attendanceScore);
                        attendanceSort = attendanceSort + s + ";";
                        attendanceAsc = true;
                    }
                    if (s.Equals("attendance_desc"))
                    {
                        playerListEnum = playerListEnum.OrderByDescending(x => x.attendanceScore);
                        attendanceSort = attendanceSort + s + ";";
                        attendanceAsc = false;
                    }
                    if (s.Equals("puzzle_asc"))
                    {
                        playerListEnum = playerListEnum.OrderBy(x => x.puzzleScore);
                        puzzleSort = puzzleSort + s + ";";
                        puzzleAsc = true;
                    }
                    if (s.Equals("puzzle_desc"))
                    {
                        playerListEnum = playerListEnum.OrderByDescending(x => x.puzzleScore);
                        puzzleSort = puzzleSort + s + ";";
                        puzzleAsc = false;
                    }
                    if (s.Equals("crossCurricular_asc"))
                    {
                        playerListEnum = playerListEnum.OrderBy(x => x.crossCurricularScore);
                        crossCurricularSort = crossCurricularSort + s + ";";
                        crossCurricularAsc = true;
                    }
                    if (s.Equals("crossCurricular_desc"))
                    {
                        playerListEnum = playerListEnum.OrderByDescending(x => x.crossCurricularScore);
                        crossCurricularSort = crossCurricularSort + s + ";";
                        crossCurricularAsc = false;
                    }
                    if (s.Equals("cooperation_asc"))
                    {
                        playerListEnum = playerListEnum.OrderBy(x => x.cooperationScore);
                        cooperationSort = cooperationSort + s + ";";
                        cooperationAsc = true;
                    }
                    if (s.Equals("cooperation_desc"))
                    {
                        playerListEnum = playerListEnum.OrderByDescending(x => x.cooperationScore);
                        cooperationSort = cooperationSort + s + ";";
                        cooperationAsc = false;
                    }
                    if (s.Equals("story_asc"))
                    {
                        playerListEnum = playerListEnum.OrderBy(x => x.storyScore);
                        storySort = storySort + s + ";";
                        storyAsc = true;
                    }
                    if (s.Equals("story_desc"))
                    {
                        playerListEnum = playerListEnum.OrderByDescending(x => x.storyScore);
                        storySort = storySort + s + ";";
                        storyAsc = false;
                    }
                }

            }


            
            
            ViewBag.currentSort = currentSort;
            ViewBag.eidSort = eidSort;
            ViewBag.TotalScoreSort = TotalScoreSort;
            ViewBag.attendanceSort = attendanceSort;
            ViewBag.puzzleSort = puzzleSort;
            ViewBag.crossCurricularSort = crossCurricularSort;
            ViewBag.cooperationSort = cooperationSort;
            ViewBag.storySort = storySort;

            ViewBag.eidAsc = eidAsc;
            ViewBag.totalAsc = totalAsc;
            ViewBag.attendanceAsc = attendanceAsc;
            ViewBag.crossCurricularAsc = crossCurricularAsc;
            ViewBag.cooperationAsc = cooperationAsc;
            ViewBag.storyAsc = storyAsc;
            ViewBag.puzzleAsc = puzzleAsc;


            

            return View(playerListEnum.ToPagedList(pageNumber, pageSize));
        }

    }
    
}
