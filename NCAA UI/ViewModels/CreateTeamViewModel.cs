﻿using Caliburn.Micro;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrackerLibrary;
using TrackerLibrary.Models;

namespace NCAA_UI.ViewModels
{
    public class CreateTeamViewModel : Conductor<object>, IHandle<PersonModel>
    {
        private string _teamName = "";
        private BindableCollection<PersonModel> _avaliableTeamMembers;
        private PersonModel _selectedTeamMemberToAdd;
        private BindableCollection<PersonModel> _selectedTeamMembers = new BindableCollection<PersonModel>();
        private PersonModel _selectedTeamMemberToRemove;
        private bool _selectedTeamMembersIsVisible = true;
        private bool _addPersonIsVisible = false;

        public CreateTeamViewModel()
        {
            AvaliableTeamMembers = new BindableCollection<PersonModel>(GlobalConfig.Connection.GetPerson_All());
            EventAggregationProvider.TrackerEventAggregator.Subscribe(this);
        }

        public string TeamName
        {
            get { return _teamName; }
            set 
            { 
                _teamName = value;
                NotifyOfPropertyChange(() => TeamName);
                NotifyOfPropertyChange(() => CanCreateTeam);
            }
        }


        public bool SelectedTeamMembersIsVisible
        {
            get { return _selectedTeamMembersIsVisible; }
            set 
            { 
                _selectedTeamMembersIsVisible = value;
                NotifyOfPropertyChange(() => SelectedTeamMembersIsVisible);
            }
        }


        public bool AddPersonIsVisible
        {
            get { return _addPersonIsVisible; }
            set 
            { 
                _addPersonIsVisible = value;
                NotifyOfPropertyChange(() => AddPersonIsVisible);
            }
        }

        public BindableCollection<PersonModel> AvaliableTeamMembers
        {
            get { return _avaliableTeamMembers; }
            set 
            { 
                _avaliableTeamMembers = value;
            }
        }

        public PersonModel SelectedTeamMemberToAdd
        {
            get { return _selectedTeamMemberToAdd; }
            set 
            { 
                _selectedTeamMemberToAdd = value;
                NotifyOfPropertyChange(() => SelectedTeamMemberToAdd);
                NotifyOfPropertyChange(() => CanAddMember);
            }
        }

        public BindableCollection<PersonModel> SelectedTeamMembers
        {
            get { return _selectedTeamMembers; }
            set 
            { 
                _selectedTeamMembers = value;
                NotifyOfPropertyChange(() => CanCreateTeam);
            }
        }

        public PersonModel SelectedTeamMemberToRemove
        {
            get { return _selectedTeamMemberToRemove; }
            set 
            { 
                _selectedTeamMemberToRemove = value;
                NotifyOfPropertyChange(() => SelectedTeamMemberToRemove);
                NotifyOfPropertyChange(() => CanRemoveMember);
            }
        }

        public bool CanAddMember
        {
            get 
            { 
                return SelectedTeamMemberToAdd != null;
            }
        }

        public void AddMember()
        {
            SelectedTeamMembers.Add(SelectedTeamMemberToAdd);
            AvaliableTeamMembers.Remove(SelectedTeamMemberToAdd);
            NotifyOfPropertyChange(() => CanCreateTeam);
        }
        
        public void CreateMember()
        {
            ActivateItem(new CreatePersonViewModel());
            SelectedTeamMembersIsVisible = false;
            AddPersonIsVisible = true;
        }
        
        public bool CanRemoveMember
        {
            get
            {
                return SelectedTeamMemberToRemove != null;
            }
        }
        
        public void RemoveMember()
        {
            AvaliableTeamMembers.Add(SelectedTeamMemberToRemove);
            SelectedTeamMembers.Remove(SelectedTeamMemberToRemove);
            NotifyOfPropertyChange(() => CanCreateTeam);
        }

        public void CancelCreation()
        {
            EventAggregationProvider.TrackerEventAggregator.PublishOnUIThread(new TeamModel());
            this.TryClose();
        }

        public bool CanCreateTeam
        {
            get
            {
                if (SelectedTeamMembers != null)
                {
                    if (TeamName.Length > 0 && SelectedTeamMembers.Count > 0) return true;
                    else return false;
                }
                else return false;
            }
        }
        
        public void CreateTeam()
        {
            TeamModel t = new TeamModel();

            t.TeamName = TeamName;
            t.TeamMembers = SelectedTeamMembers.ToList();
            GlobalConfig.Connection.CreateTeam(t);

            EventAggregationProvider.TrackerEventAggregator.PublishOnUIThread(t);
            this.TryClose();
        }

        public void Handle(PersonModel message)
        {
            if (!string.IsNullOrWhiteSpace(message.FullName))
            {
                SelectedTeamMembers.Add(message);
                NotifyOfPropertyChange(() => CanCreateTeam);
            }
            SelectedTeamMembersIsVisible = true;
            AddPersonIsVisible = false;
        }
    }
}
