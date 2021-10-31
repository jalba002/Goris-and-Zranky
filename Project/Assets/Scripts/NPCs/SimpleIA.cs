using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.PlayerLoop;

public class SimpleIA : RestartableObject
{
    public Transform destination;
    public float attackRange = 0.2f;
    public float brainUpdate = 0.5f;
    
    private NavMeshAgent agent;
    private IEnumerator updateDestination;

    public new void Awake()
    {
        base.Awake();
        agent = GetComponent<NavMeshAgent>();
        updateDestination = DestinationUpdater();
    }

    private void Start()
    {
        if (destination == null) return;

        StartCoroutine(updateDestination);
    }

    private IEnumerator DestinationUpdater()
    {
        while (enabled)
        {
            agent.SetDestination(!agent.isPathStale ? destination.position : startingPosition);
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
        if(destination != null)
            StopCoroutine(updateDestination);
    }
}