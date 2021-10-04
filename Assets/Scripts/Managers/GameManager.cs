using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : SingletonBehaviour<GameManager>
{
    public string TitleScene;
    public string MainGame;

    //public bool SkipTitle;
    public bool NeverLose;
    public bool noAutoNavigate;

    public SoundEffects sfx;

    public bool IsPlayerControllerEnabled;

    public delegate void ResetAction();
    public static event ResetAction OnReset;

    public bool isDebugMode;

    public int HorsesCreated;
    public int HorsesAlive;

    private bool isPlaying;
    private float startTime;

    private HashSet<string> completedLevels;

    public GameManager()
    {
        completedLevels = new HashSet<string>();
        completedLevels.Add("Level1");
    }
    
    public void UnlockCurrentLevel()
    {
        completedLevels.Add(SceneManager.GetActiveScene().name);
    }

    public void UnlockAllLevels()
    {
        for (var i = 0; i < 20; i++)
        {
            var levelName = $"Level{i + 1}";
            completedLevels.Add(levelName);
        }
    }
    
    protected override void Awake()
    {
        base.Awake();
        sfx = gameObject.AddComponent<SoundEffects>();
    }

    public void ShowTitleScreen()
    {
        StopAllCoroutines();
        SceneManager.LoadScene("TitleScene");
    }
    
    void Start()
    {
        DontDestroyOnLoad(this.gameObject);
        
        this.isPlaying = false;
        this.isDebugMode = Application.isEditor;

        SceneManager.sceneLoaded += OnSceneLoaded;

        if (!this.noAutoNavigate)
        {
            SceneManager.LoadScene("TitleScene");
        }
    }

    public void ReloadLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void StartGame()
    {
        this.IsPlayerControllerEnabled = true;
        BGMPlayer.Instance.PlayBGM();
    }

    public void StartLevel(string levelName)
    {
        ResetGameState();
        SceneManager.LoadScene(levelName);
    }

    public void WinGame()
    {
        this.isPlaying = false;
        this.IsPlayerControllerEnabled = false;
        
        BGMPlayer.Instance.StopBGM();
    }

    public void ReturnToTitle()
    {
        this.IsPlayerControllerEnabled = false;
        SceneManager.LoadScene(this.TitleScene);
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    private void ResetGameState()
    {
        this.NeverLose = false;

        this.startTime = Time.deltaTime;
        this.isPlaying = true;
        this.IsPlayerControllerEnabled = true;

        this.sfx.StopAudioLoop(false);
    }

    public void OnSceneLoaded(Scene scene, LoadSceneMode loadSceneMode)
    {
        if (scene.name == "Main")
        {
            this.ResetGameState();
            this.StartGame();
        }
    }

    public void KillRespawnPlayer()
    {
        if (!NeverLose)
        {
            IsPlayerControllerEnabled = false;

            StopAllCoroutines();
            StartCoroutine(RespawnPlayer());
        }
    }

    public void WinLevel()
    {
        // TODO: some kind of congratulations
        
        // Find the next level and load it
        GameObject settingsObj = GameObject.FindGameObjectWithTag("LevelSettings");
        LevelSettings settings = settingsObj?.GetComponent<LevelSettings>();

        if (settings == null) return;
        
        StartLevel(settings.nextLevel);
    }

    public bool IsLevelComplete(string levelName)
    {
        return completedLevels.Contains(levelName);
    }

    private IEnumerator RespawnPlayer()
    {
        OnReset();
        
        yield return new WaitForSeconds(3);
        yield return null;

        IsPlayerControllerEnabled = true; 
    }
}
