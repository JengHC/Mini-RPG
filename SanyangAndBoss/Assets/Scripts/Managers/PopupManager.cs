using UnityEngine;
using UnityEngine.SceneManagement;

public class PopupManager : MonoBehaviour
{
    [SerializeField]
    private GameObject gameOverPopup; // ���ӿ��� �˾�

    public void ShowGameOverPopup()
    {
        if (gameOverPopup != null)
        {
            gameOverPopup.SetActive(true); // �˾� Ȱ��ȭ
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
