using System;

namespace LeagueManager.WebUI.ViewModels
{
    public interface IMatchEventViewModel
    {
        Guid Guid { get; set; }
        string ViewName { get; }
    }
}