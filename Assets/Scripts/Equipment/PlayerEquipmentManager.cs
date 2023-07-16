using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerEquipmentManager : MonoBehaviour
{
  public BlockingCollider blockingCollider;

  [Header("Default Equipment Models")]
  public GameObject nakedHeadModel;

  [Space]
  public GameObject nakedTorsoModel;
  public GameObject nakedLeftHandModel;
  public GameObject nakedRightHandModel;

  [Space]
  public GameObject nakedHipModel;
  public GameObject nakedLeftLegModel;
  public GameObject nakedRightLegModel;

  // Equipment Model Changers for Helmet, Chest, Legs, Hands
  private HelmetModelChanger helmetModelChanger;
  private TorsoModelChanger torsoModelChanger;
  private LeftHandModelChanger leftHandModelChanger;
  private RightHandModelChanger rightHandModelChanger;
  private HipModelChanger hipModelChanger;
  private LeftLegModelChanger leftLegModelChanger;
  private RightLegModelChanger rightLegModelChanger;

  private InputHandler inputHandler;
  private PlayerInventory playerInventory;
  private PlayerStats playerStats;

  private void Awake() 
  {
    inputHandler = GetComponentInParent<InputHandler>();
    playerInventory = GetComponentInParent<PlayerInventory>();
    playerStats = GetComponentInParent<PlayerStats>();

    helmetModelChanger = GetComponentInChildren<HelmetModelChanger>();
    torsoModelChanger = GetComponentInChildren<TorsoModelChanger>();
    leftHandModelChanger = GetComponentInChildren<LeftHandModelChanger>();
    rightHandModelChanger = GetComponentInChildren<RightHandModelChanger>();
    hipModelChanger = GetComponentInChildren<HipModelChanger>();
    leftLegModelChanger = GetComponentInChildren<LeftLegModelChanger>();
    rightLegModelChanger = GetComponentInChildren<RightLegModelChanger>();
  }
  private void Start() 
  {
    EquipAllEquipmentModelsOnStart();
  }

  private void EquipAllEquipmentModelsOnStart()
  {
    // HELMET
    helmetModelChanger.UnEquipAllHelmetModels();
    if(playerInventory.currentHelmetEquipment != null)
    {
      nakedHeadModel.SetActive(false);
      helmetModelChanger.EquipHelmetModelByName(playerInventory.currentHelmetEquipment.helmetModelId);

      playerStats.damageAbsorptionHead = playerInventory.currentHelmetEquipment.physicalDefense;
      Debug.Log($"Helmet Defence Rate: {playerStats.damageAbsorptionHead}");
    }
    else
    {
      nakedHeadModel.SetActive(true);
      playerStats.damageAbsorptionHead = 0;
    }  
    
    // TORSO
    torsoModelChanger.UnEquipAllTorsoModels();
    if(playerInventory.currentTorsoEquipment != null)
    {
      nakedTorsoModel.SetActive(false);
      torsoModelChanger.EquipTorsoModelByName(playerInventory.currentTorsoEquipment.torsoModelId);  

      playerStats.damageAbsorptionBody = playerInventory.currentTorsoEquipment.physicalDefense;
      Debug.Log($"Body Defence Rate: {playerStats.damageAbsorptionBody}");
    }
    else
    {
      nakedTorsoModel.SetActive(true);

      playerStats.damageAbsorptionBody = 0;
    }

    // Hands
    leftHandModelChanger.UnEquipAllLeftHandModels();
    rightHandModelChanger.UnEquipAllRightHandModels();
    if (playerInventory.currentHandEquipment != null)
    {
      nakedLeftHandModel.SetActive(false);
      nakedRightHandModel.SetActive(false);
      leftHandModelChanger.EquipLeftHandModelByID(playerInventory.currentHandEquipment.leftHandModelId);
      rightHandModelChanger.EquipRightHandModelByID(playerInventory.currentHandEquipment.rightHandModelId);

      playerStats.damageAbsorptionHand = playerInventory.currentHandEquipment.physicalDefense;
      Debug.Log($"Hand Defence Rate: {playerStats.damageAbsorptionHand}");
    }
    else
    {
      nakedLeftHandModel.SetActive(true);
      nakedRightHandModel.SetActive(true);

      playerStats.damageAbsorptionHand = 0;
    }

    // Hip & Legs
    hipModelChanger.UnEquipAllHipModels();
    leftLegModelChanger.UnEquipAllLeftLegModels();
    rightLegModelChanger.UnEquipAllRightLegModels();

    if(playerInventory.currentLegEquipment != null)
    {
      nakedHipModel.SetActive(false);
      nakedLeftLegModel.SetActive(false);
      nakedRightLegModel.SetActive(false);

      hipModelChanger.EquipHipModelByID(playerInventory.currentLegEquipment.hipModelId);
      leftLegModelChanger.EquipLeftLegModelByID(playerInventory.currentLegEquipment.leftLegModelId);
      rightLegModelChanger.EquipRightLegModelByID(playerInventory.currentLegEquipment.rightLegModelId);

      playerStats.damageAbsorptionLegs = playerInventory.currentLegEquipment.physicalDefense;
      Debug.Log($"Legs Defence Rate: {playerStats.damageAbsorptionLegs}");
    }
    else
    {
      nakedHipModel.SetActive(true);
      nakedLeftLegModel.SetActive(true);
      nakedRightLegModel.SetActive(true);

      playerStats.damageAbsorptionLegs = 0;
    }    
  }

  public void OpenBlockingCollider()
  {
    if(inputHandler.twoHandFlag)    
      blockingCollider.SetColliderDamageAbsorption(playerInventory.rightWeapon);    
    else    
      blockingCollider.SetColliderDamageAbsorption(playerInventory.leftWeapon);

    blockingCollider.EnableBlockingCollider(); 
  }

  public void CloseBlockingCollider()
  {
    blockingCollider.DisableBlockingCollider();
  }
}
