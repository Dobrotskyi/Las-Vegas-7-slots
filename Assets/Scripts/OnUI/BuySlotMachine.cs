using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BuySlotMachine : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _priceField;
    private int _price;

    [SerializeField] private SlotMachineTypes _type = SlotMachineTypes.SlotsClassic;
    [SerializeField] private GameObject _toggleGO;
    [SerializeField] private Button _button;

    private void Awake()
    {
        _price = PlayerInfoHolder.PriceListMachines[_type];
        _priceField.text = _price.ToString();
        PlayerInfoHolder.CasinoMoneyUpdated += OnEnable;
    }

    private void OnDestroy()
    {
        PlayerInfoHolder.CasinoMoneyUpdated -= OnEnable;
    }

    private void OnEnable()
    {
        if (PlayerInfoHolder.GetSlotMachineStatus(_type))
        {
            _toggleGO.SetActive(true);
            Destroy(gameObject);
        }

        if (PlayerInfoHolder.PlayerCoins < _price)
            _button.interactable = false;
        else _button.interactable = true;
    }
}