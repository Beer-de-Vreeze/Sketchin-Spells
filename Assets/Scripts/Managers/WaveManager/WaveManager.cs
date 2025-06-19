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
                Instantiate(
                    wave.Enemy,
                    spawnPoint.position,
                    Quaternion.identity,
                    parent: UIManager.Instance.GameCanvas.transform
                );
                //find the enemy in the scene
                TurnManager.Instance.SetCurrentEnemy(
                    UIManager.Instance.GameCanvas.GetComponentInChildren<Enemy>()
                );
                CurrentWaveIndex++;
            }
        }
    }

    public void ResetWaveManager()
    {
        CurrentWaveIndex = 0;
    }
}
