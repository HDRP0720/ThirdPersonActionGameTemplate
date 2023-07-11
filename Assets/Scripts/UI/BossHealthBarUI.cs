using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class BossHealthBarUI : MonoBehaviour
{
  public TMP_Text bossNameField;

  private Slider slider;

  private void Awake() 
  {
    bossNameField = GetComponentInChildren<TMP_Text>();
    slider = GetComponentInChildren<Slider>();   
  }

  private void Start()
  {
    SetBossHealthBarToInactive();
  }

  public void SetBossName(string name)
  {
    bossNameField.text = name;
  }

  public void SetBossHealthBarToActive()
  {
    slider.gameObject.SetActive(true);
    bossNameField.gameObject.SetActive(true);
  }

  public void SetBossHealthBarToInactive()
  {
    slider.gameObject.SetActive(false);
    bossNameField.gameObject.SetActive(false);
  }

  public void SetBossMaxHealth(int maxHealth)
  {
    slider.maxValue = maxHealth;
    slider.value = maxHealth;
  }

  public void SetBossCurrentHealth(int currentHealth)
  {
    slider.value = currentHealth;
  }

}
