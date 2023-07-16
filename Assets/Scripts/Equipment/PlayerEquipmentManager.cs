using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerEquipmentManager : MonoBehaviour
{
  public BlockingCollider blockingCollider;

  [Header("Default Equipment Models")]
  public GameObject nakedHeadModel;
  public GameObject nakedTorsoModel;
  public string nakedHipModel;
  public string nakedLeftLegModel;
  public string nakedRightLegModel;

  private InputHandler inputHandler;
  private PlayerInventory playerInventory;

  // Equipment Model Changers for Helmet, Chest, Leg, Hand
  private HelmetModelChanger helmetModelChanger;
  private TorsoModelChanger torsoModelChanger;
  private HipModelChanger hipModelChanger;
  private LeftLegModelChanger leftLegModelChanger;
  private RightLegModelChanger rightLegModelChanger;
  // TODO: hand

  private void Awake() 
  {
    inputHandler = GetComponentInParent<InputHandler>();
    playerInventory = GetComponentInParent<PlayerInventory>();

    helmetModelChanger = GetComponentInChildren<HelmetModelChanger>();
    torsoModelChanger = GetComponentInChildren<TorsoModelChanger>();
    hipModelChanger = GetComponentInChildren<HipModelChanger>();
    leftLegModelChanger = GetComponentInChildren<LeftLegModelChanger>();
    rightLegModelChanger = GetComponentInChildren<RightLegModelChanger>();
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
    if(playerInventory.currentTorsoEquipment != null)
    {
      nakedTorsoModel.SetActive(false);
      torsoModelChanger.EquipTorsoModelByName(playerInventory.currentTorsoEquipment.torsoModelId);
    }
    else
      nakedTorsoModel.SetActive(true);

    // Hip & Leg
    hipModelChanger.UnEquipAllHipModels();
    leftLegModelChanger.UnEquipAllLeftLegModels();
    rightLegModelChanger.UnEquipAllRightLegModels();

    if(playerInventory.currentLegEquipment != null)
    {
      hipModelChanger.EquipHipModelByID(playerInventory.currentLegEquipment.hipModelId);
      leftLegModelChanger.EquipLeftLegModelByID(playerInventory.currentLegEquipment.leftLegModelId);
      rightLegModelChanger.EquipRightLegModelByID(playerInventory.currentLegEquipment.rightLegModelId);
    }
    else
    {
      hipModelChanger.EquipHipModelByID(nakedHipModel);
      leftLegModelChanger.EquipLeftLegModelByID(nakedLeftLegModel);
      rightLegModelChanger.EquipRightLegModelByID(nakedRightLegModel);
    }    
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
