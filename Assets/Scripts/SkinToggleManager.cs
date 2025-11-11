using System.Collections.Generic;
using UnityEngine;

public class SkinToggleManager : MonoBehaviour
{
    [Tooltip("Assign each character GameObject here in the same order as your buttons (0..N-1).")]
    public List<GameObject> skins = new List<GameObject>();

    [Tooltip("Which index to show on Start (0-based).")]
    public int defaultIndex = 0;
    public List<SSkinInfo> skinInfos = new List<SSkinInfo>();

    void Start()
    {
        if (skins == null || skins.Count == 0)
        {
            Debug.LogWarning("SkinToggleManager: no skins assigned.");
            return;
        }

        int index = Mathf.Clamp(defaultIndex, 0, skins.Count - 1);
        ShowSkin(index);
    }

    // Called from UI Buttons (via SkinButton or from Inspector)
    public void ShowSkin(int index)
    {
        // 👇 This runs first every time you click a button
        Debug.Log($"[{Time.frameCount}] Clicked skin #{index} | PlayerDataHolder loaded: {(PlayerDataHolder.CurrentPlayer != null)}");

        if (skins == null || skins.Count == 0)
        {
            Debug.LogWarning("⚠️ Skins list empty or null!");
            return;
        }

        if (index < 0 || index >= skins.Count)
        {
            Debug.LogWarning($"⚠️ Invalid index {index} for skins list of size {skins.Count}.");
            return;
        }

        // ✅ Level lock check
        if (skinInfos != null && index < skinInfos.Count)
        {
            int requiredLevel = skinInfos[index].requiredLevel;
            if (PlayerDataHolder.CurrentPlayer == null)
            {
                Debug.LogError("❌ No player data found. Can't check level.");
                return;
            }

            int playerLevel = PlayerDataHolder.CurrentPlayer.level;
            Debug.Log($"[SKIN CHECK] Player level {playerLevel} vs required {requiredLevel}");

            if (playerLevel < requiredLevel)
            {
                Debug.Log($"⛔ Skin locked! Requires level {requiredLevel}, but player is only level {playerLevel}.");
                return;
            }
        }

        // ✅ If unlocked, apply skin
        for (int i = 0; i < skins.Count; i++)
            if (skins[i] != null)
                skins[i].SetActive(i == index);

        Debug.Log($"✅ Equipped skin #{index}");
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