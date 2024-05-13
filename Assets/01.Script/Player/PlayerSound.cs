using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerSound : MonoBehaviour
{
    float currentSound;
    float maxSound;
    public Image soundGauge;

    private void Start()
    {
        currentSound = 0f;
        maxSound = 10f;
    }

    public void Sound(float X)
    {
        currentSound = X;
        soundGauge.fillAmount = currentSound / maxSound;
        if (currentSound >= 10f)
        {
            currentSound = 10f;
        }
    }
}
