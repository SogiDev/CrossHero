using System;
using System.Collections.Generic;

namespace DS
{
    [Serializable]
    public class GameData
    {
        public int currentSession = 0;

        #region Score Data
        public int totalScore = 0;
        public int spentScore = 0;
        public int remainingScore = 0;

        public bool SpendScore(int cost)
        {
            if (remainingScore < cost) { return false; }

            spentScore += cost;
            remainingScore -= cost;

            return true;
        }

        public int CheckTotalScore()
        {
            int demoScore = 0;

            foreach (var rnd in rounds)
            {
                demoScore += rnd.score;
            }
            totalScore = demoScore;
            return totalScore;
        }

        #endregion

        #region Round Data

        [Serializable]
        public struct RoundData
        {
            public int score;
            public int wave;
            public int turretsPlaced;
        }

        public RoundData currentRound = new ();
        public RoundData[] rounds;

        public void SaveRound(RoundData newRound)
        {
            List<RoundData> newData = new List<RoundData>(rounds);
            newData.Add(newRound);
            rounds = newData.ToArray();
        }
        public RoundData GetRound(int sessionID) { return rounds[sessionID]; }

        #endregion

        #region Constructor
        public GameData()
        {
            this.rounds = new RoundData[1];
        }
        public GameData(RoundData[] rounds)
        {
            this.rounds = rounds;
        }

        public GameData(GameData data)
        {
            this.rounds = data.rounds;
        }

        #endregion
    }
}