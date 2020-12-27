using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
namespace ProjectFukalite.Handlers
{
    public class MenuHandler : MonoBehaviour
    {
        [SerializeField] private Image[] settingImgs;
        [SerializeField] private Text[] settingTexts;
        [SerializeField] private List<Color> settingImgsColors = new List<Color>();
        [SerializeField] private List<Color> settingTextsColors = new List<Color>();
        [SerializeField] private GameObject settingsPanel;

        //Value Text
        [SerializeField] private Text sfxValText;
        [SerializeField] private Text musicValText;
        [SerializeField] private Text mouseSensValText;
        [SerializeField] private Text FOVValText;
        [SerializeField] private Text GFXValText;

        [SerializeField] private Color settingsPanelColor;

        private bool isSettings = false;

        [Header("Audio Sliders")]
        [SerializeField] private Slider sfxVolumeSlider;
        [SerializeField] private Slider musicVolumeSlider;

        [Header("Controls Sliders")]
        [SerializeField] private Slider mouseSensSlider;

        [Header("Video Settings")]
        [SerializeField] private Slider fovSlider; 
        [SerializeField] private Slider GFXSlider; 

        private void Start()
        {
            settingsPanel.SetActive(false);

            for (int i = 0; i < settingImgs.Length; i++)
            {
                settingImgsColors.Add(settingImgs[i].color);
            }
            
            for (int i = 0; i < settingTexts.Length; i++)
            {
                settingTextsColors.Add(settingTexts[i].color);
            }
        }

        private void Update()
        {
            settingsPanel.SetActive(isSettings);
            if (isSettings)
            {
                for (int i = 0; i < settingImgs.Length; i++)
                {
                    settingImgs[i].color = Color.Lerp(settingImgs[i].color, settingImgsColors[i], Time.fixedDeltaTime * 3.5f);
                }
                for (int i = 0; i < settingTexts.Length; i++)
                {
                    settingTexts[i].color = Color.Lerp(settingTexts[i].color, settingTextsColors[i], Time.fixedDeltaTime * 3.5f);
                }
            } else
            {
                for (int i = 0; i < settingImgs.Length; i++)
                {
                    settingImgs[i].color = Color.Lerp(settingImgs[i].color, Color.clear, Time.fixedDeltaTime * 3.5f);
                }
                for (int i = 0; i < settingTexts.Length; i++)
                {
                    settingTexts[i].color = Color.Lerp(settingTexts[i].color, Color.clear, Time.fixedDeltaTime * 3.5f);
                }
            }
            GameHandler.Settings.SFXVolume = sfxVolumeSlider.value / 100;
            GameHandler.Settings.MusicVolume = musicVolumeSlider.value / 100;
            GameHandler.Settings.MouseSens = mouseSensSlider.value / 100;
            GameHandler.Settings.FOV = fovSlider.value / 100;
            GameHandler.Settings.GFX = GFXSlider.value / 100;

            sfxValText.text = GameHandler.Settings.SFXVolume.ToString();
            musicValText.text = GameHandler.Settings.MusicVolume.ToString();
            mouseSensValText.text = GameHandler.Settings.MouseSens.ToString();
            FOVValText.text = GameHandler.Settings.FOV.ToString();
            GFXValText.text = GameHandler.Settings.GFX.ToString();
        }

        public void Load()
        {
            SceneManager.LoadScene("Tutorial");
        }

        public void NewGame()
        {

        }

        public void Settings()
        {
            isSettings = true;
        }

        public void ExitGame()
        {
            Debug.Log("Exitting game...");
            Application.Quit();
        }
    }
}