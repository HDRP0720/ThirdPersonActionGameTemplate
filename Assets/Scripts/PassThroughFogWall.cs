using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PassThroughFogWall : Interactable
{
  WorldEventManager worldEventManager;

  private void Awake() 
  {
    worldEventManager = FindObjectOfType<WorldEventManager>();
  }
  private void Update() {
    Debug.Log(Vector3.forward);
  }

  public override void Interact(PlayerManager playerManager)
  {
    base.Interact(playerManager);
    playerManager.PassThroughFogWallInteraction(transform);
    worldEventManager.ActivateBossFight();
  }

}