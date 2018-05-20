using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MinesweeperWPF
{
    class Leaderboard
    {
        public string playerName { get; private set; }
        public string mineCount { get; private set; }
        public string timeSpent { get; private set; }

        public Leaderboard(string playerName, string mineCount, string timeSpent)
        {
            this.playerName = playerName;
            this.mineCount = mineCount;
            this.timeSpent = timeSpent;
        }
    }
}
