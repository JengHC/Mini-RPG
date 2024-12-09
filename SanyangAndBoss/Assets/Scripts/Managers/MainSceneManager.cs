using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;


public class MainSceneManager : MonoBehaviour
{
    public GameObject DescriptionPanel;

    public void OnStartButton()
    {
        SceneManager.LoadScene("GameScene");
    }

    public void OnDescription()
    {
        DescriptionPanel.SetActive(true);
    }

    public void OffDescriptionButton()
    {
        DescriptionPanel.SetActive(false);
        Debug.Log("Transparent Button Clicked");
    }

    public void OnQuitButton()
    {
        Application.Quit();
    }
}
