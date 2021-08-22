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
    public class CreateTournamentViewModel : Conductor<object>.Collection.AllActive, IHandle<TeamModel>, IHandle<PriceModel>
    {
        private string _tournamentName = "";
        private decimal _entryFee;
        private BindableCollection<TeamModel> _avaliableTeams;
        private TeamModel _selectedTeamToAdd;
        private BindableCollection<TeamModel> _selectedTeams = new BindableCollection<TeamModel>();
        private TeamModel _selectedTeamToRemove;
        private Screen _activeAddTeamView;
        private BindableCollection<PriceModel> _selectedPrizes = new BindableCollection<PriceModel>();
        private PriceModel _selectedPrizeToRemove;
        private Screen _activeAddPrizeView;
        private bool _selectedTeamsIsVisible = true;
        private bool _addTeamIsVisible = false;
        private bool _selectedPrizesIsVisible = true;
        private bool _addPrizeIsVisible = false;

        public CreateTournamentViewModel()
        {
            AvaliableTeams = new BindableCollection<TeamModel>(GlobalConfig.Connection.GetTeam_All());
            EventAggregationProvider.TrackerEventAggregator.Subscribe(this);
        }

        public string TournamentName
        {
            get { return _tournamentName; }
            set 
            { 
                _tournamentName = value;
                NotifyOfPropertyChange(() => TournamentName);
                NotifyOfPropertyChange(() => CanCreateTournament);
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
                NotifyOfPropertyChange(() => CanAddTeam);
            }
        }

        public BindableCollection<TeamModel> SelectedTeams
        {
            get { return _selectedTeams; }
            set 
            { 
                _selectedTeams = value;
                NotifyOfPropertyChange(() => SelectedTeams);
                NotifyOfPropertyChange(() => CanCreateTournament);
            }
        }

        public TeamModel SelectedTeamToRemove
        {
            get { return _selectedTeamToRemove; }
            set 
            { 
                _selectedTeamToRemove = value;
                NotifyOfPropertyChange(() => SelectedTeamToRemove);
                NotifyOfPropertyChange(() => CanRemoveTeam);
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
                NotifyOfPropertyChange(() => CanRemovePrize);
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

        public bool SelectedTeamsIsVisible
        {
            get { return _selectedTeamsIsVisible; }
            set 
            { 
                _selectedTeamsIsVisible = value;
                NotifyOfPropertyChange(() => SelectedTeamsIsVisible);
            }
        }

        public bool AddTeamIsVisible
        {
            get { return _addTeamIsVisible; }
            set 
            {
                _addTeamIsVisible = value;
                NotifyOfPropertyChange(() => AddTeamIsVisible);
            }
        }

        public bool SelectedPrizesIsVisible
        {
            get { return _selectedPrizesIsVisible; }
            set 
            { 
                _selectedPrizesIsVisible = value;
                NotifyOfPropertyChange(() => SelectedPrizesIsVisible);
            }
        }

        public bool AddPrizeIsVisible
        {
            get { return _addPrizeIsVisible; }
            set 
            { 
                _addPrizeIsVisible = value;
                NotifyOfPropertyChange(() => AddPrizeIsVisible);
            }
        }

        public bool CanAddTeam
        {
            get
            {
                return SelectedTeamToAdd != null;
            }
        }

        public void AddTeam()
        {
            SelectedTeams.Add(SelectedTeamToAdd);
            AvaliableTeams.Remove(SelectedTeamToAdd);
            NotifyOfPropertyChange(() => CanCreateTournament);
        }

        public void CreateTeam()
        {
            ActiveAddTeamView = new CreateTeamViewModel();
            Items.Add(ActiveAddTeamView);
            
            SelectedTeamsIsVisible = false;
            AddTeamIsVisible = true;
        }

        public bool CanRemoveTeam
        {
            get
            {
                return SelectedTeamToRemove != null;
            }
        }

        public void RemoveTeam()
        {
            AvaliableTeams.Add(SelectedTeamToRemove);
            SelectedTeams.Remove(SelectedTeamToRemove);
            NotifyOfPropertyChange(() => CanCreateTournament);
        }

        public void CreatePrize()
        {
            ActiveAddPrizeView = new CreatePrizeFormViewModel();
            Items.Add(ActiveAddPrizeView);

            SelectedPrizesIsVisible = false;
            AddPrizeIsVisible = true;
        }

        public bool CanRemovePrize
        {
            get
            {
                return SelectedPrizeToRemove != null;
            }
        }

        public void RemovePrize()
        {
            SelectedPrizes.Remove(SelectedPrizeToRemove);
        }

        public bool CanCreateTournament
        {
            get
            {
                if(TournamentName.Length > 0 && SelectedTeams.Count > 1)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }

        public void CreateTournament()
        {
            //Create Tournament model
            TournamentModel tm = new TournamentModel();
            tm.TournamentName = TournamentName;
            tm.EntryFee = EntryFee;
            tm.Prices = SelectedPrizes.ToList();
            tm.EnteredTeams = SelectedTeams.ToList();

            // Create Matchups
            TournamentLogic.CreateRounds(tm);

            // Create Tournament entry
            // Create all of the prize entries
            // Create all team entries
            GlobalConfig.Connection.CreateTournaments(tm);
            //mail
            //tm.AlertUsersonRounds();

            EventAggregationProvider.TrackerEventAggregator.PublishOnUIThread(tm);
            this.TryClose();
        }

        public void Handle(TeamModel message)
        {
            if (!string.IsNullOrWhiteSpace(message.TeamName))
            {
                SelectedTeams.Add(message);
                NotifyOfPropertyChange(() => CanCreateTournament); 
            }

            SelectedTeamsIsVisible = true;
            AddTeamIsVisible = false;
        }

        public void Handle(PriceModel message)
        {
            if (!string.IsNullOrWhiteSpace(message.PlaceName))
            {
                SelectedPrizes.Add(message);
            }

            SelectedPrizesIsVisible = true;
            AddPrizeIsVisible = false;
        }
    }
}
