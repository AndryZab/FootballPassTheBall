using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using static UnityEngine.ParticleSystem;

public class LoadingScreen : MonoBehaviour
{
    public string sceneToLoad;
    public Image progressBar;
    public TextMeshProUGUI statusText;
    public ParticleSystem[] particleSystems;

    private void Start()
    {
        StartCoroutine(LoadSceneAsync());
    }

    private void Update()
    {
        foreach (var particleSystem in particleSystems)
        {
            ParticleSystem.MainModule mainModule = particleSystem.main;
            mainModule.startSize = 2 * particleSystem.transform.localScale.x;
        }
    }


    private IEnumerator LoadSceneAsync()
    {
        
        AsyncOperation asyncOperation = SceneManager.LoadSceneAsync(sceneToLoad);

        
        asyncOperation.allowSceneActivation = false;

        
        while (!asyncOperation.isDone)
        {
            float progress = Mathf.Clamp01(asyncOperation.progress / 0.9f);
            progressBar.fillAmount = progress;

            
            if (progress < 1f)
            {
                statusText.text = "";
            }
            else
            {
                statusText.text = "100%";
                asyncOperation.allowSceneActivation = true;
            }

            yield return null;
        }
    }

   
}
