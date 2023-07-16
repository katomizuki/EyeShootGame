
using System;

namespace View.ViewData
{
    public readonly struct RankTextViewData
    {
        private readonly float _score;
        private readonly string _playerName;
        private readonly int _rank;
        public string CellText => $"Rank:{ _rank } Name:{ _playerName } - Score:{ _score }";
        public RankTextViewData(float score, string playerName, int rank)
        {
            this._score = (float)Math.Round(score,2,MidpointRounding.AwayFromZero);
            this._playerName = playerName;
            this._rank = rank + 1;
        }
    }
}