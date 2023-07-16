using System.Collections.Generic;
using System.Threading.Tasks;
using Domain.IRepository;
using Domain.Model;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace DataStore
{
    public sealed class LoadAddressableRepository: ILoadAddressableRepository 
    {
        private static AsyncOperationHandle<GameObject> _prefabA;
        private static AsyncOperationHandle<GameObject> _prefabB;
        private static AsyncOperationHandle<GameObject> _prefabC;
        private static AsyncOperationHandle<GameObject> _softStar;
        private static AsyncOperationHandle<GameObject> _hardStar;
        private static AsyncOperationHandle<GameObject> _beveledStar;
    
        public async Task<List<GameObject>> LoadAsteroid()
        {
            _prefabA =　Addressables.LoadAssetAsync<GameObject>("Assets/Prefabs/Asteroid_A.prefab");
            _prefabB = Addressables.LoadAssetAsync<GameObject>("Assets/Prefabs/Asteroid_B.prefab");
            _prefabC = Addressables.LoadAssetAsync<GameObject>("Assets/Prefabs/Asteroid_C.prefab");
            await Task.WhenAll(_prefabA.Task, _prefabB.Task, _prefabC.Task); 
            return new List<GameObject> {_prefabA.Result, _prefabB.Result, _prefabC.Result};
        }

        public async Task<List<GameObject>> LoadStars()
        {
            _softStar = Addressables.LoadAssetAsync<GameObject>("Assets/Prefabs/SoftStar.prefab");
            _hardStar = Addressables.LoadAssetAsync<GameObject>("Assets/Prefabs/HardStar.prefab");
            _beveledStar = Addressables.LoadAssetAsync<GameObject>("Assets/Prefabs/BeveledStar.prefab"); 
            await Task.WhenAll( _softStar.Task, _hardStar.Task, _beveledStar.Task); 
            return new List<GameObject> {_softStar.Result, _hardStar.Result, _beveledStar.Result};
        }
        
        public void ReleaseAsteroid()
        {
            Addressables.Release(_prefabA);
            Addressables.Release(_prefabB);
            Addressables.Release(_prefabC);
        }

        public void ReleaseStar(StarBullet starBullet)
        {
            switch (starBullet)
            {
                case StarBullet.Beveled:
                    Addressables.Release(_beveledStar);
                    break;
                case StarBullet.Hard:
                    Addressables.Release(_hardStar);
                    break;
                case StarBullet.Soft:
                    Addressables.Release(_softStar);
                    break;
            }
        }

        public void ReleaseStars()
        {
            Addressables.Release(_softStar);
            Addressables.Release(_hardStar);
            Addressables.Release(_beveledStar);
        }

        public void ReleaseMemoryExceptSelectedStar(StarBullet starBullet)
        {
            switch (starBullet)
            {
                case StarBullet.Soft:
                    Addressables.Release(_hardStar);
                    Addressables.Release(_beveledStar);
                    break;
                case StarBullet.Beveled:
                    Addressables.Release(_softStar);
                    Addressables.Release(_hardStar);
                    break;
                case StarBullet.Hard:
                    Addressables.Release(_softStar);
                    Addressables.Release(_softStar);
                    break;
            }
        }
    }
}
