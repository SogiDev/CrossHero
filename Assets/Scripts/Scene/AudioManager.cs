using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace DS
{
    [RequireComponent(typeof(WorldManager))]
    public class AudioManager : MonoBehaviour
    {
        public static AudioManager Instance;

        private GameData playerData;
        private AudioSource camera;

        public enum AudioType
        {
            MASTER,
            MUSIC,
            MENU_SFX,
            GAMEPLAY_SFX,

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
        private float gameplayVolume;
        public float GameplayVolume
        {
            get => gameplayVolume;
            set
            {
                if (gameplayVolume != value)
                {
                    gameplayVolume = value;
                    PlayerPrefs.SetFloat("GameplayVolume", gameplayVolume);
                    PlayerPrefs.Save();
                }
            }
        }
        private float menuVolume;
        public float MenuVolume
        {
            get => menuVolume;
            set
            {
                if (menuVolume != value)
                {
                    menuVolume = value;
                    PlayerPrefs.SetFloat("MenuVolume", menuVolume);
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
        public void EnableGameplayVolume(bool volume)
        {
            int isActive = volume == true ? 1 : 0;
            PlayerPrefs.SetInt("GamePlayVolumeEnabled", isActive);
            PlayerPrefs.Save();
        }
        public void EnableMenuVolume(bool volume)
        {
            int isActive = volume == true ? 1 : 0;
            PlayerPrefs.SetInt("MenuVolumeEnabled", isActive);
            PlayerPrefs.Save();
        }

        public float GetVolume(AudioType type)
        {
            switch (type)
            {
                case AudioType.MUSIC:
                    return masterVolume * musicVolume;
                case AudioType.GAMEPLAY_SFX:
                    return masterVolume * gameplayVolume;
                case AudioType.MENU_SFX:
                    return masterVolume * menuVolume;
                default:
                    return masterVolume;
            }
        }
        #endregion

        #region Sounds

        // Global Sounds
        public AudioClip menuClick;

        #endregion

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
            gameplayVolume = PlayerPrefs.GetFloat("GameplayVolume");
            menuVolume = PlayerPrefs.GetFloat("MenuVolume");
        }

        private void Start()
        {
            playerData = GetComponent<WorldManager>().playerData;
            camera = PlayerCamera.Instance.GetComponent<AudioSource>();
            SceneManager.sceneLoaded += OnSceneLoaded;
            AddMenuClick();
        }


        private void OnSceneLoaded(Scene oldScene, LoadSceneMode mode)
        {
            AddMenuClick();
        }

        public void PlayAudio(AudioType type, AudioClip clip)
        {
            camera.clip = clip;
            camera.volume = GetVolume(type);

            // Play Sound
            camera.Play();
        }

        public void AddMenuClick()
        {
            Button[] allButtons = FindObjectsByType<Button>(
                FindObjectsInactive.Include
                );
            foreach (Button button in allButtons)
            {
                //Debug.Log(button.gameObject.name + " added Menu Click", button.gameObject);
                button.onClick.AddListener(() => PlayAudio(AudioType.MENU_SFX, menuClick));
            }
        }

    }
        
}