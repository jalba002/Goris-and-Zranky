using UnityEngine;

public class RestartableObject : MonoBehaviour
{
    protected Vector3 startingPosition;
    protected Quaternion startingRotation;

    protected MeshRenderer meshRenderer;

    public void Awake()
    {
        startingPosition = transform.localPosition;
        startingRotation = transform.localRotation;
        
        meshRenderer = GetComponent<MeshRenderer>();
        if (meshRenderer == null)
            meshRenderer = GetComponentInChildren<MeshRenderer>();
    }

    public virtual void Restart()
    {
        this.gameObject.SetActive(true);
        meshRenderer.enabled = true;
        
        transform.localPosition = startingPosition;
        transform.localRotation = startingRotation;
    }
}
