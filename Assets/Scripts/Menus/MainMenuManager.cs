using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public enum PlayerType
{
    None,
    Human,
    Random,
    MCTS
}

public class MainMenuManager : MonoBehaviour
{
    [SerializeField] private TMP_Dropdown[] playerDropdowns;

    [SerializeField] private AudioMixer audioMixer;

    [SerializeField] private TMP_Dropdown resolutionDropdown;
    private Resolution[] resolutions;
    private double currentRefreshRate;
    private int currentResolutionIndex;

    [SerializeField] private Toggle fullScreenToggle;
    private FullScreenMode fullscreen;

    [SerializeField] private RectTransform optionsMenu;
    [SerializeField] private RectTransform optionsMenuDeployed;
    private Vector3 optionsMenuRetractedPos;
    private bool isOptionsMenuDeployed = false;

    [SerializeField] private float optionsDeploySpeed = 5f;

    private Coroutine lerpCorout = null;

    private void Start()
    {
        ReadPrefs();
        SetupResolution();
        
        optionsMenuRetractedPos = optionsMenu.position;
    }

    private void SetupResolution()
    {
        resolutionDropdown.ClearOptions();

        List<string> optionLabels = new List<string>();
        for (int i = 0; i < resolutions.Length; i++)
        {
            optionLabels.Add(resolutions[i].width + " x " +
                             resolutions[i].height + " " +
                             (int)resolutions[i].refreshRateRatio.value + "Hz");
        }

        resolutionDropdown.AddOptions(optionLabels);
        resolutionDropdown.value = currentResolutionIndex;
        resolutionDropdown.RefreshShownValue();
        
        SetResolution();
    }

    public void StartGame()
    {
        SceneManager.LoadScene("SceneIannis");
    }

    public void UpdatePlayers()
    {
        for (int i = 0; i < playerDropdowns.Length; i++)
        {
            MenuToGame.Instance.playerTypes[i] = (PlayerType)playerDropdowns[i].value;
        }
    }

    public void ToggleOptions()
    {
        if (lerpCorout != null) StopCoroutine(lerpCorout);

        if (isOptionsMenuDeployed)
        {
            isOptionsMenuDeployed = false;
            lerpCorout = StartCoroutine(SlideOptions(false));
        }
        else
        {
            isOptionsMenuDeployed = true;
            lerpCorout = StartCoroutine(SlideOptions(true));
        }
    }

    IEnumerator SlideOptions(bool dir)
    {
        if (dir)
        {
            while (optionsMenu.position.x <= optionsMenuDeployed.position.x - 0.01)
            {
                Lerp(optionsMenuDeployed.position.x);
                yield return null;
            }
        }
        else
        {
            while (optionsMenu.position.x >= optionsMenuRetractedPos.x + 0.01)
            {
                Lerp(optionsMenuRetractedPos.x);
                yield return null;
            }
        }

        void Lerp(float pos)
        {
            Vector3 position = optionsMenu.position;
            float newX = Mathf.Lerp(position.x, pos, optionsDeploySpeed * Time.deltaTime);
            position = new Vector3(newX, position.y, position.z);
            optionsMenu.position = position;
        }
    }

    public void SetVolume(float volume)
    {
        audioMixer.SetFloat("Volume", volume);
    }

    public void SetResolution()
    {
        currentResolutionIndex = resolutionDropdown.value;
        PlayerPrefs.SetInt("Resolution", currentResolutionIndex);
        
        UpdateDisplay();
    }

    public void SetFullscreen()
    {
        if (fullScreenToggle.isOn)
        {
            fullscreen = FullScreenMode.ExclusiveFullScreen;
            PlayerPrefs.SetInt("Fullscreen", 1);
        }
        else
        {
            fullscreen = FullScreenMode.Windowed;
            PlayerPrefs.SetInt("Fullscreen", 0);
        }
        
        UpdateDisplay();
    }

    private void UpdateDisplay()
    {
        Resolution res = resolutions[currentResolutionIndex];
        Screen.SetResolution(res.width, res.height, fullscreen, res.refreshRateRatio);
    }

    private void ReadPrefs()
    {
        resolutions = Screen.resolutions;
        currentRefreshRate = Screen.currentResolution.refreshRateRatio.value;

        if (PlayerPrefs.HasKey("Fullscreen"))
        {
            if (PlayerPrefs.GetInt("Fullscreen") >= 1)
            {
                fullscreen = FullScreenMode.ExclusiveFullScreen;
            }
            else
            {
                fullscreen = FullScreenMode.Windowed;
            }
        }
        else
        {
            PlayerPrefs.SetInt("Fullscreen", 1);
            fullscreen = Screen.fullScreenMode;
        }

        if (PlayerPrefs.HasKey("Resolution")) currentResolutionIndex = PlayerPrefs.GetInt("Resolution");
        else currentResolutionIndex = Screen.resolutions.Length;
    }

    [Button]
    private void ClearPrefs()
    {
    }
}