using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Row : MonoBehaviour
{
    public event Action Stoped;

    [SerializeField] private RectTransform _row;
    [SerializeField] private AnimationCurve _spinningCurve;
    [SerializeField] private ParticleSystem _stopEffect;
    private float _startingSpeed = 20f;

    public bool IsStoped { private set; get; }
    public Items CurrentSlotItem => CurrentSlot.Item;
    public Slot CurrentSlot => _row.GetChild(GetClosestSlotIndex()).GetComponent<Slot>();
    private Vector2 StartingPosition => new(_row.anchoredPosition.x, -_row.rect.height / 2);

    public void StartSpinning(float time)
    {
        IsStoped = false;
        StartCoroutine(Spin(time));
    }

    private float RotationToSlot(int index)
    {
        RectTransform slot1 = _row.GetChild(0).GetComponent<RectTransform>();
        RectTransform slot2 = _row.GetChild(1).GetComponent<RectTransform>();
        float step = Mathf.Abs(slot1.anchoredPosition.y - slot2.anchoredPosition.y);
        float slotHeight = slot1.rect.height;

        return StartingPosition.y - slotHeight / 2 + step * index;
    }

    private IEnumerator Spin(float spinningTime)
    {
        yield return null;

        float t = 0;
        float step = 0;

        RectTransform slot1 = _row.GetChild(0).GetComponent<RectTransform>();
        float slotHeight = slot1.rect.height;
        bool checkForRowEnd() => _row.anchoredPosition.y > _row.rect.height / 2 - slotHeight;

        _row.anchoredPosition = new Vector2(_row.anchoredPosition.x, RotationToSlot(UnityEngine.Random.Range(0, _row.childCount)));

        float getEasing()
        {
            float normalizedProgress = t / spinningTime;
            return _spinningCurve.Evaluate(normalizedProgress);
        }

        while (t < spinningTime * 0.95f)
        {
            step = _startingSpeed * getEasing();
            t += Time.deltaTime;

            Vector2 newPosition = _row.anchoredPosition;
            newPosition.y += step;
            _row.anchoredPosition = newPosition;

            if (checkForRowEnd())
                _row.anchoredPosition = StartingPosition;
            yield return new WaitForEndOfFrame();
        }

        float startY = _row.anchoredPosition.y;
        float endY = RotationToSlot(GetClosestSlotIndex());

        while (Mathf.Abs(_row.anchoredPosition.y - endY) > 5)
        {
            Vector2 newPosition = _row.anchoredPosition;
            step = _startingSpeed * getEasing();
            newPosition.y += step;
            _row.anchoredPosition = newPosition;

            if (checkForRowEnd())
                _row.anchoredPosition = StartingPosition;

            yield return new WaitForEndOfFrame();
        }

        _row.anchoredPosition = new(_row.anchoredPosition.x, endY);

        CreateEffect(transform.position);
    }

    private void CreateEffect(Vector2 position)
    {
        ParticleSystem spawned = Instantiate(_stopEffect, position, Quaternion.identity);
        spawned.GetComponent<ParticleSystemCallback>().Stoped.AddListener(() =>
        {
            IsStoped = true;
            Stoped?.Invoke();
            Destroy(spawned.gameObject);
        });
    }

    private int GetClosestSlotIndex()
    {
        Transform closestChild = _row.GetChild(0);
        foreach (Transform child in _row.transform)
            if (child.gameObject.activeSelf)
                if (Vector2.Distance(transform.position, child.position) < Vector2.Distance(transform.position, closestChild.position))
                    closestChild = child;

        return closestChild.GetSiblingIndex();
    }

    private void Start()
    {
        StartCoroutine(Init());
    }

    private IEnumerator Init()
    {
        yield return 0;
        SpawnSlotsInRow();
        yield return 0;
        SetStartingPosition();
    }

    private void SetStartingPosition()
    {
        Vector2 newPosition = StartingPosition;
        newPosition.y -= _row.GetChild(0).GetComponent<RectTransform>().rect.height / 2;
        _row.anchoredPosition = newPosition;
    }

    private void SpawnSlotsInRow()
    {
        int rowsMerged = FindObjectOfType<SlotMachine>().RowsMerged;
        if (rowsMerged == 1)
            return;

        List<List<Transform>> rowsToMerge = new();

        List<Transform> currentSlots = new();
        foreach (Transform child in _row.transform)
            currentSlots.Add(child);

        for (int i = 0; i < rowsMerged - 1; i++)
            rowsToMerge.Add(currentSlots.ToList());

        while (rowsToMerge.Count > 0)
        {
            var randomRow = rowsToMerge[UnityEngine.Random.Range(0, rowsToMerge.Count)];
            var randomSlot = randomRow[UnityEngine.Random.Range(0, randomRow.Count)];

            Transform spawned = Instantiate(randomSlot, _row);
            spawned.transform.SetSiblingIndex(UnityEngine.Random.Range(0, _row.childCount));

            randomRow.Remove(randomSlot);
            if (randomRow.Count == 0)
                rowsToMerge.Remove(randomRow);
        }
    }
}
