using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseManager : MonoBehaviour
{
    [SerializeField]
    private GameObject pausePopup; // 일시정지 팝업 UI

    private bool isPaused = false;

    void Update()
    {
        // ESC 키 입력 감지
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
        Time.timeScale = 0; // 게임 일시정지
        if (pausePopup != null)
        {
            pausePopup.SetActive(true); // 팝업 활성화
        }
    }

    public void ResumeGame()
    {
        isPaused = false;
        Time.timeScale = 1; // 게임 재개
        if (pausePopup != null)
        {
            pausePopup.SetActive(false); // 팝업 비활성화
        }
    }

    public void RestartGame()
    {
        Time.timeScale = 1; // 게임 시간 복원
        SceneManager.LoadScene(SceneManager.GetActiveScene().name); // 현재 씬 재시작
    }

    public void GoToMainMenu()
    {
        Time.timeScale = 1; // 게임 시간 복원
        SceneManager.LoadScene("MainScene"); // 메인 메뉴 씬으로 이동
    }
}
