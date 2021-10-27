using ItemSlots;
using UnityEngine;
using UnityEditor;

//[CustomEditor(typeof(AccessoryManager))]

public class AccessoryInspector : Editor
{
    private int currentID = 101;
    public Slot currentSlot = Slot.Head;

    private void Awake()
    {
        
    }

    public override void OnInspectorGUI()
    {
        //base.OnInspectorGUI();

        AccessoryManager manager = (AccessoryManager) target;

        
        GUILayout.BeginHorizontal();
        if (GUILayout.Button("<"))
        {
            currentID--;
            manager.EquipItem(currentID);
        }
        if (EditorGUILayout.DropdownButton(GUIContent.none, FocusType.Passive))
        {
            // GenericMenu men = new GenericMenu();
            // men.AddItem(new GUIContent("Head"), currentSlot == Slot.Head, OnItemSelected, Color.white);
            // men.AddItem(new GUIContent("Feet"), currentSlot == Slot.Feet, OnItemSelected, Color.white);
            // men.ShowAsContext();
            
            
        }
        if (GUILayout.Button(">"))
        {
            currentID++;
            manager.EquipItem(currentID);
        }
        GUILayout.EndHorizontal();
    }

    void OnItemSelected(object slot)
    {
        currentSlot = (Slot)slot;
    }
}
