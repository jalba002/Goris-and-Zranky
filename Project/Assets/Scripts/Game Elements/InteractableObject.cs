using System;
using System.Collections;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Events;

public class InteractableObject : MonoBehaviour
{

    public enum InteractionType
    {
        OnlyOnce,
        Lever,
        TimedButton
    }

    [Title("Settings")]
    public InteractionType type;

    //[EnableIf("type == InteractionType.TimedButton")]
    public float buttonActivationDuration = 5f;
    
    [Title("Events")]
    public UnityEvent OnEnableInteraction = new UnityEvent();
    public UnityEvent OnDisableInteraction = new UnityEvent();

    private bool IsInteractable = true;
    
    private IEnumerator coHolder;

    private void Start()
    {
        coHolder = TimedButton();
    }

    public bool Interact()
    {
        switch (type)
        {
            case InteractionType.OnlyOnce:
                if (!IsInteractable) return false;
                Activate(true);
                IsInteractable = false;
                break;
            case InteractionType.Lever:
                Activate(IsInteractable);
                IsInteractable = !IsInteractable;
                break;
            case InteractionType.TimedButton:
                if (!IsInteractable) return false;
                IsInteractable = false;
                if (coHolder != null)
                    StopCoroutine(coHolder);
                else
                {
                    coHolder = TimedButton();
                }
                StartCoroutine(coHolder);
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }

        return true;
    }

    private void Activate(bool enable)
    {
        if(enable) 
            OnEnableInteraction.Invoke();
        else
        {
            OnDisableInteraction.Invoke();
        }
    }

    IEnumerator TimedButton()
    {
        OnEnableInteraction.Invoke();
        yield return new WaitForSeconds(buttonActivationDuration);
        OnDisableInteraction.Invoke();
        coHolder = null;
        IsInteractable = true;
    }
}
