using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
  public bool isInteracting;

  [Header("# Player Flags")]
  public bool isSprinting;
  public bool isInAir;
  public bool isGrounded;
  public bool canDoCombo;

  [HideInInspector]
  public InteractableUI interactableUI;

  private InputHandler inputHandler;
  private CameraHandler cameraHandler;
  private PlayerController playerController;
  private Animator animator;  

  private void Awake()
  {
    cameraHandler = FindObjectOfType<CameraHandler>();
  }

  private void Start() 
  {
    inputHandler = GetComponent<InputHandler>();
    playerController = GetComponent<PlayerController>();
    animator = GetComponent<Animator>();

    interactableUI = FindObjectOfType<InteractableUI>();
  }
  private void FixedUpdate()
  {
    float delta = Time.deltaTime;
    playerController.HandleMovement(delta);
    playerController.HandleFalling(delta, playerController.moveDirection);
  }
  private void Update() 
  {
    float delta = Time.deltaTime;
    isInteracting = animator.GetBool("isInteracting");
    canDoCombo = animator.GetBool("canDoCombo");
    animator.SetBool("isInAir", isInAir);

    inputHandler.TickInput(delta);
    playerController.HandleRollingAndSprinting(delta);
    playerController.HandleJumping();

    CheckForInteractableObject();
  }
  private void LateUpdate() 
  {
    float delta = Time.deltaTime;

    inputHandler.rollFlag = false;

    inputHandler.rb_Input = false;
    inputHandler.rt_Input = false;

    inputHandler.d_Pad_Up = false;
    inputHandler.d_Pad_Down = false;
    inputHandler.d_Pad_Left = false;
    inputHandler.d_Pad_Right = false;

    inputHandler.loot_Input = false;
    inputHandler.jump_Input = false;
    inputHandler.inventory_Input = false;

    if(isInAir)
      playerController.inAirTimer += Time.deltaTime;

    if (cameraHandler != null)
    {
      cameraHandler.FollowTarget(delta);
      cameraHandler.HandleCameraRotation(delta, inputHandler.mouseX, inputHandler.mouseY);
    }
  }

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
}