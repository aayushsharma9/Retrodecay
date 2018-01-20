using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static int EnergyLevel;
    public static int Score, HighScore, currentLevel = 1;
    private float multiplier;

    private float t, d, l;
    [SerializeField]
    private float DecayInterval;
    [SerializeField]
    private Text EnergyLevelText, LevelInfo, scoreText, HighScoreText, finalScoreText, countdownText;
    [SerializeField]
    private GameObject spawner1, spawner2, spawnerSet;
    [SerializeField]
    private GameObject pausePanel, GameOverPanel, CountDownPanel;
    [SerializeField]
    private AnimationClip pausePanelExitAnim, GOPanelExitAnim;
    private bool isPaused, gameOver, hasStarted;


    private void Start()
    {
        Application.targetFrameRate = 60;
        GameInit();
        StartCoroutine(StartWithCountdown());
    }

    private void GameInit()
    {
        Time.timeScale = 1;
        isPaused = false;
        gameOver = false;
        hasStarted = false;
        GameOverPanel.SetActive(false);
        pausePanel.SetActive(false);
        spawner1.SetActive(true);
        spawner2.SetActive(false);

        t = 0;
        d = 0;
        l = 0;

        gameObject.GetComponent<MouseControl>().enabled = true;
        clearPickups();

        EnergyLevel = 100;
        DecayInterval = 0.5f;
        Score = 0;
        multiplier = 1;
        currentLevel = 1; // TEMPORARY FIX
        LevelInfo.text = "LEVEL 1"; 
        HighScore = PlayerPrefs.GetInt("HighScore");

        scoreText.text = "" + Score;

        InitializeLevel(1);

        if (currentLevel == 2)
        {
            PickSpawner.IncreasePickSpeedBy(0.9f);
            InitializeLevel(2);
        }

        if (currentLevel == 3)
        {
            PickSpawner.IncreasePickSpeedBy(1.5f);
            InitializeLevelOnStart(3);
        }
    }

    private void Update()
    {
        t += Time.deltaTime;
        d += Time.deltaTime;
        l += Time.deltaTime;

        scoreText.text = "" + Score;

        if (t >= DecayInterval)
        {
            EnergyLevel--;

            if (EnergyLevel < 0)
            {
                EnergyLevel = 0;
            }
            t = 0;
        }

        EnergyLevelText.text = "" + EnergyLevel;

        if (EnergyLevel <= 0) //GAME OVER
        {
            gameOver = true;
            Debug.Log("GAME OVER");
            Time.timeScale = 0;
            GameOverPanel.SetActive(true);
            finalScoreText.text = scoreText.text;
            HighScoreText.text = "" + HighScore;
        }

        if (EnergyLevel > 100)
        {
            EnergyLevel = 100;
        }

        if (Input.GetKeyDown (KeyCode.Escape) && hasStarted)
        {
            Pause();
        }

        if (d >= 10)
        {
            PickSpawner.IncreasePickSpeedBy(0.2f);
            //DecayInterval /= 1.1f;
            Debug.Log("Decay Interval decreased to " + DecayInterval);
            d = 0;
        }
        
        if (Score == 25 && currentLevel < 2) //LEVEL 2
        {
            currentLevel++;
            InitializeLevel(2);
            l = 0;
        }

        if (Score == 50 && currentLevel < 3) //LEVEL 3
        {
            currentLevel++;
            InitializeLevel(3);
            l = 0;
        }

        if (currentLevel == 3)
        {
            spawnerSet.transform.Rotate(new Vector3(0, 1, 0) * Time.deltaTime * 20);
        }
    }

    public static void AddEnergy(int e)
    {
        EnergyLevel += e;
    }

    void clearPickups()
    {
        GameObject[] energyPicks = GameObject.FindGameObjectsWithTag("Energy");
        GameObject[] negativePicks = GameObject.FindGameObjectsWithTag("Negative");

        foreach (GameObject pick in energyPicks)
            Destroy(pick);
        foreach (GameObject pick in negativePicks)
            Destroy(pick);
    }

    private void InitializeLevel(int _level)
    {
        if (_level == 1)
        {
            PickSpawner.ResetPickSpeed();
            spawnerSet.GetComponent<Animator>().Play("SourceSpawn", -1, 0);
            spawnerSet.GetComponent<Transform>().rotation = Quaternion.Euler(new Vector3(0, 0, 0));
            spawner1.GetComponent<Transform>().position = new Vector3(0, 0, 0);
            spawner2.GetComponent<Transform>().position = new Vector3(0, 0, 0);
        }

        if (_level == 2)
        {
            LevelInfo.text = "LEVEL 2";
            DecayInterval /= 1.2f;
            PickSpawner.partRotationSpeed = 300;
            spawner2.SetActive(true);
            LevelInfo.GetComponent<Animator>().enabled = true;
            spawnerSet.GetComponent<Animator>().enabled = true;
            LevelInfo.GetComponent<Animator>().Play("LevelChange");
            spawnerSet.GetComponent<Animator>().Play("SourceSplit", -1, 0);
            spawnerSet.GetComponent<Animator>().StopPlayback();
        }

        else if (_level == 3)
        {
            LevelInfo.text = "LEVEL 3";
            DecayInterval /= 1.2f;
            LevelInfo.GetComponent<Animator>().Play("LevelChange");
        }
    }

    private void InitializeLevelOnStart(int _level)
    {
        for (int i = 2; i <= _level; i++)
        {
            InitializeLevel(i);
        }
    }
     
    // UI FUNCTIONS BELOW

    public void Pause()
    {
        isPaused = !isPaused;
        gameObject.GetComponent<MouseControl>().enabled = !gameObject.GetComponent<MouseControl>().enabled;

        if (isPaused)
        {
            Time.timeScale = 0;
            pausePanel.SetActive(true);
        }
        else
        {
            StartCoroutine(ResumeAfterDelay());
        }
    }

    public void ResetGame(string _currentState)
    {
        StartCoroutine(LoadAfterDelay(_currentState, 1));
    }

    public void ReturnToMenu(string _currentState)
    {
        StartCoroutine(LoadAfterDelay(_currentState, 0));
    }

    IEnumerator ResumeAfterDelay()
    {
        pausePanel.GetComponent<Animator>().Play("PausePanelExit");
        yield return new WaitForSecondsRealtime(pausePanelExitAnim.length);
        Time.timeScale = 1;
        pausePanel.SetActive(false);
    }

    IEnumerator LoadAfterDelay(string _state, int _sceneIndex)
    {
        if (_state == "Paused")
        {
            pausePanel.GetComponent<Animator>().Play("PausePanelExit");
            yield return new WaitForSecondsRealtime(pausePanelExitAnim.length/3*2);
        }

        else if (_state == "GameOver")
        {
            GameOverPanel.GetComponent<Animator>().Play("GameOverPanelExit");
            yield return new WaitForSecondsRealtime(GOPanelExitAnim.length);
        }

        currentLevel = 1;
        SceneManager.LoadScene(_sceneIndex);
    }

    IEnumerator StartWithCountdown()
    {
        Time.timeScale = 0;
        CountDownPanel.SetActive(true);

        for (int i = 1; i <= 3; i++)
        {
            yield return new WaitForSecondsRealtime(0.6f);
            countdownText.text = "" + (3-i);
        }

        countdownText.text = "";
        CountDownPanel.SetActive(false);
        hasStarted = true;
        Time.timeScale = 1;
    }
}
