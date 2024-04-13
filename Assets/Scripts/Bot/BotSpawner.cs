using UnityEngine;

public class BotSpawner : MonoBehaviour
{
    [SerializeField] private Bot _botPrefab;

    public Bot Spawn(Transform botSpawnPoint)
    {
        Vector3 spawnPosition = botSpawnPoint.position;
        spawnPosition.y = _botPrefab.transform.position.y;

        return Instantiate(_botPrefab, spawnPosition, Quaternion.identity);
    }
}
