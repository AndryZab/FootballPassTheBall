using System.Collections;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class soccergoal : MonoBehaviour
{
    public GameObject ball;
    public int countGoals = 0;
    public TextMeshProUGUI textcountGoals;
    public int coinsbalance = 0;
    public GameObject Goal;
    public bool goal = false;
    private Ball ballscript;
    private timerForEndGame timerForEndGame;
    private Audiomanager Audiomanager;
    private int randomCoins;
   
    private void Start()
    {
        Audiomanager = FindAnyObjectByType<Audiomanager>();
        ballscript = FindAnyObjectByType<Ball>();
        timerForEndGame = FindObjectOfType<timerForEndGame>();

        
        int currentLevel = PlayerPrefs.GetInt("currentLevel", 1);  
        string randomCoinsKey = "randomCoins_Level_" + currentLevel;

        if (PlayerPrefs.HasKey(randomCoinsKey))
        {
            randomCoins = PlayerPrefs.GetInt(randomCoinsKey);
        }
        else
        {
            randomCoins = Random.Range(27, 35);
            PlayerPrefs.SetInt(randomCoinsKey, randomCoins);
            PlayerPrefs.Save();
        }

        coinsbalance = randomCoins;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Ball"))
        {
            StartCoroutine(timeanimgoal());
            
        }
    }

    private IEnumerator timeanimgoal()
    {
        goal = true;
        countGoals++;
        ball.SetActive(false);
        coinsbalance += randomCoins;
        Debug.Log(countGoals);
        if (countGoals >= 10)
        {
            yield break;
        }
        Audiomanager.PlaySFX(Audiomanager.cheers);
        Goal.SetActive(true);
        
        yield return new WaitForSeconds(2.05f);
        ballscript.ResetBallToInitialPosition();
        Player.ResetAllPlayersToInitialPositions();
        ball.SetActive(true);
        yield return new WaitForSeconds(0.5f);
        StartCoroutine(timerForEndGame.StartTimer());


        goal = false;
        Goal.SetActive(false);
    }

    private void Update()
    {
        textcountGoals.text = "Goals: " + countGoals.ToString() + "/10";
    }
}
