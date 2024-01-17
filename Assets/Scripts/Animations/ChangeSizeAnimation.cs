using System;
using System.Collections;
using UnityEngine;

public class ChangeSizeAnimation
{
    public event Action Ended;

    private RectTransform _rectTransform;
    private Direction _direction;
    private float _duration;
    public enum Direction
    {
        Down,
        Up
    }

    public ChangeSizeAnimation(RectTransform rectTransform, Direction direction = Direction.Down, float duration = 1f)
    {
        _rectTransform = rectTransform;
        _direction = direction;
        _duration = duration;
    }

    public IEnumerator Start()
    {
        float time = 0;

        switch (_direction)
        {
            case Direction.Down:
                {
                    _rectTransform.localScale = Vector3.one;
                    break;
                }
            case Direction.Up:
                {
                    _rectTransform.localScale = Vector3.zero;
                    break;
                }
        }

        float startValue = _rectTransform.localScale.x;
        float t = 0;
        while (time < _duration)
        {
            t = time / _duration;
            t = t * t * (3f - 2f * t);
            float value = Mathf.Lerp(startValue, (float)_direction, t);
            _rectTransform.localScale = new(value, value, value);
            time += Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }
        _rectTransform.localScale = new((float)_direction, (float)_direction, (float)_direction);
        Ended?.Invoke();
        yield return new WaitForEndOfFrame();
        _rectTransform.localScale = Vector3.one;
    }
}
