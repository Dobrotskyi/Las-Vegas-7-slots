using UnityEngine;
using UnityEngine.UI;

public class SlotClone : Slot
{
    [SerializeField] private ParticleSystem _effect;
    private Image _image;
    private Sprite _default;

    public void Clone(Sprite icon)
    {
        _image.sprite = icon;
        Instantiate(_effect, transform.position, Quaternion.identity);
    }

    private void OnEnable()
    {
        SlotMachine.HandlePulled += ResetIcons;
        _image = GetComponent<Image>();
        _default = _image.sprite;
    }

    private void OnDisable()
    {
        SlotMachine.HandlePulled -= ResetIcons;
    }

    private void ResetIcons()
    {
        _image.sprite = _default;
    }
}
