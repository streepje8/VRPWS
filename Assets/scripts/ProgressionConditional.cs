using UnityEditor;
using UnityEngine;


[CreateAssetMenu(fileName = "New Conditional", menuName = "StoryManager/Progress Conditional", order = 1)]
public class ProgressionConditional : ScriptableObject
{
    public string ConditionalName = "new conditional";
    public bool ActivateDialogue;
    public int TriggerDialogue = 0;
    public bool isTriggered = false;

    public void Trigger()
    {
        isTriggered = true;
    }

    public void Toggle()
    {
        isTriggered = !isTriggered;
    }

    public void Reset()
    {
        isTriggered = false;
    }

}


[CustomEditor(typeof(ProgressionConditional))]
public class ProgressionEditor : Editor
{
    override public void OnInspectorGUI()
    {
        var myScript = target as ProgressionConditional;

        GUILayout.Label("Enter The Conditional name");
        myScript.ConditionalName = GUILayout.TextField(myScript.ConditionalName);
        myScript.ActivateDialogue = GUILayout.Toggle(myScript.ActivateDialogue, "Activate Dialogue");
        myScript.isTriggered = GUILayout.Toggle(myScript.isTriggered, "Debug toggle");

        if (myScript.ActivateDialogue)
            myScript.TriggerDialogue = EditorGUILayout.IntSlider("Dialogue ID:", myScript.TriggerDialogue, 0, 100);

        if (GUILayout.Button("Save"))
        {
            EditorUtility.SetDirty(myScript);
            AssetDatabase.SaveAssets();
        }
    }
}
