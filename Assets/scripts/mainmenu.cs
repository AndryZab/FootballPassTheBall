using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class mainmenu: MonoBehaviour
{
    public GameObject animatortransitions;
    private bool isPlayingTranistions = false;
    private bool closeboard = false;
    public GameObject upgradeCharacter;
    public GameObject upgradeSkill;


    public void upgradeCharacters()
    {
        upgradeCharacter.GetComponent<Image>().color = new Color32(195, 195, 195, 255); // RGB для #C3C3C3
        upgradeSkill.GetComponent<Image>().color = Color.white;
    }

    public void upgradeSkills()
    {
        upgradeCharacter.GetComponent<Image>().color = Color.white;
        upgradeSkill.GetComponent<Image>().color = new Color32(195, 195, 195, 255); // RGB для #C3C3C3
    }


    public void pause()
    {
        Time.timeScale = 0f;
    }
    public void UnPause()
    {
        Time.timeScale = 1f;
    }
    public void loadmenu()
    {
        StartCoroutine(loadscencemenu());
    }
   
    private IEnumerator loadscencemenu()
    {
        Time.timeScale = 1f;
        isPlayingTranistions = true;
        if (animatortransitions != null)
        {
            animatortransitions.SetActive(true);

        }
        yield return new WaitForSeconds(1.5f);
        SceneManager.LoadScene(1);
        isPlayingTranistions = false;

    }
   
    public void ButtonPlay()
    {
        if (!isPlayingTranistions)
        {
           StartCoroutine(loadscencegame());

        }
    }
    private IEnumerator loadscencegame()
    {
        isPlayingTranistions = true;
        if (animatortransitions != null)
        {
           animatortransitions.SetActive(true);

        }
        yield return new WaitForSeconds(1.5f);
        SceneManager.LoadScene(2);
        isPlayingTranistions = false;

    }
  
    
 

}
