using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[Serializable]
public class Combination
{
    [SerializeField] private List<Items> _slots = new();
    [SerializeField] private int _reward = 100;

    public IReadOnlyList<Items> Slots => _slots;
    public int Reward => _reward;

    public Combination(IEnumerable<Items> slots)
    {
        _slots = slots.ToList();
    }
}
