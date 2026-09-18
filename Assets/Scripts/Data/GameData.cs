using System;
using System.Collections.Generic;

namespace DS
{
    [Serializable]
    public class GameData
    {
        public int currentSession = 0;
        [Serializable]
        public struct RoundData
        {
            public int score;
            public int wave;
            public int turretsPlaced;
        }

        public RoundData currentRound = new ();
        public RoundData[] rounds;

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

        public void SaveRound(RoundData newRound)
        {
            List < RoundData > newData = new List<RoundData>(rounds);
            newData.Add(newRound);
            rounds = newData.ToArray();
        }
        public void SaveRound()
        {
            List < RoundData > newData = new List<RoundData>(rounds);
            newData.Add(currentRound);
            rounds = newData.ToArray();

            currentRound = new RoundData();
        }
        public RoundData GetRound(int sessionID) { return rounds[sessionID]; }

    }
}