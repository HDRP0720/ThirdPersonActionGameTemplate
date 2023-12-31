using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "TPA/Spells/Make New Healing Spell")]
public class HealingSpell : SpellItem
{
  public int healAmount;

  public override void AttemptToCastSpell(PlayerAnimatorManager animatorHandler, PlayerStatsManager playerStats, PlayerWeaponSlotManager weaponSlotManager)
  {
    base.AttemptToCastSpell(animatorHandler, playerStats, weaponSlotManager);

    GameObject warmUpSpellVFX = Instantiate(spellWarmUpFX, animatorHandler.transform);
    animatorHandler.PlayTargetAnimation(spellAnimation, true);
    Debug.Log("Attempting to cast heal spell...");
  }

  public override void SucessfullyCastSpell(PlayerAnimatorManager animatorHandler, PlayerStatsManager playerStats, CameraHandler cameraHandler, PlayerWeaponSlotManager weaponSlotManager)
  {
    base.SucessfullyCastSpell(animatorHandler, playerStats, cameraHandler, weaponSlotManager);
    
    GameObject spellVFX = Instantiate(spellCastFX, animatorHandler.transform);
    playerStats.HealPlayer(healAmount);
    Debug.Log("Cast heal spell successfully");
  }
}