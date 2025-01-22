using UnityEngine;

public class WaveManager : Singleton<WaveManager>
{
    public Wave[] b_waves;
    public int b_currentWaveIndex = 0;
    private int m_currentEnemyIndex = 0;

    public void StartNextWave()
    {
        if (b_currentWaveIndex < b_waves.Length)
        {
            m_currentEnemyIndex = 0;
            SpawnNextEnemy();
        }
        else
        {
            Debug.Log("All waves completed.");
        }
    }

    public void SpawnNextEnemy()
    {
        if (m_currentEnemyIndex < b_waves[b_currentWaveIndex].b_enemies.Count)
        {
            GameObject enemy = Instantiate(b_waves[b_currentWaveIndex].b_enemies.Dequeue(), b_waves[b_currentWaveIndex].b_spawnPoint.position, Quaternion.identity);
            TurnManager.Instance.SetCurrentEnemy(enemy.GetComponent<Enemy>());
            m_currentEnemyIndex++;
        }
        else
        {
            b_currentWaveIndex++;
            if (b_currentWaveIndex < b_waves.Length)
            {
                StartNextWave();
            }
            else
            {
                TurnManager.Instance.EndBattle();
            }
        }
    }
}