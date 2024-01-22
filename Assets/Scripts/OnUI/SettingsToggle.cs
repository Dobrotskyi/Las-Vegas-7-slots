using UnityEngine;

public class SettingsToggle : MonoBehaviour
{
    [SerializeField] private GameObject _body;
    [SerializeField] private GameObject _iconOpen;
    [SerializeField] private GameObject _iconClose;

    public void Toggle()
    {
        _body.SetActive(!_body.activeSelf);
        _iconOpen.SetActive(!_body.activeSelf);
        _iconClose.SetActive(!_iconClose.activeSelf);
    }
}
