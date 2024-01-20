using UnityEngine;
using UnityEngine.UI;

public class SlotClone : Slot
{
    [SerializeField] private ParticleSystem _effect;
    private Image _image;
    private Sprite _default;
    public Items Representing { private set; get; }

    public void Clone(Sprite icon, Items item)
    {
        _image.sprite = icon;
        Representing = item;
        Instantiate(_effect, transform.position, Quaternion.identity);
    }

    private void OnEnable()
    {
        Representing = Item;
        SlotMachine.HandlePulled += Reset;
        _image = GetComponent<Image>();
        _default = _image.sprite;
    }

    private void OnDisable()
    {
        SlotMachine.HandlePulled -= Reset;
    }

    private void Reset()
    {
        Representing = Item;
        _image.sprite = _default;
    }
}
