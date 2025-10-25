using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class SkinButton : MonoBehaviour
{
    [Tooltip("0-based index matching the SkinToggleManager.skins list.")]
    public int skinIndex;

    [Tooltip("Reference to the manager. If empty the script will FindObjectOfType at Start.")]
    public SkinToggleManager manager;

    Button _button;

    void Start()
    {
        if (manager == null)
            manager = FindObjectOfType<SkinToggleManager>();

        _button = GetComponent<Button>();
        if (_button == null)
        {
            Debug.LogWarning("SkinButton requires a UnityEngine.UI.Button on the same GameObject.");
            return;
        }

        _button.onClick.RemoveAllListeners();
        _button.onClick.AddListener(OnClick);
    }

    void OnClick()
    {
        if (manager == null)
        {
            Debug.LogWarning("SkinButton: no SkinToggleManager found.");
            return;
        }

        manager.ShowSkin(skinIndex);
    }
}