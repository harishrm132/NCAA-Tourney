using Caliburn.Micro;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrackerLibrary;
using TrackerLibrary.Models;

namespace NCAA_UI.ViewModels
{
    public class ShellViewModel : Conductor<object>, IHandle<TournamentModel>
    {
        public ShellViewModel()
        {
            //Initillize the database connection
            GlobalConfig.InitializeConnection(DatabaseType.SQL);

            _existingTournaments = new BindableCollection<TournamentModel>(GlobalConfig.Connection.GetTournaments_All());
        }

        public void CreateTournament()
        {
            ActivateItem(new CreateTournamentViewModel());
        }

        public void LoadTournament()
        {
            if (SelectedTournament != null && !string.IsNullOrWhiteSpace(SelectedTournament.TournamentName))
            {
                ActivateItem(new TournamentViewerViewModel(SelectedTournament));
            }
        }

        public void Handle(TournamentModel message)
        {
            //TODO - open the tournament viewer
            ExistingTournaments.Add(message);
            SelectedTournament = message;
        }

        private BindableCollection<TournamentModel> _existingTournaments;
        private TournamentModel _selectedTournament;

        public BindableCollection<TournamentModel> ExistingTournaments
        {
            get { return _existingTournaments; }
            set { _existingTournaments = value; }
        }

        public TournamentModel SelectedTournament
        {
            get { return _selectedTournament; }
            set { 
                _selectedTournament = value;
                NotifyOfPropertyChange(() => SelectedTournament);
                LoadTournament();
            }
        }


    }
}
