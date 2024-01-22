using UnityEngine;

public class Bonus : MonoBehaviour
{
    private Items _item;

    private void Awake()
    {
        _item = GetComponent<Slot>().Item;
        Setup();
        PlayerInfoHolder.BonusAmtChanged += Setup;
    }

    private void OnDestroy()
    {
        PlayerInfoHolder.BonusAmtChanged -= Setup;
    }

    private void Setup()
    {
        if (PlayerInfoHolder.GetBonusAmount(_item) == 0)
            Destroy(gameObject);
        else
            gameObject.SetActive(true);
    }
}
