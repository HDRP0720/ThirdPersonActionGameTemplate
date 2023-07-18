using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponPickup : Interactable
{ 
  public WeaponItem weapon;

  private InteractableUI interactableUI;

  private void Awake() 
  {
    interactableUI = FindObjectOfType<InteractableUI>();
  }

  public override void Interact(PlayerManager playerManager)
  {
    base.Interact(playerManager);

    // TODO: Pick up the item and add it to the players inventory
    PickUpItem(playerManager);
  }

  private void PickUpItem(PlayerManager playerManager)
  {
    PlayerInventoryManager playerInventory = playerManager.GetComponent<PlayerInventoryManager>();
    Rigidbody playerRigidbody = playerManager.GetComponent<Rigidbody>();
    PlayerAnimatorManager animatorHandler = playerManager.GetComponent<PlayerAnimatorManager>();

    playerRigidbody.velocity = Vector3.zero;
    animatorHandler.PlayTargetAnimation("PickupItem", true);
    playerInventory.weaponsInventory.Add(weapon);

    interactableUI.itemTextField.text = weapon.itemName;
    interactableUI.itemImage.sprite = weapon.itemIcon;
    interactableUI.itemPopup.SetActive(true);

    Destroy(gameObject);
  }
}
