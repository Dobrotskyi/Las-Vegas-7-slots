using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BonusesDisplay : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _amountField;
    [SerializeField] private Items _item;
    [Header("Can be left empty")]
    [SerializeField] private TextMeshProUGUI _priceField;
    private Button _button;

    public void Purchase()
    {
        PlayerInfoHolder.BuyBonus(_item);
    }

    private void Awake()
    {
        PlayerInfoHolder.BonusAmtChanged += UpdateField;
        if (_priceField != null)
        {
            _button = GetComponent<Button>();

            PlayerInfoHolder.PlayerCoinsUpdated += UpdateButton;

            if (!_priceField.gameObject.activeSelf)
                _priceField.gameObject.SetActive(true);

            _priceField.text = PlayerInfoHolder.PriceListBonus[_item].ToString();
        }
    }

    private void OnDestroy()
    {
        PlayerInfoHolder.BonusAmtChanged -= UpdateField;
        PlayerInfoHolder.PlayerCoinsUpdated -= UpdateButton;
    }

    private void UpdateField()
    {
        _amountField.text = PlayerInfoHolder.GetBonusAmount(_item).ToString();
    }

    private void OnEnable()
    {
        UpdateField();
        if (_button != null)
            UpdateButton();
    }

    private void UpdateButton()
    {
        if (_priceField != null)
        {
            if (PlayerInfoHolder.PlayerCoins < PlayerInfoHolder.PriceListBonus[_item])
                _button.interactable = false;
            else
                _button.interactable = true;
        }
    }
}
