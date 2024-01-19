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
        SetField();
    }

    private void OnDisable()
    {
        PlayerInfoHolder.FreeSpinsAmtChanged -= SetField;
    }

    private void SetField()
    {
        _value = PlayerInfoHolder.FREESPIN_BET;
        base.UpdateField();
        Debug.Log(PlayerInfoHolder.FreeSpinsAmt == 0);

        foreach (Button button in _buttons)
            button.interactable = PlayerInfoHolder.FreeSpinsAmt == 0;
    }
}
