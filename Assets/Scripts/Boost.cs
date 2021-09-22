using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using System;

public class Boost : MonoBehaviour, IDragHandler, IPointerUpHandler
{
    public BoostType boost;
    
    public static Action Set;
    public static Action<BoostType> Caught;

    private TMP_Text _text;
    private Collider _collider;
    
    private Vector3 _startPosition;
    private bool _setEnabled;
    private Vector3 _setPosition;
    private bool _set;

    private void Awake()
    {
        _text = GetComponentInChildren<TMP_Text>();
        _startPosition = transform.position;
        _collider = GetComponent<Collider>();
    }

    public void SetText()
    {
        switch (boost.action)
        {
            case ActionType.Add:
                _text.text = ("+ " + boost.value);
                break;
            case ActionType.Multiply:
                _text.text = ("* " + boost.value);
                break;
        }
    }

    public void OnDrag(PointerEventData data)
    {
        if (_set) return;
        
        Ray camRay = Camera.main.ScreenPointToRay(Input.mousePosition); 
        RaycastHit floorHit;
        if (Physics.Raycast(camRay, out floorHit /*, camRayLength, floorMask*/))
        {
            Vector3 mousePos = floorHit.point;
            mousePos.z = -0.25f;
            transform.position = mousePos;
        }
    }
    
    public void OnPointerUp(PointerEventData data)
    {
        if (_set) return;
        
        if (_setEnabled)
        {
            transform.position = _setPosition;
            _set = true;
            Set?.Invoke();
        }
        else transform.position = _startPosition;
    }
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("SetPlace") && !other.GetComponent<BoostPlace>().Busy)
        {
            _setEnabled = true;
            _setPosition = other.transform.position;
        }
        
        if (!_set) return;
        
        if (other.CompareTag("Player"))
        {
            _collider.enabled = false;
            Caught?.Invoke(boost);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        _setEnabled = false;
    }
}
