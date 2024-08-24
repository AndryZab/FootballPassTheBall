using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class goalkeeperfreeze : MonoBehaviour
{
    public movegoalkeeper goalkeeper;
    private bool activateSkill = false;
    private float Duration;
    private float Reload;
    public Image durationIndicator;
    public Image reloadIndicator;
    public GameObject freezeGoalKeeper;
    public GameObject DefaultGoalKeeper;
    private timerForEndGame timerforEndGame;
    public Button buttonsFreeze;
    private void Start()
    {
        timerforEndGame = FindAnyObjectByType<timerForEndGame>();
        Duration = PlayerPrefs.GetFloat("Freeze_increaseTimeDurationSkill");
        Reload = PlayerPrefs.GetFloat("Freeze_decraseTimeReloadSkill");
    }
    private void Update()
    {
        if (activateSkill)
        {
           goalkeeper.speed = 0;

        }
        else if (timerforEndGame.currentStartTimer == 0)
        {
            goalkeeper.speed = 1;
        }

    }
    public void buttonstateFreez()
    {
        if (buttonsFreeze.gameObject.activeSelf)
        {
            buttonsFreeze.interactable = false;
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
        freezeGoalKeeper.SetActive(true);
        DefaultGoalKeeper.SetActive(false);
        float remainingDuration = Duration;

        while (remainingDuration > 0)
        {
            remainingDuration -= Time.deltaTime;
            durationIndicator.fillAmount = remainingDuration / Duration;
            yield return null;
        }

        durationIndicator.fillAmount = 0;
        activateSkill = false;
        freezeGoalKeeper.SetActive(false);
        DefaultGoalKeeper.SetActive(true);
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
        buttonsFreeze.interactable = true;
        activateSkill = false;
    }

}
