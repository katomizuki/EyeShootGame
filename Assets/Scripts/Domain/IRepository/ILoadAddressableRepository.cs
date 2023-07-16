using System.Collections.Generic;
using System.Threading.Tasks;
using Domain.Model;
using UnityEngine;

namespace Domain.IRepository
{
    public interface ILoadAddressableRepository
    {
        Task<List<GameObject>> LoadAsteroid();
        Task<List<GameObject>> LoadStars();
        void ReleaseMemoryExceptSelectedStar(StarBullet starBullet);
        void ReleaseStars();
        void ReleaseStar(StarBullet starBullet);
        void ReleaseAsteroid();
    }
}