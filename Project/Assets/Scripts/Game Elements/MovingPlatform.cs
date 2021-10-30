using System;
using System.Collections;
using System.Collections.Generic;
using Interfaces;
using Player;
using UnityEngine;
// ReSharper disable InconsistentNaming

[RequireComponent(typeof(Rigidbody))]
public class MovingPlatform : MonoBehaviour, IPlayerCollide
{
    public enum PlatformType
    {
        Standard = 0,
        Trigger = 1
    }

    public PlatformType platformType;
    public List<Transform> m_PatrolPositions;
    
    public float m_MaxSpeed;
    public float timeBetweenStops;
    Vector3 movementDirection;
    Vector3 destination;
    public float distanceToReachPosition = 0.1f;

    public bool moveToNextPosition = false;

    Rigidbody m_RigidBody;
    private BoxCollider _boxCollider;
    private PlayerController player;
    public Mesh Mesh;

    private Plane platformPlane;
    private IEnumerator platformMovement;

    void Awake()
    {
        m_RigidBody = GetComponent<Rigidbody>();
        _boxCollider = GetComponent<BoxCollider>();
        CreatePlane();
    }

    private void Start()
    {
        destination = m_PatrolPositions[0].transform.position;
        EnablePlatform();
    }
    
    private void CreatePlane()
    {
        var transform1 = transform;
        platformPlane = new Plane(transform1.up, transform1.position);
    }

    private void EnablePlatform()
    {
        switch(platformType)
        {
            case PlatformType.Standard:
                movementDirection = (destination - transform.position).normalized;
                moveToNextPosition = true;
                platformMovement = PlatformMovement();
                StartCoroutine(platformMovement);
                break;
            case PlatformType.Trigger:
                // Do nothing.
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }
    
    private IEnumerator PlatformMovement()
    {
        while (this.enabled)
        {
            if ((Vector3.Distance(transform.position, destination) < distanceToReachPosition))
            {
                movementDirection = Vector3.zero;
                moveToNextPosition = false;
                IncreaseDestination();
                movementDirection = (destination - transform.position).normalized;
                yield return new WaitForSecondsRealtime(timeBetweenStops);
                moveToNextPosition = true;
            }
            movementDirection = (destination - transform.position).normalized;

            yield return null;
        }
        platformMovement = null;
    }

    void FixedUpdate()
    {
        if (moveToNextPosition)
        {
            m_RigidBody.MovePosition(transform.position + movementDirection * (m_MaxSpeed * Time.fixedDeltaTime));
        }
    }

    private void IncreaseDestination()
    {
        int idx = m_PatrolPositions.FindIndex(x => x.transform.position == destination);
        idx++;
        if (idx >=  m_PatrolPositions.Count)
        {
            idx = 0;
        }

        destination = m_PatrolPositions[idx].transform.position;
    }
    
    public void OnDrawGizmos()
    {
        for (int i = 0; i < m_PatrolPositions.Count; i++)
        {
            Gizmos.DrawLine(this.gameObject.transform.position, m_PatrolPositions[i].transform.position);
            Gizmos.DrawWireMesh(Mesh, m_PatrolPositions[i].transform.position, Quaternion.identity, transform.localScale);
        }
    }
    
    #region Interfaces

    public bool Collide(GameObject self, Vector3 collisionPoint)
    {
        if (!player) 
            player = self.GetComponent<PlayerController>();
        CreatePlane();
        if (platformPlane.GetSide(player.transform.position))
            player.SetInertia(movementDirection * m_MaxSpeed);
        return true;
    }

    public bool CollideTop()
    {
        // 
        return false;
    }

    public void StopColliding()
    {
        // throw new NotImplementedException();
        moveToNextPosition = true;
    }

    #endregion
}