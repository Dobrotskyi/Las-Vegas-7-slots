using UnityEngine;

public class BettingButton : ChangeDisplayedAmt
{
    [SerializeField] private ChangeDisplayedAmt _step;
    public override int Step => _step.Value;

    public override void Add()
    {
        //if (Value + Step > PlayerInfoHolder.PlayerCoins)
        //    Add(PlayerInfoHolder.PlayerCoins - Value);
        //else
        base.Add();
    }
}
