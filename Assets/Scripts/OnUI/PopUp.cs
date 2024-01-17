using TMPro;
using UnityEngine;

public class PopUp : MonoBehaviour
{
    [SerializeField] private RectTransform _body;
    [SerializeField] private TextMeshProUGUI _textField;
    private Animator _animator;

    public void Show(string text)
    {
        _textField.text = text;
        _body.gameObject.SetActive(true);
        _animator.SetTrigger("Show");
    }

    private void Close()
    {
        _body.gameObject.SetActive(false);
    }

    private void Awake()
    {
        _animator = GetComponent<Animator>();
    }
}
