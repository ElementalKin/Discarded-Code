using UnityEngine;
using UnityEditor;
using UnityEditorInternal;

[CustomEditor(typeof(EnemyAI))]
public class EnemyAIEditor : Editor
{

    // This was made with reference to -
    // https://blog.terresquall.com/2020/03/creating-reorderable-lists-in-the-unity-inspector/

    SerializedProperty enemyAIStruct;

    EnemyAI enStruct;

    ReorderableList list;

    private void OnEnable()
    {
        enStruct = (EnemyAI)target;

        enemyAIStruct = serializedObject.FindProperty("enemyAIStruct");
   
        list = new ReorderableList(serializedObject, enemyAIStruct, true, true, true, true);

        list.drawElementCallback = DrawListItems;
        list.drawHeaderCallback = DrawHeader;
    }

    void DrawListItems(Rect rect, int index, bool isActive, bool isFocused)
    {
        SerializedProperty element = list.serializedProperty.GetArrayElementAtIndex(index);

        EditorGUI.LabelField(new Rect(rect.x, rect.y, 100, EditorGUIUtility.singleLineHeight), "Slot1");

        EditorGUI.PropertyField(new Rect(rect.x+35, rect.y, 100, EditorGUIUtility.singleLineHeight),
            element.FindPropertyRelative("slot1"), GUIContent.none);

        EditorGUI.LabelField(new Rect(rect.x + 140, rect.y, 100, EditorGUIUtility.singleLineHeight), "Slot2");

        EditorGUI.PropertyField(new Rect(rect.x + 175, rect.y, 100, EditorGUIUtility.singleLineHeight),
            element.FindPropertyRelative("slot2"), GUIContent.none);

        EditorGUI.LabelField(new Rect(rect.x + 280, rect.y, 100, EditorGUIUtility.singleLineHeight), "Slot3");

        EditorGUI.PropertyField(new Rect(rect.x + 315, rect.y, 100, EditorGUIUtility.singleLineHeight),
            element.FindPropertyRelative("slot3"), GUIContent.none);

        EditorGUI.LabelField(new Rect(rect.x + 420, rect.y, 100, EditorGUIUtility.singleLineHeight), "Slot4");

        EditorGUI.PropertyField(new Rect(rect.x + 455, rect.y, 100, EditorGUIUtility.singleLineHeight),
            element.FindPropertyRelative("slot4"), GUIContent.none);
    }

    void DrawHeader(Rect rect)
    {
        string name = "Enemy Cards";
        EditorGUI.LabelField(rect, name);
    }

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        serializedObject.Update();
        list.DoLayoutList();
        serializedObject.ApplyModifiedProperties();
    }
}
