using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using TMPro;
using static UnityEditor.Timeline.TimelinePlaybackControls;

public class WheelFreeSpin : MonoBehaviour
{
    public float spinDuration = 5.0f;
    public float initialSpinSpeed = 1000.0f;
    public float continueSpinSpeed = 10.0f;
    public TextMeshProUGUI resultText;
    public Button spinButton;
    public TextMeshProUGUI spinCountText;
    public int spinsLeft;
    private bool isSpinning = false;
    private float currentSpinTime = 0.0f;
    private float currentSpinSpeed;
    private bool hasWon = false;
    public ParticleSystem[] particleEffects;
    public PrizeProbability[] prizes;
    private float declarationRate = 1f;
    private float currentAngle = 0f;
    public Button buttonClose;
    public TextMeshProUGUI coinsbalance;
    private int coinsINT;
    private Audiomanager audiomanager;

    private void Start()
    {
        audiomanager = FindAnyObjectByType<Audiomanager>();
    }
    public void StartSpin()
    {
        if (!isSpinning && spinsLeft > 0)
        {
            audiomanager.PlaySFX(audiomanager.WheelFortune);


            buttonClose.interactable = false;
            
            foreach (ParticleSystem particleSystem in particleEffects)
            {
                particleSystem.gameObject.SetActive(false);
            }
            isSpinning = true;
            currentSpinTime = 0.0f;
            currentSpinSpeed = initialSpinSpeed;
            hasWon = false;
            UpdateSpinsLeftText();
            StartCoroutine(ParticleSystem());
            StartCoroutine(SpinWheel());
        }
    }

    private IEnumerator ParticleSystem()
    {
        foreach (ParticleSystem particleSystem in particleEffects)
        {
            particleSystem.gameObject.SetActive(true);

            var mainModule = particleSystem.main;
            mainModule.startLifetime = new ParticleSystem.MinMaxCurve(1f);

            var emissionModule = particleSystem.emission;
            emissionModule.rateOverTime = new ParticleSystem.MinMaxCurve(1f);

            yield return new WaitForSeconds(0.5f);
        }
    }

    private void Update()
    {
        UpdateSpinsLeftText();
    }

    private IEnumerator SpinWheel()
    {
        DecreaseSpinCount();
        AnimationCurve slowdownCurve = AnimationCurve.EaseInOut(0, 0, 1, 1);
        float targetAngle = GetTargetAngleBasedOnProbability();
        float totalRotation = 360 * currentSpinSpeed + targetAngle;

        float startAngle = currentAngle;
        float startRotation = transform.eulerAngles.z;

        float currentSpinTime = 0f;
        float currentRotation = startRotation;

        while (currentSpinTime < spinDuration)
        {
            currentSpinTime += Time.deltaTime;

            float t = Mathf.Clamp01(currentSpinTime / spinDuration);

            float curveValue = slowdownCurve.Evaluate(t);

            currentRotation = Mathf.Lerp(startRotation, totalRotation, curveValue);

            transform.eulerAngles = new Vector3(0, 0, currentRotation);

            yield return null;
        }

        transform.eulerAngles = new Vector3(0, 0, targetAngle);

        isSpinning = false;
        hasWon = true;
        string prizeName = DeterminePrize(targetAngle);

        resultText.text = string.Format("Reward: " + prizeName);
        coinsINT = int.Parse(prizeName);

        Debug.Log(coinsINT);

        int coinsSava = PlayerPrefs.GetInt("CoinsBalance") + coinsINT;

        PlayerPrefs.SetInt("CoinsBalance", coinsSava);
        PlayerPrefs.Save();
        coinsbalance.text = coinsSava.ToString();

        if (coinsINT <= 100)
        {
            audiomanager.PlaySFX(audiomanager.rewardcommon);

        }
        else if (coinsINT > 100 && coinsINT <= 200)
        {
            audiomanager.PlaySFX(audiomanager.rewardrare);

        }
        else if (coinsINT > 200 && coinsINT <= 500)
        {
            audiomanager.PlaySFX(audiomanager.rewardlegendary);

        }


        foreach (ParticleSystem particleSystem in particleEffects)
        {
            particleSystem.gameObject.SetActive(false);

            var mainModule = particleSystem.main;
            mainModule.startLifetime = new ParticleSystem.MinMaxCurve(0.5f);

            var emissionModule = particleSystem.emission;
            emissionModule.rateOverTime = new ParticleSystem.MinMaxCurve(1f);

            particleSystem.gameObject.SetActive(true);
        }
        

        buttonClose.interactable = true;
        
        yield return new WaitForSeconds(3f);

        while (!isSpinning)
        {
            currentAngle += continueSpinSpeed * Time.deltaTime;
            transform.Rotate(0, 0, continueSpinSpeed * Time.deltaTime);
            yield return null;
        }
    }

    private float GetTargetAngleBasedOnProbability()
    {
        float totalProbability = 0f;
        foreach (PrizeProbability prize in prizes)
        {
            totalProbability += prize.probability;
        }

        float randomPoint = Random.Range(0f, totalProbability);
        float cumulativeProbability = 0f;

        foreach (PrizeProbability prize in prizes)
        {
            cumulativeProbability += prize.probability;
            if (randomPoint <= cumulativeProbability)
            {
                float angle = Random.Range(prize.minAngle, prize.maxAngle);
                return angle;
            }
        }

        return 0f;
    }

    private string DeterminePrize(float angle)
    {
        foreach (PrizeProbability prize in prizes)
        {
            if (angle >= prize.minAngle && angle <= prize.maxAngle)
            {
                return prize.prizeName;
            }
        }
        return null;
    }

    private void UpdateSpinsLeftText()
    {
        spinsLeft = PlayerPrefs.GetInt("0_Spin") + PlayerPrefs.GetInt("1_Spin") + PlayerPrefs.GetInt("2_Spin");
        spinCountText.text = "Spins: " + spinsLeft;
    }
    private void DecreaseSpinCount()
    {
        for (int i = 0; i < 3; i++)
        {
            string key = i + "_Spin";
            int count = PlayerPrefs.GetInt(key);
            if (count > 0)
            {
                count--;
                PlayerPrefs.SetInt(key, count);
                PlayerPrefs.Save();
                break;
            }
        }
    }
}

[System.Serializable]
public class PrizeProbability
{
    public string prizeName;
    public float probability;
    public float minAngle;
    public float maxAngle;
}
