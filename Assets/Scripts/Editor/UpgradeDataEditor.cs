using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(UpgradeData))]
public class UpgradeDataEditor : Editor
{
    private const float PreviewSize = 96f;
    // 미리보기 가로, 세로 사이즈.

    public override void OnInspectorGUI()
    {
        serializedObject.Update();
        // 인스펙터에 표시할 serializedObject를 최신 상태로 갱신.

        DrawDefaultInspector(); // 기존 UpgradeData 인스펙터 필드들을 기본 방식 그대로 그림.
        DrawIconPreview(); // 기본 필드 아래에 displayIcon 미리보기 이미지 그리기.

        serializedObject.ApplyModifiedProperties(); // 인스펙터에서 수정된 값을 실제 객체에 반영.
    }

    private void DrawIconPreview()
    {
        SerializedProperty displayIconProperty = serializedObject.FindProperty("displayIcon");
        // UpgradeData 안의 displayIcon 필드를 이름으로 찾음.
        
        if (displayIconProperty == null) // displayIcon이라는 필드(변수)가 없으면 실행한다.
        {
            EditorGUILayout.HelpBox("displayIcon 필드를 찾지 못했습니다.", MessageType.Info);
            // 인스펙터에 안내 박스를 표시.
            return;
        }

        Sprite displayIcon = displayIconProperty.objectReferenceValue as Sprite;
        // displayIcon에 들어있는 오브젝트를 Sprite로 변환해서 가져옴.

        if (displayIcon == null)
        {
            return;
        }

        GUILayout.Space(8f);
        // 기존 인스펙터와 미리보기 사이에 8만큼 여백을 줌.
        EditorGUILayout.LabelField("Display Icon Preview", EditorStyles.boldLabel);
        // 미리보기 제목을 표시.

        Rect previewRect = GUILayoutUtility.GetRect(PreviewSize, PreviewSize, GUILayout.ExpandWidth(false));
        // 96x96 크기의 미리보기용 Rect 영역을 만듬.

        Texture2D previewTexture = AssetPreview.GetAssetPreview(displayIcon);
        // 스프라이트의 미리보기 텍스처를 가져옴.

        if (previewTexture == null)
        {
            previewTexture = AssetPreview.GetMiniThumbnail(displayIcon);
            // 만약 실패했을 경우(거의 그럴일 없음) project창에서 보이는 썸네일 텍스쳐를 가져옴.
        }

        if (previewTexture != null)
        {
            GUI.DrawTexture(previewRect, previewTexture, ScaleMode.ScaleToFit);
            // 만들어둔 Rect 영역 안에 비율에 맞게 이미지를 그림.
        }
    }
}