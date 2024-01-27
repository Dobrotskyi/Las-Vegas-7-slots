using TMPro;
using UnityEngine;

public class Gifts : MonoBehaviour
{
    [SerializeField] private RectTransform _body;
    [SerializeField] private TextMeshProUGUI _giftAmtField;
    [SerializeField] private GameObject _giftBody;
    [SerializeField] private float _maxMoneyMultiplier = 0.25f;
    private bool _given;

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
        if (_given)
            return;

        _given = true;
        int added = 0;
        if (_role == GIftsFor.Casino)
        {
            added = (int)Random.Range(_difference, _difference * _maxMoneyMultiplier + _difference);
            PlayerInfoHolder.AddMoney(added);
        }
        else
        {
            added = Random.Range(1, 3);
            PlayerInfoHolder.FreeSpinsAmt += added;
        }
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
        _given = false;
    }

    private void OnEnable()
    {
        if (_role == GIftsFor.Player && PlayerInfoHolder.FreeSpinsAmt == 0)
            if (PlayerInfoHolder.PlayerCoins < BettingField.MIN_BET)
                ShowPanel();
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

    private void ShowPanel()
    {
        if (PlayerInfoHolder.FreeSpinsAmt > 0)
            return;
        _body.gameObject.SetActive(true);
        _animator.SetTrigger("SizeUp");
    }

    private void ShowPanel(int difference)
    {
        _difference = difference;
        ShowPanel();
    }
}
