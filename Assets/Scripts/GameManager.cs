using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [SerializeField] private Army player;
    [SerializeField] private List<Transform> enemiesSpawnPoints;
    [SerializeField] private Transform boostSpawnPointsRoot;
    [SerializeField] private GameObject boostPrefab;
    [SerializeField] private TMP_Text endLevelText;
    [SerializeField] private GameObject confettiPrefab;

    private List<Transform> _boostSpawnPoints;
    private BoostPlace[] _boostPlaces;
    private int _currentSpawnPoint;
    
    private void Awake()
    {
        _boostPlaces = GetComponentsInChildren<BoostPlace>(false);
        
        _boostSpawnPoints = boostSpawnPointsRoot.GetComponentsInChildren<Transform>(false).ToList();
        _boostSpawnPoints.RemoveAt(0);
    }
    
    private void OnEnable()
    {
        BoostPlace.Set += OnBoostSet;
        Army.ArmyIsDead += OnArmyDead;
        Warrior.Win += LevelCompleted;
    }
    private void OnDisable()
    {
        BoostPlace.Set -= OnBoostSet;
        Army.ArmyIsDead -= OnArmyDead;
        Warrior.Win -= LevelCompleted;
    }

    void Start()
    {
        endLevelText.text = String.Empty;
        
        for (int i = 0; i < _boostSpawnPoints.Count; i++)
        {
            var boost1 = Instantiate(boostPrefab, _boostSpawnPoints[i].position, Quaternion.identity, boostSpawnPointsRoot).GetComponent<Boost>();
            boost1.boost = BoostTypeContainer.GetBoost(i);
            boost1.SetText(); 
        }
    }

    private void OnBoostSet()
    {
        int i = 0;
        foreach (var _place in _boostPlaces)
        {
            if (_place.Busy) i++;
        }

        if (i == _boostPlaces.Length)
        {
            player.MoveArmy(enemiesSpawnPoints[_currentSpawnPoint].position);
        }
    }

    private void OnArmyDead(string tag)
    {
        if (tag == "Player")
        {
            LevelFailed();
            return;
        }
        
        _currentSpawnPoint++;

        if (enemiesSpawnPoints.Count > _currentSpawnPoint)
        {
            player.MoveArmy(enemiesSpawnPoints[_currentSpawnPoint].position);
        }
    }

    private void LevelFailed()
    {
        endLevelText.text ="LEVEL FAILED";
        Invoke(nameof(Reload), 2f);
    }

    private void Reload()
    {
        SceneManager.LoadScene(0);
    }
    
    private void LevelCompleted()
    {
        endLevelText.text ="LEVEL COMPLETED!";
        Instantiate(confettiPrefab);
    }
}
