using UnityEngine;
using VContainer;

public class SaveMenuSettings : MonoBehaviour
{
    [Inject] MenuConfig _config;

    void Awake() => _config.Language.Init();
    public void SaveLanguage() => _config.Language.SaveLanguage();
    public void DeleteKey() => _config.Language.DeleteKey();
}
