using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrackerLibrary.Models
{
    public class PersonModel
    {
        /// <summary>
        /// The unique Indentifier for prize
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Represents first name of person
        /// </summary>
        public string FirstName { get; set; }

        /// <summary>
        /// Represents last name of person
        /// </summary>
        public string LastName { get; set; }
        
        /// <summary>
        /// Represents email adress of person
        /// </summary>
        public string EmailAdress { get; set; }

        /// <summary>
        /// Represents phonenumber of person
        /// </summary>
        public string CellphoneNumber { get; set; }

        public string FullName
        {
            get { return $"{FirstName} {LastName}"; }
        }

    }
}
