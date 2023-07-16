using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeftLegModelChanger : MonoBehaviour
{
  public List<GameObject> leftLegModels;

  private void Awake()
  {
    GetAllLeftLegModels();
  }

  private void GetAllLeftLegModels()
  {
    int childrenGameObjects = transform.childCount;
    for (int i = 0; i < childrenGameObjects; i++)
    {
      leftLegModels.Add(transform.GetChild(i).gameObject);
    }
  }

  public void UnEquipAllLeftLegModels()
  {
    foreach (GameObject helmetModel in leftLegModels)
    {
      helmetModel.SetActive(false);
    }
  }

  public void EquipLeftLegModelByID(string helmetName)
  {
    for (int i = 0; i < leftLegModels.Count; i++)
    {
      if (leftLegModels[i].name == helmetName)
      {
        leftLegModels[i].SetActive(true);
      }
    }
  }
}
