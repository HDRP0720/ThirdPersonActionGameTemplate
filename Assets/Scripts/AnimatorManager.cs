using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimatorManager : MonoBehaviour
{
  [HideInInspector]
  public Animator animator;
  public bool canRotate;

  public void PlayTargetAnimation(string targetAnim, bool isInteracting)
  {
    animator.applyRootMotion = isInteracting;
    animator.SetBool("canRotate", false);
    animator.SetBool("isInteracting", isInteracting);
    animator.CrossFade(targetAnim, 0.2f);
  }

  public virtual void TakeCriticalDamageAnimationEvent()
  {
    
  }
}