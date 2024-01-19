using TMPro;
using UnityEngine;

public class ChangeDisplayedAmt : MonoBehaviour
{
    [SerializeField] protected TextMeshProUGUI _amtField;
    [SerializeField] protected int _defaultStep = 100;
    [SerializeField] protected int _value = 400;
    [SerializeField] protected string _prefix = string.Empty;

    public virtual int Step => _defaultStep;
    public int Value => _value;

    public virtual void Add() => Add(Step);

    public virtual void Decrease()
    {
        if (_value - Step >= _defaultStep)
            _value -= Step;
        else
            _value = _defaultStep;

        UpdateField();
    }

    protected void Add(int amt)
    {
        _value += amt;
        UpdateField();
    }

    protected void UpdateField() => _amtField.text = _prefix + _value.ToString();

    private void Awake()
    {
        UpdateField();
    }
}
