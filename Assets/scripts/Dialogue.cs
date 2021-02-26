using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CreateAssetMenu(fileName = "New DialogueProgression", menuName = "StoryManager/Dialogue", order = 1)]
[Serializable]
public class Dialogue : ScriptableObject
{
    [SerializeField]
    public List<DialogueElement> Elements = new List<DialogueElement>();
    private int CurrentElement = 0;
    private bool hasPlayed = false;
    
    public void reset()
    {
        CurrentElement = 0;
        hasPlayed = false;
    }

    public bool hasPlayedBefore()
    {
        return hasPlayed;
    }

    public bool isDone()
    {
        if(CurrentElement > Elements.Count)
        {
            return true;
        }
        hasPlayed = true;
        return false;
    }

    public void nextClip()
    {
        CurrentElement++;
    }

    public AudioClip currentClip()
    {
        hasPlayed = true;
        DialogueElement element = Elements[CurrentElement];
        if(element.isCondition)
        {
            if(element.conditional.isTriggered)
            {
                return element.VoiceLine;
            } else { 
                return element.VoiceLineFalse;
            }
        } else {
            if(element.isDelay)
            {
                return AudioClip.Create("DelayClip", Mathf.RoundToInt(44100 * element.Time), 1, 44100, true);
            } else
            {
                return element.VoiceLine;
            }
        }
        
    }

}


[CustomEditor(typeof(Dialogue))]
public class DialogueEditor : Editor
{
    override public void OnInspectorGUI()
    {
        var myScript = target as Dialogue;
        GUILayout.Label("Set Up A Dialogue");
        List<DialogueElement> toRemove = new List<DialogueElement>();
        foreach (DialogueElement element in myScript.Elements)
        {
            if (element != null) { 
                if (element.isCondition)
                {
                    SerializedObject obj = new SerializedObject(element);
                    EditorGUILayout.PropertyField(obj.FindProperty("conditional"));
                    EditorGUILayout.PropertyField(obj.FindProperty("VoiceLine"));
                    EditorGUILayout.PropertyField(obj.FindProperty("VoiceLineFalse"));
                    obj.ApplyModifiedProperties();
                } else
                {
                    if(element.isDelay)
                    {
                        SerializedObject obj = new SerializedObject(element);
                        GUILayout.Label("Delay:");
                        EditorGUILayout.PropertyField(obj.FindProperty("Time"));
                        obj.ApplyModifiedProperties();
                    } else { 
                        SerializedObject obj = new SerializedObject(element);
                        EditorGUILayout.PropertyField(obj.FindProperty("VoiceLine"));
                        obj.ApplyModifiedProperties();
                    }
                }
            } else
            {
                GUILayout.Label("Null dialogue found! Please reimport all dialogue elements and the dialogue script!");
            }
        }

        if (GUILayout.Button("Add Voiceline"))
        {
            DialogueElement asset = ScriptableObject.CreateInstance<DialogueElement>();
            myScript.Elements.Add(asset);
            Guid guid = Guid.NewGuid();
            string ID = guid.ToString();
            AssetDatabase.CreateAsset(asset, "Assets/Story/DialogueElements/Element" + ID + ".asset");
            AssetDatabase.SaveAssets();

            EditorUtility.FocusProjectWindow();
        }
        if (GUILayout.Button("Add Conditional"))
        {
            DialogueElement asset = ScriptableObject.CreateInstance<DialogueElement>();
            asset.isCondition = true;
            myScript.Elements.Add(asset);
            Guid guid = Guid.NewGuid();
            string ID = guid.ToString();
            AssetDatabase.CreateAsset(asset, "Assets/Story/DialogueElements/Element" + ID + ".asset");
            AssetDatabase.SaveAssets();
        }
        if (GUILayout.Button("Add Delay"))
        {
            DialogueElement asset = ScriptableObject.CreateInstance<DialogueElement>();
            asset.isDelay = true;
            myScript.Elements.Add(asset);
            Guid guid = Guid.NewGuid();
            string ID = guid.ToString();
            AssetDatabase.CreateAsset(asset, "Assets/Story/DialogueElements/Element" + ID + ".asset");
            AssetDatabase.SaveAssets();
        }
        if (GUILayout.Button("Remove Last Element (From Dialogue only!)"))
        {
            if (myScript.Elements.Count > 0)
            {
                myScript.Elements.RemoveAt(myScript.Elements.Count - 1);
            }
            AssetDatabase.SaveAssets();
        }
        EditorUtility.SetDirty(myScript);
    }
}
