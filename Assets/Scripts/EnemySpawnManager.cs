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
using System.Diagnostics.Tracing;
using UnityEngine;

public class EnemySpawnManager : MonoBehaviour
{
    #region Variables
    [Header("Spawn settings")]
    [SerializeField, Tooltip("Time, in seconds, between each spawning")]
    float spawnDelay;
    [SerializeField, Tooltip("Maximum number of enemy instances allowed at the same time")]
    int maxEnemies;
    [SerializeField, Tooltip("Percentage chance that a spawn will result in a boss spawning"), Range(0, 100)]
    int bossSpawnPercentage;
    [SerializeField, Tooltip("How should enemies be spawned?")]
    SpawnType spawnType;

    [Header("Spawners")]
    [SerializeField, Tooltip("Spawner game objects to be managed")]
    List<GameObject> spawnPoints;

    [Header("Enemies")]
    [SerializeField, Tooltip("Enemy prefabs to spawn")]
    List<GameObject> enemies;
    [SerializeField, Tooltip("Enemy boss prefabs to spawn")]
    List<GameObject> bosses;
    [SerializeField, ReadOnly, Tooltip("List of enemies spawned by this spawner")]
    List<GameObject> spawnedEntities;

    // This private variable is only used in the distributed spawn type to keep track of the current spawner to use
    private int spawnerSequence;
    #endregion

    /* This enumerator allows us to have an easy to use selection between spawn types.
     * You will see in the inspector, the serialized enum type above creates a dropdown list!
     * Below, in our StartSpawning() method, you can see how I use this enum to make the selection. */
    enum SpawnType
    {
        Random,
        Distributed
    }

    // Start is called before the first frame update
    void Start()
    {
        // Instatiate our emtpy list of entities
        spawnedEntities = new List<GameObject>();

        // Begin the sequence at the first spawner
        spawnerSequence = 0;

        // Begin spawning
        StartCoroutine(StartSpawning());
    }

    // Update is called once per frame
    void Update()
    {
        /*
         * Every frame we will check through all the entities we have spawned to
         * make sure they are still alive. If they have been destroyed by the player,
         * we can remove them from the list, allowing us to spawn more.
         * 
         * This check runs backwards so that we can remove elements from within the loop.
         * If we don't do this, it does not work as we mess with the size of the array.
         */
        for (int i = spawnedEntities.Count - 1; i >= 0; i--)
        {
            // Get the current entity we are checking
            GameObject entity = spawnedEntities[i];

            // If this entity is active, don't do anything.
            if (entity.activeSelf)
            {
                continue;
            }
            // If it is inactive, remove and delete it.
            // When destroyed, the entity will set itself inactive, allowing us to manage it here.
            else
            {
                spawnedEntities.Remove(entity);
                Destroy(entity);
            }
        }
    }

    IEnumerator StartSpawning()
    {
        // Wait the specified amount of time between spawns
        yield return new WaitForSeconds(spawnDelay);

        // If we have reached our enemy cap, don't do anything
        // We do it in this order for efficiency
        if (spawnedEntities.Count >= maxEnemies)
        {
            //Debug.Log("Cannot spawn more enemies, cap reached");
        }
        else
        {
            if (spawnType == SpawnType.Random)
            {
                // Pick a random spawn point from our list of spawners
                int randomIndex = Random.Range(0, spawnPoints.Count);

                // Spawn enemy
                SpawnEntity(RandomEnemy(), spawnPoints[randomIndex].transform.position, spawnPoints[randomIndex].transform.rotation);
            }
            else if (spawnType == SpawnType.Distributed)
            {
                // Spawn enemy at the current spawner in sequence
                SpawnEntity(RandomEnemy(), spawnPoints[spawnerSequence].transform.position, spawnPoints[spawnerSequence].transform.rotation);

                // Increment to the next spawner
                spawnerSequence++;

                // Ensure we don't overflow the number of spawners.
                // Modding the step we are on by our maximum number of spawners will
                // Always wrap around to the correct range of values
                spawnerSequence %= spawnPoints.Count;
            }
        }

        // Start the loop again
        StartCoroutine(StartSpawning());
    }

    /// <summary>
    /// This method spawns the <c>entity</c> given at, <c>position</c>, with <c>rotation</c>.
    /// It keeps track of the entities spawned autmatically.
    /// <example>
    /// For example:
    /// <code>
    /// SpawnEntity(enemy, transform.position, transform.rotation);
    /// </code>
    /// results in an instance of <c>enemy</c> instantiating at the parent objects position and rotation.
    /// </example>
    /// </summary>
    void SpawnEntity(GameObject entity, Vector3 position, Quaternion rotation)
    {
        // Instatiate the enemy entity at the location of the selected spawner.
        // Save that entity.
        GameObject spawnedEntity = Instantiate(entity, position, rotation);

        // Add the newly spawned entity to our list of spawned entities,
        // so we can keep track of the number that have spawned.
        spawnedEntities.Add(spawnedEntity);
    }

    /// <summary>
    /// Method <c>RandomEnemy</c> selects a random enemy from the list of enemies to spawn.
    /// </summary>
    GameObject RandomEnemy()
    {
        // Select a random int value between 0 (inc) and 99 (inc)
        // This is to determine whether a boss spawns or not
        float bossSpawn = Random.Range(0, 100);

        int numberOfEnemies = enemies.Count;
        int numberOfBosses = bosses.Count;

        GameObject Enemy;

        // If there are boss enemy prefabs AND we randomly chose to spawn a boss
        // spawn a boss
        if (bossSpawn < bossSpawnPercentage && numberOfBosses > 0)
        {
            // Generate a random, valid, index
            int randomIndex = Random.Range(0, numberOfBosses);
            // Pull randomly selected enemy out of list
            Enemy = bosses[randomIndex];
        }
        // Otherwise, spawn regular
        else
        {
            // Generate a random, valid, index
            int randomIndex = Random.Range(0, numberOfEnemies);
            // Pull randomly selected enemy out of list
            Enemy = enemies[randomIndex];
        }

        // Return our randomly selected enemy
        return Enemy;
    }
}
