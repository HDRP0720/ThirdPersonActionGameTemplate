using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerEquipmentManager : MonoBehaviour
{
  public BlockingCollider blockingCollider;

  [Header("Default Equipment Models")]
  public GameObject nakedHeadModel;
  public GameObject nakedTorsoModel;
  // TODO: public string nakedlHandModel;
  // TODO: public string nakedLegModel;

  private InputHandler inputHandler;
  private PlayerInventory playerInventory;

  // Equipment Model Changers for Helmet, Chest, Leg, Hand
  private HelmetModelChanger helmetModelChanger;
  private TorsoModelChanger torsoModelChanger;
  // TODO: hand
  // TODO: leg

  private void Awake() 
  {
    inputHandler = GetComponentInParent<InputHandler>();
    playerInventory = GetComponentInParent<PlayerInventory>();

    helmetModelChanger = GetComponentInChildren<HelmetModelChanger>();
    torsoModelChanger = GetComponentInChildren<TorsoModelChanger>();
  }
  private void Start() 
  {
    EquipAllEquipmentModels();
  }

  private void EquipAllEquipmentModels()
  {
    // HELMET
    helmetModelChanger.UnEquipAllHelmetModels();
    if(playerInventory.currentHelmetEquipment != null)
    {
      nakedHeadModel.SetActive(false);
      helmetModelChanger.EquipHelmetModelByName(playerInventory.currentHelmetEquipment.helmetModelId);
    }
    else
      nakedHeadModel.SetActive(true);
    
    // TORSO
    torsoModelChanger.UnEquipAllTorsoModels();
    if (playerInventory.currentTorsoEquipment != null)
    {
      nakedTorsoModel.SetActive(false);
      torsoModelChanger.EquipTorsoModelByName(playerInventory.currentTorsoEquipment.torsoModelId);
    }
    else
      nakedTorsoModel.SetActive(true);
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
