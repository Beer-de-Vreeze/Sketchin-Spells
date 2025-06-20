using UnityEngine;

public class WaveManager : Singleton<WaveManager>
{
    public Wave[] Waves;
    public int CurrentWaveIndex = 0;
    public Transform spawnPoint; // Set this in the inspector to your desired spawn location

    public void SpawnEnemy(int waveIndex)
    {
        if (waveIndex < Waves.Length)
        {
            Wave wave = Waves[waveIndex];
            if (wave != null && wave.Enemy != null && spawnPoint != null)
            {
                GameObject enemyObj = Instantiate(
                    wave.Enemy,
                    spawnPoint.position,
                    Quaternion.identity,
                    parent: UIManager.Instance.GameCanvas.transform
                );
                //find the enemy in the scene and set it as current enemy
                Enemy enemy = UIManager.Instance.GameCanvas.GetComponentInChildren<Enemy>();

                // Ensure the enemy has the correct tag
                if (enemy != null && enemy.gameObject != null)
                {
                    enemy.gameObject.tag = "Enemy";
                    Debug.Log($"Set {enemy.name} tag to 'Enemy'");
                }

                TurnManager.Instance.SetCurrentEnemy(enemy);

                // Ensure the enemy is set as the player's target
                if (UIManager.Instance.PlayerUI != null && enemy != null)
                {
                    UIManager.Instance.PlayerUI.SetTarget(enemy.gameObject);
                    Debug.Log($"Spawned enemy: {enemy.name} and set as target");
                }

                CurrentWaveIndex++;
            }
        }
    }

    public void ResetWaveManager()
    {
        CurrentWaveIndex = 0;
    }
}
