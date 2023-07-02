using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyManager : CharacterManager
{
  [Header("Enemy Settings")]
  public float detectionRadius = 20f;
  public float minimumDetectionAngle = -50f;
  public float maximumDetetionAngle = 50f;

  public float maximumAttackRange = 1.5f;
  public float rotationSpeed = 15f;

  public float currentRecoveryTime = 0;

  public State currentState;

  [HideInInspector] public NavMeshAgent navMeshAgent;

  public CharacterStats currentTarget;
  public bool isPerformingAction;
  public bool isInteracting;

  [HideInInspector] public Rigidbody enemyRigidBody;

  private EnemyMoveState enemyMoveState;
  private EnemyAnimatorManager enemyAnimatorManager;
  private EnemyStats enemyStats;

  private void Awake() 
  {
    enemyRigidBody = GetComponent<Rigidbody>();
    enemyMoveState = GetComponent<EnemyMoveState>();
    enemyAnimatorManager = GetComponent<EnemyAnimatorManager>();
    enemyStats = GetComponent<EnemyStats>();
    navMeshAgent = GetComponentInChildren<NavMeshAgent>();
    navMeshAgent.enabled = false;
  }
  private void Start()
  {
    enemyRigidBody.isKinematic = false;
  }
  private void FixedUpdate() 
  {
    HandleStateMachine();
  }
  private void Update() 
  {
    HandleRecoveryTimer();

    isInteracting = enemyAnimatorManager.animator.GetBool("isInteracting");
  }

  private void HandleStateMachine()
  {
    if(currentState != null)
    {
      State nextState = currentState.Tick(this, enemyStats, enemyAnimatorManager);
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
