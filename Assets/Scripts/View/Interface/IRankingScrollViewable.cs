using System.Collections.Generic;
using View.ViewData;

namespace View.Interface
{
    public interface IRankingScrollViewable
    {
        void ShowScoreResults(List<RankTextViewData> viewData);
        void ShowErrorAlert();
        void ShowIndicator();
        void HideIndicator();
    }
}

