using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class scalecountmoney : MonoBehaviour
{
    public TextMeshProUGUI textmoney;
    private void Update()
    {
        if (PlayerPrefs.HasKey("CoinsBalance"))
        {
            int coins = PlayerPrefs.GetInt("CoinsBalance");
            if (coins < 1000)
            {
                textmoney.fontSize = 73;
            }
            if (coins >= 1000)
            {
                textmoney.fontSize = 63;
            }
            if (coins >= 10000)
            {
                textmoney.fontSize = 59;
            }
            if (coins >= 100000)
            {
                textmoney.fontSize = 51;
            }
        }

    }
}
