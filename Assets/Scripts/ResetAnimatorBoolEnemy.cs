using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResetAnimatorBoolEnemy : ResetAnimatorBool
{
  public string isPhaseShifting = "isPhaseShifting";
  public bool isPhaseShiftingStatus = false;

  public override void OnStateEnter(Animator animator, AnimatorStateInfo animatorStateInfo, int layerIndex)
  {
    base.OnStateEnter(animator, animatorStateInfo, layerIndex);
    animator.SetBool(isPhaseShifting, isPhaseShiftingStatus);
  }
}
