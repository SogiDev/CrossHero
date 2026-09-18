using UnityEngine;

namespace DS
{
    public class WorldManager : MonoBehaviour
    {
        public static WorldManager Instance;
        public GameData playerData;

        [SerializeField] private bool saveGame = false;
        [SerializeField] private bool loadGame = false;

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

    }
}