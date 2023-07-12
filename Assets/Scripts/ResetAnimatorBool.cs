using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResetAnimatorBool : StateMachineBehaviour
{
  public string isInvulnerable = "isInvulnerable";
  public bool isInvulnerableStatus = false;

  public string isInteractingBool = "isInteracting";
  public bool isInteractingStatus = false;

  public string isFiringSpellBool = "isFiringSpell";
  public bool isFiringSpellStatus = false;

  public string isRotatingWithRootMotionBool = "isRotatingWithRootMotion";
  public bool isRotatingWithRootMotionStatus = true;

  public string canRotateBool = "canRotate";
  public bool canRotateStatus = true;

  public override void OnStateEnter(Animator animator, AnimatorStateInfo animatorStateInfo, int layerIndex) 
  {
    animator.SetBool(isInvulnerable, isInvulnerableStatus);
    animator.SetBool(isInteractingBool, isInteractingStatus);
    animator.SetBool(isFiringSpellBool, isFiringSpellStatus);
    animator.SetBool(isRotatingWithRootMotionBool, isRotatingWithRootMotionStatus);
    animator.SetBool(canRotateBool, canRotateStatus);
  }
}