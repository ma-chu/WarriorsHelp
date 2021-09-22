using System;
using System.Collections;
using UnityEngine;
using TMPro;
using Random = UnityEngine.Random;

public class Army : MovableUnit
{
    private const float SpawnRadius = 0.025f;

    public static Action<string> ArmyIsDead;
    public  Action<Vector3> MoveWarriors;

    public bool attacked;

    [SerializeField] private int health;
    [SerializeField] private Warrior warriorPrefab;
    [SerializeField] private Color color;

    private TMP_Text _text;
    
    private void Awake()
    {
        _text = GetComponentInChildren<TMP_Text>();
    }

    private void OnEnable()
    {
        Warrior.IamDead += OnWarriorDead;
        Boost.Caught += OnBoostCaught;
    }
    private void OnDisable()
    {
        Warrior.IamDead -= OnWarriorDead;
        Boost.Caught -= OnBoostCaught;
    }

    protected override void Start()
    {
        base.Start();
        
        _text.text = health.ToString();
        
        for (int i = 0; i < health; i++)
        {
            SpawnWarrior();
        }
    }

    private void SpawnWarrior()
    {
        var warrior = Instantiate(warriorPrefab, transform.position + (Vector3) Random.insideUnitCircle * SpawnRadius * health,
            Quaternion.identity, transform.parent);
        warrior.GetComponent<MeshRenderer>().material.color = color;
        warrior.tag = tag;
    }

    private void OnWarriorDead(Army army)
    {
        if (army != this) return;

        if (!attacked)
        {
            attacked = true;
            speed = SpeedAttacked;
            MoveWarriors?.Invoke(destination);
        }
        
        health--;
        _text.text = health == 0 ? string.Empty : health.ToString();
        
        if (health == 0)
        {
            ArmyIsDead?.Invoke(tag);
            Destroy(gameObject);
        }
    }

    public void MoveArmy(Vector3 where)
    {
        attacked = false;
        speed = SpeedUnattacked;
        MoveWarriors?.Invoke(where);
        OnMoveWarriors(where);
    }
    
    private void OnBoostCaught(BoostType boost)
    {
        if (CompareTag("Enemy")) return;
        attacked = false;
        speed = SpeedUnattacked;
        StartCoroutine(AddBoostAndMove(boost));
    }
    
    private IEnumerator AddBoostAndMove (BoostType boost)
    {
        yield return AddBoost(boost);
        MoveWarriors?.Invoke(destination);
    }
    
    private IEnumerator AddBoost(BoostType boost)
    {
        int valueToAdd;
        switch (boost.action)
        {
            case ActionType.Add:
                valueToAdd = boost.value;
                break;
            case ActionType.Multiply:
                valueToAdd = health * boost.value - health;
                break;
            default:
                valueToAdd = 0;
                break;
        }

        var oldHealth = health;
        var newHealth = health + valueToAdd;
        
        for (int i = oldHealth; i < newHealth; i++)
        {
            SpawnWarrior();
            health++;
            _text.text = health.ToString();
        }
        
        yield return true;
    }
}
