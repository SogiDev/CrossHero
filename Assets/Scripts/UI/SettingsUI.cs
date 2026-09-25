using UnityEngine;
using UnityEngine.UI;

namespace DS
{

    public class SettingsUI : MonoBehaviour
    {
        [Header("Audio")]
        [SerializeField] private Button masterButton;
        [SerializeField] private Button musicButton, sfxButton;
        [SerializeField] private Slider masterSlider, musicSlider, sfxSlider;

        private void OnEnable()
        {
            EnableAudioUI();
        }

        private void OnDisable()
        {
            DisableAudioUI();
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