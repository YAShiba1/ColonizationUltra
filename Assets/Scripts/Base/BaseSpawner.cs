using System;
using UnityEngine;

public class BaseSpawner : MonoBehaviour
{
    [SerializeField] private Base _basePrefab;
    [SerializeField] private Transform _baseSpawnPoint;
    [SerializeField] private GoldPositionManager _goldManager;
    [SerializeField] private BotFactory _botFactory;

    private void Start()
    {
        SpawnStartingBase();
    }

    private Base Spawn(Vector3 baseSpawnPosition)
    {
        Base goldBase = Instantiate(_basePrefab, baseSpawnPosition, Quaternion.identity);

        goldBase.Initialize(this, _goldManager, _botFactory);

        return goldBase;
    }

    public void SpawnNewColonyBase(Vector3 baseSpawnPosition, Bot givenBot)
    {
        if(baseSpawnPosition == null)
        {
            throw new ArgumentNullException(nameof(baseSpawnPosition));
        }

        if (givenBot == null)
        {
            throw new ArgumentNullException(nameof(baseSpawnPosition));
        }

        Base newBase = Spawn(baseSpawnPosition);

        givenBot.SetParentBase(newBase);
        newBase.AddBot(givenBot);
    }

    private void SpawnStartingBase()
    {
        const int AmountOfStartingBots = 5;

        Base startBase = Spawn(_baseSpawnPoint.position);

        for (int i = 0; i < AmountOfStartingBots; i++)
        {
            startBase.AddBot(_botFactory.Spawn(startBase.BotSpawnPoint));
        }
    }
}
