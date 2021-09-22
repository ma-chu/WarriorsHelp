using System.Collections.Generic;
using UnityEngine;
using System;

public enum ActionType
{
    Add,
    Multiply
}

[Serializable]
public class BoostType
{
    public ActionType action;
    public int value;

    public BoostType(ActionType action, int value)
    {
        this.action = action;
        this.value = value;
    }
}


[CreateAssetMenu(fileName = "BoostTypeContainer", menuName = "_WarriorsHelp/BoostTypeContainer", order = 1)]
public class BoostTypeContainer : ScriptableObject
{
    [SerializeField] private List<BoostType> _boosts;

    private static BoostTypeContainer _instance;
        
    public static BoostTypeContainer Instance 
    {
        get
        {
            if (_instance != null) return _instance;
            _instance = Resources.Load<BoostTypeContainer>("BoostTypeContainer");
            return _instance;
        }
    }

    public static BoostType GetBoost(int i)
    {
        return Instance._boosts[i];
    }
}
