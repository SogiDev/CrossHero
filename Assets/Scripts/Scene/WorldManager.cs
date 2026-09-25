using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace DS
{
    [RequireComponent(typeof(AudioManager))]
    public class WorldManager : MonoBehaviour
    {
        public static WorldManager Instance;
        private AudioManager audioManager;

        [Header("GameData")]
        public GameData playerData;

        [SerializeField] private bool saveGame = false;
        [SerializeField] private bool loadGame = false;
        public int defaultScore = 1000;

        [Header("Turrets")]
        public Spawner spawner;
        [SerializeField] private TurretData[] allTurretData;

        public Sprite[] projectiles { get; private set; }

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(gameObject);
            }
            else
            {
                Destroy(gameObject);
            }

            audioManager = GetComponent<AudioManager>();


            playerData = SaveManager.LoadGame();
            if (playerData == null)
            {
                playerData = new GameData();
                SaveManager.SaveGame(playerData);
            }
        }

        private void FixedUpdate()
        {
            if (saveGame)
            {
                Save();
                saveGame = false;
            }
            if (loadGame)
            {
                Load();
                loadGame = false;
            }
        }

        #region Scene Management
        public IEnumerator LoadScene(int sceneIndex)
        {
            //PlayerUI.Instance.loadingScreen.SetActive(true);
            var activeScene = SceneManager.GetActiveScene();

            AsyncOperation loadOperation = SceneManager.LoadSceneAsync(sceneIndex, LoadSceneMode.Single);
            
            // Loading Bar
            while (!loadOperation.isDone)
            {
                float progressValue = Mathf.Clamp01(loadOperation.progress / 0.9f);
                //PlayerUI.Instance.loadingBar.Value = progressValue;
                yield return null;
            }

            if (loadOperation.isDone)
            {
                // Set Up Scene
            }
        
        }
#endregion

        #region Save Data
        public void Save() { SaveManager.SaveGame(playerData); }
        public GameData Load() { return SaveManager.LoadGame(); }
        #endregion

        #region Round
        public TurretData[] GetTurretData() { return allTurretData; }

        public void StartGame()
        {
            playerData.currentRound = new GameData.RoundData();
            playerData.currentRound.AddScore(defaultScore);
        }

        private readonly WaitForSeconds returnTime = new(120);
        public IEnumerator CompleteGame()
        {
            playerData.SaveRound(playerData.currentRound);
            playerData.CheckTotalScore();
            Save();

            yield return returnTime;

            StartCoroutine(LoadScene(0));
        }
        #endregion
    }
}