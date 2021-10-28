﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.PlayerLoop;

public class SimpleIA : MonoBehaviour
{
    public Transform destination;
    private NavMeshAgent agent;
    private IEnumerator updateDestination;

    public void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        updateDestination = DestinationUpdater();
    }

    private void Start()
    {
        StartCoroutine(updateDestination);
    }

    private IEnumerator DestinationUpdater()
    {
        while (enabled)
        {
            agent.SetDestination(destination.position);
            yield return new WaitForSeconds(1f);
        }

        yield return null;
    }

    private void OnEnable()
    {
        StartCoroutine(updateDestination);
    }

    private void OnDisable()
    {
        StopCoroutine(updateDestination);
    }
}
