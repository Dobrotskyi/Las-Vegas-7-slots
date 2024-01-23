using TMPro;
using UnityEngine;

public class ChangeDisplayedAmt : MonoBehaviour
{
    [SerializeField] protected TextMeshProUGUI _amtField;
    [SerializeField] protected int _defaultStep = 100;
    [SerializeField] private int _value = 400;
    [SerializeField] protected string _prefix = string.Empty;

    public virtual int Step => _defaultStep;
    public int Value
    {
        get => _value;
        protected set
        {
            _value = value;
            UpdateField();
        }
    }

    public virtual void Add() => Add(Step);

    public virtual void Decrease()
    {
        if (_value - Step >= _defaultStep)
            Value -= Step;
        else
            Value = _defaultStep;
    }

    protected void Add(int amt)
    {
        Value += amt;
    }

    protected void UpdateField() => _amtField.text = _prefix + _value.ToString();

    private void Awake()
    {
        UpdateField();
    }
}
