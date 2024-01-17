using System;
using UnityEngine;

public static class PlayerInfoHolder
{
    public static event Action<int> NotEnoughMoney;

    private const string CASINO_MONEY = "Casino Money";
    private const string PLAYER_COINS = "Player Coins";

    public static event Action CasinoMoneyUpdated;
    public static event Action PlayerCoinsUpdated;

    public static int CasinoMoney
    {
        get => PlayerPrefs.GetInt(CASINO_MONEY, 0);
        private set
        {
            PlayerPrefs.SetInt(CASINO_MONEY, value);
            CasinoMoneyUpdated?.Invoke();
        }
    }
    public static int GetCasinoMoney() => CasinoMoney;

    public static int PlayerCoins
    {
        get => PlayerPrefs.GetInt(PLAYER_COINS, 0);
        private set
        {
            PlayerPrefs.SetInt(PLAYER_COINS, value);
            PlayerCoinsUpdated?.Invoke();
        }
    }
    public static int GetPlayerCoins() => PlayerCoins;

    public static void AddMoney(int money)
    {
        if (money < 0) return;
        CasinoMoney += money;
    }

    public static void AddCoins(int coins)
    {
        if (coins < 0) return;
        PlayerCoins += coins;
    }

    public static void WithdrawMoney(int money)
    {
        if (CasinoMoney < money)
            NotEnoughMoney?.Invoke(money - CasinoMoney);

        CasinoMoney -= money;
    }

    public static void WithdrawCoins(int coins)
    {
        if (PlayerCoins < coins) return;
        PlayerCoins -= coins;
    }
}
