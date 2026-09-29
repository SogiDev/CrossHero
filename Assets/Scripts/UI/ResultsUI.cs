using TMPro;
using UnityEngine;

namespace DS {
    public class ResultsUI : MonoBehaviour
    {

        private GameData playerData;
        [SerializeField] private TMP_Text scoreText, waveText, turretText;


        private void Start()
        {
            playerData = WorldManager.Instance.playerData;
        }
        private void FixedUpdate()
        {
            scoreText.text = "Total Score: " + playerData.currentRound.score.ToString();
            waveText.text = "Waves Survived: " + playerData.currentRound.wave.ToString();
            turretText.text = "Turrets Placed: " + playerData.currentRound.turretsPlaced.ToString();
        }
    }
}