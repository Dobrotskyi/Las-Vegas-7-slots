using System;
using TMPro;
using UnityEngine;

public class CurrencyUI : MonoBehaviour
{
    public enum Currency
    {
        PlayerCoins,
        CasinoMoney
    }
    [SerializeField] private Currency _displayedCurrency;
    [SerializeField] private TextMeshProUGUI _currencyField;

    private Func<int> GetCurrency;

    private void Awake()
    {
        if (_displayedCurrency == Currency.PlayerCoins)
        {
            GetCurrency = PlayerInfoHolder.GetPlayerCoins;
            PlayerInfoHolder.PlayerCoinsUpdated += UpdateField;
        }
        else if (_displayedCurrency == Currency.CasinoMoney)
        {
            GetCurrency = PlayerInfoHolder.GetCasinoMoney;
            PlayerInfoHolder.CasinoMoneyUpdated += UpdateField;
        }

        UpdateField();
    }

    private void OnDestroy()
    {
        if (_displayedCurrency == Currency.PlayerCoins)
            PlayerInfoHolder.PlayerCoinsUpdated -= UpdateField;
        else if (_displayedCurrency == Currency.CasinoMoney)
            PlayerInfoHolder.CasinoMoneyUpdated -= UpdateField;
    }

    private void UpdateField() => _currencyField.text = GetCurrency().ToString();

}
