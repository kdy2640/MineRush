using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(UpgradeData))]
public class UpgradeDataEditor : Editor
{
    private const float PreviewSize = 96f;

    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        DrawDefaultInspector();
        DrawIconPreview();

        serializedObject.ApplyModifiedProperties();
    }

    private void DrawIconPreview()
    {
        SerializedProperty displayIconProperty = serializedObject.FindProperty("displayIcon");

        if (displayIconProperty == null)
        {
            EditorGUILayout.HelpBox("displayIcon 필드를 찾지 못했습니다.", MessageType.Info);
            return;
        }

        Sprite displayIcon = displayIconProperty.objectReferenceValue as Sprite;

        if (displayIcon == null)
        {
            return;
        }

        GUILayout.Space(8f);
        EditorGUILayout.LabelField("Display Icon Preview", EditorStyles.boldLabel);

        Rect previewRect = GUILayoutUtility.GetRect(PreviewSize, PreviewSize, GUILayout.ExpandWidth(false));

        Texture2D previewTexture = AssetPreview.GetAssetPreview(displayIcon);

        if (previewTexture == null)
        {
            previewTexture = AssetPreview.GetMiniThumbnail(displayIcon);
        }

        if (previewTexture != null)
        {
            GUI.DrawTexture(previewRect, previewTexture, ScaleMode.ScaleToFit);
        }
    }
}