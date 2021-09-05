using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class ThrustersCoreTemp : MonoBehaviour
{
    public Slider slider;
    public Image sliderFill;

    public void Update()
    {
        sliderFill.color = Color.Lerp(Color.green, Color.red, (slider.value / slider.maxValue));
    }

    public void SetMaxCoreTemp(int temp)
    {
        slider.maxValue = temp;
        slider.value = temp;
    }

    public void SetCoreTemp(int temp)
    {
        slider.value = temp;
    }
}
