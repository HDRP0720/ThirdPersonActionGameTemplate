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
  }
  private void Update() 
  {
    float delta = Time.deltaTime;

    isInteracting = animator.GetBool("isInteracting");
    canDoCombo = animator.GetBool("canDoCombo");

    inputHandler.TickInput(delta);

    playerController.HandleMovement(delta);

    playerController.HandleRollingAndSprinting(delta);

    playerController.HandleFalling(delta, playerController.moveDirection);

    CheckForInteractableObject();
  } 

  private void FixedUpdate()
  {
    float delta = Time.deltaTime;
    if (cameraHandler != null)
    {
      cameraHandler.FollowTarget(delta);
      cameraHandler.HandleCameraRotation(delta, inputHandler.mouseX, inputHandler.mouseY);
    }
  }

  private void LateUpdate() 
  {
    inputHandler.rollFlag = false;
    inputHandler.sprintFlag = false;

    inputHandler.rb_Input = false;
    inputHandler.rt_Input = false;

    inputHandler.d_Pad_Up = false;
    inputHandler.d_Pad_Down = false;
    inputHandler.d_Pad_Left = false;
    inputHandler.d_Pad_Right = false;

    inputHandler.a_Input = false;

    if(isInAir)
      playerController.inAirTimer += Time.deltaTime;
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
          // TODO: Set the ui text to the interactable object's text
          // TODO: Set the text pop up to true

          if(inputHandler.a_Input)
          {
            hit.collider.GetComponent<Interactable>().Interact(this);
          }
        }
      }
    }
  }
}