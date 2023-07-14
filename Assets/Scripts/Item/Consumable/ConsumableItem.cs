using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConsumableItem : Item
{
  [Header("Item Quantity")]
  public int maxItemAmount;
  public int currentItemAmount;

  [Header("Item Prefab")]
  public GameObject itemPrefab;

  [Header("Animations")]
  public string consumeAnimationName;
  public bool isInteracting;

  public virtual void AttemptToConsumeItem(PlayerAnimatorManager playerAnimatorManager, WeaponSlotManager weaponSlotManager, PlayerEffectsManager playerEffectsManager)
  {
    if(currentItemAmount > 0)
    {
      playerAnimatorManager.PlayTargetAnimation(consumeAnimationName, isInteracting, true);
    }
    else
    {
      playerAnimatorManager.PlayTargetAnimation("Shrug", true);
    }
  }
}
