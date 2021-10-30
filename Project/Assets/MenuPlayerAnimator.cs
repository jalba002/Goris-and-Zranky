using Player;
using UnityEngine;

public class MenuPlayerAnimator : MonoBehaviour
{
    private PlayerController pc;
    private ObjectPicker _pickupZone;
    private Animator animator;

    // Start is called before the first frame update
    void Awake()
    {
        pc = GetComponent<PlayerController>();
        _pickupZone = GetComponentInChildren<ObjectPicker>();
        animator = GetComponentInChildren<Animator>();
    }

    public void StartWalk()
    {
        animator.SetFloat("Movement", 15f);
    }
    public void StartIdle()
    {
        animator.SetFloat("Movement", 0f);
    }
    public void StartSpin()
    {
        animator.SetTrigger("Spin");
    }
}
