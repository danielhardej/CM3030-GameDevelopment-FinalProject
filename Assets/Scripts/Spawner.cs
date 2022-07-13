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

public class Spawner : MonoBehaviour
{
    [ReadOnly, Tooltip("List of entities spawned by this spawner")]
    public List<GameObject> spawnedEntities;

    private void Update()
    {
        foreach (GameObject entity in spawnedEntities)
        {
            if (entity == null)
            {
                spawnedEntities.Remove(entity);
            }
        }
    }

    /// <summary>
    /// This method spawns the entity passed at the spawner's location.
    /// <example>
    /// For example:
    /// <code>
    /// spawner.Spawn(GameObject enemy);
    /// </code>
    /// results in an instance of SphereEnemy instantiating in the spawner's location.
    /// </example>
    /// </summary>
    public void Spawn(GameObject entity)
    {
        spawnedEntities.Add(Instantiate(entity));
    }
}
