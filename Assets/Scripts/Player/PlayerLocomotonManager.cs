using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLocomotonManager : MonoBehaviour
{
  [HideInInspector] public Transform myTransform;

  public GameObject normalCamera; // lock on camera
  public Vector3 moveDirection;

  public CapsuleCollider characterCollider;
  public CapsuleCollider characterCollisionBlockerCollider;

  [Header("# Ground & Air Detection Stats")]
  [SerializeField] private float groundDetectionRayStartPoint = 0.5f;
  [SerializeField] private float minimumHeightToBeginFall = 1f;
  [SerializeField] private float groundDirectionRayDistance = 0.2f;
  LayerMask ignoreForGroundCheck;
  public float inAirTimer;

  [Header("# Movement Stats")]
  [SerializeField] private float walkingSpeed = 2.5f;
  [SerializeField] private float movementSpeed = 5f;
  [SerializeField] private float sprintSpeed = 7f;
  [SerializeField] private float rotationSpeed = 10f;
  [SerializeField] private float fallingSpeed = 250f;

  [Header("# Stamina Cost")]
  [SerializeField] private int rollStaminaCost = 15;
  [SerializeField] private int backstepStaminaCost = 12;
  [SerializeField] private int sprintStaminaCost = 1;

  private Rigidbody rb;
  private InputHandler inputHandler;
  private Transform cameraTransform;
  private CameraHandler cameraHandler;

  private PlayerManager playerManager;
  private PlayerStatsManager playerStats;
  private PlayerAnimatorManager playerAnimatorManager;

  private Vector3 normalVector;
  private Vector3 targetPosition;

  private void Awake() 
  {
    rb = GetComponent<Rigidbody>();
    inputHandler = GetComponent<InputHandler>();
    cameraHandler = FindObjectOfType<CameraHandler>();

    playerManager = GetComponent<PlayerManager>();
    playerStats = GetComponent<PlayerStatsManager>();
    playerAnimatorManager = GetComponent<PlayerAnimatorManager>();
  }
  private void Start() 
  {
    cameraTransform = Camera.main.transform;
    myTransform = transform;

    playerManager.isGrounded = true;
    ignoreForGroundCheck = ~(1 << 8 | 1 << 11);

    Physics.IgnoreCollision(characterCollider, characterCollisionBlockerCollider, true);

    // Cursor.lockState = CursorLockMode.Locked;
    // Cursor.visible = false;
  }

  public void HandleMovement(float delta)
  {
    if(inputHandler.rollFlag) return;

    if(playerManager.isInteracting) return;

    moveDirection = cameraTransform.forward * inputHandler.vertical;
    moveDirection += cameraTransform.right * inputHandler.horizontal;
    moveDirection.Normalize();
    moveDirection.y = 0;

    float speed  = movementSpeed;

    if(inputHandler.sprintFlag && inputHandler.moveAmount > 0.5f)
    {
      speed = sprintSpeed;
      playerManager.isSprinting = true;
      moveDirection *= speed;
      playerStats.TakeStamina(sprintStaminaCost);
    }
    else
    {
      if(inputHandler.moveAmount < 0.5f)
      {
        moveDirection *= walkingSpeed;
        playerManager.isSprinting = false;
      }
      else
      {
        moveDirection *= speed;
        playerManager.isSprinting = false;
      }
    }

    Vector3 projectedVelocity = Vector3.ProjectOnPlane(moveDirection, normalVector);
    rb.velocity = projectedVelocity;

    if(inputHandler.lockOnFlag && !inputHandler.sprintFlag)
    {
      playerAnimatorManager.UpdateAnimatorValue(inputHandler.vertical, inputHandler.horizontal, playerManager.isSprinting);
    }
    else // TODO;
    {
      playerAnimatorManager.UpdateAnimatorValue(inputHandler.moveAmount, 0, playerManager.isSprinting);
    }
  }

  public void HandleRotation(float delta)
  {
    if (playerAnimatorManager.canRotate)
    {
      if (inputHandler.lockOnFlag)
      {
        if (inputHandler.sprintFlag || inputHandler.rollFlag)
        {
          Vector3 targetDirection = Vector3.zero;
          targetDirection = cameraTransform.forward * inputHandler.vertical;
          targetDirection += cameraTransform.right * inputHandler.horizontal;
          targetDirection.Normalize();
          targetDirection.y = 0;

          if (targetDirection == Vector3.zero)
            targetDirection = transform.forward;

          Quaternion tr = Quaternion.LookRotation(targetDirection);
          Quaternion targetRotation = Quaternion.Slerp(transform.rotation, tr, rotationSpeed * Time.deltaTime);
          transform.rotation = targetRotation;
        }
        else
        {
          Vector3 rotationDirection = moveDirection;
          rotationDirection = cameraHandler.currentLockOnTarget.transform.position - transform.position;
          rotationDirection.y = 0;
          rotationDirection.Normalize();

          Quaternion tr = Quaternion.LookRotation(rotationDirection);
          Quaternion targetRotation = Quaternion.Slerp(transform.rotation, tr, rotationSpeed * Time.deltaTime);
          transform.rotation = targetRotation;
        }
      }
      else
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
    }        
  }

  public void HandleRollingAndSprinting(float delta)
  {
    if(playerAnimatorManager.animator.GetBool("isInteracting")) return;

    if(playerStats.currentStamina <= 0) return;

    if (inputHandler.rollFlag)
    {
      moveDirection = cameraTransform.forward * inputHandler.vertical;
      moveDirection += cameraTransform.right * inputHandler.horizontal;

      if(inputHandler.moveAmount > 0)
      {
        playerAnimatorManager.PlayTargetAnimation("Rolling", true);
        moveDirection.y = 0;
        Quaternion rollRotation = Quaternion.LookRotation(moveDirection);
        myTransform.rotation = rollRotation;
        playerStats.TakeStamina(rollStaminaCost);
      }
      else
      {
        playerAnimatorManager.PlayTargetAnimation("Backstep", true);
        playerStats.TakeStamina(backstepStaminaCost);
      }
    }
  }

  public void HandleFalling(float delta, Vector3 moveDirection)
  {
    playerManager.isGrounded = false;
    RaycastHit hit;
    Vector3 rayOrigin = myTransform.position;
    rayOrigin.y += groundDetectionRayStartPoint;

    if(Physics.Raycast(rayOrigin, myTransform.forward, out hit, 0.4f))    
      moveDirection = Vector3.zero;    

    if(playerManager.isInAir)
    {
      rb.AddForce(Vector3.down * fallingSpeed);
      rb.AddForce(moveDirection * fallingSpeed / 10f);
    }

    Vector3 dir = moveDirection;
    dir.Normalize();
    rayOrigin += dir * groundDirectionRayDistance;

    targetPosition = myTransform.position;

    Debug.DrawRay(rayOrigin, Vector3.down * minimumHeightToBeginFall, Color.red, 0.1f, false);

    if(Physics.Raycast(rayOrigin, Vector3.down, out hit, minimumHeightToBeginFall, ignoreForGroundCheck))
    {
      normalVector = hit.normal;
      Vector3 tp = hit.point;
      playerManager.isGrounded = true;
      targetPosition.y = tp.y;

      if(playerManager.isInAir)
      {
        if(inAirTimer > 0.5f)
        {
          Debug.Log($"You were in the air for {inAirTimer}");
          playerAnimatorManager.PlayTargetAnimation("Landing", true);
          inAirTimer = 0;
        }
        else
        {
          playerAnimatorManager.PlayTargetAnimation("Empty", false);
          inAirTimer = 0;
        }

        playerManager.isInAir = false;
      }
    }
    else
    {
      if(playerManager.isGrounded)
        playerManager.isGrounded = false;

      if(playerManager.isInAir == false)
      {
        if(playerManager.isInteracting == false)
        {
          playerAnimatorManager.PlayTargetAnimation("Falling", true);
        }

        Vector3 vel = rb.velocity;
        vel.Normalize();
        rb.velocity = vel * (movementSpeed / 2);
        playerManager.isInAir = true;
      }
    }

    if(playerManager.isInteracting || inputHandler.moveAmount > 0)
    {
      myTransform.position = Vector3.Lerp(myTransform.position, targetPosition, Time.deltaTime / 0.1f);
    }
    else
    {
      myTransform.position = targetPosition;
    }

    if(playerManager.isGrounded)
    {
      if(playerManager.isInteracting || inputHandler.moveAmount > 0)
      {
        myTransform.position = Vector3.Lerp(myTransform.position, targetPosition, Time.deltaTime);
      }
      else
      {
        myTransform.position = targetPosition;
      }
    }
  }

  public void HandleJumping()
  {
    if(playerManager.isInteracting) return;

    if(playerStats.currentStamina <= 0) return;

    if(inputHandler.jump_Input)
    {
      if(inputHandler.moveAmount > 0)
      {
        moveDirection = cameraTransform.forward * inputHandler.vertical;
        moveDirection += cameraTransform.right * inputHandler.horizontal;

        playerAnimatorManager.PlayTargetAnimation("Jump", true);

        moveDirection.y = 0;
        Quaternion jumpRotation = Quaternion.LookRotation(moveDirection);
        myTransform.rotation = jumpRotation;
      }
    }
  }
}