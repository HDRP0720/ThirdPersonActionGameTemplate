using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
  private InputHandler inputHandler;
  private Animator animator;

  private void Start() 
  {
    inputHandler = GetComponent<InputHandler>();
    animator = GetComponent<Animator>();
  }
  private void Update() 
  {
    inputHandler.isInteracting = animator.GetBool("isInteracting");
    inputHandler.rollFlag = false;
    inputHandler.sprintFlag = false;
  }
}
