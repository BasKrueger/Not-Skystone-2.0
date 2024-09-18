using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class SkyStoneSpike : MonoBehaviour
{
    private TextMeshProUGUI text;

    private void Awake()
    {
        text = GetComponentInChildren<TextMeshProUGUI>();
    }

    public void SetValue(float value)
    {
        gameObject.SetActive(value > 0);
        text.text = value.ToString();
    }
}
