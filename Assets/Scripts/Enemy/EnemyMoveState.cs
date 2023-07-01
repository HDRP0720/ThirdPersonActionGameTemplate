using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyMoveState : MonoBehaviour
{
  public CharacterStats currentTarget;
  public LayerMask detectionLayer;

  public float rotationSpeed = 15f;
  public float distanceFromTarget;
  public float stoppingDistance = 1f;

  private EnemyManager enemyManager;
  private EnemyAnimatorManager enemyAnimatorManager;
  private NavMeshAgent navMeshAgent;
  private Rigidbody enemyRigidBody;

  private void Awake() 
  {
    enemyManager = GetComponent<EnemyManager>();
    enemyAnimatorManager = GetComponent<EnemyAnimatorManager>();
    navMeshAgent = GetComponentInChildren<NavMeshAgent>();
    enemyRigidBody = GetComponent<Rigidbody>();
  }
  private void Start() 
  {
    navMeshAgent.enabled = false;
    enemyRigidBody.isKinematic = false;
  }
  
  public void HandleDetection()
  {
    Collider[] colliders = Physics.OverlapSphere(transform.position, enemyManager.detectionRadius, detectionLayer);
    for (int i = 0; i < colliders.Length; i++)
    {
      CharacterStats characterStats = colliders[i].transform.GetComponent<CharacterStats>();
      if(characterStats != null)
      {
        Vector3 targetDirection = characterStats.transform.position - transform.position;
        float viewableAngle = Vector3.Angle(targetDirection, transform.forward);
        if(viewableAngle > enemyManager.minimumDetectionAngle && viewableAngle < enemyManager.maximumDetetionAngle)
        {
          currentTarget = characterStats;
        }
      }
    }
  }

  public void HandleMoveToTarget()
  {
    if(enemyManager.isPerformingAction) return;
    
    Vector3 targetDirection = currentTarget.transform.position - transform.position;
    distanceFromTarget = Vector3.Distance(currentTarget.transform.position, transform.position);
    float viewableAngle = Vector3.Angle(targetDirection, transform.forward);

    if(enemyManager.isPerformingAction)
    {
      enemyAnimatorManager.animator.SetFloat("Vertical", 0, 0.1f, Time.deltaTime);
      navMeshAgent.enabled = false;
    }
    else
    {
      if(distanceFromTarget > stoppingDistance)
      {
        enemyAnimatorManager.animator.SetFloat("Vertical", 1, 0.1f, Time.deltaTime);
      }
      else if(distanceFromTarget <= stoppingDistance)
      {
        enemyAnimatorManager.animator.SetFloat("Vertical", 0, 0.1f, Time.deltaTime);
      }
    }

    HandleRotateTowardsTarget();

    navMeshAgent.transform.localPosition = Vector3.zero;
    navMeshAgent.transform.localRotation = Quaternion.identity;
  }
  private void HandleRotateTowardsTarget()
  {
    if(enemyManager.isPerformingAction)
    {
      // Rotate manually
      Vector3 direction = currentTarget.transform.position - transform.position;
      direction.y = 0;
      direction.Normalize();

      if(direction == Vector3.zero)
        direction = transform.forward;

      Quaternion targetRotation = Quaternion.LookRotation(direction);
      transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed / Time.deltaTime);
    }
    else
    {
      // Rotate with pathfinding (navmesh)
      Vector3 relativeDirection = transform.InverseTransformDirection(navMeshAgent.desiredVelocity);
      Vector3 targetVelocity = enemyRigidBody.velocity;

      navMeshAgent.enabled = true;
      navMeshAgent.SetDestination(currentTarget.transform.position);
      enemyRigidBody.velocity = targetVelocity;
      transform.rotation = Quaternion.Slerp(transform.rotation, navMeshAgent.transform.rotation, rotationSpeed / Time.deltaTime);
    }
  }
}
