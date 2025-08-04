using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

[ExecuteInEditMode]
public class TreeToGameObjects : MonoBehaviour
{
    public Terrain terrain;
    public GameObject[] treePrefabs;

    [ContextMenu("Convert Trees To GameObjects")]
    public void ConvertTrees()
    {
#if UNITY_EDITOR
        if (terrain == null)
        {
            Debug.LogError("Terrain is not assigned.");
            return;
        }

        if (treePrefabs == null || treePrefabs.Length == 0)
        {
            Debug.LogError("treePrefabs array is empty or not assigned.");
            return;
        }

        // Clear existing children (optional)
        foreach (Transform child in transform)
        {
            Undo.DestroyObjectImmediate(child.gameObject);
        }

        TerrainData tData = terrain.terrainData;
        Vector3 terrainPos = terrain.transform.position;

        foreach (TreeInstance tree in tData.treeInstances)
        {
            int index = tree.prototypeIndex;

            if (index < 0 || index >= treePrefabs.Length || treePrefabs[index] == null)
            {
                Debug.LogWarning($"Skipping tree with invalid prefab at index {index}.");
                continue;
            }

            GameObject prefab = treePrefabs[index];

            Vector3 worldPos = Vector3.Scale(tree.position, tData.size) + terrainPos;

            GameObject treeObj = (GameObject)PrefabUtility.InstantiatePrefab(prefab);
            if (treeObj == null)
            {
                Debug.LogWarning($"Failed to instantiate prefab at index {index}.");
                continue;
            }

            Undo.RegisterCreatedObjectUndo(treeObj, "Convert Tree");

            treeObj.transform.position = worldPos;

            // Apply rotation (Y-axis only)
            float rotationY = tree.rotation * Mathf.Rad2Deg;
            treeObj.transform.rotation = Quaternion.Euler(0f, rotationY, 0f);

            // Apply scale
            treeObj.transform.localScale = new Vector3(tree.widthScale, tree.heightScale, tree.widthScale);

            treeObj.transform.SetParent(transform);
        }

        Debug.Log("Tree conversion complete.");
#else
        Debug.LogWarning("ConvertTrees() is editor-only and won't run in builds.");
#endif
    }
}
