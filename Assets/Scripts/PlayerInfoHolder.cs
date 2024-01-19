using System;
using System.Collections.Generic;
using UnityEngine;

public static class PlayerInfoHolder
{
    public static event Action<int> NotEnoughMoney;
    public static event Action CasinoMoneyUpdated;
    public static event Action PlayerCoinsUpdated;
    public static event Action BonusAmtChanged;
    public static event Action FreeSpinsAmtChanged;

    private const string CASINO_MONEY = "Casino Money";
    private const string PLAYER_COINS = "Player Coins";
    private const string FREESPINS_AMT = "Free Spins Amt";
    public const int FREESPIN_BET = 500;

    public static Dictionary<Items, int> PriceList = new()
    {
        {
            Items.X2,
            0
        },
        {
            Items.FreeSpin,
            0
        },
        {
            Items.Clone,
            0
        }
    };

    public static int GetBonusAmount(Items item)
    {
        if (!PriceList.ContainsKey(item))
            return -1;

        return PlayerPrefs.GetInt(item.ToString(), 0);
    }

    public static void BuyBonus(Items item)
    {
        if (!PriceList.ContainsKey(item))
            return;
        if (PlayerCoins < PriceList[item])
            return;

        PlayerCoins -= PriceList[item];
        PlayerPrefs.SetInt(item.ToString(), GetBonusAmount(item) + 1);
        BonusAmtChanged?.Invoke();
    }

    public static int FreeSpinsAmt
    {
        get => PlayerPrefs.GetInt(FREESPINS_AMT, 0);
        set
        {
            PlayerPrefs.SetInt(FREESPINS_AMT, value);
            FreeSpinsAmtChanged?.Invoke();
        }
    }

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
