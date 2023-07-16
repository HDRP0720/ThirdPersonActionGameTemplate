using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInventory : MonoBehaviour
{
  [Header("# Current Spell")]
  public SpellItem currentSpell;

  [Header("# Current Consumable Item")]
  public ConsumableItem currentConsumableItem;

  [Header("# Current Equipment")]
  public HelmetEquipment currentHelmetEquipment;
  public TorsoEquipment currentTorsoEquipment;
  public HandEquipment currentHandEquipment;
  public LegEquipment currentLegEquipment;

  [Header("# Current Weapons")]
  public WeaponItem rightWeapon;
  public WeaponItem leftWeapon;

  public WeaponItem unarmedWeapon;

  [Space]
  public WeaponItem[] weaponsInRightHandSlots = new WeaponItem[1];
  public WeaponItem[] weaponsInLeftHandSlots = new WeaponItem[1];
  
  [Space]
  public int currentRightWeaponIndex = -1;
  public int currentLeftWeaponIndex = -1;

  [Space]
  public List<WeaponItem> weaponsInventory;

  private WeaponSlotManager weaponSlotManager;

  private void Awake() 
  {
    weaponSlotManager = GetComponentInChildren<WeaponSlotManager>();
  }
  private void Start() 
  {
    leftWeapon = weaponsInLeftHandSlots[0];
    rightWeapon = weaponsInRightHandSlots[0];

    weaponSlotManager.LoadWeaponOnSlot(leftWeapon, true);
    weaponSlotManager.LoadWeaponOnSlot(rightWeapon, false);
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