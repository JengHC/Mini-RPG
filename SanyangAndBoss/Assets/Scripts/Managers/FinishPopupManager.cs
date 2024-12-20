using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FinishPopupManager : MonoBehaviour
{
   [SerializeField]
    private GameObject finishPopup; // ���� �� �˾�

    public void GameFinishPopup()
    {
        if (finishPopup != null)
        {
            finishPopup.SetActive(true); // �˾� Ȱ��ȭ
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
