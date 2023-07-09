using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "TPA/Spells/Make New Projectile Spell")]
public class ProjectileSpell : SpellItem
{
  public float baseDamage;
  public float projectileVelocity;

  private Rigidbody rigidbody;

  public override void AttemptToCastSpell(PlayerAnimatorManager animatorHandler, PlayerStats playerStats, WeaponSlotManager weaponSlotManager)
  {
    base.AttemptToCastSpell(animatorHandler, playerStats, weaponSlotManager);

    GameObject warmUpSpellVFX = Instantiate(spellWarmUpFX, weaponSlotManager.rightHandSlot.transform);
    // warmUpSpellVFX.gameObject.transform.localScale = new Vector3(2, 2, 2);
    animatorHandler.PlayTargetAnimation(spellAnimation, true);
  }

  public override void SucessfullyCastSpell(PlayerAnimatorManager animatorHandler, PlayerStats playerStats)
  {
    base.SucessfullyCastSpell(animatorHandler, playerStats);
  }
}
