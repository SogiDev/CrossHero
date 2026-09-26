using UnityEngine;
using UnityEngine.UI;

namespace DS
{
    public class PlayerUI : MonoBehaviour
    {

        public static PlayerUI Instance;

        [SerializeField] internal RoundUI roundUI;
        [SerializeField] internal StoreUI storeUI;
        [SerializeField] internal TurretUI turretUI;
        [SerializeField] internal ResultsUI resultsUI;
        [SerializeField] internal SettingsUI settingsUI;
        public Button settingsButton;

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
            }
            else
            {
                Destroy(gameObject);
            }

            if (roundUI == null) roundUI = GetComponentInChildren<RoundUI>();
            if (storeUI == null) storeUI = GetComponentInChildren<StoreUI>();
            if (turretUI == null) turretUI = GetComponentInChildren<TurretUI>();
            if (resultsUI == null) resultsUI = GetComponentInChildren<ResultsUI>();
        }


        private void OnEnable()
        {
            
            settingsButton.onClick.AddListener(ShowSettings);
        }

        private void OnDisable()
        {
            settingsButton.onClick.RemoveListener(ShowSettings);
        }

        public void ShowStore(TurretSpawner turretSurface)
        {
            storeUI.gameObject.SetActive(true);
            storeUI.crystalStore.SetActive(false);
            storeUI.turretStore.SetActive(true);
            storeUI.SetPurchaseButton(turretSurface);
        }
        public void ShowStore(Crystal crystal)
        {
            storeUI.gameObject.SetActive(true);
            storeUI.turretStore.SetActive(false);
            storeUI.crystalStore.SetActive(true);
            storeUI.SetData(crystal);
        }

        public void ShowSettings()
        {
            settingsUI.gameObject.SetActive(!settingsUI.gameObject.activeSelf);
        }

        public void ShowResults()
        {
            resultsUI.gameObject.SetActive(!resultsUI.gameObject.activeSelf);
        }

        internal void CompleteGame(bool isWinner)
        {

        }
    }
}