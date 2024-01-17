using System.Collections;
using TMPro;
using UnityEngine;

public class CasinoGifts : MonoBehaviour
{
    [SerializeField] private RectTransform _body;
    [SerializeField] private TextMeshProUGUI _giftAmtField;
    [SerializeField] private GameObject _giftBody;
    [SerializeField] private float _maxMoneyMultiplier = 0.25f;
    private float _difference = 0;
    private Animator _animator;

    public void GiftChosen()
    {
        int added = (int)Random.Range(_difference, _difference * _maxMoneyMultiplier + _difference);
        PlayerInfoHolder.AddMoney(added);
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
        PlayerInfoHolder.NotEnoughMoney += NotEnoughMoney;
        if (_body.gameObject.activeSelf)
            _body.gameObject.SetActive(false);
    }

    private void OnDestroy()
    {
        PlayerInfoHolder.NotEnoughMoney -= NotEnoughMoney;
    }

    private void NotEnoughMoney(int difference)
    {
        _difference = difference;
        _body.gameObject.SetActive(true);
        _animator.SetTrigger("SizeUp");
    }
}
