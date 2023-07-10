using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResestAnimatorBool : StateMachineBehaviour
{
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
    animator.SetBool(isInteractingBool, isInteractingStatus);
    animator.SetBool(isFiringSpellBool, isFiringSpellStatus);
    animator.SetBool(isRotatingWithRootMotionBool, isRotatingWithRootMotionStatus);
    animator.SetBool(canRotateBool, canRotateStatus);
  }
  
  // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
  // override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
  // {
  //   animator.SetBool("isInteracting", false);
  // }
}