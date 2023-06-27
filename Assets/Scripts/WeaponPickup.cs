using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponPickup : Interactable
{ 
  public WeaponItem weapon;

  public override void Interact(PlayerManager playerManager)
  {
    base.Interact(playerManager);

    // TODO: Pick up the item and add it to the players inventory
    PickUpItem(playerManager);
  }

  private void PickUpItem(PlayerManager playerManager)
  {
    PlayerInventory playerInventory = playerManager.GetComponent<PlayerInventory>();
    Rigidbody playerRigidbody = playerManager.GetComponent<Rigidbody>();
    AnimatorHandler animatorHandler = playerManager.GetComponent<AnimatorHandler>();

    playerRigidbody.velocity = Vector3.zero;
    animatorHandler.PlayTargetAnimation("PickupItem", true);
    playerInventory.weaponsInventory.Add(weapon);

    Destroy(gameObject);
  }
}