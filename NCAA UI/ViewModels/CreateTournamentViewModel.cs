using Caliburn.Micro;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrackerLibrary.Models;

namespace NCAA_UI.ViewModels
{
    public class CreateTournamentViewModel : Conductor<object>.Collection.AllActive
    {
        private string _tournamentName;
        private decimal _entryFee;
        private BindableCollection<TeamModel> _avaliableTeams;
        private TeamModel _selectedTeamToAdd;
        private BindableCollection<TeamModel> _selectedTeams;
        private TeamModel _selectedTeamToRemove;
        private Screen _activeAddTeamView;
        private BindableCollection<PriceModel> _selectedPrizes;
        private PriceModel _selectedPrizeToRemove;
        private Screen _activeAddPrizeView;

        public string TournamentName
        {
            get { return _tournamentName; }
            set 
            { 
                _tournamentName = value;
                NotifyOfPropertyChange(() => TournamentName);
            }
        }

        public decimal EntryFee
        {
            get { return _entryFee; }
            set 
            { 
                _entryFee = value;
                NotifyOfPropertyChange(() => EntryFee);
            }
        }

        public BindableCollection<TeamModel> AvaliableTeams
        {
            get { return _avaliableTeams; }
            set { _avaliableTeams = value; }
        }

        public TeamModel SelectedTeamToAdd
        {
            get { return _selectedTeamToAdd; }
            set 
            { 
                _selectedTeamToAdd = value;
                NotifyOfPropertyChange(() => SelectedTeamToAdd);
            }
        }

        public BindableCollection<TeamModel> SelectedTeams
        {
            get { return _selectedTeams; }
            set 
            { 
                _selectedTeams = value;
                NotifyOfPropertyChange(() => SelectedTeams);
            }
        }

        public TeamModel SelectedTeamToRemove
        {
            get { return _selectedTeamToRemove; }
            set 
            { 
                _selectedTeamToRemove = value;
                NotifyOfPropertyChange(() => SelectedTeamToRemove);
            }
        }

        public Screen ActiveAddTeamView
        {
            get { return _activeAddTeamView; }
            set 
            { 
                _activeAddTeamView = value;
                NotifyOfPropertyChange(() => ActiveAddTeamView);
            }
        }

        public BindableCollection<PriceModel> SelectedPrizes
        {
            get { return _selectedPrizes; }
            set { _selectedPrizes = value; }
        }

        public PriceModel SelectedPrizeToRemove
        {
            get { return _selectedPrizeToRemove; }
            set 
            { 
                _selectedPrizeToRemove = value;
                NotifyOfPropertyChange(() => SelectedPrizeToRemove);
            }
        }

        public Screen ActiveAddPrizeView
        {
            get { return _activeAddPrizeView; }
            set 
            { 
                _activeAddPrizeView = value;
                NotifyOfPropertyChange(() => ActiveAddPrizeView);
            }
        }

        public bool CanAddTeam()
        {
            return SelectedTeamToAdd != null;
        }
        
        public void AddTeam()
        {

        }

        public void CreateTeam()
        {

        }

        public bool CanRemoveTeam()
        {
            return SelectedPrizeToRemove != null;
        }
        
        public void RemoveTeam()
        {

        }

        public void CreatePrize()
        {

        }

        public bool CanRemovePrize()
        {
            return SelectedPrizeToRemove != null;
        }
        
        public void RemovePrize()
        {

        }

        public bool CanCreateTournament()
        {
            //TODDO - Add Logic for creating tournms
            return true;
        }
        
        public void CreateTournament()
        {

        }
    }
}
