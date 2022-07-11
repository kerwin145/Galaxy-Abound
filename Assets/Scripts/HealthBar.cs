using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    [SerializeField] Slider slider;
    
    public void SetMaxValue(int health)
    {
        slider.maxValue = health;
    }
    public void SetValue(int h)
    {
        slider.value = h;
    }
}
