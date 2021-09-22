using System;
using UnityEngine;

public class BoostPlace : MonoBehaviour
{
    public bool Busy { get; private set; }

    public static Action Set;

    private SpriteRenderer _spriteRenderer;
    private bool _setEnabled;
    
    private void Awake()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void OnEnable()
    {
        Boost.Set += OnSet;
    }
    private void OnDisable()
    {
        Boost.Set -= OnSet;
    }

    private void OnSet()
    {
        if (_setEnabled)
        {
            Busy = true;
            Set?.Invoke();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (Busy) return;
        
        if (other.CompareTag("Boost"))
        {
            _setEnabled = true;
            _spriteRenderer.color = new Color(0,1,0,0.25f);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (Busy) return;
        
        if (other.CompareTag("Boost"))
        {
            _setEnabled = false;
            _spriteRenderer.color = new Color(1,0,0,0.25f);
        }
    }
}

