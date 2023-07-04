using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimatorManager : AnimatorManager
{  
  public bool CanRotate;

  private PlayerManager playerManager;
  private PlayerStats playerStats;
  private InputHandler inputHandler;
  private Rigidbody playerRigidbody;

  private int vertical;
  private int horizontal;

  public void Init()
  {
    animator = GetComponent<Animator>();

    playerManager = GetComponentInParent<PlayerManager>();
    playerStats = GetComponentInParent<PlayerStats>();
    inputHandler = GetComponentInParent<InputHandler>();       
    playerRigidbody = GetComponentInParent<Rigidbody>();

    vertical = Animator.StringToHash("Vertical");
    horizontal = Animator.StringToHash("Horizontal");
  }

  public void UpdateAnimatorValue(float verticalMovement, float horizontalMovement, bool isSprinting)
  {
    #region Clamp Vertical Input Value
    float v = 0;
    if(verticalMovement > 0 && verticalMovement < 0.55f)
      v = 0.5f;
    else if (verticalMovement > 0.55f)
      v = 1f;
    else if (verticalMovement < 0 && verticalMovement > -0.55f)
      v = -0.5f;
    else if (verticalMovement < -0.55f)
      v = -1f;
    else
      v = 0;
    #endregion

    #region Clamp Horizontal Input Value
    float h = 0;
    if (horizontalMovement > 0 && horizontalMovement < 0.55f)
      h = 0.5f;
    else if (horizontalMovement > 0.55f)
      h = 1f;
    else if (horizontalMovement < 0 && horizontalMovement > -0.55f)
      h = -0.5f;
    else if (horizontalMovement < -0.55f)
      h = -1f;
    else
      h = 0;
    #endregion

    if(isSprinting)
    {
      v = 2;
      h = horizontalMovement;
    }

    animator.SetFloat(vertical, v, 0.1f, Time.deltaTime);
    animator.SetFloat(horizontal, h, 0.1f, Time.deltaTime);
  }  

  public void ActivateRotation()
  {
    CanRotate = true;
  }

  public void StopRotation()
  {
    CanRotate = false;
  }

  public void EnableCombo()
  {
    animator.SetBool("canDoCombo", true);
  }

  public void DisableCombo()
  {
    animator.SetBool("canDoCombo", false);
  }

  public void EnableIsInvulnerable()
  {
    animator.SetBool("isInvulnerable", true);
  }

  public void DisableIsInvulnerable()
  {
    animator.SetBool("isInvulnerable", false);
  }

  public override void TakeCriticalDamageAnimationEvent()
  {
    playerStats.TakeDamageWithoutAnimation(playerManager.pendingCriticalDamage);
    playerManager.pendingCriticalDamage = 0;
  }

  private void OnAnimatorMove()
  {
    if (playerManager.isInteracting == false) return;

    float delta = Time.deltaTime;

    playerRigidbody.drag = 0;

    Vector3 deltaPos = animator.deltaPosition;
    deltaPos.y = 0;
    Vector3 velocity = deltaPos / delta;

    playerRigidbody.velocity = velocity;
  }
}