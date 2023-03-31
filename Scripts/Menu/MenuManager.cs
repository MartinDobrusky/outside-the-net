using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuManager : MonoBehaviour
{
    private static bool _showFPS;
    [SerializeField] private Toggle fpsToggle;
    [SerializeField] private TextMeshProUGUI fpsText;
    [SerializeField] private float hudRefreshRate = 1f;

    [SerializeField] private TextMeshProUGUI versionText;

    [SerializeField] private Animator transition;
    [SerializeField] private float transitionTime = 1f;

    public Slider volumeSlider;

    [SerializeField] private RenderPipelineAsset[] renderPipelineAssets;
    [SerializeField] private TMP_Dropdown renderPipelineDropdown;

    [SerializeField] private TMP_Dropdown dificultyDropdown;
    private float _timer;
    
    [SerializeField] private TMP_Dropdown resolutionDropdown;
    [SerializeField] private TMP_Dropdown screenModeDropdown;

    private void Start()
    {
        // fpsText.gameObject.SetActive(_showFPS);

        if (versionText != null) versionText.text = "Early beta version " + Application.version;

        if (fpsToggle != null)
        {
            var fpsBool = PlayerPrefs.GetInt("ShowFPS", 0);
            _showFPS = fpsBool == 1;

            fpsToggle.SetIsOnWithoutNotify(_showFPS);
        }

        if (volumeSlider != null)
        {
            volumeSlider.onValueChanged.AddListener(delegate { SetVolume(volumeSlider.value); });
            volumeSlider.value = PlayerPrefs.GetFloat("volume", 1f);
            
            dificultyDropdown.onValueChanged.AddListener(delegate { SetDificulty(dificultyDropdown.value); });
        }

        if (renderPipelineDropdown != null)
        {
            renderPipelineDropdown.value = QualitySettings.GetQualityLevel();
            renderPipelineDropdown.value = PlayerPrefs.GetInt("quality", 0);

            ChangeQuality();
        }
        
        // Get the resolutions supported by the user's monitor
        if (resolutionDropdown != null)
        {
            List<Resolution> resolutions = new List<Resolution>(Screen.resolutions);
            
            resolutionDropdown.ClearOptions();
            
            // Add each resolution to the dropdown
            foreach (Resolution resolution in resolutions)
            {
                resolutionDropdown.options.Add(new TMP_Dropdown.OptionData(resolution.ToString()));
            }
            
            // Set the default resolution
            resolutionDropdown.value = resolutions.FindIndex(r => r.width == Screen.currentResolution.width && r.height == Screen.currentResolution.height);
            
            // Add a listener to the resolution dropdown so we can detect when a resolution is selected
            resolutionDropdown.onValueChanged.AddListener(delegate { OnResolutionChanged(); });
            
            // Add the screen modes to the screen mode dropdown
            screenModeDropdown.options.Add(new TMP_Dropdown.OptionData("Fullscreen"));
            screenModeDropdown.options.Add(new TMP_Dropdown.OptionData("Windowed"));
            screenModeDropdown.options.Add(new TMP_Dropdown.OptionData("Fullscreen Windowed"));

            // Set the default screen mode
            if (Screen.fullScreen)
            {
                screenModeDropdown.value = 0;
            }
            else if (Screen.fullScreenMode == FullScreenMode.Windowed)
            {
                screenModeDropdown.value = 1;
            }
            else if (Screen.fullScreenMode == FullScreenMode.FullScreenWindow)
            {
                screenModeDropdown.value = 2;
            }

            // Add a listener to the screen mode dropdown so we can detect when a mode is selected
            screenModeDropdown.onValueChanged.AddListener(delegate { OnScreenModeChanged(); });

            if (dificultyDropdown != null) dificultyDropdown.value = PlayerPrefs.GetInt("difficulty", 0);
        }
    }

    private void Update()
    {
        if (!_showFPS || !(Time.unscaledTime > _timer)) return;
        var fps = (int) (1f / Time.unscaledDeltaTime);
        fpsText.text = fps.ToString();
        _timer = Time.unscaledTime + hudRefreshRate;
    }
    
    private void OnResolutionChanged()
    {
        // Get the selected resolution from the dropdown
        Resolution selectedResolution = Screen.resolutions[resolutionDropdown.value];

        // Change the resolution of the game
        Screen.SetResolution(selectedResolution.width, selectedResolution.height, Screen.fullScreen);
    }
    
    void OnScreenModeChanged()
    {
        // Get the selected screen mode from the dropdown
        string selectedScreenMode = screenModeDropdown.options[screenModeDropdown.value].text;

        // Change the screen mode of the game
        switch (selectedScreenMode)
        {
            case "Fullscreen":
                Screen.fullScreenMode = FullScreenMode.FullScreenWindow;
                break;
            case "Windowed":
                Screen.fullScreenMode = FullScreenMode.Windowed;
                break;
            case "Fullscreen Windowed":
                Screen.fullScreenMode = FullScreenMode.FullScreenWindow;
                break;
            default:
                Debug.LogError("Invalid screen mode selected");
                break;
        }
    }

    // Universal method for loading scenes
    public void LoadScene(string sceneName)
    {
        // Calling the transition animation
        StartCoroutine(LoadSceneCoroutine(sceneName));
    }

    // Coroutine for the transition animation with waiting time of trasitionTime
    private IEnumerator LoadSceneCoroutine(string sceneName)
    {
        transition.SetTrigger("Start");

        yield return new WaitForSeconds(transitionTime);

        SceneManager.LoadScene(sceneName);
    }
    
    public void SetPlayerPrefs(string key)
    {
        PlayerPrefs.SetInt(key, 1);
    }

    // Function for quitting the game
    public void QuitGame()
    {
        Application.Quit();
    }

    // ---------- SETTINGS MENU ----------
    public void ToggleFPS()
    {
        _showFPS = !_showFPS;
        fpsText.gameObject.SetActive(_showFPS);
        PlayerPrefs.SetInt("showFPS", _showFPS ? 1 : 0);
    }

    // public void SetQuality(int qualityIndex)
    // {
    //     QualitySettings.SetQualityLevel(qualityIndex);
    // }

    public void SetDificulty(int difficulty)
    {
        PlayerPrefs.SetInt("difficulty", difficulty);
    }

    public void SetVolume(float volume)
    {
        AudioListener.volume = volume;
        PlayerPrefs.SetFloat("volume", volume);
    }

    public void ChangeQuality()
    {
        QualitySettings.SetQualityLevel(renderPipelineDropdown.value);
        PlayerPrefs.SetInt("quality", renderPipelineDropdown.value);
        QualitySettings.renderPipeline = renderPipelineAssets[renderPipelineDropdown.value];
    }
}