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
        public GameObject Meteorite => meteorites[(int)Random.Range(0, meteorites.Count - 1)];
        [SerializeField] private List<GameObject> meteorites = new();
        public StarBullet starBullet = StarBullet.Soft;
        public GameObject Bullet => bullets[(int)starBullet];
        [SerializeField] private List<GameObject> bullets = new();
        
        public void SetupMeteorites(List<GameObject> meteorites)
        {
            this.meteorites = meteorites;
        }
        
        public void SetupBullets(List<GameObject> bullets)
        {
            this.bullets = bullets;
        }
    }
}