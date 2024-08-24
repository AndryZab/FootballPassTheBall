using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.TextCore.Text;
using UnityEngine.UI;

public class UpgradeSkills : MonoBehaviour
{
    [Serializable]
    public class SkillsUpgrade
    {
        public string name;
        public Image bar1;
        public Image bar2;
        public float increaseTime;
        public float decreaseTime;
        public GameObject panelbar1;
        public GameObject panelbar2;
        public TextMeshProUGUI coinsbar1Text;
        public TextMeshProUGUI coinsbar2Text;

        public int upgradeCostbar1;
        public int upgradeCostbar2;
    }

    public TextMeshProUGUI coinsBalanceText;
    public List<SkillsUpgrade> Skillss = new List<SkillsUpgrade>();
    private int coinsBalance;
    private Audiomanager audiomanager;

    private void Start()
    {
        audiomanager = FindAnyObjectByType<Audiomanager>();
        foreach (SkillsUpgrade Skills in Skillss)
        {
            Skills.increaseTime = PlayerPrefs.GetFloat(Skills.name + "_increaseTimeDurationSkill", Skills.increaseTime);
            Skills.decreaseTime = PlayerPrefs.GetFloat(Skills.name + "_decraseTimeReloadSkill", Skills.decreaseTime);

            PlayerPrefs.SetFloat(Skills.name + "_increaseTimeDurationSkill", Skills.increaseTime);
            PlayerPrefs.SetFloat(Skills.name + "_decraseTimeReloadSkill", Skills.decreaseTime);



            if (Skills.bar1 != null && PlayerPrefs.HasKey(Skills.name + "_Bar1FillAmount"))
            {
                Skills.bar1.fillAmount = PlayerPrefs.GetFloat(Skills.name + "Bar1FillAmount", 0f);
                if (Skills.bar1.fillAmount >= 1f && Skills.panelbar1 != null)
                {
                    Skills.panelbar1.SetActive(true);

                }
            }
            if (Skills.bar2 != null && PlayerPrefs.HasKey(Skills.name + "_Bar2FillAmount"))
            {
                Skills.bar2.fillAmount = PlayerPrefs.GetFloat(Skills.name + "_Bar2FillAmount", 0f);
                if (Skills.bar2.fillAmount >= 1f && Skills.panelbar2 != null)
                {
                    Skills.panelbar2.SetActive(true);
                }
            }
           


            if (Skills.panelbar1 != null && PlayerPrefs.HasKey(Skills.name + "_PanelBar1Active"))
            {
                Skills.panelbar1.SetActive(PlayerPrefs.GetInt(Skills.name + "_PanelBar1Active", 0) == 1);
                Skills.coinsbar1Text.gameObject.SetActive(false);

            }
            if (Skills.panelbar2 != null && PlayerPrefs.HasKey(Skills.name + "_PanelBar2Active"))
            {
                Skills.panelbar2.SetActive(PlayerPrefs.GetInt(Skills.name + "_PanelBar2Active", 0) == 1);
                Skills.coinsbar2Text.gameObject.SetActive(false);

            }
           


            if (Skills.coinsbar1Text != null)
            {
                Skills.coinsbar1Text.text = "Cost:   " + Skills.upgradeCostbar1.ToString();
            }
            if (Skills.coinsbar2Text != null)
            {
                Skills.coinsbar2Text.text = "Cost:   " + Skills.upgradeCostbar2.ToString();
            }
            coinsbalancecheck();
        }
    }
    void coinsbalancecheck()
    {
        coinsBalance = PlayerPrefs.GetInt("CoinsBalance");
    }
    void UpdateCoinsBalanceDisplay()
    {

        coinsBalanceText.text = coinsBalance.ToString();
    }
   
    public void UpgradeIncreaseDurationSkill(string SkillsName)
    {
        coinsbalancecheck();
        SkillsUpgrade Skills = Skillss.Find(x => x.name == SkillsName);
        if (Skills != null)
        {
            if (coinsBalance >= Skills.upgradeCostbar1)
            {
                audiomanager.PlaySFX(audiomanager.upgradebutton);

                coinsBalance -= Skills.upgradeCostbar1;
                PlayerPrefs.SetInt("CoinsBalance", coinsBalance);
                PlayerPrefs.Save();
                UpdateCoinsBalanceDisplay();

                Skills.increaseTime = PlayerPrefs.GetFloat(Skills.name + "_increaseTimeDurationSkill", Skills.increaseTime) + 0.15f;

                PlayerPrefs.SetFloat(Skills.name + "_increaseTimeDurationSkill", Skills.increaseTime);

                PlayerPrefs.Save();

                if (Skills.bar1 != null)
                {
                    Skills.bar1.fillAmount += 0.0710f;
                    PlayerPrefs.SetFloat(Skills.name + "_Bar1FillAmount", Skills.bar1.fillAmount);

                    if (Skills.bar1.fillAmount >= 1f)
                    {
                        if (Skills.panelbar1 != null)
                        {

                            Skills.panelbar1.SetActive(true);
                            PlayerPrefs.SetInt(Skills.name + "_PanelBar1Active", 1);
                            Skills.coinsbar1Text.gameObject.SetActive(false);
                            Debug.Log("succseful3");


                        }
                    }
                }

                Skills.upgradeCostbar1 += 4;
                Skills.coinsbar1Text.text = "Cost:   " + Skills.upgradeCostbar1.ToString();
            }

        }

    }
    public void UpgradeDecraseReloadSkill(string SkillsName)
    {
        coinsbalancecheck();
        SkillsUpgrade Skills = Skillss.Find(x => x.name == SkillsName);
        if (Skills != null)
        {
            if (coinsBalance >= Skills.upgradeCostbar2)
            {
                audiomanager.PlaySFX(audiomanager.upgradebutton);
                coinsBalance -= Skills.upgradeCostbar2;
                PlayerPrefs.SetInt("CoinsBalance", coinsBalance);
                PlayerPrefs.Save();
                UpdateCoinsBalanceDisplay();

                Skills.decreaseTime = PlayerPrefs.GetFloat(Skills.name + "_decraseTimeReloadSkill") - 0.23f;

                PlayerPrefs.SetFloat(Skills.name + "_decraseTimeReloadSkill", Skills.decreaseTime);

                PlayerPrefs.Save();

                if (Skills.bar2 != null)
                {
                    Skills.bar2.fillAmount += 0.0710f;
                    PlayerPrefs.SetFloat(Skills.name + "_Bar2FillAmount", Skills.bar2.fillAmount);

                    if (Skills.bar2.fillAmount >= 1f)
                    {
                        if (Skills.panelbar2 != null)
                        {

                            Skills.panelbar2.SetActive(true);
                            PlayerPrefs.SetInt(Skills.name + "_PanelBar2Active", 1);
                            Skills.coinsbar2Text.gameObject.SetActive(false);


                        }
                    }
                }

                Skills.upgradeCostbar2 += 5;
                Skills.coinsbar2Text.text = "Cost:   " + Skills.upgradeCostbar2.ToString();
            }

        }

    }
  


}
