using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyHealthBarUI : MonoBehaviour
{
  private Slider slider;
  private float timeUntilBarIsHidden = 0f;

  private void Awake() 
  {
    slider = GetComponentInChildren<Slider>();
  }
  private void Update()
  {
    timeUntilBarIsHidden -= Time.deltaTime;

    if(slider != null)
    {
      if (timeUntilBarIsHidden <= 0)
      {
        timeUntilBarIsHidden = 0;
        slider.gameObject.SetActive(false);
      }
      else
      {
        if (!slider.gameObject.activeInHierarchy)
        {
          slider.gameObject.SetActive(true);
        }
      }

      if (slider.value <= 0)
      {
        Destroy(slider.gameObject);
      }
    }   
  }

  public void SetHealth(int health)
  { 
    slider.value = health;
    timeUntilBarIsHidden = 3;
  }

  public void SetMaxHealth(int maxHealth)
  {
    slider.maxValue = maxHealth;
    slider.value = maxHealth;
  }  
}
