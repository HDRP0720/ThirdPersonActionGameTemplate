using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : CharacterManager
{
  public bool isInteracting;

  [Header("# Player Flags")]
  public bool isSprinting;
  public bool isInAir;
  public bool isGrounded;
  public bool canDoCombo;
  public bool isUsingRightHand;
  public bool isUsingLeftHand;

  [HideInInspector]
  public InteractableUI interactableUI;

  private InputHandler inputHandler;
  private CameraHandler cameraHandler;
  private PlayerController playerController;
  private PlayerAnimatorManager playerAnimatorManager;
  private PlayerStats playerStats;

  private Rigidbody rb;

  private void Awake()
  {
    cameraHandler = FindObjectOfType<CameraHandler>();
    backStabCollider = GetComponentInChildren<SpecialAttackCollider>();

    inputHandler = GetComponent<InputHandler>();
    playerController = GetComponent<PlayerController>();
    playerAnimatorManager = GetComponentInChildren<PlayerAnimatorManager>();
    playerStats = GetComponent<PlayerStats>();

    interactableUI = FindObjectOfType<InteractableUI>();

    rb = GetComponent<Rigidbody>();
  }

  private void FixedUpdate()
  {
    float delta = Time.deltaTime;

    playerController.HandleMovement(delta);
    playerController.HandleRotation(delta);
    playerController.HandleFalling(delta, playerController.moveDirection);
  }
  private void Update() 
  {
    float delta = Time.deltaTime;

    isInteracting = playerAnimatorManager.animator.GetBool("isInteracting");
    canDoCombo = playerAnimatorManager.animator.GetBool("canDoCombo");
    isUsingLeftHand = playerAnimatorManager.animator.GetBool("isUsingLeftHand");
    isUsingRightHand = playerAnimatorManager.animator.GetBool("isUsingRightHand");
    isInvulnerable = playerAnimatorManager.animator.GetBool("isInvulnerable");
    isFiringSpell = playerAnimatorManager.animator.GetBool("isFiringSpell");

    playerAnimatorManager.animator.SetBool("isInAir", isInAir);
    playerAnimatorManager.animator.SetBool("isBlocking", isBlocking);
    playerAnimatorManager.animator.SetBool("isDead", playerStats.isDead);
    
    inputHandler.TickInput(delta);
    playerAnimatorManager.canRotate = playerAnimatorManager.animator.GetBool("canRotate");
    playerController.HandleRollingAndSprinting(delta);
    playerController.HandleJumping();
    playerStats.RegenerateStamina();

    CheckForInteractableObject();
  }
  private void LateUpdate() 
  {
    float delta = Time.deltaTime;

    inputHandler.rollFlag = false;

    inputHandler.lightAttack_Input = false;
    inputHandler.heavyAttack_Input = false;   

    inputHandler.d_Pad_Up = false;
    inputHandler.d_Pad_Down = false;
    inputHandler.d_Pad_Left = false;
    inputHandler.d_Pad_Right = false;

    inputHandler.loot_Input = false;
    inputHandler.jump_Input = false;
    inputHandler.inventory_Input = false;
    inputHandler.parry_Input = false;

    if(isInAir)
      playerController.inAirTimer += Time.deltaTime;

    if (cameraHandler != null)
    {
      cameraHandler.FollowTarget(delta);
      cameraHandler.HandleCameraRotation(delta, inputHandler.mouseX, inputHandler.mouseY);
    }
  }
  
  #region  Player Interactions
  public void CheckForInteractableObject()
  {
    RaycastHit hit;
    if(Physics.SphereCast(transform.position, 0.3f, transform.forward, out hit, 1f, cameraHandler.ignoreLayers))
    {
      Debug.DrawRay(transform.position, transform.forward * 0.3f, Color.red, 0.1f, false);
      if(hit.collider.tag == "Interactable")
      {
        Interactable interactableObject = hit.collider.GetComponent<Interactable>();
        if(interactableObject != null)
        {
          string interactableText = interactableObject.interactableText;          
          interactableUI.interactableTextField.text = interactableText;
          interactableUI.interactionPopup.SetActive(true);

          if(inputHandler.loot_Input)
          {
            hit.collider.GetComponent<Interactable>().Interact(this);
          }
        }
      }
    }
    else
    {
      if (interactableUI.interactionPopup != null)
        interactableUI.interactionPopup.SetActive(false);      

      if(interactableUI.itemPopup != null && inputHandler.loot_Input)      
        interactableUI.itemPopup.SetActive(false);      
    }
  }
  public void OpenChestInteraction(Transform playerStandingPosition)
  {
    rb.velocity = Vector3.zero;

    transform.position = playerStandingPosition.position;
    playerAnimatorManager.PlayTargetAnimation("PickupItem", true);
  }
  public void PassThroughFogWallInteraction(Transform fogwallEntrance)
  {
    rb.velocity = Vector3.zero;
    Vector3 rotationDirection = fogwallEntrance.transform.forward;
    Quaternion turnRotation = Quaternion.LookRotation(rotationDirection);
    transform.rotation = turnRotation;

    playerAnimatorManager.PlayTargetAnimation("Pass Through", true);
    transform.forward += Vector3.forward * 2;
  }
  #endregion
}