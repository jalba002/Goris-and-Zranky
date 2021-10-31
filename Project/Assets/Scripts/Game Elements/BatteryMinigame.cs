using System.Collections.Generic;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
using UnityEngine;
using UnityEngine.Events;

public class BatteryMinigame : MonoBehaviour
{
    public bool acceptItems = true;
    // Cuando el objeto se completa por contacto, se desactiva el asociado y se activa el render.
    // Luego se destruye el antiguo y este script maybe. (Mejor no)

    // public MeshRenderer _attachedObjectRenderer;
    private int idx = 0;

    public int requiredObjects = 5;
    public int takenObjects = 0;

    public UnityEvent OnCompletion = new UnityEvent();

    private void OnTriggerEnter(Collider other)
    {
        CheckItem(other);
    }

    protected virtual void CheckItem(Collider other)
    {
        if (!acceptItems) return;

        // si el objeto colisionado es el correcto, desactiva el antiguo y activa el nuevo.
        var x = other.gameObject.GetComponent<BatteryItem>();
        if (x == null) return;
        
        x.Deactivate();
        takenObjects++;
        
        if(takenObjects >= requiredObjects)
            OnCompletion.Invoke();
    }

    [Button("Complete")]
    private void Complete()
    {
        OnCompletion.Invoke();
    }
}