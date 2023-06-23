using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
  [Header("Movement Stats")]
  [SerializeField] private float movementSpeed = 5f;
  [SerializeField] private float sprintSpeed = 7f;
  [SerializeField] private float rotationSpeed = 10f;

  [HideInInspector]
  public Transform myTransform;

  public GameObject normalCamera; // lock on camera  

  private Rigidbody rb;
  private InputHandler inputHandler;
  private Transform cameraTransform;
  private Vector3 moveDirection;

  private PlayerManager playerManager;
  private AnimatorHandler animatorHandler;

  Vector3 normalVector;
  Vector3 targetPosition;

  private void Start() 
  {
    rb = GetComponent<Rigidbody>();
    inputHandler = GetComponent<InputHandler>();
    cameraTransform = Camera.main.transform;
    myTransform = transform;

    playerManager = GetComponent<PlayerManager>();
    animatorHandler = GetComponent<AnimatorHandler>();
    animatorHandler.Init();
  }

  public void HandleMovement(float delta)
  {
    if(inputHandler.rollFlag) return;

    moveDirection = cameraTransform.forward * inputHandler.vertical;
    moveDirection += cameraTransform.right * inputHandler.horizontal;
    moveDirection.Normalize();
    moveDirection.y = 0;

    float speed  = movementSpeed;

    if(inputHandler.sprintFlag)
    {
      speed = sprintSpeed;
      playerManager.isSprinting = true;
      moveDirection *= speed;
    }
    else
    {
      moveDirection *= speed;
    }

    Vector3 projectedVelocity = Vector3.ProjectOnPlane(moveDirection, normalVector);
    rb.velocity = projectedVelocity;

    animatorHandler.UpdateAnimatorValue(inputHandler.moveAmount, 0, playerManager.isSprinting);

    if (animatorHandler.CanRotate)
      HandleRotation(delta);
  }
  private void HandleRotation(float delta)
  {
    Vector3 targetDir = Vector3.zero;
    float moveOverride = inputHandler.moveAmount;

    targetDir = cameraTransform.forward * inputHandler.vertical;
    targetDir += cameraTransform.right * inputHandler.horizontal;

    targetDir.Normalize();
    targetDir.y = 0;

    if (targetDir == Vector3.zero)
      targetDir = transform.forward;

    Quaternion tr = Quaternion.LookRotation(targetDir);
    Quaternion targetRotation = Quaternion.Slerp(myTransform.rotation, tr, rotationSpeed * delta);

    myTransform.rotation = targetRotation;
  }

  public void HandleRollingAndSprinting(float delta)
  {
    if (animatorHandler.animator.GetBool("isInteracting")) return;

    if (inputHandler.rollFlag)
    {
      moveDirection = cameraTransform.forward * inputHandler.vertical;
      moveDirection += cameraTransform.right * inputHandler.horizontal;

      if(inputHandler.moveAmount > 0)
      {
        animatorHandler.PlayTargetAnimation("Rolling", true);
        moveDirection.y = 0;
        Quaternion rollRotation = Quaternion.LookRotation(moveDirection);
        myTransform.rotation = rollRotation;
      }
      else
      {
        animatorHandler.PlayTargetAnimation("Backstep", true);
      }
    }
  }
}