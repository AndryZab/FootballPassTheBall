using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class upgradeCharacters : MonoBehaviour
{
    [Serializable]
    public class CharacterUpgrade
    {
        public string name;
        public Image StrengthBar;
        public Image SpeedBar;
        public Image EnduranceBar;
        public float speedLevel;
        public float maxStamina;
        public float staminaDecreaseRate;
        public float staminaRecoveryRate;
        public float staminaDecreaseWhileHoldingBall;
        public float staminaDecreaseOnKick;
        public float maxKickForce;
        public GameObject panelStrenght;
        public GameObject panelSpeed;
        public GameObject panelEndurance;
        public TextMeshProUGUI coinsStrengthText; 
        public TextMeshProUGUI coinsSpeedText; 
        public TextMeshProUGUI coinsEnduranceText; 

        public int upgradeCostStrength; 
        public int upgradeCostSpeed; 
        public int upgradeCostEndurance; 
    }

    public TextMeshProUGUI coinsBalanceText; 
    public List<CharacterUpgrade> characters = new List<CharacterUpgrade>();
    private int coinsBalance;
    private Audiomanager audiomanager;

    
    void Start()
    {
        audiomanager = FindAnyObjectByType<Audiomanager>();

        coinsBalance = PlayerPrefs.GetInt("CoinsBalance");
        foreach (CharacterUpgrade character in characters)
        {
            character.speedLevel = PlayerPrefs.GetFloat(character.name + "_SpeedLevel", character.speedLevel);
            character.maxStamina = PlayerPrefs.GetFloat(character.name + "_MaxStamina", character.maxStamina);
            character.staminaDecreaseRate = PlayerPrefs.GetFloat(character.name + "_StaminaDecreaseRate", character.staminaDecreaseRate);
            character.staminaRecoveryRate = PlayerPrefs.GetFloat(character.name + "_StaminaRecoveryRate", character.staminaRecoveryRate);
            character.staminaDecreaseWhileHoldingBall = PlayerPrefs.GetFloat(character.name + "_StaminaDecreaseWhileHoldingBall", character.staminaDecreaseWhileHoldingBall);
            character.staminaDecreaseOnKick = PlayerPrefs.GetFloat(character.name + "_StaminaDecreaseOnKick", character.staminaDecreaseOnKick);
            character.maxKickForce = PlayerPrefs.GetFloat(character.name + "_MaxKickForce", character.maxKickForce);

            PlayerPrefs.SetFloat(character.name + "_SpeedLevel", character.speedLevel);
            PlayerPrefs.SetFloat(character.name + "_MaxStamina", character.maxStamina);
            PlayerPrefs.SetFloat(character.name + "_StaminaDecreaseRate", character.staminaDecreaseRate);
            PlayerPrefs.SetFloat(character.name + "_StaminaRecoveryRate", character.staminaRecoveryRate);
            PlayerPrefs.SetFloat(character.name + "_StaminaDecreaseWhileHoldingBall", character.staminaDecreaseWhileHoldingBall);
            PlayerPrefs.SetFloat(character.name + "_StaminaDecreaseOnKick", character.staminaDecreaseOnKick);
            PlayerPrefs.SetFloat(character.name + "_MaxKickForce", character.maxKickForce);


            if (character.StrengthBar != null && PlayerPrefs.HasKey(character.name + "_StrengthBarFillAmount"))
            {
                character.StrengthBar.fillAmount = PlayerPrefs.GetFloat(character.name + "_StrengthBarFillAmount", 0f);
                if (character.StrengthBar.fillAmount >= 1f && character.panelStrenght != null)
                {
                    character.panelStrenght.SetActive(true);

                }
            }
            if (character.EnduranceBar != null && PlayerPrefs.HasKey(character.name + "_EnduranceBarFillAmount"))
            {
                character.EnduranceBar.fillAmount = PlayerPrefs.GetFloat(character.name + "_EnduranceBarFillAmount", 0f);
                if (character.EnduranceBar.fillAmount >= 1f && character.panelEndurance != null)
                {
                    character.panelEndurance.SetActive(true);
                }
            }
            if (character.SpeedBar != null && PlayerPrefs.HasKey(character.name + "_SpeedBarFillAmount"))
            {
                character.SpeedBar.fillAmount = PlayerPrefs.GetFloat(character.name + "_SpeedBarFillAmount", 0f);
                if (character.SpeedBar.fillAmount >= 1f && character.panelSpeed != null)
                {
                    character.panelSpeed.SetActive(true);
                }
            }

            
            if (character.panelStrenght != null && PlayerPrefs.HasKey(character.name + "_PanelStrengthActive"))
            {
                character.panelStrenght.SetActive(PlayerPrefs.GetInt(character.name + "_PanelStrengthActive", 0) == 1);
                character.coinsStrengthText.gameObject.SetActive(false);

            }
            if (character.panelSpeed != null && PlayerPrefs.HasKey(character.name + "_PanelSpeedActive"))
            {
                character.panelSpeed.SetActive(PlayerPrefs.GetInt(character.name + "_PanelSpeedActive", 0) == 1);
                character.coinsSpeedText.gameObject.SetActive(false);

            }
            if (character.panelEndurance != null && PlayerPrefs.HasKey(character.name + "_PanelEnduranceActive"))
            {
                character.panelEndurance.SetActive(PlayerPrefs.GetInt(character.name + "_PanelEnduranceActive", 0) == 1);
                character.coinsEnduranceText.gameObject.SetActive(false);

            }

            
            if (character.coinsStrengthText != null)
            {
                character.coinsStrengthText.text = "Cost:   " + character.upgradeCostStrength.ToString();
            }
            if (character.coinsSpeedText != null)
            {
                character.coinsSpeedText.text = "Cost:   " + character.upgradeCostSpeed.ToString();
            }
            if (character.coinsEnduranceText != null)
            {
                character.coinsEnduranceText.text = "Cost:   " + character.upgradeCostEndurance.ToString();
            }
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

    public void UpgradeSpeed(string characterName)
    {
        coinsbalancecheck();
        CharacterUpgrade character = characters.Find(x => x.name == characterName);
        if (character != null)
        {
            if (coinsBalance >= character.upgradeCostSpeed)
            {
                audiomanager.PlaySFX(audiomanager.upgradebutton);
                coinsBalance -= character.upgradeCostSpeed;
                PlayerPrefs.SetInt("CoinsBalance", coinsBalance);
                PlayerPrefs.Save();
                UpdateCoinsBalanceDisplay();

                character.speedLevel = PlayerPrefs.GetFloat(character.name + "_SpeedLevel") + 0.07f;
                PlayerPrefs.SetFloat(character.name + "_SpeedLevel", character.speedLevel);
                PlayerPrefs.Save();

                if (character.SpeedBar != null)
                {
                    character.SpeedBar.fillAmount += 0.0710f;
                    PlayerPrefs.SetFloat(character.name + "_SpeedBarFillAmount", character.SpeedBar.fillAmount);

                    if (character.SpeedBar.fillAmount >= 1f)
                    {
                        if (character.panelSpeed != null)
                        {
                            character.panelSpeed.SetActive(true);
                            PlayerPrefs.SetInt(character.name + "_PanelSpeedActive", 1);
                            character.coinsSpeedText.gameObject.SetActive(false);

                        }
                    }
                }

                character.upgradeCostSpeed += 7;  
                character.coinsSpeedText.text = "Cost:   " + character.upgradeCostSpeed.ToString(); 
            }
            
        }
    }

    public void UpgradeStrength(string characterName)
    {
        coinsbalancecheck();
        CharacterUpgrade character = characters.Find(x => x.name == characterName);
        if (character != null)
        {
            if (coinsBalance >= character.upgradeCostStrength)
            {
                audiomanager.PlaySFX(audiomanager.upgradebutton);

                coinsBalance -= character.upgradeCostStrength;
                PlayerPrefs.SetInt("CoinsBalance", coinsBalance);
                PlayerPrefs.Save();
                UpdateCoinsBalanceDisplay();

                character.maxStamina = PlayerPrefs.GetFloat(character.name + "_MaxStamina") + 2f;
                character.maxKickForce = PlayerPrefs.GetFloat(character.name + "_MaxKickForce") + 0.33f;

                PlayerPrefs.SetFloat(character.name + "_MaxStamina", character.maxStamina);
                PlayerPrefs.SetFloat(character.name + "_MaxKickForce", character.maxKickForce);

                PlayerPrefs.Save();

                if (character.StrengthBar != null)
                {
                    character.StrengthBar.fillAmount += 0.0710f;
                    PlayerPrefs.SetFloat(character.name + "_StrengthBarFillAmount", character.StrengthBar.fillAmount);

                    if (character.StrengthBar.fillAmount >= 1f)
                    {
                        if (character.panelStrenght != null)
                        {

                            character.panelStrenght.SetActive(true);
                            PlayerPrefs.SetInt(character.name + "_PanelStrengthActive", 1);
                            character.coinsStrengthText.gameObject.SetActive(false);


                        }
                    }
                }

                character.upgradeCostStrength += 6; 
                character.coinsStrengthText.text = "Cost:   " + character.upgradeCostStrength.ToString(); 
            }
            
        }
    }

    public void UpgradeEndurance(string characterName)
    {
        coinsbalancecheck();
        CharacterUpgrade character = characters.Find(x => x.name == characterName);
        if (character != null)
        {

            if (coinsBalance >= character.upgradeCostEndurance)
            {
                audiomanager.PlaySFX(audiomanager.upgradebutton);

                coinsBalance -= character.upgradeCostEndurance;
                PlayerPrefs.SetInt("CoinsBalance", coinsBalance);
                PlayerPrefs.Save();
                UpdateCoinsBalanceDisplay();

                character.staminaDecreaseRate = PlayerPrefs.GetFloat(character.name + "_StaminaDecreaseRate") - 0.3f;
                character.staminaRecoveryRate = PlayerPrefs.GetFloat(character.name + "_StaminaRecoveryRate") - 0.1f;
                character.staminaDecreaseWhileHoldingBall = PlayerPrefs.GetFloat(character.name + "_StaminaDecreaseWhileHoldingBall") - 0.1f;
                character.staminaDecreaseOnKick = PlayerPrefs.GetFloat(character.name + "_StaminaDecreaseOnKick") - 0.3f;

                PlayerPrefs.SetFloat(character.name + "_StaminaDecreaseRate", character.staminaDecreaseRate);
                PlayerPrefs.SetFloat(character.name + "_StaminaRecoveryRate", character.staminaRecoveryRate);
                PlayerPrefs.SetFloat(character.name + "_StaminaDecreaseWhileHoldingBall", character.staminaDecreaseWhileHoldingBall);
                PlayerPrefs.SetFloat(character.name + "_StaminaDecreaseOnKick", character.staminaDecreaseOnKick);


                PlayerPrefs.Save();

                if (character.EnduranceBar != null)
                {
                    character.EnduranceBar.fillAmount += 0.0710f;
                    PlayerPrefs.SetFloat(character.name + "_EnduranceBarFillAmount", character.EnduranceBar.fillAmount);

                    if (character.EnduranceBar.fillAmount >= 1f)
                    {
                        if (character.panelEndurance != null)
                        {
                            character.panelEndurance.SetActive(true);
                            PlayerPrefs.SetInt(character.name + "_PanelEnduranceActive", 1);
                            character.coinsEnduranceText.gameObject.SetActive(false);

                        }
                    }
                }

                character.upgradeCostEndurance += 8; 
                character.coinsEnduranceText.text = "Cost:   " + character.upgradeCostEndurance.ToString();
            }
            
        }
    }
}
