using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ResultsShower : MonoBehaviour
{
    [SerializeField] private List<GameObject> _bodies;
    [SerializeField] private RectTransform _combinationGroup;
    private List<CombinationShower> _combinations;
    private Animator _animator;

    public void Clicked()
    {
        if (_bodies[0].activeSelf)
            TurnOff();
        else
            TurnOn();
    }

    private void TurnOn()
    {
        foreach (var b in _bodies)
            b.SetActive(true);
        _animator.SetTrigger("SizeUp");
    }

    private void TurnOff()
    {
        _animator.SetTrigger("SizeDown");
    }

    private void Close()
    {
        foreach (var b in _bodies)
            b.SetActive(false);
    }

    private void Awake()
    {
        _animator = GetComponent<Animator>();
        var slotMachine = FindObjectOfType<SlotMachine>(true);
        _combinations = _combinationGroup.GetComponentsInChildren<CombinationShower>(true).ToList();

        int i = 0;
        foreach (var combination in slotMachine.Combinations)
        {
            _combinations[i].SetMultiplier(combination.Multiplier);
            i++;
        }
    }
}
