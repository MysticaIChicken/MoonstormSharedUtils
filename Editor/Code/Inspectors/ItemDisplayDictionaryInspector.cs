﻿using MSU.Editor.EditorWindows;
using RoR2.Editor.EditorWindows;
using UnityEditor;
using UnityEngine.UIElements;

namespace MSU.Editor.Inspectors
{
    [CustomEditor(typeof(ItemDisplayDictionary))]
    public class ItemDisplayDictionaryInspector : UnityEditor.Editor
    {
        public override VisualElement CreateInspectorGUI()
        {
            VisualElement element = new VisualElement();
            Button button = new Button(Open)
            {
                text = $"Open ItemDisplayDictionary Editor Window"
            };
            element.Add(button);
            element.Add(new IMGUIContainer(OnInspectorGUI));
            return element;
        }

        private void Open()
        {
            ExtendedEditorWindow.OpenEditorWindow<ItemDisplayDictionaryEditorWindow>(serializedObject);
        }

        [MenuItem("Tools/MSEU/Windows/ItemDisplayDictionary Editor Window")]
#pragma warning disable IDE0051 // Remove unused private members
        private static void OpenStatic()
#pragma warning restore IDE0051 // Remove unused private members
        {
            ExtendedEditorWindow.OpenEditorWindow<ItemDisplayDictionaryEditorWindow>(false);
        }
    }
}