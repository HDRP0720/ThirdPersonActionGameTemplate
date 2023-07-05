using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SoulCountsUI : MonoBehaviour
{
  public TMP_Text soulCountTextField;

  public void SetSoulCount(int soulCount)
  {
    soulCountTextField.text = soulCount.ToString();
  }
}
