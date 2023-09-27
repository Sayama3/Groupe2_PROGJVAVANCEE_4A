using System.Collections;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public enum PlayerType { None, Human, Random, MCTS }

public class MainMenuManager : MonoBehaviour
{
    [SerializeField] private TMP_Dropdown[] playerDropdowns;
    
    [SerializeField] private AudioMixer audioMixer;

    [SerializeField] private TMP_Dropdown resolutionDropdown;
    private Resolution[] resolutions;
    private int currentResolutionIndex = 0;

    [SerializeField] private Toggle fullScreenToggle;
    private bool isFullscreen;

    [SerializeField] private RectTransform optionsMenu;
    [SerializeField] private RectTransform optionsMenuDeployed;
    private Vector3 optionsMenuRetractedPos;
    private bool isOptionsMenuDeployed = false;

    [SerializeField] private float optionsDeploySpeed = 0.5f;

    private Coroutine lerpCorout = null;
    
    private void Start()
    {
        resolutionDropdown.ClearOptions();
        resolutions = Screen.resolutions;

        optionsMenuRetractedPos = optionsMenu.position;

        ReadPrefs();
    }

    public void StartGame()
    {
        SceneManager.LoadScene("SceneIannis");
    }

    public void ChangedPlayer()
    {
        for (int i = 0; i < playerDropdowns.Length; i++)
        {
            MenuToGame.Instance.playerTypes[i] = (PlayerType)playerDropdowns[i].value;
        }
    }

    public void ToggleOptions()
    {
        if(lerpCorout != null) StopCoroutine(lerpCorout); // This is bugged
        
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
                Vector3 position = optionsMenu.position;
                float newX = Mathf.Lerp(position.x, optionsMenuDeployed.position.x,
                    optionsDeploySpeed * Time.deltaTime);
                position = new Vector3(newX, position.y, position.z);
                optionsMenu.transform.position = position;
                yield return null;
            }
        }
        else
        {
            while (optionsMenu.position.x >= optionsMenuRetractedPos.x + 0.01)
            {
                Vector3 position = optionsMenu.transform.position;
                float newX = Mathf.Lerp(position.x, optionsMenuRetractedPos.x,
                    optionsDeploySpeed * Time.deltaTime);
                position = new Vector3(newX, position.y, position.z);
                optionsMenu.transform.position = position;
                yield return null;
            }
        }
    }

    public void SetVolume(float volume)
    {
        audioMixer.SetFloat("Volume", volume);
    }

    public void SetFullscreen(bool isFullscreen)
    {
        Screen.fullScreen = isFullscreen;
    }

    private void ReadPrefs()
    {
        if (PlayerPrefs.HasKey("Fullscreen"))
        {
            isFullscreen = (PlayerPrefs.GetInt("Fullscreen") >= 1);
        }
        else isFullscreen = Screen.fullScreen;

        if (PlayerPrefs.HasKey("Resolution"))
        {
            resolutionDropdown.value = PlayerPrefs.GetInt("Resolution");

            Resolution readResolution = resolutions[resolutionDropdown.value];
            Screen.SetResolution(readResolution.width, readResolution.height, isFullscreen);
        }
        else resolutionDropdown.value = currentResolutionIndex;
    }

    [Button]
    private void ClearPrefs()
    {
    }
}