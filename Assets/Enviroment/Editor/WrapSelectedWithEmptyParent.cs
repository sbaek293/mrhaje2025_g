// Assets/Editor/WrapSelectedWithEmptyParent.cs
#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

public static class WrapSelectedWithEmptyParent
{
    [MenuItem("Tools/Hierarchy/Wrap Each Selected With Empty Parent %#w")]
    private static void WrapSelected()
    {
        // 현재 선택된 게임오브젝트들만(히어라키 기준) 처리, 비활성 포함
        var selection = Selection.GetFiltered<GameObject>(SelectionMode.Editable | SelectionMode.TopLevel | SelectionMode.ExcludePrefab);
        if (selection == null || selection.Length == 0)
        {
            Debug.LogWarning("[Wrap] 선택된 오브젝트가 없습니다.");
            return;
        }

        // 실행 시간 단일 Undo 그룹
        Undo.IncrementCurrentGroup();
        int undoGroup = Undo.GetCurrentGroup();

        foreach (var go in selection)
        {
            if (go == null) continue;

            Transform t = go.transform;
            Transform oldParent = t.parent;
            int oldSiblingIndex = t.GetSiblingIndex();

            // 새 부모 생성 (기존 오브젝트와 같은 이름)
            GameObject wrapper = new GameObject(go.name);
            // Undo 등록
            Undo.RegisterCreatedObjectUndo(wrapper, "Create Wrapper");

            // 원래 부모와 같은 레벨로 배치 + 기존 형제 순서 위치에 삽입
            wrapper.transform.SetParent(oldParent, false);
            wrapper.transform.SetSiblingIndex(oldSiblingIndex);

            // 새 부모를 기존 오브젝트의 월드 TR과 동일하게 배치
            wrapper.transform.SetPositionAndRotation(t.position, t.rotation);
            wrapper.transform.localScale = Vector3.one;

            // 기존 오브젝트를 새 부모 아래로 이동 (월드 좌표 유지)
            Undo.SetTransformParent(t, wrapper.transform, "Reparent To Wrapper");

            // (선택) 부모와 자식 이름이 같아 헷갈릴 수 있으니, 자식 쪽에 접미사를 붙이고 싶다면 아래 주석 해제
            // Undo.RecordObject(go, "Rename Child");
            // go.name = go.name + "_Child";
        }

        Undo.CollapseUndoOperations(undoGroup);
        Debug.Log($"[Wrap] {selection.Length}개 오브젝트를 각각 새 빈 오브젝트로 감쌌습니다.");
    }

    // 메뉴 활성화 조건
    [MenuItem("Tools/Hierarchy/Wrap Each Selected With Empty Parent %#w", true)]
    private static bool Validate_WrapSelected()
    {
        return Selection.gameObjects != null && Selection.gameObjects.Length > 0;
    }
}
#endif
