using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.UI;

public class SlotMachine : MonoBehaviour
{
    public static event Action HandlePulled;
    public static event Action FirstRowStoped;
    public static event Action RoundEnded;

    [SerializeField] private List<Row> _rows = new();
    [SerializeField] private List<Combination> _combinations = new();
    [SerializeField] private Button _handle;
    private BettingField _bettingField;

    public float SpinningTime { private set; get; } = 3f;
    public float TimeStep { private set; get; } = 2f;
    public int Bet { private set; get; }
    private bool IsRoundEnded => _rows.Count(r => r.IsStoped) == _rows.Count;

    public void LaunchMachine()
    {
        Bet = _bettingField.Value;
        PlayerInfoHolder.WithdrawCoins(Bet);

        HandlePulled?.Invoke();
        for (int i = 0; i < _rows.Count; i++)
            _rows[i].StartSpinning(SpinningTime + TimeStep * i);
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

        Combination currentCombination = new(_rows.Select(r => r.CurrentSlotItem).ToList());
        Combination match = FindWinningCombinationIn(currentCombination);
        if (match != null)
        {
            switch (Roles.CurrentRole)
            {
                case Roles.Role.Player:
                    {
                        Debug.Log("Player won");
                        PlayerInfoHolder.AddCoins((int)(Bet + Bet * match.Multiplier));
                        break;
                    }
                case Roles.Role.Dealer:
                    {
                        Debug.Log("Casino lost");
                        PlayerInfoHolder.WithdrawMoney((int)(Bet * match.Multiplier));
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
                Debug.Log("Dealer Won");
                PlayerInfoHolder.AddMoney(Bet);
            }
        }

        RoundEnded?.Invoke();
    }

    private Combination FindWinningCombinationIn(Combination combination)
    {
        List<Combination> matchingCombinations = new();

        for (int i = 0; i < _combinations.Count; i++)
            if (combination.Slots.ContainsSequence(_combinations[i].Slots))
                matchingCombinations.Add(_combinations[i]);

        if (matchingCombinations.Count == 0)
            return null;

        for (int i = 0; i < matchingCombinations.Count; i++)
            for (int j = 0; j < matchingCombinations.Count; j++)
                if (matchingCombinations[i].Slots.Count > matchingCombinations[j].Slots.Count)
                    if (matchingCombinations[i].Slots.ContainsSequence(matchingCombinations[j].Slots))
                        matchingCombinations.RemoveAt(j);

        //return whole List
        return matchingCombinations[0];
    }

}
