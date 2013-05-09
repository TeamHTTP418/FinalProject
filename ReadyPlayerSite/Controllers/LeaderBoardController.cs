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















    }
    
}
