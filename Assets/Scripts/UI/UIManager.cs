using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
  public PlayerInventoryManager playerInventory;
  public EquipmentWindowUI equipmentWindowUI;

  [Header("UI Windows")]
  public GameObject hudWindow;
  public GameObject selectableWindow;
  public GameObject equipmentWindow;
  public GameObject weaponInventoryWindow;

  [Header("Equipment Window Slot Selected")]
  public bool rightHandSlot01Selected;
  public bool rightHandSlot02Selected;
  public bool leftHandSlot01Selected;
  public bool leftHandSlot02Selected;

  [Header("Weapon Inventory")]
  public GameObject weaponInventorySlotPrefab;
  public Transform weaponInvetorySlotsParent;

  private WeaponInventorySlot[] weaponInventorySlots;

  private void Awake() 
  {

  }
  private void Start() 
  {
    weaponInventorySlots = weaponInvetorySlotsParent.GetComponentsInChildren<WeaponInventorySlot>();
    equipmentWindowUI.LoadWeaponsOnEquipmentScreen(playerInventory);
  }

  public void UpdateUI()
  {
    #region Weapon Inventory Slots
    for (int i = 0; i < weaponInventorySlots.Length; i++)
    {
      if(i < playerInventory.weaponsInventory.Count)
      {
        if(weaponInventorySlots.Length < playerInventory.weaponsInventory.Count)
        {
          Instantiate(weaponInventorySlotPrefab, weaponInvetorySlotsParent);
          weaponInventorySlots = weaponInvetorySlotsParent.GetComponentsInChildren<WeaponInventorySlot>();
        }
        weaponInventorySlots[i].AddItem(playerInventory.weaponsInventory[i]);
      }
      else
      {
        weaponInventorySlots[i].ClearInventorySlot();
      }
    }
    #endregion
  }

  public void OpenSelectableWindow()
  {
    selectableWindow.SetActive(true);
  }

  public void CloseSelectableWindow()
  {
    selectableWindow.SetActive(false);
  }

  public void CloseAllInventoryWindows()
  {
    ResetAllSelectedSlots();
    weaponInventoryWindow.SetActive(false);
    equipmentWindow.SetActive(false);
  }

  public void ResetAllSelectedSlots()
  {
    rightHandSlot01Selected = false;
    rightHandSlot02Selected = false;
    leftHandSlot01Selected = false;
    leftHandSlot02Selected = false;
  }
}
