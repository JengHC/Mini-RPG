using UnityEngine;
using UnityEngine.SceneManagement;

public class PopupManager : MonoBehaviour
{
    [SerializeField]
    private GameObject gameOverPopup; // 게임오버 팝업

    public void ShowGameOverPopup()
    {
        if (gameOverPopup != null)
        {
            gameOverPopup.SetActive(true); // 팝업 활성화
        }
    }

    public void OnMainMenuButton()
    {
        SceneManager.LoadScene("MainMenu");
    }

    public void OnRestartButton()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void OnQuitButton()
    {
        Application.Quit();
    }
}
