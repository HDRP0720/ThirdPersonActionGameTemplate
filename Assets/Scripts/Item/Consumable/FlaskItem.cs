using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "TPA/Items/Make New Flask Item")]
public class FlaskItem : ConsumableItem
{
  [Header("Flask Type")]
  public bool healthFlask;
  public bool manaFlask;

  [Header("Recovery Amount")]
  public int healthRecoverAmount;
  public int manaRecoverAmount;

  [Header("Recovery VFX")]
  public GameObject recoveryVFX;

  public override void AttemptToConsumeItem(PlayerAnimatorManager playerAnimatorManager, PlayerWeaponSlotManager weaponSlotManager, PlayerEffectsManager playerEffectsManager)
  {
    base.AttemptToConsumeItem(playerAnimatorManager, weaponSlotManager, playerEffectsManager);

    weaponSlotManager.leftHandSlot.UnloadWeapon();
    GameObject flask = Instantiate(itemPrefab, weaponSlotManager.leftHandSlot.transform);
    playerEffectsManager.currentVFX = recoveryVFX;
    playerEffectsManager.amountToBeHealed = healthRecoverAmount;
    playerEffectsManager.instaniatedItemPrefab = flask;

  

 
    // TODO: Add health or mana
    // TODO: Instantiate Flask in Hand and Play Drink Animation
    // TODO: Play Recovery VFX When we drink without being hit

  }
}
