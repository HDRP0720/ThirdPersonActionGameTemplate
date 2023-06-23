using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimatorHandler : MonoBehaviour
{
  public Animator animator;
  public InputHandler inputHandler;
  public Rigidbody playerRigidbody;

  int vertical;
  int horizontal;

  public bool CanRotate;

  public void Init()
  {
    animator = GetComponent<Animator>();
    inputHandler = GetComponent<InputHandler>();
    playerRigidbody = GetComponent<Rigidbody>();

    vertical = Animator.StringToHash("Vertical");
    horizontal = Animator.StringToHash("Horizontal");
  }

  public void UpdateAnimatorValue(float verticalMovement, float horizontalMovement)
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

    animator.SetFloat(vertical, v, 0.1f, Time.deltaTime);
    animator.SetFloat(horizontal, h, 0.1f, Time.deltaTime);
  }

  public void PlayTargetAnimation(string targetAnim, bool isInteracting)
  {
    animator.applyRootMotion = isInteracting;
    animator.SetBool("isInteracting", isInteracting);
    animator.CrossFade(targetAnim, 0.2f);
  }

  public void ActivateRotation()
  {
    CanRotate = true;
  }

  public void StopRotation()
  {
    CanRotate = false;
  }

  private void OnAnimatorMove()
  {
    if (inputHandler.isInteracting == false) return;

    float delta = Time.deltaTime;

    playerRigidbody.drag = 0;

    Vector3 deltaPos = animator.deltaPosition;
    deltaPos.y = 0;
    Vector3 velocity = deltaPos / delta;

    playerRigidbody.velocity = velocity;
  
  }
}
