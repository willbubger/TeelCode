using System.Collections.Generic;
using UnityEngine;

public class SkinToggleManager : MonoBehaviour
{
    [Tooltip("Assign each character GameObject here in the same order as your buttons (0..N-1).")]
    public List<GameObject> skins = new List<GameObject>();

    [Tooltip("Which index to show on Start (0-based).")]
    public int defaultIndex = 0;

    const string PlayerPrefKey = "SelectedSkinIndex";

    void Start()
    {
        if (skins == null || skins.Count == 0)
        {
            Debug.LogWarning("SkinToggleManager: no skins assigned.");
            return;
        }

        int saved = PlayerPrefs.GetInt(PlayerPrefKey, -1);
        int index = (saved >= 0 && saved < skins.Count) ? saved : Mathf.Clamp(defaultIndex, 0, skins.Count - 1);
        ShowSkin(index);
    }

    // Called from UI Buttons (via SkinButton or from Inspector)
    public void ShowSkin(int index)
    {
        if (skins == null || skins.Count == 0)
        {
            Debug.LogWarning("ShowSkin called but no skins are assigned.");
            return;
        }

        if (index < 0 || index >= skins.Count)
        {
            Debug.LogWarning($"ShowSkin: index {index} out of range (0..{skins.Count - 1}).");
            return;
        }

        for (int i = 0; i < skins.Count; i++)
        {
            var go = skins[i];
            if (go == null) continue;
            go.SetActive(i == index);
        }

        PlayerPrefs.SetInt(PlayerPrefKey, index);
        PlayerPrefs.Save();
    }

    // Convenience: show by GameObject reference
    public void ShowSkinByGameObject(GameObject go)
    {
        if (go == null) return;
        int idx = skins.IndexOf(go);
        if (idx >= 0) ShowSkin(idx);
        else Debug.LogWarning("ShowSkinByGameObject: GameObject not found in skins list.");
    }
}