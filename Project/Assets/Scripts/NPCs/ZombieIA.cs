using System;
using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class ZombieIA : RestartableObject
{
    
    private Transform _destination;
    public Transform destination
    {
        get
        {
            if (_destination == null)
                _destination = GameManager.GM.GetPlayerGO().transform;
            return _destination;
        }
        set { _destination = value; }
    }
    
    [Header("Settings")]
    public float attackRadius = 1f;
    public float brainUpdate = 0.5f;
    public float detectionRadius = 5f;

    [Header("Components")]
    private Animator animator;
    private NavMeshAgent agent;
    private IEnumerator updateDestination;

    public new void Awake()
    {
        base.Awake();
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponentInChildren<Animator>();
    }

    private void Start()
    {
        //if (destination == null) return;
        if(updateDestination!=null) StopCoroutine(updateDestination);
        updateDestination = DestinationUpdater();
        StartCoroutine(updateDestination);
    }

    public void Update()
    {
        Vector3 vel = agent.velocity;
        vel.y = 0f;
        animator.SetFloat("Movement", vel.magnitude);
    }

    public void HitReaction()
    {
        animator.SetTrigger("Hit");
    }

    public void Kill()
    {
        animator.SetTrigger("Death");
        StopAllCoroutines();
        agent.enabled = false;
        GetComponent<Collider>().enabled = false;
        this.enabled = false;
    }
    
    private IEnumerator DestinationUpdater()
    {
        yield return null;
        while (enabled)
        {
            float distance = Utils.Utils.DistanceBetween(this.gameObject, GameManager.GM.GetPlayerGO());
            if (distance < detectionRadius && distance > attackRadius && !agent.isPathStale)
            {
                agent.SetDestination(destination.position);
            }
            else if (distance < attackRadius && !agent.isPathStale)
            {
                animator.SetTrigger("Attack");
                agent.isStopped = true;
                //Debug.Log("ATTACK");
            }
            else
            {
                agent.SetDestination(startingPosition);
            }
            yield return new WaitForSeconds(brainUpdate);
        }

        yield return null;
    }

    private void OnEnable()
    {
        Start();
    }

    private void OnDisable()
    {
        //(destination != null)
        StopCoroutine(updateDestination);
    }
}