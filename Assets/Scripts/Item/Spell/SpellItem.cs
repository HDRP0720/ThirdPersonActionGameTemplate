using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpellItem : Item
{
  public GameObject spellWarmUpFX;
  public GameObject spellCastFX;

  public string spellAnimation;

  [Header("# Spell Type")]
  public bool isFaithSpell;
  public bool isMagicSpell;
  public bool isPyroSpell;

  [Header("# Spell Description")]
  [TextArea] public string spellDescription;

  public virtual void AttemptToCastSpell(AnimatorHandler animatorHandler, PlayerStats playerStats)
  {
    Debug.Log("You attempt to cast a spell");
  }

  public virtual void SucessfullyCastSpell(AnimatorHandler animatorHandler, PlayerStats playerStats)
  {
    Debug.Log("You successfully cast a spell");
  }
}