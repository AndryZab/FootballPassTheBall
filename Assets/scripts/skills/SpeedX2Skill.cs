using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SpeedSkill : MonoBehaviour
{
    public Player[] players;
    private bool activateSkill = false;
    private float Duration;
    private float Reload;
    public Image durationIndicator;
    public Image reloadIndicator;
    public Button buttonSpeed;

    private void Start()
    {
        Duration = PlayerPrefs.GetFloat("Speed_increaseTimeDurationSkill");
        Reload = PlayerPrefs.GetFloat("Speed_decraseTimeReloadSkill");
       
    }
    public void buttonstateSpeed()
    {
        if (buttonSpeed.gameObject.activeSelf)
        {
            buttonSpeed.interactable = false;
        }

    }
    private void Update()
    {
        if (activateSkill)
        {
            for (int i = 0; i < players.Length; i++)
            {
                string key = $"Player{i + 1}_SpeedLevel";
                if (PlayerPrefs.HasKey(key))
                {
                    players[i].speed = PlayerPrefs.GetFloat(key) * 1.35f;
                }

            }

        }
        else
        {
            for (int i = 0; i < players.Length; i++)
            {
                string key = $"Player{i + 1}_SpeedLevel";
                if (PlayerPrefs.HasKey(key))
                {
                    players[i].speed = PlayerPrefs.GetFloat(key);
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
        buttonSpeed.interactable = true;
        activateSkill = false;
    }

}
