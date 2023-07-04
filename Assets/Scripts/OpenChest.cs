using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenChest : Interactable
{
  public Transform playerStandingPosition;
  public GameObject itemSpawner;
  public WeaponItem itemInChest;

  private Animator animator;
  private OpenChest openChest;

  private void Awake() 
  {
    animator = GetComponent<Animator>();
    openChest = GetComponent<OpenChest>();
  }

  public override void Interact(PlayerManager playerManager)
  {
    // TODO: Roate player towards the chest box    
    Vector3 rotationDirection = transform.position - playerManager.transform.position;
    rotationDirection.y = 0;
    rotationDirection.Normalize();

    Quaternion tr = Quaternion.LookRotation(rotationDirection);
    Quaternion targetRotation = Quaternion.Slerp(playerManager.transform.rotation, tr, 300 * Time.deltaTime);
    playerManager.transform.rotation = targetRotation;

    // TODO: Lock player transform to a certain point infront of the chest box 
    playerManager.OpenChestInteraction(playerStandingPosition);

    // TODO: Open the chest box lid, and animate the player
    animator.Play("ChestOpen");

    // TODO: Spawn an item inside the chest, player can pick up
    StartCoroutine(SpawnItemInChest());

    WeaponPickup weaponPickup = itemSpawner.GetComponent<WeaponPickup>();
    if(weaponPickup != null)
    {
      weaponPickup.weapon = itemInChest;
    }
  }

  private IEnumerator SpawnItemInChest()
  {
    yield return new WaitForSeconds(1f);
    Instantiate(itemSpawner, transform);

    Destroy(openChest);
  }
}
