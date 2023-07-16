using System.Collections.Generic;
using DataStore.SciptableObject;
using Domain.DTO;
using Domain.IRepository;
using Domain.Model;
using UnityEngine;

namespace DataStore.Repository
{
    public class GameSettingRepository: IGameSettingRepository
    {
        private readonly GameSettingScriptableObject _gameSetting;

        public GameSettingRepository(GameSettingScriptableObject gameSetting)
        {
            _gameSetting = gameSetting;
        }

        public void SetupMeteorites(List<GameObject> meteorites)
        {
            _gameSetting.SetupMeteorites(meteorites);
        }

        public void SetupBullets(List<GameObject> bullets)
        {
            _gameSetting.SetupBullets(bullets);
        }

        public GameSettingDto GetGameSetting()
        {
            return new GameSettingDto(
                _gameSetting.gameTimeLimit,
                _gameSetting.numberOfEnemies,
                _gameSetting.playerName,
                _gameSetting.gameAttackMode,
                _gameSetting.Meteorite,
                _gameSetting.starBullet);
        }
        
        public void ChangePlayerName(string playerName)
        {
            _gameSetting.playerName = playerName;
        }
        
        public void ChangeGameTimeLimit(float gameTimeLimit)
        {
            _gameSetting.gameTimeLimit = (int)gameTimeLimit;
        }
        
        public void ChangeNumberOfEnemies(float numberOfEnemies)
        {
            _gameSetting.numberOfEnemies = (int)numberOfEnemies;
        }
        
        public void ChangeGameAttackMode(GameAttackMode gameAttackMode)
        {
            _gameSetting.gameAttackMode = gameAttackMode;
        }
        
        public void ChangeStarBullet(StarBullet starBullet)
        {
            _gameSetting.starBullet = starBullet;
        }
    }
}