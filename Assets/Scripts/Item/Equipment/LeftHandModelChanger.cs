using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeftHandModelChanger : MonoBehaviour
{
  public List<GameObject> leftHandModels;

  private void Awake()
  {
    GetAllLeftHandModels();
  }

  private void GetAllLeftHandModels()
  {
    int childrenGameObjects = transform.childCount;
    for (int i = 0; i < childrenGameObjects; i++)
    {
      leftHandModels.Add(transform.GetChild(i).gameObject);
    }
  }

  public void UnEquipAllLeftHandModels()
  {
    foreach (GameObject helmetModel in leftHandModels)
    {
      helmetModel.SetActive(false);
    }
  }

  public void EquipLeftHandModelByID(string helmetName)
  {
    for (int i = 0; i < leftHandModels.Count; i++)
    {
      if (leftHandModels[i].name == helmetName)
      {
        leftHandModels[i].SetActive(true);
      }
    }
  }
}
