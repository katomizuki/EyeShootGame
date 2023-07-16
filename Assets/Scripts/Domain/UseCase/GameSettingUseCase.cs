using Domain.IRepository;
using Domain.Model;
using Domain.UseCase.Interface;

namespace Domain.UseCase
{
    public sealed class GameSettingUseCase: IGameSettingUseCase
    {
        private readonly IGameSettingRepository _gameSettingRepository;
        private readonly ILoadAddressableRepository _loadAddressableRepository;
        public GameSettingUseCase(
            IGameSettingRepository gameSettingRepository,
            ILoadAddressableRepository loadAddressableRepository)
        {
            this._gameSettingRepository = gameSettingRepository;
            this._loadAddressableRepository = loadAddressableRepository;
            SetupPrefab();
        }
        
        public async void SetupPrefab()
        {   
           var asteroids = await _loadAddressableRepository.LoadAsteroid();
           var stars = await _loadAddressableRepository.LoadStars();
           _gameSettingRepository.SetupMeteorites(asteroids);
           _gameSettingRepository.SetupBullets(stars);
        }
        
        public void ChangePlayerName(string playerName)
        {
            _gameSettingRepository.ChangePlayerName(playerName);
        }
        
        public void ChangeGameTimeLimit(float gameTimeLimit)
        {
            _gameSettingRepository.ChangeGameTimeLimit(gameTimeLimit);
        }
        
        public void ChangeNumberOfEnemies(float numberOfEnemies)
        {
            _gameSettingRepository.ChangeNumberOfEnemies(numberOfEnemies);
        }
        
        public void ChangeGameAttackMode(GameAttackMode mode)
        {
            _gameSettingRepository.ChangeGameAttackMode(mode);
        }
        
        public void ChangeStarBullet(StarBullet bullet)
        {
            _gameSettingRepository.ChangeStarBullet(bullet);
        }
        
        public void ReleaseUnNeededGameObjects()
        {
            var gameSetting = _gameSettingRepository.GetGameSetting();
            var bullet = gameSetting.GameAttackMode;
            if (bullet == GameAttackMode.Bullet)
                _loadAddressableRepository.ReleaseMemoryExceptSelectedStar(gameSetting.StarBullet);
            else
                _loadAddressableRepository.ReleaseStars();
        }
    }
}