using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
  public bool isInteracting;

  [Header("# Player Flags")]
  public bool isSprinting;

  private InputHandler inputHandler;
  private CameraHandler cameraHandler;
  private PlayerController playerController;
  private Animator animator;

  private void Awake()
  {
    cameraHandler = CameraHandler.instance;
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

    inputHandler.TickInput(delta);

    playerController.HandleMovement(delta);

    playerController.HandleRollingAndSprinting(delta);
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
    // float delta = Time.deltaTime;
    // if (cameraHandler != null)
    // {
    //   cameraHandler.FollowTarget(delta);
    //   cameraHandler.HandleCameraRotation(delta, inputHandler.mouseX, inputHandler.mouseY);
    // }

    inputHandler.rollFlag = false;
    inputHandler.sprintFlag = false;
    isSprinting = inputHandler.b_Input;
  }
}
