using UnityEngine;
using UnityEngine.UI;

namespace DS
{
    public class PlayerUI : MonoBehaviour
    {

        public static PlayerUI Instance;
        public UnityEngine.EventSystems.EventSystem eventSystem;


        [SerializeField] internal RoundUI roundUI;
        [SerializeField] internal StoreUI storeUI;
        [SerializeField] internal TurretUI turretUI;
        [SerializeField] internal ResultsUI resultsUI;
        [SerializeField] internal SettingsUI settingsUI;
        [SerializeField] private Button settingsButton, homeButton, returnButton, restartButton;

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
            if (eventSystem == null) eventSystem = GetComponentInChildren<UnityEngine.EventSystems.EventSystem>();

        }


        private void OnEnable()
        {
            
            settingsButton.onClick.AddListener(ShowSettings);
            homeButton.onClick.AddListener(Home);
            returnButton.onClick.AddListener(Home);
            restartButton.onClick.AddListener(Restart);
            restartButton.onClick.AddListener(() => WorldManager.Instance.StartGame());

            eventSystem.SetSelectedGameObject(settingsButton.gameObject);

        }

        private void OnDisable()
        {
            settingsButton.onClick.RemoveListener(ShowSettings);
            homeButton.onClick.RemoveListener(Home);
            returnButton.onClick.RemoveListener(Home);
            restartButton.onClick.RemoveListener(Restart);
            restartButton.onClick.RemoveListener(() => WorldManager.Instance.StartGame());
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

        public void Home()
        {
            WorldManager.Instance.Save();
            StartCoroutine(WorldManager.Instance.LoadScene(0));
        }
        public void Restart()
        {
            WorldManager.Instance.Save();
            WorldManager.Instance.StartGame();
            StartCoroutine(WorldManager.Instance.LoadScene(1));

        }

        public void ShowResults()
        {
            resultsUI.gameObject.SetActive(!resultsUI.gameObject.activeSelf);
        }
    }
}