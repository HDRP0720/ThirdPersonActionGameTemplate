using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimatorHandler : MonoBehaviour
{
  private Animator animator;

  int vertical;
  int horizontal;

  public bool CanRotate;

  public void Init()
  {
    animator = GetComponent<Animator>();
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

  public void ActivateRotation()
  {
    CanRotate = true;
  }

  public void StopRotation()
  {
    CanRotate = false;
  }
}
