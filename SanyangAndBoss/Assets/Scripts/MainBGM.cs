using UnityEngine;

public class MainBGM : MonoBehaviour
{
    public AudioClip lobbyBGM;
    private AudioSource audioSource;

    void Awake()
    {
        // �� ������Ʈ�� �� ��ȯ �� �ı����� �ʵ��� ����
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
