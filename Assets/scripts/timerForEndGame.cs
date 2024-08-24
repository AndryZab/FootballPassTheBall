using System.Collections;
using UnityEngine;
using TMPro;

public class timerForEndGame : MonoBehaviour
{
    public int minMinutes = 1;
    public int minSeconds = 19;
    public int maxMinutes = 1;
    public int maxSeconds = 35;
    private float levelTime;
    private float elapsedTime;
    private bool levelCompleted = false;
    public bool win = false;
    public bool lose = false;
    public TextMeshProUGUI timerStart;
    public TextMeshProUGUI[] timerText;

    private float startTimerDuration = 3f;
    public float currentStartTimer;
    public GameObject panelforblock;
    public soccergoal[] soccergoal;
    private movegoalkeeper movegoalkeeper;
    public Animator AnimatorGoalKeeper;
    private Audiomanager Audiomanager;
    private bool soundPlayed = false;
    private int currentLevel = 1;
    private float winningTime = 0f; 
    private stateitem stateitem;
    public bool playMusic = false;
    void Start()
    {
        stateitem = FindAnyObjectByType<stateitem>();
        Audiomanager = FindAnyObjectByType<Audiomanager>();
        movegoalkeeper = FindAnyObjectByType<movegoalkeeper>();

        LoadLevelProgress(); 
        GenerateLevelTime(); 

        StartCoroutine(StartTimer());
    }

    void Update()
    {
        if (!levelCompleted)
        {
            if (currentStartTimer > 0)
            {
                UpdateStartTimerText();
                panelforblock.SetActive(true);
                movegoalkeeper.speed = 0;
                AnimatorGoalKeeper.enabled = false;
                return;
            }
            else
            {
                elapsedTime += Time.deltaTime;
                AnimatorGoalKeeper.enabled = true;
                panelforblock.SetActive(false);
            }

            UpdateTimerText();

            foreach (soccergoal Soccergoal in soccergoal)
            {
                if (Soccergoal != null && Soccergoal.gameObject.activeSelf)
                {
                    if (Soccergoal.countGoals >= 10)
                    {
                        win = true;
                        CompleteLevel();
                    }

                    if (CheckTimerTextForLoseCondition())
                    {
                        lose = true;
                    }
                }
            }
           
            
        }
       

        

    }
    private bool CheckTimerTextForLoseCondition()
    {
        foreach (var timer in timerText)
        {
            if (timer.text == "Time: 00:00")
            {
                return true;
            }
        }
        return false;
    }

    private void GenerateLevelTime()
    {
        if (PlayerPrefs.HasKey("LevelTime"))
        {
            levelTime = PlayerPrefs.GetFloat("LevelTime");
        }
        else
        {
            float minTotalSeconds = minMinutes * 60 + minSeconds;
            float maxTotalSeconds = maxMinutes * 60 + maxSeconds;
            levelTime = Random.Range(minTotalSeconds, maxTotalSeconds);
            PlayerPrefs.SetFloat("LevelTime", levelTime);
            PlayerPrefs.Save();
        }
    }

    public IEnumerator StartTimer()
    {
        playMusic = false;
        foreach (soccergoal Soccergoal in soccergoal)
        {
          if (Soccergoal.countGoals == 10) yield break;

        }
        currentStartTimer = startTimerDuration;
        Audiomanager.musicSource.enabled = false;
        Audiomanager.musicsourcedefault.enabled = false;

        while (currentStartTimer > 0)
        {
            if (Time.timeScale != 0)
            {
               UpdateStartTimerText();
               yield return new WaitForSeconds(1f);
               currentStartTimer -= 1f;
               if (!soundPlayed)
               {
                    Audiomanager.PlaySFX(Audiomanager.timer);
                    soundPlayed = true;
               }

            }
            
        }

        timerStart.text = "";
        Audiomanager.musicSource.enabled = true;
        Audiomanager.musicsourcedefault.enabled = true;
        stateitem.LoadMusicState();
        Audiomanager.PlaySFX(Audiomanager.whistle);
        soundPlayed = false;
        playMusic = true; 
    }


    private void CompleteLevel()
    {
        levelCompleted = true;

        if (win)
        {
            winningTime = elapsedTime;
            currentLevel++;
            PlayerPrefs.DeleteKey("LevelTime");
            PlayerPrefs.SetInt("currentLevel", currentLevel);
            PlayerPrefs.Save();
        }

        StopAllCoroutines();
    }

    private void LoadLevelProgress()
    {
        currentLevel = PlayerPrefs.GetInt("currentLevel", 1);
    }

    private void UpdateStartTimerText()
    {
        int remainingSeconds = Mathf.CeilToInt(currentStartTimer);
        timerStart.text = remainingSeconds > 0 ? remainingSeconds.ToString() : "";
    }

    private void UpdateTimerText()
    {
        if (!lose)
        {
            float remainingTime = Mathf.Max(levelTime - elapsedTime, 0);

            if (win)
            {
                remainingTime = winningTime;
            }

            int minutes = Mathf.FloorToInt(remainingTime / 60);
            int seconds = Mathf.FloorToInt(remainingTime % 60);
            string text = string.Format("Time: {0:00}:{1:00}", minutes, seconds);
            SetActiveTimerText(timerText, text);
        }
    }

    private void SetActiveTimerText(TextMeshProUGUI[] timerArray, string text)
    {
        foreach (var timer in timerArray)
        {
            if (timer.gameObject.activeInHierarchy)
            {
                timer.text = text;
            }
        }
    }
}
