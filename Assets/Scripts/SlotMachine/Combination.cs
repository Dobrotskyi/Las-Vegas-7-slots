using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[Serializable]
public class Combination
{
    [SerializeField] private List<Items> _slots = new();
    [SerializeField] private float _multiplier = 1;

    public IReadOnlyList<Items> Slots => _slots;
    public float Multiplier => _multiplier;

    public Combination(IEnumerable<Items> slots)
    {
        _slots = slots.ToList();
    }
}
