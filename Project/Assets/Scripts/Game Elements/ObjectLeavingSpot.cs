using UnityEngine;
using UnityEngine.Events;

public class ObjectLeavingSpot : MonoBehaviour
{
    // Cuando el objeto se completa por contacto, se desactiva el asociado y se activa el render.
    // Luego se destruye el antiguo y este script maybe. (Mejor no)
    [Tooltip("Here goes the object that will appear when the requirement has been completed.")] 
    public MeshRenderer _attachedObjectRenderer;
    
    public PickableObject requiredObj;
    public UnityEvent OnCompletion = new UnityEvent(); 
    
    private void OnTriggerEnter(Collider other)
    {
        if (requiredObj == null) return;
        // si el objeto colisionado es el correcto, desactiva el antiguo y activa el nuevo.
        if (other.gameObject == requiredObj.gameObject)
        {
            requiredObj.Deactivate();
            _attachedObjectRenderer.enabled = true;
            OnCompletion.Invoke();
        }
    }
}
