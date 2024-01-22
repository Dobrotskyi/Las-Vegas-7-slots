using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class FreeSpins : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _field;

    private void Awake()
    {
        PlayerInfoHolder.FreeSpinsAmtChanged += UpdateField;
        UpdateField();
    }

    private void OnDestroy()
    {
        PlayerInfoHolder.FreeSpinsAmtChanged -= UpdateField;
    }

    private void UpdateField() => _field.text = PlayerInfoHolder.FreeSpinsAmt.ToString();
}
