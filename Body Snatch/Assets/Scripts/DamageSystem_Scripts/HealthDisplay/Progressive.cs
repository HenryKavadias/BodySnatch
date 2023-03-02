using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public abstract class Progressive : MonoBehaviour
{
    [SerializeField] private float _initial;
    private float _current;

    public float Current
    {
        get
        {
            return _current;
        }
        set
        {
            _current = value;
            OnChange?.Invoke();
        }
    }

    public float Initial => _initial;

    public float Ratio => _current / _initial;
    public Action OnChange;

    private void Awake()
    {
        _current = _initial;
    }

    public void Sub(float amount)
    {
        Current -= amount;

        OnChange?.Invoke();

        if (Current < 0)
        {
            Current = 0;
        }
    }

    public void Add(float amount)
    {
        Current += amount;

        if (Current > Initial)
        {
            Current = Initial;
        }
    }
}
