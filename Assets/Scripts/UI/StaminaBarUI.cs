using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StaminaBarUI : MonoBehaviour
{
  public Slider slider;

  public void SetMaxStamina(int maxStamina)
  {
    slider.maxValue = maxStamina;
    slider.value = maxStamina;
  }

  public void SetCurrentStamina(int currentStamina)
  {
    slider.value = currentStamina;
  }
}
