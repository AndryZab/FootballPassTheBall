using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class SkillsUnlockScript : MonoBehaviour
{
    private Audiomanager audiomanager;
    private List<int> equippedIndices = new List<int>(); 

    public GameObject[] equipButtons;
    public GameObject[] unequipButtons;
    public GameObject[] panelforUnlock;
    public GameObject[] panelforUnlockUpgrade;

  
    private void Start()
    {
        audiomanager = FindObjectOfType<Audiomanager>();
        
        LoadButtonStates();
        DeactivatePanels();
    }
    private void DeactivatePanels()
    {
        string[] winKeys = new string[]
        {
            "win_10", "win_11", "win_12", "win_13", "win_14",
            "win_15", "win_16", "win_17", "win_18", "win_19",
            "win_20", "win_21", "win_22", "win_23", "win_24",
            "win_25", "win_26", "win_27", "win_28", "win_29",
            "win_30", "win_31", "win_32", "win_33", "win_34",
            "win_35"
        };

        foreach (string key in winKeys)
        {
            if (PlayerPrefs.HasKey(key))
            {
                int winValue = int.Parse(key.Substring(4)); 

                int panelCount = CalculatePanelCount(winValue);

                for (int i = 0; i < panelCount; i++)
                {
                    if (i < panelforUnlock.Length)
                    {
                        DeactivatePanel(panelforUnlock[i]);
                    }

                    if (i < panelforUnlockUpgrade.Length)
                    {
                        DeactivatePanel(panelforUnlockUpgrade[i]);
                    }
                }
            }
        }
    }

    private int CalculatePanelCount(int winValue)
    {
        if (winValue >= 10 && winValue < 15)
        {
            return 1; 
        }
        else if (winValue >= 15 && winValue < 20)
        {
            return 2; 
        }
        else if (winValue >= 20 && winValue < 25)
        {
            return 3; 
        }
        else if (winValue >= 25 && winValue < 35)
        {
            return 4; 
        }
        else if (winValue == 35)
        {
            return 5; 
        }
        else
        {
            return 0; 
        }
    }

    private void DeactivatePanel(GameObject panel)
    {
        if (panel != null)
        {
            panel.SetActive(false);
        }
    }




    public void EquipButton(int index)
    {
        if (equippedIndices.Count >= 3)
        {
            int firstEquippedIndex = equippedIndices[0];
            UnequipButton(firstEquippedIndex);
            PlayerPrefs.DeleteKey("Skill_" + firstEquippedIndex);
        }

        audiomanager.PlaySFX(audiomanager.equipitem);

        unequipButtons[index].SetActive(true);
        equipButtons[index].SetActive(false);
        PlayerPrefs.SetInt("Skill_" + index, index);
        SaveButtonState(index, true);

        equippedIndices.Add(index);
    }

    public void UnequipButton(int index)
    {
        audiomanager.PlaySFX(audiomanager.unequipitem);

        unequipButtons[index].SetActive(false);
        equipButtons[index].SetActive(true);
        PlayerPrefs.DeleteKey("Skill_" + index);
        SaveButtonState(index, false);

        equippedIndices.Remove(index);
    }

    private void SaveButtonState(int index, bool isEquipped)
    {
        PlayerPrefs.SetInt("ButtonState_" + index, isEquipped ? 1 : 0);
        PlayerPrefs.Save();
    }

    private void LoadButtonStates()
    {
        for (int i = 0; i < equipButtons.Length; i++)
        {
            if (PlayerPrefs.HasKey("ButtonState_" + i))
            {
                int state = PlayerPrefs.GetInt("ButtonState_" + i);
                bool isEquipped = state == 1;

                equipButtons[i].SetActive(!isEquipped);
                unequipButtons[i].SetActive(isEquipped);

                if (isEquipped)
                {
                    equippedIndices.Add(i);
                }

            }
        }
    }
    

}
