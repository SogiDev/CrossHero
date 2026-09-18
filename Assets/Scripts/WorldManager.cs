using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

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

            masterVolume = PlayerPrefs.GetFloat("MasterVolume");
            musicVolume = PlayerPrefs.GetFloat("MusicVolume");
            sfxVolume = PlayerPrefs.GetFloat("SFXVolume");


            playerData = SaveManager.LoadGame();
            if (playerData == null)
            {
                playerData = new GameData();
                SaveManager.SaveGame(playerData);
            }
        }

        #region Audio
        private float masterVolume;
        public float MasterVolume
        {
            get => masterVolume;
            set
            {
                if (masterVolume != value)
                {
                    masterVolume = value;
                    PlayerPrefs.SetFloat("MasterVolume", masterVolume);
                    PlayerPrefs.Save();
                }
            }
        }
        private float musicVolume;
        public float MusicVolume
        {
            get => musicVolume;
            set
            {
                if (musicVolume != value)
                {
                    musicVolume = value;
                    PlayerPrefs.SetFloat("MusicVolume", musicVolume);
                    PlayerPrefs.Save();
                }
            }
        }
        private float sfxVolume;
        public float SFXVolume
        {
            get => sfxVolume;
            set
            {
                if (sfxVolume != value)
                {
                    sfxVolume = value;
                    PlayerPrefs.SetFloat("SFXVolume", sfxVolume);
                    PlayerPrefs.Save();
                }
            }
        }

        public void EnableMasterVolume(bool volume)
        {
            int isActive = volume == true ? 1 : 0;
            PlayerPrefs.SetInt("MasterVolumeEnabled", isActive);
            PlayerPrefs.Save();
        }
        public void EnableMusicVolume(bool volume)
        {
            int isActive = volume == true ? 1 : 0;
            PlayerPrefs.SetInt("MusicVolumeEnabled", isActive);
            PlayerPrefs.Save();
        }
        public void EnableSFXVolume(bool volume)
        {
            int isActive = volume == true ? 1 : 0;
            PlayerPrefs.SetInt("SFXVolumeEnabled", isActive);
            PlayerPrefs.Save();
        }
        #endregion

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

        #region Scene Management
        public IEnumerator LoadScene()
        {

        }

        #region Save Data
        public void Save() { SaveManager.SaveGame(playerData); }
        public GameData Load() { return SaveManager.LoadGame(); }
        #endregion
        public TurretData[] GetTurretData() { return allTurretData; }

    }
}