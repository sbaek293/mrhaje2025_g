#if UNITY_EDITOR
using System.Linq;
using UnityEditor;
using UnityEngine;

/// <summary>
/// 선택된 최상위 씬 오브젝트들을, Project에서 함께 선택한 폴더 내 동일한 이름의 프리팹으로 교체.
/// Hierarchy 오브젝트 이름이 "MyPrefab (1)" 또는 "(Clone)" 이어도 원본 프리팹 이름으로 비교.
/// </summary>
public static class ReplaceSelectedWithPrefabsByOriginalName
{
    [MenuItem("Tools/Replace Selected Roots With Prefabs (by Original Prefab Name)")]
    public static void Replace()
    {
        var selectedObjects = Selection.objects;
        var sceneGOs = Selection.gameObjects.Where(go => go.scene.IsValid()).ToList();
        var folderAssets = selectedObjects
            .Select(obj => AssetDatabase.GetAssetPath(obj))
            .Where(path => !string.IsNullOrEmpty(path) && AssetDatabase.IsValidFolder(path))
            .Distinct()
            .ToList();

        if (folderAssets.Count == 0)
        {
            EditorUtility.DisplayDialog("폴더 미선택",
                "Project 창에서 교체할 프리팹들이 들어있는 폴더 1개를 함께 선택하세요.", "OK");
            return;
        }
        if (folderAssets.Count > 1)
        {
            EditorUtility.DisplayDialog("폴더 다중선택",
                "폴더는 1개만 선택해야 합니다.", "OK");
            return;
        }

        string folderPath = folderAssets[0];

        // 최상위 선택만 남기기 (같은 부모 선택된 경우 제거)
        var sceneGOSet = sceneGOs.ToHashSet();
        var topLevel = sceneGOs
            .Where(go =>
            {
                var t = go.transform.parent;
                while (t != null)
                {
                    if (sceneGOSet.Contains(t.gameObject)) return false;
                    t = t.parent;
                }
                return true;
            })
            .ToList();

        if (topLevel.Count == 0)
        {
            EditorUtility.DisplayDialog("오브젝트 없음",
                "Hierarchy에서 교체할 최상위 오브젝트를 선택하세요.", "OK");
            return;
        }

        int replaced = 0, skipped = 0;
        var newSelection = new System.Collections.Generic.List<GameObject>();

        try
        {
            Undo.IncrementCurrentGroup();
            int undoGroup = Undo.GetCurrentGroup();

            foreach (var target in topLevel)
            {
                if (target == null) continue;

                // ✅ 원본 프리팹 이름 가져오기
                string prefabName = GetOriginalPrefabName(target) ?? CleanName(target.name);

                // 폴더 내 프리팹 검색
                var prefab = FindPrefabInFolderByExactName(folderPath, prefabName);
                if (prefab == null)
                {
                    Debug.LogWarning($"[Replace] 스킵: '{target.name}' → '{prefabName}'에 해당하는 프리팹을 찾을 수 없습니다 ({folderPath})");
                    skipped++;
                    continue;
                }

                // 기존 상태 저장
                var parent = target.transform.parent;
                int siblingIndex = target.transform.GetSiblingIndex();
                var localPos = target.transform.localPosition;
                var localRot = target.transform.localRotation;
                var localScale = target.transform.localScale;
                bool wasActive = target.activeSelf;
                int layer = target.layer;
                string tag = target.tag;

                // 새 프리팹 인스턴스 생성
                GameObject newInstance = (GameObject)PrefabUtility.InstantiatePrefab(prefab, target.scene);
                if (newInstance == null)
                {
                    Debug.LogWarning($"[Replace] '{prefabName}' 인스턴스 생성 실패");
                    skipped++;
                    continue;
                }

                Undo.RegisterCreatedObjectUndo(newInstance, "Create New Prefab Instance");

                // 부모 및 트랜스폼 복원
                newInstance.transform.SetParent(parent, worldPositionStays: false);
                newInstance.transform.localPosition = localPos;
                newInstance.transform.localRotation = localRot;
                newInstance.transform.localScale = localScale;
                newInstance.transform.SetSiblingIndex(siblingIndex);
                newInstance.SetActive(wasActive);
                newInstance.layer = layer;
                try { newInstance.tag = tag; } catch { }

                // 기존 오브젝트 제거 (Undo 가능)
                Undo.RegisterFullObjectHierarchyUndo(target, "Replace With Prefab");
                Undo.DestroyObjectImmediate(target);

                newSelection.Add(newInstance);
                replaced++;
            }

            Undo.CollapseUndoOperations(Undo.GetCurrentGroup());

            if (newSelection.Count > 0)
                Selection.objects = newSelection.ToArray();

            EditorUtility.DisplayDialog("교체 완료",
                $"교체된 오브젝트: {replaced}\n스킵된 오브젝트: {skipped}\n폴더: {folderPath}",
                "OK");
        }
        catch (System.Exception ex)
        {
            Debug.LogError("[Replace] 오류: " + ex);
            EditorUtility.DisplayDialog("오류 발생", "콘솔을 확인하세요.\n\n" + ex.Message, "OK");
        }
    }

    /// <summary>
    /// Hierarchy 오브젝트가 프리팹 인스턴스일 경우 원본 프리팹 이름을 반환.
    /// </summary>
    private static string GetOriginalPrefabName(GameObject instance)
    {
        if (instance == null) return null;
        var source = PrefabUtility.GetCorrespondingObjectFromSource(instance);
        return source ? source.name : null;
    }

    /// <summary>
    /// 이름 끝의 (숫자)나 (Clone) 제거한 기본 이름 반환.
    /// </summary>
    private static string CleanName(string name)
    {
        if (string.IsNullOrEmpty(name)) return name;
        string cleaned = name;
        // "(숫자)" 제거
        if (cleaned.EndsWith(")"))
        {
            int idx = cleaned.LastIndexOf('(');
            if (idx > 0 && int.TryParse(cleaned.Substring(idx + 1, cleaned.Length - idx - 2), out _))
                cleaned = cleaned.Substring(0, idx).TrimEnd();
        }
        // "(Clone)" 제거
        if (cleaned.EndsWith("(Clone)"))
            cleaned = cleaned.Replace("(Clone)", "").TrimEnd();
        return cleaned;
    }

    /// <summary>
    /// 폴더 내에서 정확히 같은 이름의 프리팹 찾기.
    /// </summary>
    private static GameObject FindPrefabInFolderByExactName(string folderPath, string prefabName)
    {
        // 정확히 일치하는 파일명 우선 검색
        string[] guids = AssetDatabase.FindAssets($"t:Prefab {prefabName}", new[] { folderPath });
        foreach (var guid in guids)
        {
            string path = AssetDatabase.GUIDToAssetPath(guid);
            string fileNameNoExt = System.IO.Path.GetFileNameWithoutExtension(path);
            if (fileNameNoExt == prefabName)
                return AssetDatabase.LoadAssetAtPath<GameObject>(path);
        }

        // fallback: 폴더 전체 탐색
        string[] allGuids = AssetDatabase.FindAssets("t:Prefab", new[] { folderPath });
        foreach (var guid in allGuids)
        {
            string path = AssetDatabase.GUIDToAssetPath(guid);
            string fileNameNoExt = System.IO.Path.GetFileNameWithoutExtension(path);
            if (fileNameNoExt == prefabName)
                return AssetDatabase.LoadAssetAtPath<GameObject>(path);
        }
        return null;
    }
}
#endif
