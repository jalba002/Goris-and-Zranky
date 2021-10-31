using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class MultipleObjectsRequired : RestartableObject
{
    public bool acceptItems = true;

    // Cuando el objeto se completa por contacto, se desactiva el asociado y se activa el render.
    // Luego se destruye el antiguo y este script maybe. (Mejor no)
    [Tooltip("Here goes the object that will appear when the requirement has been completed.")]
    public List<MeshRenderer> renderedObjects;

    // public MeshRenderer _attachedObjectRenderer;
    private int idx = 0;

    public List<PickableObject> triggerObjects;

    // public FillerObjectScript requiredObj;
    public UnityEvent OnCompletion = new UnityEvent();

    private void Start()
    {
        ToggleVisualMeshes(false);
    }

    protected void ToggleVisualMeshes(bool enable)
    {
        foreach (var item in renderedObjects)
        {
            item.enabled = enable;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        CheckItem(other);
    }

    protected virtual void CheckItem(Collider other)
    {
        if (!acceptItems) return;

        // si el objeto colisionado es el correcto, desactiva el antiguo y activa el nuevo.
        var x = other.gameObject.GetComponent<PickableObject>();
        if (triggerObjects.Contains(x))
        {
            x.Deactivate();
            // triggerObjects.Remove(x);

            if (ToggleListRenderer(idx, true))
                idx++;

            OnCompletion.Invoke();
        }
    }

    private bool ToggleListRenderer(int idx, bool enable)
    {
        if (idx >= renderedObjects.Count) return false;
        renderedObjects[idx].enabled = enable;
        return true;

    }

    public override void Restart()
    {
        base.Restart();
        idx = 0;
    }

    public void AddRequiredItem(PickableObject item)
    {
        triggerObjects.Add(item);
    }
    
    public void ClearList()
    {
        triggerObjects = new List<PickableObject>();   
    }
}