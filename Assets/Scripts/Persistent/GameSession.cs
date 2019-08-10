using UnityEngine;

public class GameSession : MonoBehaviour
{
    public static GameSession instance = null;

    [Header("Scores")]
    [SerializeField] private int lvl1Upgrade = 800;
    [SerializeField] private int lvl2Upgrade = 1500;

    [Header("Sounds")]
    [SerializeField] private AudioClip lvlUpVoice = null;
    [SerializeField] private float lvlUpVoiceVolume = 0.5f;
    [SerializeField] private AudioClip lvlUpSound = null;
    [SerializeField] private float lvlUpSoundVolume = 0.5f;

    private int lvl = 0;
    private int score = 0;

    private ScoreDisplay scoreDisplay = null;
    private Player player = null;
    private Vector3 mainCameraPos;

    private void Awake()
    {
        SetUpSingleton();

        mainCameraPos = Camera.main.transform.localPosition;
    }

    private void SetUpSingleton()
    {
        if (instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }

    private void Update()
    {
        if (lvl == 0 && score >= lvl1Upgrade)
        {
            LevelUp();
        }
        else if (lvl == 1 && score >= lvl2Upgrade)
        {
            LevelUp();
        }
    }

    public int GetScore()
    {
        return score;
    }

    public void AddToScore(int points)
    {
        score += points;
        scoreDisplay.SetScore(score.ToString());
    }

    private void LevelUp()
    {
        if(player)
        {
            player.LevelUp();
            lvl++;

            AudioSource.PlayClipAtPoint(lvlUpSound, mainCameraPos, lvlUpSoundVolume);
            AudioSource.PlayClipAtPoint(lvlUpVoice, mainCameraPos, lvlUpVoiceVolume);
        }
    }

    public void ResetGame()
    {
        score = 0;
        lvl = 0;
    }

    public void SetPlayer(Player player)
    {
        this.player = player;
    }

    public void SetScoreDisplay(ScoreDisplay scoreDisplay)
    {
        this.scoreDisplay = scoreDisplay;
    }
}