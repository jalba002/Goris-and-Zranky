using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.PlayerLoop;

public class SimpleIA : RestartableObject
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
    public float attackRange = 0.2f;
    public float brainUpdate = 0.5f;
    public float detectionRadius = 5f;

    private NavMeshAgent agent;
    private IEnumerator updateDestination;

    public new void Awake()
    {
        base.Awake();
        agent = GetComponent<NavMeshAgent>();
    }

    private void Start()
    {
        //if (destination == null) return;
        if(updateDestination!=null) StopCoroutine(updateDestination);
        updateDestination = DestinationUpdater();
        StartCoroutine(updateDestination);
    }

    private IEnumerator DestinationUpdater()
    {
        yield return null;
        while (enabled)
        {
            if (Utils.Utils.DistanceBetween(this.gameObject, GameManager.GM.GetPlayerGO()) < detectionRadius && !agent.isPathStale)
            {
                agent.SetDestination(destination.position);
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