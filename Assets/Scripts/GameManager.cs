using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    [SerializeField] DifficultySettings[] difficulties = new DifficultySettings[3];
    DifficultySettings difficulty;

    [Header("Prefabs and such")]

    [SerializeField] Arrow arrowPrefab;
    [SerializeField] Arrow bombPrefab;

    [SerializeField] private SpriteRenderer[] boxArrows = new SpriteRenderer[4];

    [SerializeField] ParticleSystem highScoreParticleSystem;

    [Space]

    [SerializeField] Transform spawnLocation;

    [Header("UI")]
    [SerializeField] GameObject gameOverPanel;
    [SerializeField] GameObject pausePanel;
    [SerializeField] Text scoreText;
    [SerializeField] Text newHighScoreText;
    [SerializeField] Text highScoreText;
    [SerializeField] Text highestStreakText;
    [SerializeField] Text hardcoreModeText;

    [SerializeField] Text inGameScoreText;
    [SerializeField] Text inGameStreakText;

    public int beatsPerMinute { get; private set; } = 120;

    int totalBeatsPerSong => difficulty.totalBeatsForGame;

    private float spawnEveryAmountOfSeconds => 1f / (beatsPerMinute / 60f);
    
    private const float constSpeedMultiplier = 2.5f;
    public float gameSpeed => beatsPerMinute / 120f;
    private float arrowSpeed => constSpeedMultiplier * difficulty.speedMultiplier;

    private float spawnTimer = 0f;

    private const float arrowOffset = 0.5f;
    private const float arrowSpacing = 1f;

    public bool isPaused { get; private set; } = true;

    int lastLevel = -1;
    public int currentLevel => streak / difficulty.levelUpAfterLevels;
    private int hue = -1;
    public int currentBeat { get; private set; } = 0;
    private int streak = 0;
    private int highestStreak = 0;

    private int spaceCounter = 0;

    [Space]

    [SerializeField] float bombShakeIntensity = 0.5f;

    [SerializeField] int showStreakAfter = 5;

    int levelUpCounter = 0;

    private List<int> previousHues = new List<int>();

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        pausePanel.SetActive(false);

        difficulty = difficulties[Data.difficulty - 1];

        StartGame();
    }

    private void Update()
    {
        if(spawnTimer <= 0f && !isPaused && currentBeat < totalBeatsPerSong)
        {
            currentBeat++;

            spawnTimer += spawnEveryAmountOfSeconds;

            if(spaceCounter <= 0)
            {
                spaceCounter = GetSpaceCount();

                int spawnAmount = GetSpawnAmount();

                int spawned = 0;

                const int maxArrows = 4;

                bool upArrow = false;

                for (int i = 0; i < maxArrows; i++)
                {
                    if (i == 2 && upArrow) continue;

                    if (Random.Range(0, maxArrows - i) == 0)
                    {
                        if (i == 0) upArrow = true;

                        SpawnArrow(i, currentLevel > difficulty.spawnBombsAfterLevel ? (Random.value <= difficulty.spawnBombChance) : false);
                        spawned++;
                    }

                    if (spawned >= spawnAmount)
                    {
                        break;
                    }
                }
            } else
            {
                spaceCounter--;
            }
        }

        if(spawnTimer > 0f)
            spawnTimer -= Time.deltaTime;

        CheckLevel();

        if(currentBeat >= totalBeatsPerSong && spawnLocation.childCount == 0)
        {
            GameOver();
        }

        if (Input.GetKeyDown(KeyCode.P) && !isPaused)
        {
            PauseGame();
        }

        if (Input.GetKeyUp(KeyCode.Backspace))
            SceneManager.LoadScene(0);
    }

    bool gameIsPaused = false;

    public void PauseGame()
    {
        gameIsPaused = !gameIsPaused;
        if (gameIsPaused)
        {
            pausePanel.SetActive(true);
            Time.timeScale = 0f;
        }
        else
        {
            pausePanel.SetActive(false);
            Time.timeScale = 1;
        }
    }

    private int GetSpawnAmount()
    {
        //if the level is over the spawn level, and if the percent to spawn one is within the range, return 2
        if(currentLevel > difficulty.spawnAddArrowsAfterLevel && Random.value <= Mathf.Clamp(difficulty.minAddArrowsChance + difficulty.addArrowsIncrementPerLevel * (currentLevel - difficulty.spawnAddArrowsAfterLevel), difficulty.minAddArrowsChance, difficulty.maxAddArrowsChance))
        {
            return 2;
        } else
        {
            return 1;
        }
    }

    private int GetSpaceCount()
    {
        if(currentLevel >= difficulty.oneSpaceUntilLevel)
        {
            return 0;
        } else if (currentLevel >= difficulty.twoSpacesUntilLevel)
        {
            return 1;
        } else
        {
            return 2;
        }
    }

    private void SetColorForAllArrows()
    {
        for (int i = 0; i < spawnLocation.childCount; i++)
        {
            Transform child = spawnLocation.GetChild(i);
            child.GetComponent<Arrow>().SetColor(GetArrowColor());
        }

        foreach(SpriteRenderer sr in boxArrows)
        {
            sr.color = GetArrowColor();
        }
    }

    private Color GetArrowColor()
    {
        if (hue == -1) return Color.white;

        return Color.HSVToRGB(hue / 360f, 1f, 1f);
    }

    private void SpawnArrow(int id, bool bomb)
    {
        Arrow prefab;

        if (bomb)
            prefab = bombPrefab;
        else
            prefab = arrowPrefab;

        Arrow arrow = Instantiate(prefab, spawnLocation.position + Vector3.right * arrowOffset + Vector3.right * arrowSpacing * id, Quaternion.Euler(0f, 0f, 90f * id), spawnLocation);

        arrow.rotationID = id;
        arrow.SetSpeed(arrowSpeed);
        arrow.isBomb = bomb;
        arrow.SetColor(GetArrowColor());
        arrow.ScaleHitboxSize(difficulty.arrowHitboxSize);
    }

    private void ChangeHue()
    {
        if(currentLevel >= previousHues.Count)
        {
            previousHues.Add(hue);

            hue = (hue + Random.Range(10, 350)) % 360;
        } else
        {
            hue = previousHues[currentLevel];
        }
    }

    public void Bad()
    {
        if (Data.hardcoreMode)
        {
            streak = 0;
        }
        else
        {
            if (levelUpCounter <= 0)
            {
                streak -= difficulty.levelUpAfterLevels;
            }
            else
            {
                streak -= levelUpCounter + difficulty.levelUpAfterLevels;
            }
        }

        streak = Mathf.Max(0, streak);
        levelUpCounter = 0;

        //CheckLevel();

        SetInGameTexts();

        CameraShaker.instance.Shake(bombShakeIntensity);
    }

    private void CheckLevel()
    {
        if (currentLevel != lastLevel)
        {
            if (currentLevel > lastLevel)
            {
                levelUpCounter = 0;
            }
            ChangeHue();
            SetColorForAllArrows();
            lastLevel = currentLevel;
        }
    }

    public void Good()
    {
        levelUpCounter++;
        streak++;

        //CheckLevel();

        Data.score += streak;

        if(streak > highestStreak)
        {
            highestStreak = streak;
        }

        SetInGameTexts();
    }

    private void SetInGameTexts()
    {
        inGameScoreText.gameObject.SetActive(true);
        inGameScoreText.text = "Score: " + Data.score;

        if (streak > showStreakAfter)
        {
            inGameStreakText.gameObject.SetActive(true);
            inGameStreakText.text = streak.ToString();
        }
        else
        {
            inGameStreakText.gameObject.SetActive(false);
        }
    }

    public void StartGame()
    {
        if (!isPaused) return;

        isPaused = false;

        previousHues.Clear();

        MusicPlayer.instance.Restart();

        currentBeat = 0;
        streak = 0;
        highestStreak = 0;
        hue = -1;
        lastLevel = 0;
        levelUpCounter = 0;
        spaceCounter = GetSpaceCount();

        SetInGameTexts();

        gameOverPanel.SetActive(false);
    }

    private void GameOver()
    {
        if (isPaused) return;

        isPaused = true;

        for(int i = 0; i < spawnLocation.childCount; i++)
        {
            Transform child = spawnLocation.GetChild(i);
            child.GetComponent<Animator>().SetTrigger("Shrink");

            Destroy(child.gameObject, 1f);
        }

        inGameScoreText.gameObject.SetActive(false);
        inGameStreakText.gameObject.SetActive(false);

        gameOverPanel.SetActive(true);

        scoreText.text = "Score: " + Data.score;
        highestStreakText.text = "Highest Streak: " + highestStreak;

        hardcoreModeText.gameObject.SetActive(Data.hardcoreMode);

        if (Data.score > Data.highScore)
        {
            Data.highScore = Data.score;
            newHighScoreText.gameObject.SetActive(true);
            highScoreParticleSystem.Play();
        }
        else
        {
            newHighScoreText.gameObject.SetActive(false);
        }

        highScoreText.gameObject.SetActive(true);
        highScoreText.text = difficulty.difficultyName + " High Score: " + Data.highScore;

        Data.score = 0;
        highestStreak = 0;
    }

    public void GoToMainMenu()
    {
        Time.timeScale = 1f;

        SceneManager.LoadScene(0);//main menu scene
    }

    public void Retry()
    {
        StartGame();
    }
}
