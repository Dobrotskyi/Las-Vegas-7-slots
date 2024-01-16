using System;
using System.Collections;
using UnityEngine;

public class Row : MonoBehaviour
{
    public event Action Stoped;

    [SerializeField] private RectTransform _row;
    [SerializeField] private AnimationCurve _spinningCurve;
    private float _startingSpeed = 20f;

    public bool IsStoped { private set; get; }
    public Items CurrentSlotItem => _row.GetChild(GetClosestSlotIndex()).GetComponent<Slot>().Item;
    private Vector2 StartingPosition => new(_row.anchoredPosition.x, -_row.rect.height / 2);

    public void StartSpinning(float time)
    {
        IsStoped = false;
        StartCoroutine(Spin(time));
    }

    private IEnumerator Spin(float spinningTime)
    {
        float t = 0;
        float speed = 0;

        RectTransform slot1 = _row.GetChild(0).GetComponent<RectTransform>();
        RectTransform slot2 = _row.GetChild(1).GetComponent<RectTransform>();

        float step = Mathf.Abs(slot1.anchoredPosition.y - slot2.anchoredPosition.y);
        float slotHeight = slot1.rect.height;
        bool checkForRowEnd() => _row.anchoredPosition.y > _row.rect.height / 2 - slotHeight;
        float rotationToSlot(int index) => StartingPosition.y - slotHeight / 2 + step * index;

        _row.anchoredPosition = new Vector2(_row.anchoredPosition.x, rotationToSlot(UnityEngine.Random.Range(0, _row.childCount)));

        while (t < spinningTime)
        {
            float normalizedProgress = t / spinningTime;
            float easing = _spinningCurve.Evaluate(normalizedProgress);
            speed = _startingSpeed * easing;

            t += Time.deltaTime;
            Vector2 newPosition = _row.anchoredPosition;
            newPosition.y += speed;
            _row.anchoredPosition = newPosition;

            if (checkForRowEnd())
                _row.anchoredPosition = StartingPosition;
            yield return new WaitForEndOfFrame();
        }

        t = 0;
        float startY = _row.anchoredPosition.y;
        float endY = rotationToSlot(GetClosestSlotIndex());

        while (t < 1f)
        {
            Vector2 newPosition = _row.anchoredPosition;
            newPosition.y = Mathf.Lerp(startY, endY, t / 1);
            _row.anchoredPosition = newPosition;
            t += Time.deltaTime;

            if (checkForRowEnd())
                _row.anchoredPosition = StartingPosition;
            yield return new WaitForEndOfFrame();
        }

        IsStoped = true;
        Stoped?.Invoke();
    }

    private int GetClosestSlotIndex()
    {
        Transform closestChild = _row.GetChild(0);
        foreach (Transform child in _row.transform)
            if (Vector2.Distance(transform.position, child.position) < Vector2.Distance(transform.position, closestChild.position))
                closestChild = child;

        return closestChild.GetSiblingIndex();
    }

    private void Start()
    {
        StartCoroutine(SetSlotsPositionAfterFrame());
    }

    private IEnumerator SetSlotsPositionAfterFrame()
    {
        yield return 0;

        Vector2 newPosition = StartingPosition;
        newPosition.y -= _row.GetChild(0).GetComponent<RectTransform>().sizeDelta.y / 2;

        _row.anchoredPosition = newPosition;
    }
}
