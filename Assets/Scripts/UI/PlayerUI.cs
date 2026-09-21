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
        //[SerializeField] internal SettingsUI settingsUI;

        [SerializeField] internal Button roundButton;
        private Spawner spawner;

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

        private void Start()
        {
            spawner = FindAnyObjectByType<Spawner>();
            ShowRound();
        }

        private void OnEnable()
        {
            roundButton.onClick.AddListener(() => StartCoroutine(spawner.StartRound()));
            roundButton.onClick.AddListener(() => roundButton.gameObject.SetActive(false));
        }

        private void OnDisable()
        {
            roundButton.onClick.RemoveListener(() => StartCoroutine(spawner.StartRound()));
        }

        public void ShowStore(TurretSurface turretSurface)
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

        public void ShowResults()
        {
            resultsUI.gameObject.SetActive(!resultsUI.gameObject.activeSelf);
        }

        public void ShowRound()
        {
            roundButton.gameObject.SetActive(true);
        }

        internal void CompleteGame(bool isWinner)
        {

        }
    }
}