using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private Spacecraft _spacecraft;
    [SerializeField] private BuildingSimulationManager _buildSimulationManager;

    public static GameManager Instance;
    public Spacecraft Spacecraft => _spacecraft;
    public BuildingSimulationManager BuildingSimulationManager => _buildSimulationManager;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void ResetGame()
    {

    }
}