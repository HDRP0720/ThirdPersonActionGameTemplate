using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInventory : MonoBehaviour
{
  public WeaponItem rightWeapon;
  public WeaponItem leftWeapon;

  public WeaponItem unarmedWeapon;

  public WeaponItem[] weaponsInRightHandSlots = new WeaponItem[1];
  public WeaponItem[] weaponsInLeftHandSlots = new WeaponItem[1];

  public int currentRightWeaponIndex = -1;
  public int currentLeftWeaponIndex = -1;

  public List<WeaponItem> weaponsInventory;

  private WeaponSlotManager weaponSlotManager;

  private void Awake() 
  {
    weaponSlotManager = GetComponent<WeaponSlotManager>();
  }
  private void Start() 
  {
    leftWeapon = unarmedWeapon;
    rightWeapon = unarmedWeapon;

    // rightWeapon = weaponsInRightHandSlots[currentRightWeaponIndex];
    // leftWeapon = weaponsInLeftHandSlots[currentLeftWeaponIndex];

    // weaponSlotManager.LoadWeaponOnSlot(rightWeapon, false);
    // weaponSlotManager.LoadWeaponOnSlot(leftWeapon, true);
  }

  public void ChangeRightWeapon()
  {
    currentRightWeaponIndex += 1;

    if(currentRightWeaponIndex == 0 && weaponsInRightHandSlots[0] != null)
    {
      rightWeapon = weaponsInRightHandSlots[currentRightWeaponIndex];
      weaponSlotManager.LoadWeaponOnSlot(weaponsInRightHandSlots[currentRightWeaponIndex], false);
    }
    else if(currentRightWeaponIndex == 0 && weaponsInRightHandSlots[0] == null)
    {
      currentRightWeaponIndex += 1;
    }
    else if(currentRightWeaponIndex == 1 && weaponsInRightHandSlots[1] != null)
    {
      rightWeapon = weaponsInRightHandSlots[currentRightWeaponIndex];
      weaponSlotManager.LoadWeaponOnSlot(weaponsInRightHandSlots[currentRightWeaponIndex], false);
    }
    else
    {
      currentRightWeaponIndex += 1;
    }

    if(currentRightWeaponIndex > weaponsInRightHandSlots.Length - 1)
    {
      currentRightWeaponIndex = -1;
      rightWeapon = unarmedWeapon;
      weaponSlotManager.LoadWeaponOnSlot(unarmedWeapon, false);
    }
  }

  public void ChangeLeftWeapon()
  {
    currentLeftWeaponIndex += 1;

    if (currentLeftWeaponIndex == 0 && weaponsInLeftHandSlots[0] != null)
    {
      leftWeapon = weaponsInLeftHandSlots[currentLeftWeaponIndex];
      weaponSlotManager.LoadWeaponOnSlot(weaponsInLeftHandSlots[currentLeftWeaponIndex], true);
    }
    else if (currentLeftWeaponIndex == 0 && weaponsInLeftHandSlots[0] == null)
    {
      currentLeftWeaponIndex += 1;
    }
    else if (currentLeftWeaponIndex == 1 && weaponsInLeftHandSlots[1] != null)
    {
      leftWeapon = weaponsInLeftHandSlots[currentLeftWeaponIndex];
      weaponSlotManager.LoadWeaponOnSlot(weaponsInLeftHandSlots[currentLeftWeaponIndex], true);
    }
    else
    {
      currentLeftWeaponIndex += 1;
    }

    if (currentLeftWeaponIndex > weaponsInLeftHandSlots.Length - 1)
    {
      currentLeftWeaponIndex = -1;
      leftWeapon = unarmedWeapon;
      weaponSlotManager.LoadWeaponOnSlot(unarmedWeapon, true);
    }
  }
}