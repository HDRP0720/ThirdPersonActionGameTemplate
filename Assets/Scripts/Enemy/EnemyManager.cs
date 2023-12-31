using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyManager : CharacterManager
{
  [Header("# Enemy Settings")]
  public float detectionRadius = 20f;
  public float minimumDetectionAngle = -50f;
  public float maximumDetetionAngle = 50f;

  public float maximumAggroRadius = 5f;
  public float rotationSpeed = 15f;

  public float currentRecoveryTime = 0;

  [Header("# Current State")]
  public State currentState;

  [HideInInspector] public NavMeshAgent navMeshAgent;

  public CharacterStatsManager currentTarget;
  public bool isPerformingAction;

  [Header("# Enemy Combat Settings")]
  public bool allowEnemyToPerformCombos;
  public bool isPhaseShifting;
  public float comboChancePercentage;

  [HideInInspector] public Rigidbody enemyRigidBody;

  private EnemyLocomotionManager enemyMoveState;
  private EnemyAnimatorManager enemyAnimatorManager;
  private EnemyStatsManager enemyStatsManager;

  private void Awake() 
  {
    enemyRigidBody = GetComponent<Rigidbody>();
    enemyMoveState = GetComponent<EnemyLocomotionManager>();
    enemyAnimatorManager = GetComponent<EnemyAnimatorManager>();
    enemyStatsManager = GetComponent<EnemyStatsManager>();
    backStabCollider = GetComponentInChildren<SpecialAttackCollider>();

    navMeshAgent = GetComponentInChildren<NavMeshAgent>();
    navMeshAgent.enabled = false;
  }
  private void Start()
  {
    enemyRigidBody.isKinematic = false;
  }
  private void Update() 
  {
    HandleRecoveryTimer();
    HandleStateMachine();

    isRotatingWithRootMotion = enemyAnimatorManager.animator.GetBool("isRotatingWithRootMotion");
    isInvulnerable = enemyAnimatorManager.animator.GetBool("isInvulnerable");
    isInteracting = enemyAnimatorManager.animator.GetBool("isInteracting");
    isPhaseShifting = enemyAnimatorManager.animator.GetBool("isPhaseShifting");
    canRotate = enemyAnimatorManager.animator.GetBool("canRotate");
    canDoCombo = enemyAnimatorManager.animator.GetBool("canDoCombo");

    enemyAnimatorManager.animator.SetBool("isDead", enemyStatsManager.isDead);
  }
  private void LateUpdate()
  {
    navMeshAgent.transform.localPosition = Vector3.zero;
    navMeshAgent.transform.localRotation = Quaternion.identity;
  }

  private void HandleStateMachine()
  {
    if(currentState != null)
    {
      State nextState = currentState.Tick(this, enemyStatsManager, enemyAnimatorManager);
      if(nextState != null)
      {
        SwitchToNextState(nextState);
      }
    }
  }
  private void SwitchToNextState(State newState)
  {
    currentState = newState;
  }

  private void HandleRecoveryTimer()
  {
    if(currentRecoveryTime > 0)    
      currentRecoveryTime -= Time.deltaTime;    

    if(isPerformingAction)
    {
      if(currentRecoveryTime <= 0)      
        isPerformingAction = false;      
    }
  }
}
