using System;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;

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

        public void AddScore(float score)
        {
            currentRound.AddScore(Mathf.RoundToInt(score));
            remainingScore += Mathf.RoundToInt(score);
        }
        public void RemoveScore(float score)
        {
            currentRound.RemoveScore(Mathf.RoundToInt(score));
            remainingScore -= Mathf.RoundToInt(score);
            spentScore -= Mathf.RoundToInt(score);
        }

        #endregion

        #region Round Data

        [Serializable]
        public struct RoundData
        {
            public void AddScore(int sc) { score += sc; }
            public void RemoveScore(int sc) { score -= sc; }
            public int score { get; private set; }
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
            currentRound = new RoundData();
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