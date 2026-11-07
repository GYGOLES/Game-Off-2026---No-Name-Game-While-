using TMPro;
using UnityEngine;
using VContainer;

public class PlayerUI : MonoBehaviour
{
    private PlayerStates _states;
    private PlayerCarry _carry;
    private PlayerConfig _config;

    [SerializeField] TextMeshProUGUI _bagStorage;
    [SerializeField] TextMeshProUGUI _stamina;
        
    [Inject]
    public void C(PlayerStates states, PlayerCarry carry, PlayerConfig config)
    {
        _states = states;
        _carry = carry;
        _config = config;

        if (_bagStorage)
        {
            _bagStorage.text = _carry.BagCollection.ToString() + "/" + _config.MaxBagStorage.ToString();
        }
        else
        {
            Debug.LogWarning("ERROR THERE'S NO TEXT");
        }
        
    }
    void OnEnable()
    {
        if(_carry == null)return;

        _carry.BagIntChange += BagText;
    }
    void OnDisable()
    {
        if(_carry == null)return;

        _carry.BagIntChange -= BagText;
    }

    public void BagText(int f)
    {
        if(_bagStorage == null)return;

        _bagStorage.text = f.ToString() + "/" + _config.MaxBagStorage.ToString();
    }
    public void Stamina()
    {
        if(_stamina == null || _states == null)return;

        _stamina.text = _states.Stamina.ToString("F0");
    }
    void Update()
    {
        if(_stamina == null || _states == null)return;

        Stamina();
    }
}
