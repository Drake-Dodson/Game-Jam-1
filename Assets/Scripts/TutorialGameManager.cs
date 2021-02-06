using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class TutorialGameManager : MonoBehaviour
{
    public static TutorialGameManager instance;

    [SerializeField] AudioSource musicSource;

    [Header("Prefabs and such")]

    [SerializeField] Arrow arrowPrefab;
    [SerializeField] Arrow bombPrefab;

    [Space]

    [SerializeField] Transform spawnLocation;

    [Header("UI")]
    [SerializeField] GameObject gameOverPanel;
    //[SerializeField] Text scoreText;
    //[SerializeField] Text newHighScoreText;
    //[SerializeField] Text highScoreText;
    [SerializeField] GameObject arrowHelpText;
    [SerializeField] GameObject bombText;
    

    [Space]
    [Header("Customization")]

    [SerializeField] float oddsOfGeneratingABomb = 0.02f;

    [Space]

    [SerializeField] int easyStartingBPM = 120;
    [SerializeField] int mediumStartingBPM = 140;
    [SerializeField] int hardStartingBPM = 160;
    int beatsPerMinute;

    public float spawnEveryAmountOfSeconds;
    [Space]
    [SerializeField] private float speedMultiplier = 1f;
    public float arrowSpeed;
    private float spawnTimer = 0f;
    [SerializeField] [Range(1, 4)] private int mostKeysSpawnedAtOnce = 2;
    [SerializeField] private float offset = 0.5f;

    [Space]

    [SerializeField] private float spacing = 1f;

    public bool isPaused { get; private set; } = true;

    [Header("Levels")]

    [SerializeField] private int beatsForNextLevel = 20;
    private int currentLevel = 0;
    private int currentBeat = 0;
    [SerializeField] int tempoIncreaseWithLevel = 5;

    private int tutorialstage = 0;
    private bool laststagepass = false;
    private bool isInProgress = false;


    private void Awake()
    {
        instance = this;


    }

    private void Start()
    {
        StartGame();
    }

    private void Update()
    {
        if (Input.GetKeyUp(KeyCode.Backspace))
            SceneManager.LoadScene(0);
        if (Input.GetKeyUp(KeyCode.Return) || Input.GetKeyUp("enter"))
        {
            Data.passTutorial = true;
            SceneManager.LoadScene(3);
        }        if (!isPaused)
        {
            if (tutorialstage == 0 && !isInProgress)
            {
                SpawnArrow(0, false);
                //tutorialstage++;
                isInProgress = true;
            }
            if (tutorialstage == 1 && !isInProgress)
            {
                SpawnArrow(1, false);
                //tutorialstage++;
                isInProgress = true;
            }
            if (tutorialstage == 2 && !isInProgress)
            {
                SpawnArrow(2, false);
                //tutorialstage++;
                isInProgress = true;
            }
            if (tutorialstage == 3 && !isInProgress)
            {
                SpawnArrow(3, false);
                //tutorialstage++;
                isInProgress = true;
                
            }
            if (tutorialstage == 4 && !isInProgress)
            {
                arrowHelpText.SetActive(false);
                bombText.SetActive(true);
                SpawnArrow(0, true);
                //tutorialstage++;
                isInProgress = true;
            }
            if (tutorialstage == 5)
                GameOver();
        }
    }

    public void setInProgress(bool v)
    {
        isInProgress = v;
    }

    public void addstage()
    {
        tutorialstage++;
    }

    public void lastStage()
    {
        tutorialstage = Mathf.Max(0, tutorialstage - 1);
    }

    public void passLastStage()
    {
        laststagepass = true;
    }

    public bool getLastStage()
    {
        return laststagepass;
    }

    private void NextLevel()
    {
        currentLevel++;
        beatsPerMinute += tempoIncreaseWithLevel * Data.difficulty;

        for (int i = 0; i < spawnLocation.childCount; i++)
        {
            Transform child = spawnLocation.GetChild(i);
            child.GetComponent<Arrow>().SetSpeed(arrowSpeed);
        }
    }

    private void SpawnArrow(int id, bool bomb)
    {
        Arrow prefab;

        if (bomb)
            prefab = bombPrefab;
        else
            prefab = arrowPrefab;

        Arrow arrow = Instantiate(prefab, spawnLocation.position + Vector3.right * offset + Vector3.right * spacing * id, Quaternion.Euler(0f, 0f, 90f * id), spawnLocation);

        arrow.rotationID = id;
        arrow.SetSpeed(arrowSpeed);
        arrow.isBomb = bomb;
        arrow.SetColor(Color.white);
    }

    public void MissedArrow()
    {
        isInProgress = false;
        if (!laststagepass)
            lastStage();
    }

    public void Bad()
    {
        Debug.Log("Bad, isInProgress: " + isInProgress + "TutorialStage: " + tutorialstage);
    }

    public void Good()
    {
        passLastStage();
        isInProgress = false;
        addstage();
    }

    public void StartGame()
    {
        if (!isPaused) return;

        isPaused = false;

        currentBeat = 0;
        currentLevel = 0;

        if (Data.difficulty == 1)
        {
            beatsPerMinute = easyStartingBPM;
        }
        else if (Data.difficulty == 2)
        {
            beatsPerMinute = mediumStartingBPM;
        }
        else if (Data.difficulty == 3)
        {
            beatsPerMinute = hardStartingBPM;
        }

        if (musicSource.clip != null)
            musicSource.Play();

        gameOverPanel.SetActive(false);
        bombText.SetActive(false);
    }

    private void GameOver()
    {
        if (isPaused) return;

        isPaused = true;

        musicSource.Stop();

        for (int i = 0; i < spawnLocation.childCount; i++)
        {
            Transform child = spawnLocation.GetChild(i);
            child.GetComponent<Animator>().SetTrigger("Shrink");

            Destroy(child.gameObject, 1f);
        }
        bombText.SetActive(false);
        gameOverPanel.SetActive(true);

        Data.passTutorial = true;

        /*scoreText.text = "Score: " + Data.score;

        if (Data.score > Data.highScore)
        {
            Data.highScore = Data.score;
            newHighScoreText.gameObject.SetActive(true);
        }
        else
        {
            newHighScoreText.gameObject.SetActive(false);
        }

        Data.score = 0;

        if (Data.difficulty == 1)
        {
            highScoreText.text = "Easy ";
        }
        else if (Data.difficulty == 2)
        {
            highScoreText.text = "Medium ";
        }
        else if (Data.difficulty == 3)
        {
            highScoreText.text = "Hard ";
        }

        highScoreText.text += "High Score: " + Data.highScore;*/

        //gameOverPanel.GetComponent<Animator>().SetTrigger("Fade In");
    }

    public void GoToMainMenu()
    {
        SceneManager.LoadScene(0);//main menu scene
    }

    public void Play()
    {
        SceneManager.LoadScene(3);
    }
}
