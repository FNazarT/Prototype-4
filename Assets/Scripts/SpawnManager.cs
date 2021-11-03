using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    [SerializeField] private GameObject[] enemyPrefabs;
    [SerializeField] private GameObject[] powerupPrefabs;

    private int randomEnemy = 0;
    private int randomPowerup = 0;
    private int waveNumber = 1;     //How many new enemies will be spawned in every wave
    private float spawnRange = 9f;

    void Start() => SpawnEnemyWave(waveNumber);

    private void Update()
    {
        if (FindObjectsOfType<Enemy>().Length == 0) 
        {
            waveNumber++;
            SpawnEnemyWave(waveNumber);
        }
    }

    private void SpawnEnemyWave(int enemiesToSpawn)
    {
        randomPowerup = Random.Range(0, powerupPrefabs.Length);
        Instantiate(powerupPrefabs[randomPowerup], SpawnPosition(), powerupPrefabs[randomPowerup].transform.rotation);

        for (int i = 0; i < enemiesToSpawn; i++)
        {
            randomEnemy = Random.Range(0, enemyPrefabs.Length);
            Instantiate(enemyPrefabs[randomEnemy], SpawnPosition(), enemyPrefabs[randomEnemy].transform.rotation);
        }
    }

    private Vector3 SpawnPosition()
    {
        float spawnPositionX = Random.Range(-spawnRange, spawnRange);
        float spawnPositionZ = Random.Range(-spawnRange, spawnRange);
        Vector3 randomPosition = new Vector3 (spawnPositionX, 0f, spawnPositionZ);
        
        return randomPosition;
    }
}
