using System.Collections;
using System.Reflection.Metadata.Ecma335;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;


namespace DS
{
    
    public class AudioManager : MonoBehaviour
    {
        public static AudioManager Instance;

        private GameData playerData;

        public enum AudioType
        {
            MASTER,
            MUSIC,
            SFX
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

        public float GetVolume(AudioType type)
        {
            switch (type)
            {
                case AudioType.MUSIC:
                    return masterVolume * musicVolume;
                case AudioType.SFX:
                    return masterVolume * sfxVolume;
                default:
                    return masterVolume;
            }
        }

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
        }

        private void Start()
        {
            playerData = GetComponent<WorldManager>().playerData;
        }

        
    }
        
}