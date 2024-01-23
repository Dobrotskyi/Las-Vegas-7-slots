using TMPro;
using UnityEngine;

public class Gifts : MonoBehaviour
{
    [SerializeField] private RectTransform _body;
    [SerializeField] private TextMeshProUGUI _giftAmtField;
    [SerializeField] private GameObject _giftBody;
    [SerializeField] private float _maxMoneyMultiplier = 0.25f;

    private enum GIftsFor
    {
        Player,
        Casino
    }
    [SerializeField] private GIftsFor _role = GIftsFor.Casino;

    private float _difference = 0;
    private Animator _animator;

    public void GiftChosen()
    {
        int added = (int)Random.Range(_difference, _difference * _maxMoneyMultiplier + _difference);
        if (_role == GIftsFor.Casino)
            PlayerInfoHolder.AddMoney(added);
        else
            PlayerInfoHolder.AddCoins(added);
        _giftAmtField.text = "+" + added.ToString();
        _giftBody.SetActive(true);

        ChangeSizeAnimation animation = new(_giftBody.GetComponent<RectTransform>(), ChangeSizeAnimation.Direction.Up);
        StartCoroutine(animation.Start());
    }

    private void Update()
    {
        if (!_giftBody.activeSelf)
            return;

        if (Input.touchCount > 0 && Input.touches[0].phase == TouchPhase.Began)
            _animator.SetTrigger("SizeDown");
    }

    private void Close()
    {
        _giftBody.SetActive(false);
        _body.gameObject.SetActive(false);
        _difference = 0;
    }

    private void Awake()
    {
        _animator = GetComponent<Animator>();
        if (_role == GIftsFor.Casino)
            PlayerInfoHolder.NotEnoughMoney += ShowPanel;
        else
            PlayerInfoHolder.NotEnoughCoins += ShowPanel;
        if (_body.gameObject.activeSelf)
            _body.gameObject.SetActive(false);
    }

    private void OnDestroy()
    {
        if (_role == GIftsFor.Casino)
            PlayerInfoHolder.NotEnoughMoney -= ShowPanel;
        else
            PlayerInfoHolder.NotEnoughCoins -= ShowPanel;
    }

    private void ShowPanel(int difference)
    {
        _difference = difference;
        _body.gameObject.SetActive(true);
        _animator.SetTrigger("SizeUp");
    }
}
