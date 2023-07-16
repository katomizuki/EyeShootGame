using System.Collections.Generic;
using Domain.DTO;
using Domain.Model;
using UnityEngine;

namespace Domain.IRepository
{
    public interface IGameSettingRepository
    {
        void SetupMeteorites(List<GameObject> meteorites);
        void SetupBullets(List<GameObject> bullets);
        GameSettingDto GetGameSetting();
        void ChangePlayerName(string playerName);
        void ChangeGameTimeLimit(float gameTimeLimit);
        void ChangeNumberOfEnemies(float numberOfEnemies);
        void ChangeGameAttackMode(GameAttackMode gameAttackMode); 
        void ChangeStarBullet(StarBullet starBullet);
    }
}