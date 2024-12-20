using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseManager : MonoBehaviour
{
    [SerializeField]
    private GameObject pausePopup; // �Ͻ����� �˾� UI

    private bool isPaused = false;

    void Update()
    {
        // ESC Ű �Է� ����
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isPaused)
            {
                ResumeGame();
            }
            else
            {
                PauseGame();
            }
        }
    }

    public void PauseGame()
    {
        isPaused = true;
        Time.timeScale = 0; // ���� �Ͻ�����
        if (pausePopup != null)
        {
            pausePopup.SetActive(true); // �˾� Ȱ��ȭ
        }
    }

    public void ResumeGame()
    {
        isPaused = false;
        Time.timeScale = 1; // ���� �簳
        if (pausePopup != null)
        {
            pausePopup.SetActive(false); // �˾� ��Ȱ��ȭ
        }
    }

    public void RestartGame()
    {
        Time.timeScale = 1; // ���� �ð� ����
        SceneManager.LoadScene(SceneManager.GetActiveScene().name); // ���� �� �����
    }

    public void GoToMainMenu()
    {
        Time.timeScale = 1; // ���� �ð� ����
        SceneManager.LoadScene("MainScene"); // ���� �޴� ������ �̵�
    }
}
