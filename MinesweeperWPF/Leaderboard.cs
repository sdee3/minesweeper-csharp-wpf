using System;
using System.IO;

namespace MinesweeperWPF
{
    class Leaderboard
    {
        public string playerName { get; private set; }
        public string mineCount { get; private set; }
        public string timeSpent { get; private set; }
        private static int playerCount { get; set; }
        private string existingText { get; set; }

        private static string filePath;

        public Leaderboard(string playerName, string mineCount, string timeSpent)
        {
            filePath = "leaderboard.dat";
            this.playerName = playerName;
            this.mineCount = mineCount;
            this.timeSpent = timeSpent;

            if (!File.Exists(filePath))
                File.Create(filePath);
            else
                ReadDataFromFile();
        }

        private void ReadDataFromFile()
        {
            existingText = File.ReadAllText(filePath);
            playerCount = existingText.Split(new[] { Environment.NewLine }, StringSplitOptions.None).Length;
        }

        public void WriteDataToFile()
        {
            string result = existingText + 
                playerCount.ToString() + ". " + this.playerName + " sweeped " + this.mineCount + " mine(s) in " + this.timeSpent + " seconds!"
                + Environment.NewLine;
            playerCount++;
            File.WriteAllText(filePath, result);
        }
    }
}
