using UnityEngine;
using TMPro;
using UnityEngine.UI;
namespace DS
{
    public class RoundUI : MonoBehaviour
    {
        [SerializeField] private Button roundButton;
        [SerializeField] private Spawner spawner;
        [SerializeField] private TMP_Text waveCounter;
        [SerializeField] private TMP_Text scoreCounter;
        [SerializeField] private TMP_Text turretCounter;

        private GameData data;
        private void Start()
        {
            if (waveCounter == null) { Debug.LogError("Null Wave Counter", gameObject); }
            if (scoreCounter == null) { Debug.LogError("Null Score Counter", gameObject); }
            if (turretCounter == null) { Debug.LogError("Null Turret Counter", gameObject); }
            spawner = FindAnyObjectByType<Spawner>();
            data = WorldManager.Instance.playerData;
        }

        private void OnEnable()
        {
            roundButton.onClick.AddListener(() => spawner.StartRound());
            roundButton.onClick.AddListener(() => roundButton.gameObject.SetActive(false));
        }

        private void OnDisable()
        {
            roundButton.onClick.RemoveListener(() => spawner.StartRound());
        }


        private void FixedUpdate()
        {
            waveCounter.text = "Waves: " + data.currentRound.wave.ToString();
            scoreCounter.text = data.currentRound.score.ToString();
            turretCounter.text = data.currentRound.turretsPlaced.ToString();

            HandleRoundButton();

        }


        private void HandleRoundButton()
        {
            
            if (spawner.autoStart)
            {
                roundButton.gameObject.SetActive(false);
                return;
            }

            if (spawner.IsRoundActive)
            {
                roundButton.gameObject.SetActive(false);
                return;
            }
            

            roundButton.gameObject.SetActive(true);

        }
    }
}