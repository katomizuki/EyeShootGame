using System.Threading.Tasks;
using Domain.Model;

namespace Domain.UseCase.Interface
{
    public interface IGameSettingUseCase
    { 
        Task<bool> SetupPrefab();
        void ChangePlayerName(string playerName);
        void ChangeGameTimeLimit(float gameTimeLimit);
        void ChangeNumberOfEnemies(float numberOfEnemies);
        void ChangeGameAttackMode(GameAttackMode mode);
        void ChangeStarBullet(StarBullet bullet);

        void ReleaseUnNeededGameObjects();
    }
}