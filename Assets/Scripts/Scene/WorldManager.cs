using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace DS
{
    [RequireComponent(typeof(AudioManager))]
    [RequireComponent(typeof(RoundManager))]
    public class WorldManager : MonoBehaviour
    {
        public static WorldManager Instance;
        private AudioManager audioManager;
        private RoundManager roundManager;

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

            defaultScore = PlayerPrefs.GetInt("Default Score", 1000);
            
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
            Save();
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
                Load();
            }
        
        }
#endregion

        #region Save Data
        public void Save() { 
            SaveManager.SaveGame(playerData);
        }
        public GameData Load() { return SaveManager.LoadGame(); }
        #endregion

        #region Round
        public TurretData[] GetTurretData() { return allTurretData; }

        public void StartGame()
        {
            playerData.currentRound = new GameData.RoundData();
            defaultScore = PlayerPrefs.GetInt("Default Score", 1000);
            playerData.currentRound.AddScore(defaultScore);
        }

        private readonly WaitForSeconds returnTime = new(120);
        public void CompleteGame()
        {
            foreach (var ship in FindObjectsByType<SpaceShipManager>())
            {
                Destroy(ship);
            }
            foreach (var spawn in FindObjectsByType<Spawner>())
            {
                spawn.gameObject.SetActive(false);
            }

            // Enable PlayerUI
            PlayerUI.Instance.ShowResults();
            AudioManager.Instance.PlayAudio(AudioManager.AudioType.MUSIC, AudioManager.Instance.finishSound);

            
            // Save Player Data
            playerData.SaveRound(playerData.currentRound);
            playerData.CheckTotalScore();
            Save();
        }
        #endregion
    }
}