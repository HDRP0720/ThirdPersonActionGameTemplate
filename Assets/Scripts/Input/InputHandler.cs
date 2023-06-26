using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputHandler : MonoBehaviour
{
  public float horizontal;
  public float vertical;
  public float moveAmount;
  public float mouseX;
  public float mouseY;

  public bool b_Input;
  public bool rb_Input;
  public bool rt_Input;

  public bool d_Pad_Up;
  public bool d_Pad_Down;  
  public bool d_Pad_Left;
  public bool d_Pad_Right;

  public bool rollFlag;
  public bool sprintFlag;
  public bool comboFlag;
  public float rollInputTimer;

  private PlayerControls inputActions;  
  private PlayerManager playerManager;
  private PlayerAttackState playerAttackState;
  private PlayerInventory playerInventory;

  private Vector2 movementInput;
  private Vector2 cameraInput;

  private void Awake() 
  {   
    playerManager = GetComponent<PlayerManager>();
    playerAttackState = GetComponent<PlayerAttackState>();
    playerInventory = GetComponent<PlayerInventory>();
  }

  private void OnEnable() 
  {
    if(inputActions == null)
    {
      inputActions = new PlayerControls();
      inputActions.PlayerMovement.Movement.performed += inputActions => movementInput = inputActions.ReadValue<Vector2>();
      inputActions.PlayerMovement.Camera.performed += i => cameraInput = i.ReadValue<Vector2>();
    }

    inputActions.Enable();
  }
  private void OnDisable() 
  {
    inputActions.Disable();
  }

  public void TickInput(float delta)
  {
    MoveInput(delta);
    HandleRollInput(delta);
    HandleAttackInput(delta);
    HandleQuickSlotsInput();
  }

  private void MoveInput(float delta)
  {
    horizontal = movementInput.x;
    vertical = movementInput.y;

    moveAmount = Mathf.Clamp01(Mathf.Abs(horizontal) + Mathf.Abs(vertical));

    mouseX = cameraInput.x;
    mouseY = cameraInput.y;    
  }

  private void HandleRollInput(float delta)
  {
    b_Input = inputActions.PlayerActions.Roll.phase == UnityEngine.InputSystem.InputActionPhase.Performed;

    if(b_Input)
    {
      rollInputTimer += delta;
      sprintFlag = true;
    }
    else
    {
      if(rollInputTimer > 0 && rollInputTimer < 0.5f)
      {
        sprintFlag = false;
        rollFlag = true;
      }

      rollInputTimer = 0;
    }  
  }

  private void HandleAttackInput(float delta)
  {
    inputActions.PlayerActions.RB.performed += i => rb_Input = true;
    inputActions.PlayerActions.RT.performed += i => rt_Input = true;

    // RB Input handles the RIGHT hand weapon's light attack
    if(rb_Input)
    {
      if(playerManager.canDoCombo)
      {
        comboFlag = true;
        playerAttackState.HandleWeaponCombo(playerInventory.rightWeapon);
        comboFlag = false;
      }
      else
      {
        if(playerManager.isInteracting) return;

        if(playerManager.canDoCombo) return;

        playerAttackState.HandleLightAttack(playerInventory.rightWeapon);
      }  
    }

    // RT Input handles the RIGHT hand weapon's heavy attack
    if(rt_Input)
    {
      if (playerManager.isInteracting) return;

      if (playerManager.canDoCombo) return;

      playerAttackState.HandleHeavyAttack(playerInventory.rightWeapon);
    }
  }

  private void HandleQuickSlotsInput()
  {
    // inputActions.PlayerQuickslots.DPadUp.performed += i => d_Pad_Up = true;
    // inputActions.PlayerQuickslots.DPadDown.performed += i => d_Pad_Down = true;
    inputActions.PlayerQuickslots.DPadLeft.performed += i => d_Pad_Left = true;
    inputActions.PlayerQuickslots.DPadRight.performed += i => d_Pad_Right = true;

    if(d_Pad_Right)
    {
      playerInventory.ChangeRightWeapon();
    }
    else if(d_Pad_Left)
    {
      playerInventory.ChangeLeftWeapon();
    }
  }
}