using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ReadyPlayerSite.Models;

namespace ReadyPlayerSite.ViewModels
{
    public class ScoreboardDetails
    {
        public int rank;
        public Player player;
        public int value;
        public IEnumerable<IconDetails> iconList;
    }
    public class IconDetails
    {
        public string name;
        public string iconName;
    }
}