using System.Collections.Generic;
using UnityEngine;

public class BotFactory : MonoBehaviour
{
    [SerializeField] private Bot _botPrefab;

    public Bot Spawn(Transform botSpawnPoint)
    {
        Vector3 spawnPosition = botSpawnPoint.position;
        spawnPosition.y = _botPrefab.transform.position.y;

        return Instantiate(_botPrefab, spawnPosition, Quaternion.identity);
    }

    public Bot GetFreeBot(List<Bot> bots)
    {
        Bot freeBot = null;

        foreach (Bot bot in bots)
        {
            if (bot.CurrentTarget == null)
            {
                freeBot = bot;

                break;
            }
        }

        return freeBot;
    }
}
