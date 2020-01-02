using UnityEditor;
using UnityEditor.UI;
using UnityEngine;

[CustomEditor(typeof(ExpandAreaImage), true)]
[CanEditMultipleObjects]
public class ExpandAreaImageEditor: ImageEditor
{
    private SerializedProperty Xfactor;
    private SerializedProperty Yfactor;
    GUIContent XfactorContent;
    GUIContent YfactorContent;

    protected override void OnEnable()
    {
        base.OnEnable();
        XfactorContent = EditorGUIUtility.TrTextContent("XFactor");
        YfactorContent = EditorGUIUtility.TrTextContent("YFactor");
        this.Xfactor = this.serializedObject.FindProperty("Xfactor");
        this.Yfactor = this.serializedObject.FindProperty("Yfactor");
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();
        EditorGUILayout.PropertyField(Xfactor);
        EditorGUILayout.PropertyField(Yfactor);
        serializedObject.ApplyModifiedProperties();
        
        

        base.OnInspectorGUI();
    }
}
