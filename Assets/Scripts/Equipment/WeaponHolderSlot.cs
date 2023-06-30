using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponHolderSlot : MonoBehaviour
{
  public Transform parentOverride;
  public WeaponItem currentWeapon;
  public bool isLeftHandSlot;
  public bool isRightHandSlot;
  public bool isBackSlot;

  public GameObject currentWeaponModel;

  public void UnloadWeapon()
  {
    if(currentWeaponModel != null)
      currentWeaponModel.SetActive(false);
  }

  public void UnloadWeaponAndDestroy()
  {
    if(currentWeaponModel != null)
      Destroy(currentWeaponModel);
  }

  public void LoadWeaponModel(WeaponItem weaponItem)
  {
    UnloadWeaponAndDestroy();

    if(weaponItem == null)
    {
      UnloadWeapon();
      return;
    }

    GameObject weaponModel = Instantiate(weaponItem.weaponPrefab) as GameObject;
    if(weaponModel != null)
    {
      if(parentOverride != null)      
        weaponModel.transform.parent = parentOverride;      
      else      
        weaponModel.transform.parent = transform;      

      weaponModel.transform.localPosition = Vector3.zero;
      weaponModel.transform.localRotation = Quaternion.identity;
      weaponModel.transform.localScale = Vector3.one;
    }

    currentWeaponModel = weaponModel;
  }
}
