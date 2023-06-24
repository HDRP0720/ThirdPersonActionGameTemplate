using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInventory : MonoBehaviour
{
  public WeaponItem rightWeapon;
  public WeaponItem leftWeapon;

  private WeaponSlotManager weaponSlotManager;

  private void Awake() 
  {
    weaponSlotManager = GetComponent<WeaponSlotManager>();
  }
  private void Start() 
  {
    weaponSlotManager.LoadWeaponOnSlot(rightWeapon, false);
    weaponSlotManager.LoadWeaponOnSlot(leftWeapon, true);
  }
}