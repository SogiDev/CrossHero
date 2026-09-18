using UnityEngine;
using TMPro;
namespace DS
{
    public class RoundUI : MonoBehaviour
    {
        [SerializeField] private TMP_Text waveCounter;
        [SerializeField] private TMP_Text scoreCounter;
        [SerializeField] private TMP_Text turretCounter;

        private GameData data;
        private void Start()
        {
            if (waveCounter == null) { Debug.LogError("Null Wave Counter", gameObject); }
            if (scoreCounter == null) { Debug.LogError("Null Score Counter", gameObject); }
            if (turretCounter == null) { Debug.LogError("Null Turret Counter", gameObject); }
            data = WorldManager.Instance.playerData;
        }
        private void FixedUpdate()
        {
            waveCounter.text = "Waves: " + data.currentRound.wave.ToString();
            scoreCounter.text = data.currentRound.score.ToString();
            turretCounter.text = data.currentRound.turretsPlaced.ToString();
        }

    }
}