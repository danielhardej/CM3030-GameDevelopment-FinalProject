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
    [SerializeField, Tooltip("Time, in seconds, between each spawning")]
    float spawnRate;


    [Header("Spawners")]
    [SerializeField, Tooltip("Spawner game objects to be managed. These are empty Game Objects denoting the location to spawn")]
    List<GameObject> spawners;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
