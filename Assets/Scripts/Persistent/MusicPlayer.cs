using UnityEngine;

public class MusicPlayer : MonoBehaviour
{
    [Header("Audioclips")]
    [SerializeField] private AudioClip startMenu = null;
    [SerializeField] private float startMenuVolume = 0.5f;

    [SerializeField] private AudioClip game = null;
    [SerializeField] private float gameVolume = 0.5f;

    [SerializeField] private AudioClip gameOverMenu = null;
    [SerializeField] private float gameOverMenuVolume = 0.5f;

    private AudioSource audioSource = null;
    private void Awake()
    {
        SetUpSingleton();
    }

    private void SetUpSingleton()
    {
        if (FindObjectsOfType(GetType()).Length > 1)
        {
            Destroy(gameObject);
        }
        else
        {
            DontDestroyOnLoad(gameObject);
            audioSource = GetComponent<AudioSource>();
        }
    }

    internal void PlayStartMenu()
    {
        audioSource.Stop();
        audioSource.clip = startMenu;
        audioSource.volume = startMenuVolume;
        audioSource.Play();
    }

    internal void PlayGame()
    {
        audioSource.Stop();
        audioSource.clip = game;
        audioSource.volume = gameVolume;
        audioSource.Play();
    }

    internal void PlayGameOver()
    {
        audioSource.Stop();
        audioSource.clip = gameOverMenu;
        audioSource.volume = gameOverMenuVolume;
        audioSource.Play();
    }
}