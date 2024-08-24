using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class Victory : MonoBehaviour
{
    public soccergoal[] soccergoal;
    private timerForEndGame timerEndGame;
    public GameObject[] SkillButtons;
    public TextMeshProUGUI MoneyEarnWin;
    public TextMeshProUGUI MoneyEarnLoose;
    public GameObject winBoard;
    public GameObject LooseBoard;
    private Audiomanager Audiomanager;
    private bool allowWin = true;
    private int winCounter = 0; 

    void Start()
    {
        Audiomanager = FindAnyObjectByType<Audiomanager>();
        timerEndGame = FindAnyObjectByType<timerForEndGame>();
        string[] skillKeys = { "Skill_0", "Skill_1", "Skill_2", "Skill_3", "Skill_4" };

        for (int i = 0; i < skillKeys.Length; i++)
        {
            if (PlayerPrefs.HasKey(skillKeys[i]))
            {
                SkillButtons[i].SetActive(true);
            }
            else
            {
                SkillButtons[i].SetActive(false);
            }
        }

    }

    private void SaveNewWin()
    {
        
        int maxWinValue = 0;

        
        for (int i = 1; i <= 35; i++) 
        {
            if (PlayerPrefs.HasKey("win_" + i))
            {
                int winValue = PlayerPrefs.GetInt("win_" + i, 0);
                if (winValue > maxWinValue)
                {
                    maxWinValue = winValue;
                }
            }
        }

        
        int newWinValue = maxWinValue + 1;
        PlayerPrefs.SetInt("win_" + newWinValue, newWinValue);

        
        PlayerPrefs.Save();
        
    }

    private void Update()
    {
        if (timerEndGame.win && allowWin)
        {
            Audiomanager.PlaySFX(Audiomanager.Victory);
            Audiomanager.PlaySFX(Audiomanager.coins);

            SaveNewWin();

            soccergoal activeSoccerGoal = FindActiveSoccerGoal();
            if (activeSoccerGoal != null)
            {
                int currentCoinsBalance = PlayerPrefs.GetInt("CoinsBalance", 0);
                currentCoinsBalance += activeSoccerGoal.coinsbalance;
                PlayerPrefs.SetInt("CoinsBalance", currentCoinsBalance);
                PlayerPrefs.Save();
                MoneyEarnWin.text = activeSoccerGoal.coinsbalance.ToString();
                winBoard.SetActive(true);
            }

            allowWin = false;
        }
        else if (timerEndGame.lose && allowWin)
        {
            Audiomanager.PlaySFX(Audiomanager.Loose);
            Audiomanager.PlaySFX(Audiomanager.coins);

            soccergoal activeSoccerGoal = FindActiveSoccerGoal();
            if (activeSoccerGoal != null)
            {
                int currentCoinsBalance = PlayerPrefs.GetInt("CoinsBalance", 0);
                currentCoinsBalance += activeSoccerGoal.coinsbalance;
                PlayerPrefs.SetInt("CoinsBalance", currentCoinsBalance);
                PlayerPrefs.Save();
                Debug.Log(currentCoinsBalance);
                activeSoccerGoal.coinsbalance /= 2;
                MoneyEarnLoose.text = activeSoccerGoal.coinsbalance.ToString();
                Debug.Log(activeSoccerGoal.coinsbalance);
                LooseBoard.SetActive(true);
            }

            allowWin = false;
        }
    }

    private soccergoal FindActiveSoccerGoal()
    {
        foreach (var goal in soccergoal)
        {
            if (goal.gameObject.activeInHierarchy)
            {
                return goal;
            }
        }
        return null; 
    }
}

