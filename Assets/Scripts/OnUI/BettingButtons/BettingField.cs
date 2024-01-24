using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BettingField : ChangeDisplayedAmt
{
    [SerializeField] private ChangeDisplayedAmt _step;
    [SerializeField] private List<Button> _buttons;

    public override int Step => _step.Value;

    public override void Add()
    {
#if UNITY_EDITOR
        if (Value + Step > PlayerInfoHolder.PlayerCoins)
            Add(PlayerInfoHolder.PlayerCoins - Value);
        else
#endif
            base.Add();
    }

    private void OnEnable()
    {
        PlayerInfoHolder.FreeSpinsAmtChanged += SetField;
        PlayerInfoHolder.PlayerCoinsUpdated += SetField;
        SetField();
        if (Value > PlayerInfoHolder.PlayerCoins && PlayerInfoHolder.FreeSpinsAmt == 0)
            Value = PlayerInfoHolder.PlayerCoins;
    }

    private void OnDisable()
    {
        PlayerInfoHolder.FreeSpinsAmtChanged -= SetField;
        PlayerInfoHolder.PlayerCoinsUpdated -= SetField;
    }

    private void SetField()
    {
        if (PlayerInfoHolder.FreeSpinsAmt != 0)
            Value = PlayerInfoHolder.FREESPIN_BET;
        else if (Value < _defaultStep)
            Value = _defaultStep;
        else if (PlayerInfoHolder.PlayerCoins < Value)
            Value = PlayerInfoHolder.PlayerCoins;

        base.UpdateField();

        foreach (Button button in _buttons)
            button.interactable = PlayerInfoHolder.FreeSpinsAmt == 0;
    }
}
