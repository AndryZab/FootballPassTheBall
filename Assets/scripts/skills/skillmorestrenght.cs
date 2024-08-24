using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class skillmorestrenght : MonoBehaviour
{
    public Player[] players;
    private bool activateSkill = false;
    private bool curentstaminaUp = false;
    private float Duration;
    private float Reload;
    public Image durationIndicator; 
    public Image reloadIndicator;
    public Button buttonsStrength;


    private void Start()
    {
        Duration = PlayerPrefs.GetFloat("Strength_increaseTimeDurationSkill");
        Reload = PlayerPrefs.GetFloat("Strength_decraseTimeReloadSkill");
    }
    private void Update()
    {
        if (activateSkill)
        {
            for (int i = 0; i < players.Length; i++)
            {
                string key = $"Player{i + 1}_MaxStamina";
                string key1 = $"Player{i + 1}_MaxKickForce";

                if (PlayerPrefs.HasKey(key))
                {
                    players[i].maxStamina = PlayerPrefs.GetFloat(key) * 1.5f;
                    players[i].maxKickForce = PlayerPrefs.GetFloat(key1) * 1.5f;
                }

            }
            if (curentstaminaUp)
            {
                for (int i = 0; i < players.Length; i++)
                {
                    string key = $"Player{i + 1}_MaxStamina";

                    if (PlayerPrefs.HasKey(key))
                    {
                        players[i].currentStamina = PlayerPrefs.GetFloat(key) * 1.5f;
                    }

                }

            }

        }
        else
        {
            for (int i = 0; i < players.Length; i++)
            {
                string key = $"Player{i + 1}_MaxStamina";
                string key1 = $"Player{i + 1}_MaxKickForce";

                if (PlayerPrefs.HasKey(key))
                {
                    players[i].maxStamina = PlayerPrefs.GetFloat(key);
                    players[i].maxKickForce = PlayerPrefs.GetFloat(key1);
                }

            }


        }

    }
    private IEnumerator staminaMore()
    {
        curentstaminaUp = true;
        yield return new WaitForSeconds(0.3f);
        curentstaminaUp = false;
    }
  
    public void skillButton()
    {
        if (!activateSkill && reloadIndicator.fillAmount == 1f)
        {
           StartCoroutine(staminaMore());
           StartCoroutine(SkillDuration());

        }
        
    }
    public void buttonstateStrength()
    {
        if (buttonsStrength.gameObject.activeSelf)
        {
            buttonsStrength.interactable = false;
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
        buttonsStrength.interactable = true;
        activateSkill = false;
    }
}
