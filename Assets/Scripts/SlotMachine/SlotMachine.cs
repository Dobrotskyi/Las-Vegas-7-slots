using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SlotMachine : MonoBehaviour
{
    public event Action HandlePulled;

    [SerializeField] private List<Row> _rows = new();
    [SerializeField] private List<Combination> _combinations = new();

    public float SpinningTime { private set; get; } = 3f;
    public float TimeStep { private set; get; } = 0.5f;
    private bool RoundEnded => _rows.Count(r => r.IsStoped) == _rows.Count;

    public void LaunchMachine()
    {
        HandlePulled?.Invoke();
        for (int i = 0; i < _rows.Count; i++)
            _rows[i].StartSpinning(SpinningTime + TimeStep * i);
    }

    private void OnEnable()
    {
        foreach (var row in _rows)
            row.Stoped += RowStoped;
    }

    private void OnDisable()
    {
        foreach (var row in _rows)
            row.Stoped -= RowStoped;
    }

    private void RowStoped()
    {
        if (!RoundEnded) return;

        Combination currentCombination = new(_rows.Select(r => r.CurrentSlotItem).ToList());
        Combination match = FindWinningCombinationIn(currentCombination);
        if (match != null)
        {
            //Win
            Debug.Log("Win");
        }
        else
        {
            Debug.Log("Lose");
            //lose
        }
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
