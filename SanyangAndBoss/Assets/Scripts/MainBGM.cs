using UnityEngine;

public class MainBGM : MonoBehaviour
{
    public AudioClip lobbyBGM;
    private AudioSource audioSource;

    void Awake()
    {
        // 이 오브젝트가 씬 전환 시 파괴되지 않도록 설정
        DontDestroyOnLoad(gameObject);

        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.clip = lobbyBGM;
        audioSource.loop = true;
        audioSource.playOnAwake = true;
        audioSource.Play();
    }
    void Start()
    {
        MainBGM[] bgmPlayers = FindObjectsOfType<MainBGM>();
        if (bgmPlayers.Length > 1)
        {
            Destroy(gameObject);
        }
    }
}
