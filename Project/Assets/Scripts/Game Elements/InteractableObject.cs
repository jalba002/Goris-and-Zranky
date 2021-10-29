using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Events;

public class InteractableObject : MonoBehaviour
{

    public enum InteractionType
    {
        OnlyOnce,
        Lever,
        Button
    }

    public InteractionType type;
    
    [Title("Events")]
    public UnityEvent OnInteractionStart = new UnityEvent();
    public UnityEvent OnInteractionEnd = new UnityEvent();
    
    public void Interact()
    {
        switch (type)
        {
            case InteractionType.OnlyOnce:
                
                break;
            case InteractionType.Lever:
                
                break;
            case InteractionType.Button:
                
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }
}
