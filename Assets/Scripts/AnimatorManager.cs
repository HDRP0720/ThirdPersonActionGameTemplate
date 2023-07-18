using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimatorManager : MonoBehaviour
{
  [HideInInspector]
  public Animator animator;
  public bool canRotate;

  protected CharacterManager characterManager;
  protected CharacterStatsManager characterStatsManager;

  protected virtual void Awake() 
  {
    characterManager = GetComponent<CharacterManager>();
    characterStatsManager = GetComponent<CharacterStatsManager>();
  }

  public void PlayTargetAnimation(string targetAnim, bool isInteracting, bool canRotate = false)
  {
    animator.applyRootMotion = isInteracting;
    animator.SetBool("canRotate", canRotate);
    animator.SetBool("isInteracting", isInteracting);
    animator.CrossFade(targetAnim, 0.2f);
  }

  public void PlayTargetAnimationWithRootRotation(string targetAnim, bool isInteracting, bool canRotate = false)
  {
    animator.applyRootMotion = isInteracting;
    animator.SetBool("isRotatingWithRootMotion", true);
    animator.SetBool("isInteracting", isInteracting);
    animator.CrossFade(targetAnim, 0.2f);
  }

  public virtual void ActivateRotation()
  {
    animator.SetBool("canRotate", true);
  }
  public virtual void StopRotation()
  {
    animator.SetBool("canRotate", false);
  }

  public virtual void EnableCombo()
  {
    animator.SetBool("canDoCombo", true);
  }
  public virtual void DisableCombo()
  {
    animator.SetBool("canDoCombo", false);
  }

  public virtual void EnableIsInvulnerable()
  {
    animator.SetBool("isInvulnerable", true);
  }
  public virtual void DisableIsInvulnerable()
  {
    animator.SetBool("isInvulnerable", false);
  }

  public virtual void EnableIsParrying()
  {
    characterManager.isParrying = true;
  }
  public virtual void DisableIsParrying()
  {
    characterManager.isParrying = false;
  }

  public virtual void EnableCanBeRiposted()
  {
    characterManager.canBeRiposted = true;
  }
  public virtual void DisableCanBeRiposted()
  {
    characterManager.canBeRiposted = false;
  }

  public virtual void TakeCriticalDamageAnimationEvent()
  {
    characterStatsManager.TakeDamageWithoutAnimation(characterManager.pendingCriticalDamage);
    characterManager.pendingCriticalDamage = 0;
  }
}