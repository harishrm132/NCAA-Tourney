using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using TrackerLibrary;
using TrackerLibrary.Models;

namespace TrackerUI
{
    public partial class CreateTeamForm : Form
    {
        private List<PersonModel> AvaliableTeamMembers = GlobalConfig.Connection.GetPerson_All();
        private List<PersonModel> SelectedTeamMembers = new List<PersonModel>();

        public CreateTeamForm()
        {
            InitializeComponent();
            //CreateSamaple();
            WireUpLists();
        }

        private void CreateSamaple()
        {
            AvaliableTeamMembers.Add(new PersonModel { FirstName = "dasas", LastName = "sasq" });
            AvaliableTeamMembers.Add(new PersonModel { FirstName = "asd", LastName = "wer" });

            SelectedTeamMembers.Add(new PersonModel { FirstName = "Hari", LastName = "Prasa" });
            SelectedTeamMembers.Add(new PersonModel { FirstName = "aahi", LastName = "ram" });
        }

        private void WireUpLists()
        {

            SelectTeamMemberDropDown.DataSource = null;
            SelectTeamMemberDropDown.DataSource = AvaliableTeamMembers;
            SelectTeamMemberDropDown.DisplayMember = "FullName";

            TeamMemberslistBox.DataSource = null;
            TeamMemberslistBox.DataSource = SelectedTeamMembers;
            TeamMemberslistBox.DisplayMember = "FullName";
        }

        private void CreateMemberbutton_Click(object sender, EventArgs e)
        {
            if (ValidateForm())
            {
                PersonModel p = new PersonModel();
                p.FirstName = FirstNameValue.Text;
                p.LastName = LastNameValue.Text;
                p.EmailAdress = EmailValue.Text;
                p.CellphoneNumber = PhoneNoValue.Text;

                p = GlobalConfig.Connection.CreatePerson(p);
                SelectedTeamMembers.Add(p);
                WireUpLists();

                FirstNameValue.Text = "";
                LastNameValue.Text = "";
                EmailValue.Text = "";
                PhoneNoValue.Text = "";
            }
            else
            {
                MessageBox.Show("Forms has invalid information", "Input Validation");
            }
        }

        private bool ValidateForm()
        {
            if (FirstNameValue.Text.Length == 0)
            {
                return false;
            }
            if (LastNameValue.Text.Length == 0)
            {
                return false;
            }
            if (EmailValue.Text.Length == 0)
            {
                return false;
            }
            if (PhoneNoValue.Text.Length == 0)
            {
                return false;
            }


            return true;
        }

        private void AddMemberbutton_Click(object sender, EventArgs e)
        {
            PersonModel p = (PersonModel)SelectTeamMemberDropDown.SelectedItem;

            if (p != null)
            {
                AvaliableTeamMembers.Remove(p);
                SelectedTeamMembers.Add(p);
                WireUpLists();
            }
        }

        private void RemoveTeamMembersbutton_Click(object sender, EventArgs e)
        {
            PersonModel p = (PersonModel)TeamMemberslistBox.SelectedItem;

            if(p != null)
            {
                SelectedTeamMembers.Remove(p);
                AvaliableTeamMembers.Add(p);
                WireUpLists();
            }
        }

        private void CreateTeambutton_Click(object sender, EventArgs e)
        {
            TeamModel t = new TeamModel();

            t.TeamName = TeamNameValue.Text;
            t.TeamMembers = SelectedTeamMembers;
            t = GlobalConfig.Connection.CreateTeam(t);

            // TODO - If we didnt close this form after creation, reset the form
        }
    }
}
