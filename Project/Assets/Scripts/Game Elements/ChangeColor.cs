using UnityEngine;

public class ChangeColor : MonoBehaviour
{
    public Color myColor = Color.green;
    
    public void ApplyNewColor()
    {
        var lights = GetComponentsInChildren<Light>();
        foreach (var light in lights)
        {
            light.color = myColor;
        }
    }
}
