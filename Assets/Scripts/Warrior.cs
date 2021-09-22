using System;
using UnityEngine;
using Random = UnityEngine.Random;

public class Warrior : MovableUnit
{
    static readonly Quaternion SpotRotation = Quaternion.Euler(0, 0, -70);

    public static Action<Army> IamDead;
    public static Action Win;

    private Army _army;
    private Collider _collider;

    [SerializeField] private Transform spotPrefab;
    
    private void Awake()
    {
        _army = transform.parent.GetComponentInChildren<Army>();
        _collider = GetComponent<Collider>();
    }

    private void OnEnable()
    {
        _army.MoveWarriors += OnMoveWarriors;
    }
    private void OnDisable()
    {
        _army.MoveWarriors -= OnMoveWarriors;
    }
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy") && CompareTag("Player") || 
            other.CompareTag("Player") && CompareTag("Enemy"))
        {
            _collider.enabled = false;    // одного столкновения достаточно
            
            IamDead?.Invoke(_army);
            
            var spot = Instantiate(spotPrefab, transform.position, SpotRotation, transform.parent);
            spot.GetComponent<SpriteRenderer>().color =
                CompareTag("Player") ? new Color(0, 0, 1, 0.7f) : new Color(1, 0, 0, 0.7f);
            
            Destroy(gameObject);
        }
        
        if (other.CompareTag("Flag") && CompareTag("Player"))
        {
            Win?.Invoke();
        }
    }

    protected override void OnMoveWarriors(Vector3 destination)
    {
        var exc = destination - transform.position;
        exc = CompareTag("Player")
            ? Vector3.ClampMagnitude(exc, 0.25f) + (Vector3) Random.insideUnitCircle * 0.25f  // чуть дальше, чем центр
            : Vector3.ClampMagnitude(exc, 0.15f) - (Vector3) Random.insideUnitCircle * 0.25f; // чуть ближе, чем центр
        
        speed = _army.attacked ? SpeedAttacked : SpeedUnattacked;
        
        this.destination = destination + exc;    
        movingEnabled = true;
    }
}
