//using System.Collections;
//using System.Collections.Generic;
//using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class TractorBeam : MonoBehaviour
{
    public Slider slider;
    public Image sliderFill;

private void Start()
    {
        sliderFill.color = new Color(0.1679423f, 0.4368157f, 0.9622642f, 0.4627451f);
    }

    public void SetMaxTractorBeam(int temp)
    {
        slider.maxValue = temp;
    }

    public void SetTractorBeam(int temp)
    {
        slider.value = temp;
    }
}
