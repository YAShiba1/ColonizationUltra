using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Base : MonoBehaviour
{
    [SerializeField] private Transform _botSpawnPoint;
    [SerializeField] private Scanner _scanner;
    [SerializeField] private int _maximumNumberOfBots;

    private List<Bot> _bots;

    private BotFactory _botFactory;
    private BaseSpawner _baseSpawner;
    private GoldPositionManager _goldManager;

    private Flag _flag;
    private Bot _botForNewColonyBase;

    private int _goldCount = 0;

    public event UnityAction<int> GoldChanged;

    public Transform BotSpawnPoint => _botSpawnPoint;

    private void Awake()
    {
        _bots = new List<Bot>();
    }

    private void Update()
    {
        ScanArea();
        TrySendFreeBotToFlag();
        TryBuyNewBot();
    }

    public void Initialize(BaseSpawner baseSpawner, GoldPositionManager goldManager, BotFactory botFactory)
    {
        _baseSpawner = baseSpawner ?? throw new ArgumentNullException(nameof(baseSpawner));
        _goldManager = goldManager ?? throw new ArgumentNullException(nameof(goldManager));
        _botFactory = botFactory ?? throw new ArgumentNullException(nameof(botFactory));
    }

    public void SetFlag(Flag flag)
    {
        _flag = flag;
    }

    public void TakeGold()
    {
        _goldCount++;
        GoldChanged?.Invoke(_goldCount);
    }

    public void AddBot(Bot givenBot)
    {
        if (givenBot != null)
        {
            _bots.Add(givenBot);
            givenBot.SetParentBase(this);
        }
    }

    private void ScanArea()
    {
        Transform scanGoldPoint = _scanner.TryGetNextGold();

        if (scanGoldPoint != null)
        {
            _goldManager.TakeGoldPosition(scanGoldPoint);
        }

        if (_goldManager.GetFiltredGoldsPositionsCount > 0)
        {
            TrySendFreeBotForGold();
        }
    }

    private void TrySendFreeBotForGold()
    {
        Bot freeBot = _botFactory.GetFreeBot(_bots);

        if (freeBot != null && _goldManager.GetFiltredGoldsPositionsCount > 0)
        {
            Transform goldPosition = _goldManager.GetFiltredGoldPosition();

            if (goldPosition != null)
            {
                freeBot.SetTarget(goldPosition);
            }
        }
    }

    private void TrySendFreeBotToFlag()
    {
        const int MinimumNumberOfBaseBots = 1;
        const int NewBasePrice = 5;

        if (_flag != null)
        {
            Bot freeBot = _botFactory.GetFreeBot(_bots);

            if (freeBot != null && _bots.Count > MinimumNumberOfBaseBots && _goldCount >= NewBasePrice)
            {
                _goldCount -= NewBasePrice;
                GoldChanged?.Invoke(_goldCount);

                _botForNewColonyBase = freeBot;
                _botForNewColonyBase.SetTarget(_flag.transform);
                _bots.Remove(_botForNewColonyBase);

                _flag = null;

                _botForNewColonyBase.FlagReached += OnBotFlagReached;
            }
        }
    }

    private void TryBuyNewBot()
    {
        const int BotPrice = 3;

        if (_goldCount >= BotPrice && _bots.Count <= _maximumNumberOfBots - 1)
        {
            AddBot(_botFactory.Spawn(_botSpawnPoint));

            _goldCount -= BotPrice;
            GoldChanged?.Invoke(_goldCount);
        }
    }

    private void OnBotFlagReached()
    {
        int distanceFromBotToNewBase = 3;

        Vector3 baseSpawnPosition = _botForNewColonyBase.transform.position;
        baseSpawnPosition.y = this.transform.position.y;
        baseSpawnPosition.z += distanceFromBotToNewBase;

        _baseSpawner.SpawnNewColonyBase(baseSpawnPosition, _botForNewColonyBase);

        _botForNewColonyBase.FlagReached -= OnBotFlagReached;
        _botForNewColonyBase = null;
    }
}
