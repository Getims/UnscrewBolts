using System;
using Scripts.Data.Core;
using Scripts.Infrastructure.Providers.Events;

namespace Scripts.Data.Services
{
    public interface IPlayerDataService
    {
        bool IsSoundOn { get; }
        bool IsMusicOn { get; }
        int Money { get; }
        void SetSoundState(bool isSoundOn, bool autosave = true);
        void SetMusicState(bool isMusicOn, bool autosave = true);
        void AddMoney(int amount, bool autosave = true);
        void SetMoney(int amount, bool autosave = true);
        void SpendMoney(int amount, bool autosave = true);
    }
    
    public class PlayerDataService : DataService, IPlayerDataService
    {
        private readonly PlayerData _playerData;
        private readonly GlobalEventProvider _globalEventProvider;

        public bool IsSoundOn => _playerData.IsSoundOn;
        public bool IsMusicOn => _playerData.IsMusicOn;
        public int Money => _playerData.Money;

        public PlayerDataService(IDatabase database, GlobalEventProvider globalEventProvider) : base(database)
        {
            _globalEventProvider = globalEventProvider;
            _playerData = database.GetData<PlayerData>();
        }

        public void SetSoundState(bool isSoundOn, bool autosave = true)
        {
            if (_playerData.IsSoundOn == isSoundOn)
                return;

            _playerData.IsSoundOn = isSoundOn;
            TryToSave(autosave);
        }

        public void SetMusicState(bool isMusicOn, bool autosave = true)
        {
            if (_playerData.IsMusicOn == isMusicOn)
                return;

            _playerData.IsMusicOn = isMusicOn;
            TryToSave(autosave);
        }
        
        public void AddMoney(int amount, bool autosave = true)
        {
            if (amount <= 0)
                return;

            _playerData.Money += amount;
            TryToSave(autosave);

            _globalEventProvider.Invoke<MoneyChangedEvent, int>(_playerData.Money);
        }

        public void SetMoney(int amount, bool autosave = true)
        {
            if (amount == _playerData.Money)
                return;

            _playerData.Money = Math.Max(amount, 0);
            TryToSave(autosave);
            
            _globalEventProvider.Invoke<MoneyChangedEvent, int>(_playerData.Money);
        }

        public void SpendMoney(int amount, bool autosave = true)
        {
            if (amount <= 0)
                return;

            _playerData.Money = Math.Max(_playerData.Money - amount, 0);
            TryToSave(autosave);
            
            _globalEventProvider.Invoke<MoneyChangedEvent, int>(_playerData.Money);
        }
    }
}