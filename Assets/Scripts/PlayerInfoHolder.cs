using System;
using System.Collections.Generic;
using UnityEngine;

public static class PlayerInfoHolder
{
    public static event Action<int> NotEnoughMoney;
    public static event Action<int> NotEnoughCoins;
    public static event Action CasinoMoneyUpdated;
    public static event Action PlayerCoinsUpdated;
    public static event Action BonusAmtChanged;
    public static event Action FreeSpinsAmtChanged;

    private const string CASINO_MONEY = "Casino Money";
    private const string PLAYER_COINS = "Player Coins";
    private const string FREESPINS_AMT = "Free Spins Amt";
    public const int FREESPIN_BET = 500;
    public const int MIN_PLAYER_COINS = 100;

    public static Dictionary<Items, int> PriceListBonus = new()
    {
        {
            Items.X2,
            100
        },
        {
            Items.FreeSpin,
            200
        },
        {
            Items.Clone,
            400
        }
    };

    public static void BuySlotMachine(SlotMachineTypes machine)
    {
        if (PriceListMachines[machine] > CasinoMoney)
            return;

        CasinoMoney -= PriceListMachines[machine];
        PlayerPrefs.SetString(machine.ToString(), machine.ToString());
    }

    public static bool GetSlotMachineStatus(SlotMachineTypes machine)
    {
        if (machine == SlotMachineTypes.SlotsClassic)
            return true;
        return PlayerPrefs.HasKey(machine.ToString());
    }

    public static Dictionary<SlotMachineTypes, int> PriceListMachines = new()
    {
        {SlotMachineTypes.Slots3x3, 1000 },
        {SlotMachineTypes.Slots4x4, 2000 }
    };

    public static int GetBonusAmount(Items item)
    {
        if (!PriceListBonus.ContainsKey(item))
            return -1;

        return PlayerPrefs.GetInt(item.ToString(), 0);
    }

    public static void BonusUsed(Items item)
    {
        if (!ValidateBonus(item)) return;

        PlayerPrefs.SetInt(item.ToString(), GetBonusAmount(item) - 1);
        BonusAmtChanged?.Invoke();
    }

    public static void BuyBonus(Items item)
    {
        if (!ValidateBonus(item)) return;

        PlayerCoins -= PriceListBonus[item];
        PlayerPrefs.SetInt(item.ToString(), GetBonusAmount(item) + 1);
        BonusAmtChanged?.Invoke();
    }

    private static bool ValidateBonus(Items item)
    {
        if (!PriceListBonus.ContainsKey(item))
            return false;
        if (PlayerCoins < PriceListBonus[item])
            return false;
        return true;
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
        get => PlayerPrefs.GetInt(CASINO_MONEY, 500);
        private set
        {
            PlayerPrefs.SetInt(CASINO_MONEY, value);
            CasinoMoneyUpdated?.Invoke();
        }
    }
    public static int GetCasinoMoney() => CasinoMoney;

    public static int PlayerCoins
    {
        get => PlayerPrefs.GetInt(PLAYER_COINS, 1000);
        private set
        {
            PlayerPrefs.SetInt(PLAYER_COINS, value);
            PlayerCoinsUpdated?.Invoke();
        }
    }

    public static void TryNotEnoughCoinsInvoke()
    {
        if (MIN_PLAYER_COINS > PlayerCoins)
            NotEnoughCoins?.Invoke(MIN_PLAYER_COINS - PlayerCoins);
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
        PlayerCoins -= coins;
    }
}
