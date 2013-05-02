using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data;
using System.Data.Entity;
using ReadyPlayerSite.Models;
using ReadyPlayerSite.Repository;





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


            var playerListEnum = playerList.AsEnumerable();


            String[] sorts = sortOrder.Split(';');

            
            int lasteid = -1;
            int lastTotalScore = -1;
            int lastattendance = -1;
            int lastcrossCurricular = -1;
            int lastcooperation = -1;
            int laststory = -1;



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
                        hoursSort = hoursSort + s + ";";
                        numAsc = true;
                    }
                    if (s.Equals("eid_desc"))
                    {
                        playerListEnum = playerListEnum.OrderByDescending(x => x.eid);
                        titleSort = titleSort + s + ";";
                        hoursSort = hoursSort + s + ";";
                        numAsc = false;
                    }
                    if (s.Equals("TotalScore_asc"))
                    {
                        playerListEnum = playerListEnum.OrderBy(x => x.TotalScore);
                        numSort = numSort + s + ";";
                        hoursSort = hoursSort + s + ";";
                        titleAsc = true;
                    }
                    if (s.Equals("TotalScore_desc"))
                    {
                        playerListEnum = playerListEnum.OrderByDescending(x => x.TotalScore);
                        numSort = numSort + s + ";";
                        hoursSort = hoursSort + s + ";";
                        titleAsc = false;
                    }
                    if (s.Equals("attendance_asc"))
                    {
                        playerListEnum = playerListEnum.OrderBy(x => x.attendanceScore);
                        numSort = numSort + s + ";";
                        titleSort = titleSort + s + ";";
                        hoursAsc = true;
                    }
                    if (s.Equals("attendance_desc"))
                    {
                        playerListEnum = playerListEnum.OrderByDescending(x => x.attendanceScore);
                        numSort = numSort + s + ";";
                        titleSort = titleSort + s + ";";
                        hoursAsc = false;
                    }
                    if (s.Equals("puzzle_asc"))
                    {
                        playerListEnum = playerListEnum.OrderBy(x => x.puzzleScore);
                        numSort = numSort + s + ";";
                        titleSort = titleSort + s + ";";
                        hoursAsc = true;
                    }
                    if (s.Equals("puzzle_desc"))
                    {
                        playerListEnum = playerListEnum.OrderByDescending(x => x.puzzleScore);
                        numSort = numSort + s + ";";
                        titleSort = titleSort + s + ";";
                        hoursAsc = false;
                    }
                    if (s.Equals("crossCurricular_asc"))
                    {
                        playerListEnum = playerListEnum.OrderBy(x => x.crossCurricularScore);
                        numSort = numSort + s + ";";
                        titleSort = titleSort + s + ";";
                        hoursAsc = true;
                    }
                    if (s.Equals("crossCurricular_desc"))
                    {
                        playerListEnum = playerListEnum.OrderByDescending(x => x.crossCurricularScore);
                        numSort = numSort + s + ";";
                        titleSort = titleSort + s + ";";
                        hoursAsc = false;
                    }
                    if (s.Equals("cooperation_asc"))
                    {
                        playerListEnum = playerListEnum.OrderBy(x => x.cooperationScore);
                        numSort = numSort + s + ";";
                        titleSort = titleSort + s + ";";
                        hoursAsc = true;
                    }
                    if (s.Equals("cooperation_desc"))
                    {
                        playerListEnum = playerListEnum.OrderByDescending(x => x.cooperationScore);
                        numSort = numSort + s + ";";
                        titleSort = titleSort + s + ";";
                        hoursAsc = false;
                    }
                    if (s.Equals("story_asc"))
                    {
                        playerListEnum = playerListEnum.OrderBy(x => x.storyScore);
                        numSort = numSort + s + ";";
                        titleSort = titleSort + s + ";";
                        hoursAsc = true;
                    }
                    if (s.Equals("story_desc"))
                    {
                        playerListEnum = playerListEnum.OrderByDescending(x => x.storyScore);
                        numSort = numSort + s + ";";
                        titleSort = titleSort + s + ";";
                        hoursAsc = false;
                    }
                }


                return View(playerListEnum.ToPagedList(pageNumber, pageSize));
            }

        }
    }
}
