using TMPro;
using UnityEngine;

public class EndGame : MonoBehaviour
{
    [SerializeField] private GameObject EndGameObject = null;
    [SerializeField] private TextMeshProUGUI EndGameText = null;
    [SerializeField] private int indexMenuScene;
    private string endGameText = "";
    private void Start()
    {
        GameManager.Instance.OnGameEnd += GameEnded;
        GameManager.Instance.OnGameTie += GameTie;
        GameManager.Instance.OnPlayerWin += PlayerWin;
    }

    private void PlayerWin(IPlayerController arg1, int arg2)
    {
        endGameText = $"Player {arg2+1} won !";
    }

    private void GameTie()
    {
        endGameText = "The game is tie !";
    }

    private void OnDestroy()
    {
        GameManager.Instance.OnGameEnd -= GameEnded;
        GameManager.Instance.OnGameTie -= GameTie;
        GameManager.Instance.OnPlayerWin -= PlayerWin;
    }

    private void GameEnded()
    {
        EndGameObject.SetActive(true);
        if (EndGameText != null)
        {
            EndGameText.text = endGameText;
        }
    }

    public void ReturnToMenu()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(indexMenuScene);
    }
}
