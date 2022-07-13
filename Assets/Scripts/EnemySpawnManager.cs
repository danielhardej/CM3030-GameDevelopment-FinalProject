/*
 * 
 * Code by: 
 *      Dimitrios Vlachos
 *      djv1@student.london.ac.uk
 *      dimitri.j.vlachos@gmail.com
 *      
 * Adapted from our FSM lecture
 * 
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawnManager : MonoBehaviour
{
    [Header("Spawn settings")]
    [SerializeField, Tooltip("Toggle spawner on/off")]
    bool active;
    [SerializeField, Tooltip("Time, in seconds, between each spawning")]
    float spawnRate;
    [SerializeField, Tooltip("Maximum number of enemy instances allowed at the same time")]
    int maxEnemies;

    [Header("Spawners")]
    [SerializeField, Tooltip("Spawner game objects to be managed")]
    List<GameObject> spawnPoints;

    [Header("Enemies")]
    [SerializeField, Tooltip("Enemy prefabs to spawn")]
    List<GameObject> enemies;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator spawn()
    {
        // Wait the specified amount of time between spawns
        yield return new WaitForSeconds(spawnRate);

        
    }
}
