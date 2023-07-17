using System.Collections.Generic;
using Domain.Model;
using UnityEngine;
using Random = UnityEngine.Random;

namespace DataStore.SciptableObject
{
    [CreateAssetMenu(fileName = "GameSettingScriptableObject", menuName = "GameSettingScriptableObject", order = 0)]
    public class GameSettingScriptableObject : ScriptableObject
    { 
        public int gameTimeLimit = 60;
        public int numberOfEnemies = 10;
        public GameAttackMode gameAttackMode = GameAttackMode.Bullet;
        public string playerName = "John Doe";
        public GameObject Meteorite => _meteorites[(int)Random.Range(0, _meteorites.Count - 1)];
        private List<GameObject> _meteorites = new();
        public StarBullet starBullet = StarBullet.Soft;
        public GameObject Bullet => _bullets[(int)starBullet];
        private List<GameObject> _bullets = new();
        
        public void SetupMeteorites(List<GameObject> values)
        {
            this._meteorites = values;
        }
        
        public void SetupBullets(List<GameObject> values)
        {
            this._bullets = values;
        }
    }
}