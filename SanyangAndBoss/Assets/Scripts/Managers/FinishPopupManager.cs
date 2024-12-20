using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FinishPopupManager : MonoBehaviour
{
   [SerializeField]
    private GameObject finishPopup; // °ÔÀÓ ³¡ ÆË¾÷

    public void GameFinishPopup()
    {
        if (finishPopup != null)
        {
            finishPopup.SetActive(true); // ÆË¾÷ È°¼ºÈ­
        }
    }

    public void OnMainMenuButton()
    {
        SceneManager.LoadScene("MainScene");
    }

    public void OnRestartButton()
    {
        SceneManager.LoadScene("GameScene");
    }

    public void OnQuitButton()
    {
        Application.Quit();
    }
}
