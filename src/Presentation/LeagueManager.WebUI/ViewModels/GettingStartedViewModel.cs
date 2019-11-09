using System.ComponentModel;

namespace LeagueManager.WebUI.ViewModels
{
    public class GettingStartedViewModel
    {
        [DisplayName("Database Server")]
        public string DatabaseServer { get; set; }
        [DisplayName("Database Name")]
        public string DatabaseName { get; set; }
    }
}