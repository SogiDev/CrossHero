using UnityEngine;

namespace DS
{
    public class WorldManager : MonoBehaviour
    {
        public static WorldManager Instance;

        [Header("GameData")]
        public GameData playerData;

        [SerializeField] private bool saveGame = false;
        [SerializeField] private bool loadGame = false;

        [Header("Turrets")]
        [SerializeField] private TurretData[] allTurretData;

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


            playerData = SaveManager.LoadGame();
            if (playerData == null)
            {
                playerData = new GameData();
                SaveManager.SaveGame(playerData);
            }
        }

        private void Start()
        {
            
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

        public void Save() { SaveManager.SaveGame(playerData); }
        public GameData Load() { return SaveManager.LoadGame(); }

        public TurretData[] GetTurretData() { return allTurretData; }

    }
}