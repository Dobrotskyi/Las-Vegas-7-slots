using TMPro;
using UnityEngine;

public class CombinationShower : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _multiplierField;

    public void SetMultiplier(float multiplier) => _multiplierField.text = "x" + multiplier.ToString();
}
