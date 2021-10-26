using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IPickable
{
    void Connect(Rigidbody rb);
    void Disconnect();
    void Throw(Vector3 direction);
    int GetStrengthRequirement();
    bool isEnabled();

    Rigidbody GetConnectedRB();
}
