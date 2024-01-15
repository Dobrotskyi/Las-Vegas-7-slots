using UnityEngine;

public class Slot : MonoBehaviour
{
    [SerializeField] private Items _item;
    public Items Item => _item;
}
