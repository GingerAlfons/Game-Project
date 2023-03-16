using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Healthbar : MonoBehaviour
{
    public Slider slider;
    //s�tter sliderns value till maxhealth
    public void SetMaxHealth(int health)
    {
        slider.maxValue = health;
        slider.value = health;
    }
    //samma som ovan fast till nuvarande v�rde
    public void SetHealth(int health)
    {
        slider.value = health;
    }

}
