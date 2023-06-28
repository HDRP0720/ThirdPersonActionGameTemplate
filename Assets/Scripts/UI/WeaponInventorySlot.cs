using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WeaponInventorySlot : MonoBehaviour
{
  public Image weaponIcon;
  private WeaponItem weaponItem;

  public void AddItem(WeaponItem newItem)
  {
    weaponItem = newItem;
    weaponIcon.sprite = weaponItem.itemIcon;
    weaponIcon.enabled = true;
    gameObject.SetActive(true);
  }

  public void ClearInventorySlot()
  {
    weaponItem = null;
    weaponIcon.sprite = null;
    weaponIcon.enabled = false;
    gameObject.SetActive(false);    
  }
}
