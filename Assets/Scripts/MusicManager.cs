using UnityEngine;

public class MusicManager : MonoBehaviour
{

    public static AudioSource audioSource;

    private static AudioClip menuMusic;
    private static AudioClip levelMusic;

    void Awake()
    {
        DontDestroyOnLoad(this.gameObject);
        audioSource = GetComponent<AudioSource>();
        audioSource.mute = GameManager.mute;
        menuMusic = Resources.Load<AudioClip>("Audio/Musics/Snap");
        levelMusic = Resources.Load<AudioClip>("Audio/Musics/Outcast");
        playMenuMusic();
    }

    public static void soundClicked()
    {
        audioSource.mute = GameManager.mute;
    }

    public static void playMenuMusic()
    {
        audioSource.clip = menuMusic;
        audioSource.volume = 0.4f;
        audioSource.Play();
    }

    public static void playLevelMusic()
    {
        audioSource.clip = levelMusic;
        audioSource.volume = 1.0f;
        audioSource.Play();
    }
}
