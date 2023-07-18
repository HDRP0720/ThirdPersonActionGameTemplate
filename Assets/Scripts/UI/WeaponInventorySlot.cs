using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WeaponInventorySlot : MonoBehaviour
{
  public Image weaponIcon;

  private PlayerInventoryManager playerInventory;
  private UIManager uiManager;
  private PlayerWeaponSlotManager weaponSlotManager;
  private WeaponItem weaponItem;

  private void Awake() 
  {
    playerInventory = FindObjectOfType<PlayerInventoryManager>();
    weaponSlotManager = FindObjectOfType<PlayerWeaponSlotManager>();
    uiManager = FindObjectOfType<UIManager>();
  }

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

  public void EquipThisItem()
  {
    if(uiManager.rightHandSlot01Selected)
    {
      playerInventory.weaponsInventory.Add(playerInventory.weaponsInRightHandSlots[0]);
      playerInventory.weaponsInRightHandSlots[0] = weaponItem;
      playerInventory.weaponsInventory.Remove(weaponItem);   
    }
    else if (uiManager.rightHandSlot02Selected)
    {
      playerInventory.weaponsInventory.Add(playerInventory.weaponsInRightHandSlots[1]);
      playerInventory.weaponsInRightHandSlots[1] = weaponItem;
      playerInventory.weaponsInventory.Remove(weaponItem);
    }
    else if(uiManager.leftHandSlot01Selected)
    {
      playerInventory.weaponsInventory.Add(playerInventory.weaponsInLeftHandSlots[0]);
      playerInventory.weaponsInLeftHandSlots[0] = weaponItem;
      playerInventory.weaponsInventory.Remove(weaponItem);
    }
    else if(uiManager.leftHandSlot02Selected)
    {
      playerInventory.weaponsInventory.Add(playerInventory.weaponsInLeftHandSlots[1]);
      playerInventory.weaponsInLeftHandSlots[1] = weaponItem;
      playerInventory.weaponsInventory.Remove(weaponItem);
    }
    else
    {
      return;
    }

    playerInventory.rightWeapon = playerInventory.weaponsInRightHandSlots[playerInventory.currentRightWeaponIndex];
    playerInventory.leftWeapon = playerInventory.weaponsInLeftHandSlots[playerInventory.currentLeftWeaponIndex];

    weaponSlotManager.LoadWeaponOnSlot(playerInventory.rightWeapon, false);
    weaponSlotManager.LoadWeaponOnSlot(playerInventory.leftWeapon, true);

    uiManager.equipmentWindowUI.LoadWeaponsOnEquipmentScreen(playerInventory);
    uiManager.ResetAllSelectedSlots();
  }
}
