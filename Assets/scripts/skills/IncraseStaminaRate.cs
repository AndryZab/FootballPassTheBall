using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class EnduranceSkill : MonoBehaviour
{
    public Player[] players;
    private bool activateSkill = false;
    private float Duration;
    private float Reload;
    public Image durationIndicator;
    public Image reloadIndicator;
    public Button buttonStamina;

    private void Start()
    {
        Duration = PlayerPrefs.GetFloat("Endurance_increaseTimeDurationSkill");
        Reload = PlayerPrefs.GetFloat("Endurance_decraseTimeReloadSkill");
    }

    public void buttonstateEndurance()
    {
        if (buttonStamina.gameObject.activeSelf)
        {
            buttonStamina.interactable = false;
        }

    }
    private void Update()
    {
        if (activateSkill)
        {
            for (int i = 0; i < players.Length; i++)
            {
                string key = $"Player{i + 1}_StaminaDecreaseRate";
                string key1 = $"Player{i + 1}_StaminaRecoveryRate";
                string key2 = $"Player{i + 1}_StaminaDecreaseWhileHoldingBall";
                string key3 = $"Player{i + 1}_StaminaDecreaseOnKick";

                if (PlayerPrefs.HasKey(key))
                {
                    players[i].staminaDecreaseRate = PlayerPrefs.GetFloat(key) * 1.3f;
                    players[i].staminaRecoveryRate = PlayerPrefs.GetFloat(key1) * 1.3f;
                    players[i].staminaDecreaseWhileHoldingBall = PlayerPrefs.GetFloat(key2) * 1.3f;
                    players[i].staminaDecreaseOnKick = PlayerPrefs.GetFloat(key3) * 1.3f;
                }

            }
           

        }
        else
        {
            for (int i = 0; i < players.Length; i++)
            {
                string key = $"Player{i + 1}_StaminaDecreaseRate";
                string key1 = $"Player{i + 1}_StaminaRecoveryRate";
                string key2 = $"Player{i + 1}_StaminaDecreaseWhileHoldingBall";
                string key3 = $"Player{i + 1}_StaminaDecreaseOnKick";

                if (PlayerPrefs.HasKey(key))
                {
                    players[i].staminaDecreaseRate = PlayerPrefs.GetFloat(key);
                    players[i].staminaRecoveryRate = PlayerPrefs.GetFloat(key1);
                    players[i].staminaDecreaseWhileHoldingBall = PlayerPrefs.GetFloat(key2);
                    players[i].staminaDecreaseOnKick = PlayerPrefs.GetFloat(key3);
                }

            }


        }

    }

    public void skillButton()
    {
        if (!activateSkill && reloadIndicator.fillAmount == 1f)
        {
            StartCoroutine(SkillDuration());

        }

    }
    private IEnumerator SkillDuration()
    {
        activateSkill = true;
        float remainingDuration = Duration;

        while (remainingDuration > 0)
        {
            remainingDuration -= Time.deltaTime;
            durationIndicator.fillAmount = remainingDuration / Duration;
            yield return null;
        }

        durationIndicator.fillAmount = 0;
        activateSkill = false;
        StartCoroutine(ReloadSkill());
    }

    private IEnumerator ReloadSkill()
    {
        float elapsedReloadTime = 0;

        while (elapsedReloadTime < Reload)
        {
            elapsedReloadTime += Time.deltaTime;
            reloadIndicator.fillAmount = elapsedReloadTime / Reload;
            yield return null;
        }

        reloadIndicator.fillAmount = 1; 
        buttonStamina.interactable = true;
        activateSkill = false;
    }
}
