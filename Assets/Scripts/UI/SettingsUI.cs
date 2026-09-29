using TMPro;
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

        [Header("Button")]
        [SerializeField] private Sprite disabledButton;
        [SerializeField] private Sprite enabledButton;


        [Header("Audio")]
        [SerializeField] private Button masterButton;
        [SerializeField] private Button musicButton, gameplayButton, menuButton;
        [SerializeField] private Slider masterSlider, musicSlider, gameplaySlider, menuSlider;

        [Header("Game")]
        [SerializeField] private bool IsAutoActive = false;
        [SerializeField] private bool InfiniteHealth = false;
        private int defaultScore = 1000;
        [SerializeField] private TMP_InputField scoreText;

        private float waveScale = 1;
        [SerializeField] private TMP_InputField waveText;
        [SerializeField] private Button autoStartButton, infiniteHealthButton;
        

        private void OnEnable()
        {
            defaultScore = PlayerPrefs.GetInt("Default Score", 1000);
            waveScale = PlayerPrefs.GetFloat("Wave Scale", 1);

            EnableAudioUI();
            EnableGameUI();

            // Set Default Value
            IsAutoActive = PlayerPrefs.GetInt("Auto Start", 0) == 0 ? false : true;
            ChangeButtonState(autoStartButton, IsAutoActive);
            InfiniteHealth = PlayerPrefs.GetInt("Infinite Health", 0) == 0 ? false : true;
            ChangeButtonState(infiniteHealthButton, InfiniteHealth);
        }

        private void OnDisable()
        {
            DisableAudioUI();
            DisableGameUI();
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

        #region Game Settings

        private void EnableGameUI()
        {
            prevButton.onClick.AddListener(PreviousMenu);
            nextButton.onClick.AddListener(NextMenu);
            
            autoStartButton.onClick.AddListener(() => SetAuto());
            infiniteHealthButton.onClick.AddListener(() => SetInfinite());

        }
        private void DisableGameUI()
        {

            prevButton.onClick.RemoveAllListeners();
            nextButton.onClick.RemoveAllListeners();
            autoStartButton.onClick.RemoveAllListeners();
            infiniteHealthButton.onClick.RemoveAllListeners();
        }

        private void ChangeButtonState(Button button, bool isActive)
        {
            var buttonRenderer = button.GetComponent<Image>();
            buttonRenderer.sprite = isActive ? enabledButton : disabledButton;
        }
        private void SetAuto() 
        {
            IsAutoActive = !IsAutoActive;
            PlayerPrefs.SetInt("Auto Start", IsAutoActive ? 1 : 0);
            PlayerPrefs.Save();
            ChangeButtonState(autoStartButton, IsAutoActive);

            RoundManager.Instance.autoStart = IsAutoActive;

        }
        private void SetInfinite() 
        {
            InfiniteHealth = !InfiniteHealth;
            PlayerPrefs.SetInt("Infinite Health", InfiniteHealth ? 1 : 0);
            PlayerPrefs.Save();
            ChangeButtonState(infiniteHealthButton, InfiniteHealth);
        }

        public void HandleScore()
        {
            if (int.TryParse(scoreText.text, out var score))
            {
                defaultScore = score;
                PlayerPrefs.SetInt("Default Score", score);
                PlayerPrefs.Save();
            }
        }
        public void HandleWaveScale()
        {
            if (float.TryParse(waveText.text, out var wave))
            {
                waveScale = wave;
                PlayerPrefs.SetFloat("Wave Scale", wave);
                PlayerPrefs.Save();
            }
        }
        #endregion

        #region Audio

        private void EnableAudioUI()
        {
            masterButton.onClick.AddListener(() => EnableMasterVolume());
            musicButton.onClick.AddListener(() => EnableMusicVolume());
            gameplayButton.onClick.AddListener(() => EnableGameplayVolume());
            menuButton.onClick.AddListener(() => EnableMenuVolume());

            // Set Value
            masterSlider.value = AudioManager.Instance.MasterVolume;
            musicSlider.value = AudioManager.Instance.MusicVolume;
            gameplaySlider.value = AudioManager.Instance.GameplayVolume;
            menuSlider.value = AudioManager.Instance.MenuVolume;

            // Set Buttons
            masterSlider.onValueChanged.AddListener(value => UpdateMasterVolume(masterSlider.value));
            musicSlider.onValueChanged.AddListener(value => UpdateMusicVolume(musicSlider.value));
            gameplaySlider.onValueChanged.AddListener(value => UpdateGameplayVolume(gameplaySlider.value));
            menuSlider.onValueChanged.AddListener(value => UpdateMenuVolume(menuSlider.value));

        }

        private void DisableAudioUI()
        {
            masterButton.onClick.RemoveListener(() => EnableMasterVolume());
            musicButton.onClick.RemoveListener(() => EnableMusicVolume());
            gameplayButton.onClick.RemoveListener(() => EnableGameplayVolume());
            menuButton.onClick.RemoveListener(() => EnableMenuVolume());

            masterSlider.onValueChanged.RemoveListener(value => UpdateMasterVolume(masterSlider.value));
            musicSlider.onValueChanged.RemoveListener(value => UpdateMusicVolume(musicSlider.value));
            gameplaySlider.onValueChanged.RemoveListener(value => UpdateGameplayVolume(gameplaySlider.value));
            menuSlider.onValueChanged.RemoveListener(value => UpdateMenuVolume(menuSlider.value));

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
        private void EnableGameplayVolume() 
        { 
            PlayerPrefs.SetInt("GameplayVolumeEnabled", PlayerPrefs.GetInt("GameplayVolumeEnabled") != 0 ? PlayerPrefs.GetInt("GameplayVolumeEnabled") : 1);
            PlayerPrefs.Save();
            PlayerPrefs.Save();
        }
        private void EnableMenuVolume() 
        { 
            PlayerPrefs.SetInt("MenuVolumeEnabled", PlayerPrefs.GetInt("MenuVolumeEnabled") != 0 ? PlayerPrefs.GetInt("MenuVolumeEnabled") : 1);
            PlayerPrefs.Save();
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

        private void UpdateGameplayVolume(float value)
        {

            AudioManager.Instance.GameplayVolume = value;
            PlayerPrefs.Save();
        }
        private void UpdateMenuVolume(float value)
        {

            AudioManager.Instance.MenuVolume = value;
            PlayerPrefs.Save();
        }

        #endregion

    }
}