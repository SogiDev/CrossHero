using System;

namespace DS
{
    [Serializable]
    public class GameData
    {

        public int currentScore = 0;
        public int currentWave = 0;
        public int placedTurrets;

        [Serializable]
        public struct RoundData
        {
            public int score;
            public int wave;
            public int turretsPlaced;
        }

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

    }
}