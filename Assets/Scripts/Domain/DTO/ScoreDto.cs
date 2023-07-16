namespace Domain.DTO
{
    public readonly struct ScoreDto
    {
        public readonly float Value;
        public readonly string PlayerName;

        public ScoreDto(string playerName, float value)
        {
            this.Value = value;
            this.PlayerName = playerName;
        }
    }
}