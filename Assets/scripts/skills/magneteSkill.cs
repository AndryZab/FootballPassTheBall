using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MagneteSkill : MonoBehaviour
{
    public bool activateSkill = false;
    private float Duration;
    private float Reload;
    public Image durationIndicator;
    public Image reloadIndicator;
    public Button buttonMagnete;


    private void Start()
    {
        Duration = PlayerPrefs.GetFloat("Magnete_increaseTimeDurationSkill");
        Reload = PlayerPrefs.GetFloat("Magnete_decraseTimeReloadSkill");
    }

    public void buttonstateMagnete()
    {
        if (buttonMagnete.gameObject.activeSelf)
        {
            buttonMagnete.interactable = false;
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
        buttonMagnete.interactable = true;
        activateSkill = false;
    }
}
