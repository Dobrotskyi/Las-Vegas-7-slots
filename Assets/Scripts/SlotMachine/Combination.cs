using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

[Serializable]
public class Combination
{
    [SerializeField] private List<Slot> _slots = new();
    [SerializeField] private float _multiplier = 1;
    private List<Items> _slotsItems = new();

    public IReadOnlyList<Slot> Slots => _slots;
    public IReadOnlyList<Items> SlotsItems
    {
        get
        {
            if (_slotsItems == null || _slotsItems.Count == 0)
                _slotsItems = _slots.Select(s => s.Item).ToList();
            return _slotsItems;
        }
    }
    public float Multiplier => _multiplier;

    public Combination(IEnumerable<Slot> slots)
    {
        _slots = slots.ToList();
        _slotsItems = _slots.Select(s => s.Item).ToList();
    }

    public void ChangeSlot(int index, Items item)
    {
        if (index < 0 || index >= _slots.Count)
            return;
        _slotsItems[index] = item;
    }
}

[Serializable]
public class WinningCombination
{
    [SerializeField] private float _multiplier = 1;
    [SerializeField] private List<Items> _slotsItems = new();
    public IReadOnlyList<Items> Items => _slotsItems;
    public float Multiplier => _multiplier;
}
