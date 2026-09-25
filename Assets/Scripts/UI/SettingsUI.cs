using UnityEngine;
using UnityEngine.UI;

namespace DS
{

    public class SettingsUI : MonoBehaviour
    {
        [Header("Menus")]
        [SerializeField] private GameObject[] menus;
        private int currentMenu;
        [SerializeField] private Button prevButton, nextButton;


        [Header("Audio")]
        [SerializeField] private Button masterButton;
        [SerializeField] private Button musicButton, sfxButton;
        [SerializeField] private Slider masterSlider, musicSlider, sfxSlider;
        

        private void OnEnable()
        {
            EnableAudioUI();
            prevButton.onClick.AddListener(PreviousMenu);
            nextButton.onClick.AddListener(NextMenu);
        }

        private void OnDisable()
        {
            DisableAudioUI();
            prevButton.onClick.AddListener(PreviousMenu);
            nextButton.onClick.AddListener(NextMenu);
        }

        private void PreviousMenu()
        {
            currentMenu--;
            if (currentMenu < 0)
            {
                currentMenu = menus.Length;
            }
            currentMenu = Mathf.Clamp(currentMenu, 0, menus.Length);

            for (int i = 0; i < menus.Length; i++)
            {
                var menu = menus[i];
                menu.SetActive(i == currentMenu);
            }
        }
        private void NextMenu()
        {
            currentMenu++;
            if (currentMenu > menus.Length)
            {
                currentMenu = 0;
            }
            currentMenu = Mathf.Clamp(currentMenu, 0, menus.Length);

            for (int i = 0; i < menus.Length; i++)
            {
                var menu = menus[i];
                menu.SetActive(i == currentMenu);
            }
        }
        

        #region Audio

        private void EnableAudioUI()
        {
            masterButton.onClick.AddListener(() => EnableMasterVolume());
            musicButton.onClick.AddListener(() => EnableMusicVolume());
            sfxButton.onClick.AddListener(() => EnableSFXVolume());

            // Set Value
            masterSlider.value = AudioManager.Instance.MasterVolume;
            musicSlider.value = AudioManager.Instance.MusicVolume;
            sfxSlider.value = AudioManager.Instance.SFXVolume;

            // Set Buttons
            masterSlider.onValueChanged.AddListener((float value) => UpdateMasterVolume(masterSlider.value));
            musicSlider.onValueChanged.AddListener((float value) => UpdateMusicVolume(musicSlider.value));
            sfxSlider.onValueChanged.AddListener((float value) => UpdateSFXVolume(sfxSlider.value));

        }

        private void DisableAudioUI()
        {
            masterButton.onClick.RemoveListener(() => EnableMasterVolume());
            musicButton.onClick.RemoveListener(() => EnableMusicVolume());
            sfxButton.onClick.RemoveListener(() => EnableSFXVolume());

            masterSlider.onValueChanged.RemoveListener((float value) => UpdateMasterVolume(masterSlider.value));
            musicSlider.onValueChanged.RemoveListener((float value) => UpdateMusicVolume(musicSlider.value));
            sfxSlider.onValueChanged.RemoveListener((float value) => UpdateSFXVolume(sfxSlider.value));

        }

        private void EnableMasterVolume() 
        { 
            PlayerPrefs.SetInt("MasterVolumeEnabled", PlayerPrefs.GetInt("MasterVolumeEnabled") != 0 ? PlayerPrefs.GetInt("MasterVolumeEnabled") : 1);
            PlayerPrefs.Save();
        }
        private void EnableMusicVolume() 
        { 
            PlayerPrefs.SetInt("MusicVolumeEnabled", PlayerPrefs.GetInt("MusicVolumeEnabled") != 0 ? PlayerPrefs.GetInt("MusicVolumeEnabled") : 1);
            PlayerPrefs.Save();
        }
        private void EnableSFXVolume() 
        { 
            PlayerPrefs.SetInt("SFXVolumeEnabled", PlayerPrefs.GetInt("SFXVolumeEnabled") != 0 ? PlayerPrefs.GetInt("SFXVolumeEnabled") : 1);
            PlayerPrefs.Save();
        }
        private void UpdateMasterVolume(float value)
        {

            AudioManager.Instance.MasterVolume = value;
            PlayerPrefs.Save();
        }

        private void UpdateMusicVolume(float value)
        {

            AudioManager.Instance.MusicVolume = value;
            PlayerPrefs.Save();
        }

        private void UpdateSFXVolume(float value)
        {

            AudioManager.Instance.SFXVolume = value;
            PlayerPrefs.Save();
        }

        #endregion

    }
}