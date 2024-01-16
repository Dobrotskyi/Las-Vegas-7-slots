using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Roles : MonoBehaviour
{
    [SerializeField] private List<Toggle> _toggles = new();

    public enum Role
    {
        Dealer,
        Player
    }
    public Role CurrentRole { private set; get; } = Role.Player;

    public void DealerTogglePressed(Toggle toggle)
    {
        if (toggle.isOn)
            CurrentRole = Role.Dealer;
        else
            CurrentRole = Role.Player;
    }

    private void Awake()
    {
        SlotMachine.FirstRowStoped += DisableToggles;
        SlotMachine.RoundEnded += EnableToggles;
    }

    private void OnDestroy()
    {
        SlotMachine.FirstRowStoped -= DisableToggles;
        SlotMachine.RoundEnded -= EnableToggles;
    }

    private void DisableToggles() => SetTogglesInteractable(false);

    private void EnableToggles() => SetTogglesInteractable(true);

    private void SetTogglesInteractable(bool value)
    {
        foreach (Toggle toggle in _toggles)
            toggle.interactable = value;
    }
}
