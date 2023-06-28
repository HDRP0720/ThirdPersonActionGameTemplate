using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
  public GameObject selectableWindow;

  public void OpenSelectableWindow()
  {
    selectableWindow.SetActive(true);
  }

  public void CloseSelectableWindow()
  {
    selectableWindow.SetActive(false);
  }
}
