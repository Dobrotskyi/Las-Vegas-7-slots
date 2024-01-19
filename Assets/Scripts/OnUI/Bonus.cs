using UnityEngine;

public class Bonus : MonoBehaviour
{
    private Items _item;

    private void Awake()
    {
        Debug.Log("Awake");
        _item = GetComponent<Slot>().Item;
        if (PlayerInfoHolder.GetBonusAmount(_item) == 0)
            gameObject.SetActive(false);
        else
            gameObject.SetActive(true);
    }
}
