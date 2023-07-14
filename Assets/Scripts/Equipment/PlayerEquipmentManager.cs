using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerEquipmentManager : MonoBehaviour
{
  public BlockingCollider blockingCollider;

  private InputHandler inputHandler;
  private PlayerInventory playerInventory;

  // Equipment Model Changers for Helmet, Chest, Leg, Hand
  private HelmetModelChanger helmetModelChanger;

  private void Awake() 
  {
    inputHandler = GetComponentInParent<InputHandler>();
    playerInventory = GetComponentInParent<PlayerInventory>();

    helmetModelChanger = GetComponentInChildren<HelmetModelChanger>();
  }
  private void Start() 
  {
    helmetModelChanger.UnEquipAllHelmetModels();
    helmetModelChanger.EquipHelmetModelByName(playerInventory.currentHelmetEquipment.helmetModelId);
  }

  public void OpenBlockingCollider()
  {
    if(inputHandler.twoHandFlag)
    {
      blockingCollider.SetColliderDamageAbsorption(playerInventory.rightWeapon);
    }
    else
    {
      blockingCollider.SetColliderDamageAbsorption(playerInventory.leftWeapon);     
    }

    blockingCollider.EnableBlockingCollider(); 
  }

  public void CloseBlockingCollider()
  {
    blockingCollider.DisableBlockingCollider();
  }
}
