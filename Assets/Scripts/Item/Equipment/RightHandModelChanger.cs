using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RightHandModelChanger : MonoBehaviour
{
  public List<GameObject> rightHandModels;

  private void Awake()
  {
    GetAllRightHandModels();
  }

  private void GetAllRightHandModels()
  {
    int childrenGameObjects = transform.childCount;
    for (int i = 0; i < childrenGameObjects; i++)
    {
      rightHandModels.Add(transform.GetChild(i).gameObject);
    }
  }

  public void UnEquipAllRightHandModels()
  {
    foreach (GameObject helmetModel in rightHandModels)
    {
      helmetModel.SetActive(false);
    }
  }

  public void EquipRightHandModelByID(string helmetName)
  {
    for (int i = 0; i < rightHandModels.Count; i++)
    {
      if (rightHandModels[i].name == helmetName)
      {
        rightHandModels[i].SetActive(true);
      }
    }
  }
}
