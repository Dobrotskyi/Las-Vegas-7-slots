using System;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class SlotMachine : MonoBehaviour
{
    public static event Action HandlePulled;
    public static event Action FirstRowStoped;
    public static event Action RoundEnded;

    private static readonly HashSet<Items> Bonuses = new HashSet<Items>() { Items.X2, Items.FreeSpin, Items.Clone };

    [SerializeField] private List<Row> _rows = new();
    [SerializeField] private List<WinningCombination> _combinations = new();
    [SerializeField] private Button _handle;
    [SerializeField] private int _visibleSlots = 1;
    private BettingField _bettingField;
    [SerializeField] private AudioSource _spinningAS;
    [SerializeField] private AudioSource _winningAS;

    public float SpinningTime { private set; get; } = 3f;
    public float TimeStep { private set; get; } = 2f;
    public int Bet { private set; get; }
    public int VisibleSlots => _visibleSlots;
    public IList<WinningCombination> Combinations => _combinations.AsReadOnly();
    private bool IsRoundEnded => _rows.Count(r => r.IsStoped) == _rows.Count;

    public void LaunchMachine()
    {
        _spinningAS.Play();
        Bet = _bettingField.Value;
        if (PlayerInfoHolder.FreeSpinsAmt == 0)
            PlayerInfoHolder.WithdrawCoins(Bet);
        else
            PlayerInfoHolder.FreeSpinsAmt -= 1;

        HandlePulled?.Invoke();
        WithdrawBonuses();

        for (int i = 0; i < _rows.Count; i++)
            _rows[i].StartSpinning(SpinningTime + TimeStep * i);
    }

    private void WithdrawBonuses()
    {
        foreach (var bonus in Bonuses)
            if (PlayerInfoHolder.GetBonusAmount(bonus) > 0)
                PlayerInfoHolder.BonusUsed(bonus);
    }

    private void OnEnable()
    {
        _bettingField = FindObjectOfType<BettingField>(true);

        foreach (var row in _rows)
            row.Stoped += RowStoped;

        _handle.onClick.AddListener(DisableButton);
        RoundEnded += EnableButton;
    }

    private void OnDisable()
    {
        foreach (var row in _rows)
            row.Stoped -= RowStoped;

        _handle.onClick.RemoveListener(DisableButton);
        RoundEnded -= EnableButton;
    }

    private void EnableButton() => SetButtonInteractable(true);

    private void DisableButton() => SetButtonInteractable(false);

    private void SetButtonInteractable(bool value) => _handle.interactable = value;

    private void RowStoped()
    {
        if (_rows.Count(r => r.IsStoped) == 1)
            FirstRowStoped?.Invoke();
        if (!IsRoundEnded) return;

        _spinningAS.Stop();

        List<Combination> currentCombinations = new();
        if (VisibleSlots == 1)
            currentCombinations.Add(new(_rows.Select(r => r.CurrentSlot)));

        else
        {
            for (int i = 0; i < _rows.Count; i++)
                currentCombinations.Add(_rows[i].GetVerticalCombination());

            for (int i = 0; i < _rows.Count; i++)
            {
                Slot[] itemsInHorizontal = new Slot[_rows.Count];
                for (int j = 0; j < itemsInHorizontal.Length; j++)
                    itemsInHorizontal[j] = currentCombinations[j].Slots[i];

                currentCombinations.Add(new(itemsInHorizontal));
            }
        }

        float multipliers = 0;
        int x2Count = 0;
        foreach (var combination in currentCombinations)
        {
            HandleCloneBonus(combination);
            List<WinningCombination> matches = FindWinningCombinationIn(combination).ToList();
            if (matches.Count > 0)
                multipliers += matches.Sum(m => m.Multiplier);
        }

        for (int i = 0; i < _rows.Count; i++)
        {
            HandleFreeSpinBonus(currentCombinations[i]);
            if (currentCombinations[i].SlotsItems.Contains(Items.X2))
                x2Count++;
        }

        multipliers *= x2Count > 0 ? (int)Mathf.Pow(2, x2Count) : 1;

        if (multipliers != 0)
        {
            switch (Roles.CurrentRole)
            {
                case Roles.Role.Player:
                    {
                        int winning = (int)(Bet * multipliers);
                        Debug.Log("Player won");
                        _winningAS.Play();
                        PlayerInfoHolder.AddCoins(winning);
                        break;
                    }
                case Roles.Role.Dealer:
                    {
                        Debug.Log("Casino lost");
                        PlayerInfoHolder.WithdrawMoney((int)(Bet * multipliers));
                        break;
                    }
            }
        }
        else
        {
            if (Roles.CurrentRole == Roles.Role.Player)
                Debug.Log("Visitor lost");
            if (Roles.CurrentRole == Roles.Role.Dealer)
            {
                _winningAS.Play();
                PlayerInfoHolder.AddMoney(Bet);
            }
        }

        RoundEnded?.Invoke();
    }

    private void HandleFreeSpinBonus(Combination combination)
    {
        if (combination.SlotsItems.Contains(Items.FreeSpin))
            PlayerInfoHolder.FreeSpinsAmt += combination.Slots.Count(s => s.Item == Items.FreeSpin);
    }

    private void HandleCloneBonus(Combination combination)
    {
        if (combination.SlotsItems.Contains(Items.Clone))
        {
            int index = combination.Slots.ToList().FindIndex(x => x.Item == Items.Clone);
            int leftIndex = index > 0 ? index - 1 : 0;
            int rightIndex = index < combination.Slots.Count() - 1 ? index + 1 : leftIndex;
            int randomIndex = UnityEngine.Random.Range(0, 2) == 0 ? leftIndex : rightIndex;

            if (Bonuses.Contains(combination.SlotsItems[randomIndex]))
            {
                if (leftIndex != rightIndex)
                    randomIndex = randomIndex == rightIndex ? leftIndex : rightIndex;
            }

            SlotClone clone = (SlotClone)combination.Slots[index];
            Slot slotToClone = combination.Slots[randomIndex];

            Items clonedItem = slotToClone.Item;
            if (slotToClone is SlotClone clone2)
            {
                if (clone2.Representing != Items.Clone)
                    clonedItem = clone2.Representing;
                else
                {
                    while (slotToClone is SlotClone)
                    {
                        if (randomIndex + 1 < _rows.Count)
                            randomIndex++;
                        else
                            return;

                        if (randomIndex == index)
                            continue;

                        slotToClone = _rows[randomIndex].CurrentSlot;
                    }
                }
            }

            clone.Clone(slotToClone.GetComponent<Image>().sprite, combination.SlotsItems[randomIndex]);
            combination.ChangeSlot(index, clonedItem);

            HandleCloneBonus(combination);
        }
    }

    private IEnumerable<WinningCombination> FindWinningCombinationIn(Combination combination)
    {
        List<WinningCombination> matchingCombinations = new();

        for (int i = 0; i < _combinations.Count; i++)
            if (combination.SlotsItems.ContainsSequence(_combinations[i].Items))
                matchingCombinations.Add(_combinations[i]);

        if (matchingCombinations.Count == 0)
            return matchingCombinations;

        for (int i = 0; i < matchingCombinations.Count; i++)
            for (int j = 0; j < matchingCombinations.Count; j++)
                if (matchingCombinations[i].Items.Count > matchingCombinations[j].Items.Count)
                    if (matchingCombinations[i].Items.ContainsSequence(matchingCombinations[j].Items))
                        matchingCombinations.RemoveAt(j);

        return matchingCombinations;
    }
}
