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

        public void Handle(TournamentModel message)
        {
            //TODO - open the tournament viewer
            throw new NotImplementedException();
        }

        private BindableCollection<TournamentModel> _existingTournaments;

        public BindableCollection<TournamentModel> ExistingTournaments
        {
            get { return _existingTournaments; }
            set { _existingTournaments = value; }
        }

        private TournamentModel _selectedTournament;

        public TournamentModel SelectedTournament
        {
            get { return _selectedTournament; }
            set { 
                _selectedTournament = value;
                NotifyOfPropertyChange(() => SelectedTournament);
            }
        }


    }
}
