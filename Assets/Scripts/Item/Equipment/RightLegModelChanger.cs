using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RightLegModelChanger : MonoBehaviour
{
  public List<GameObject> rightLegModels;

  private void Awake()
  {
    GetAllRightLegModels();
  }

  private void GetAllRightLegModels()
  {
    int childrenGameObjects = transform.childCount;
    for (int i = 0; i < childrenGameObjects; i++)
    {
      rightLegModels.Add(transform.GetChild(i).gameObject);
    }
  }

  public void UnEquipAllRightLegModels()
  {
    foreach (GameObject helmetModel in rightLegModels)
    {
      helmetModel.SetActive(false);
    }
  }

  public void EquipRightLegModelByID(string helmetName)
  {
    for (int i = 0; i < rightLegModels.Count; i++)
    {
      if (rightLegModels[i].name == helmetName)
      {
        rightLegModels[i].SetActive(true);
      }
    }
  }
}
