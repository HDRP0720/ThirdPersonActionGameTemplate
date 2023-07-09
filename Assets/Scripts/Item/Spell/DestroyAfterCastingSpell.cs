using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyAfterCastingSpell : MonoBehaviour
{
  CharacterManager chracterCastingSpell;

  private void Awake() 
  {
    chracterCastingSpell = GetComponentInParent<CharacterManager>();
  }
  private void Update() 
  {
    if(chracterCastingSpell.isFiringSpell)
    {
      Destroy(gameObject);
    }
  }
}
