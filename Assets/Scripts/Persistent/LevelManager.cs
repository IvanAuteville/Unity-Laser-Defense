using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    public static LevelManager instance = null;

    [SerializeField] private float delayInSeconds = 2f;
    private MusicPlayer musicPlayer = null;
    private GameSession gameSession = null;
    private bool startGame = false;

    private void Awake()
    {
        SetUpSingleton();
    }

    private void SetUpSingleton()
    {
        if(instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }

    private void Start()
    {
        musicPlayer = FindObjectOfType<MusicPlayer>();
        gameSession = FindObjectOfType<GameSession>();


        PreLoadGame(0);
    }

    public void LoadStartMenu()
    {
        StartCoroutine(StartMenuRoutine());
    }

    public void LoadGameOver()
    {
        StartCoroutine(GameOverRoutine());
    }

    public void PreLoadGame(int previousSceneIndex)
    {
       StartCoroutine(LoadGameInBackground(previousSceneIndex));
    }

    private IEnumerator StartMenuRoutine()
    {
        // Load Main Menu Scene and Unload everything else
        SceneManager.LoadScene(0);

        yield return null;

        musicPlayer.PlayStartMenu();

        PreLoadGame(0);
    }

    private IEnumerator GameOverRoutine()
    {
        yield return new WaitForSeconds(delayInSeconds);

        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(2);
        while (!asyncLoad.isDone)
        {
            yield return null;
        }

        musicPlayer.PlayGameOver();

        PreLoadGame(2);
    }

    IEnumerator LoadGameInBackground(int previousSceneIndex)
    {
        // Reset Static Vars
        PrefabPoolingSystem.Reset();
        gameSession.ResetGame();
        //-------------------------

        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(1, LoadSceneMode.Additive);
        asyncLoad.allowSceneActivation = false;

        // Wait until the scene fully loads
        // (Stops at 0.9 due to allowSceneActivation = false)
        while (asyncLoad.progress < 0.9f)
        {
            yield return null;
        }

        // Wait for Input
        // (the player press "Play")
        while (!startGame)
        {
            yield return null;
        }

        // Wait a Frame until the Scene is Active
        yield return (asyncLoad.allowSceneActivation = true);

        // Optional
        //SceneManager.SetActiveScene(SceneManager.GetSceneByBuildIndex(1));

        // Wait a Frame until the previous Scene is Unloaded
        Scene previousScene = SceneManager.GetSceneByBuildIndex(previousSceneIndex);

        if (previousScene.isLoaded)
        {
            yield return SceneManager.UnloadSceneAsync(previousSceneIndex);
        }

        musicPlayer.PlayGame();
        startGame = false;
    }

    public void StartGame()
    {
        startGame = true;
    }

    public void Quit()
    {
        Application.Quit();
    }
}