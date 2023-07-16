using Domain.Model;
using UnityEngine;

namespace Domain.DTO
{
    public readonly struct GameSettingDto
    {
        public readonly float GameTimeLimit;
        public readonly int NumberOfEnemies;
        public readonly GameAttackMode GameAttackMode;
        public readonly string PlayerName;
        public readonly GameObject Meteorite;
        public readonly StarBullet StarBullet;
        
        public GameSettingDto(
            float timeLimit,
            int numberOfEnemies, 
            string playerName,
            GameAttackMode gameAttackMode,
            GameObject meteorite, 
            StarBullet starBullet)
        {
            this.PlayerName = playerName;
            this.GameTimeLimit = timeLimit;
            this.NumberOfEnemies = numberOfEnemies;
            this.GameAttackMode = gameAttackMode;
            this.Meteorite = meteorite;
            this.StarBullet = starBullet;
        }
    }
}