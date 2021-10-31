using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Events;

public class FrankensteinLeavingSpot : MonoBehaviour
{
    // Cuando el objeto se completa por contacto, se desactiva el asociado y se activa el render.
    // Luego se destruye el antiguo y este script maybe. (Mejor no)
    [Tooltip("Here goes the object that will appear when the requirement has been completed.")]
    public MeshRenderer _attachedObjectRenderer;

    public Transform _attachedTransform;

    public bool useItemMesh = false;

    [Title("Settings")] public int activatableTimes = 1;
    private int timesActivated = 0;

    public UnityEvent OnCompletion = new UnityEvent();

    private void Start()
    {
        _attachedObjectRenderer = GetComponent<MeshRenderer>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (timesActivated >= activatableTimes && activatableTimes != -1) return;
        var item = other.gameObject.GetComponent<PickableObject>();
        // si el objeto colisionado es el correcto, desactiva el antiguo y activa el nuevo.
        if (item == null) return;

        if (useItemMesh)
        {
            InstantiateMeshRenderer(item);
        }
        else
        {
            item.Deactivate();
            _attachedObjectRenderer.enabled = true;
        }

        timesActivated++;
        OnCompletion.Invoke();
    }

    private void InstantiateMeshRenderer(PickableObject item)
    {
        var go = item.gameObject;
        item.Inert();
        Destroy(item);
        go.transform.SetPositionAndRotation(
            _attachedTransform.transform.position,
            _attachedTransform.transform.rotation);
        go.transform.parent = this.transform;
    }
}