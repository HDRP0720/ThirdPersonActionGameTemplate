using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpellItem : Item
{
  public GameObject spellWarmUpFX;
  public GameObject spellCastFX;

  public string spellAnimation;

  [Header("# Mana Cost")]
  public int manaCost;

  [Header("# Spell Type")]
  public bool isFaithSpell;
  public bool isMagicSpell;
  public bool isPyroSpell;

  [Header("# Spell Description")]
  [TextArea] public string spellDescription;

  public virtual void AttemptToCastSpell(PlayerAnimatorManager animatorHandler, PlayerStats playerStats)
  {
    Debug.Log("You attempt to cast a spell");

  }

  public virtual void SucessfullyCastSpell(PlayerAnimatorManager animatorHandler, PlayerStats playerStats)
  {
    Debug.Log("You successfully cast a spell");
    playerStats.DeductMana(manaCost);
  }
}
