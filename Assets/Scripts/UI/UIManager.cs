using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
  public PlayerInventory playerInventory;
  EquipmentWindowUI equipmentWindowUI;

  [Header("UI Windows")]
  public GameObject hudWindow;
  public GameObject selectableWindow;
  public GameObject weaponInventoryWindow;  

  [Header("Weapon Inventory")]
  public GameObject weaponInventorySlotPrefab;
  public Transform weaponInvetorySlotsParent;

  private WeaponInventorySlot[] weaponInventorySlots;

  private void Awake() 
  {
    equipmentWindowUI = FindObjectOfType<EquipmentWindowUI>();
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
    weaponInventoryWindow.SetActive(false);
  }
}
