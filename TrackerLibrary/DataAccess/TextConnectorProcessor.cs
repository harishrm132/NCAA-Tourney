using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrackerLibrary.Models;

namespace TrackerLibrary.DataAccess.TextHelpers
{
    public static class TextConnectorProcessor
    {
        public static string FullFilePath(this string filename)
        {
             return $"{ConfigurationManager.AppSettings.Get("filePath")}\\{filename}";
        }

        public static List<string> LoadFile(this string file)
        {
            if (!File.Exists(file))
            {
                return new List<string>();
            }
            return File.ReadAllLines(file).ToList();
        }

        public static List<PriceModel> ConverttoPrizeModel(this List<string> lines)
        {
            List<PriceModel> output = new List<PriceModel>();

            foreach (string line in lines)
            {
                string[] cols = line.Split(',');

                PriceModel p = new PriceModel();
                p.Id = int.Parse(cols[0]); 
                p.PlaceNumber = int.Parse(cols[1]); 
                p.PlaceName = cols[2]; 
                p.PriceAmount = decimal.Parse(cols[3]); 
                p.PricePercentage = double.Parse(cols[4]);
                
                output.Add(p);
            }
            return output;
        }

        public static List<PersonModel> ConverttoPersonModel(this List<string> lines)
        {
            List<PersonModel> output = new List<PersonModel>();

            foreach (string line in lines)
            {
                string[] cols = line.Split(',');

                PersonModel p = new PersonModel();
                p.Id = int.Parse(cols[0]);
                p.FirstName = cols[1];
                p.LastName = cols[2];
                p.EmailAdress = cols[3];
                p.CellphoneNumber = cols[4];

                output.Add(p);
            }

            return output;
        }

        public static List<TeamModel> ConverttoTeamModel(this List<string> lines, string personfilename)
        {
            //id, team name, list of ids seperated by pipes
            List<TeamModel> output = new List<TeamModel>();
            List<PersonModel> persons = personfilename.FullFilePath().LoadFile().ConverttoPersonModel();

            foreach (string line in lines)
            {
                string[] cols = line.Split(',');

                TeamModel t = new TeamModel();
                t.Id = int.Parse(cols[0]);
                t.TeamName = cols[1];

                string[] PersonIds = cols[2].Split('|');

                foreach (string id in PersonIds)
                {
                    t.TeamMembers.Add(persons.Where(x => x.Id == int.Parse(id)).First());
                }

                output.Add(t);
            }

            return output;
        }

        public static void SavetoPrizeFile(this List<PriceModel> models, string filename)
        {
            List<string> lines = new List<string>();

            foreach (PriceModel p in models)
            {
                lines.Add($"{p.Id},{p.PlaceNumber},{p.PlaceName},{p.PriceAmount},{p.PricePercentage}");
            }

            File.WriteAllLines(filename.FullFilePath(), lines);
        }

        public static void SavetoPersonFile(this List<PersonModel> models, string filename)
        {
            List<string> lines = new List<string>();

            foreach (PersonModel p in models)
            {
                lines.Add($"{p.Id},{p.FirstName},{p.LastName},{p.EmailAdress},{p.CellphoneNumber}");
            }

            File.WriteAllLines(filename.FullFilePath(), lines);
        }

        public static void SavetoTeamsFile(this List<TeamModel> models, string filename)
        {
            List<string> lines = new List<string>();

            foreach (TeamModel t in models)
            {
                lines.Add($"{t.Id},{t.TeamName},{ConvertPeopleListtosting(t.TeamMembers)}");
            }

            File.WriteAllLines(filename.FullFilePath(), lines);
        }

        private static string ConvertPeopleListtosting(List<PersonModel> person)
        {
            string output = "";
            if (person.Count == 0) return output;

            foreach (PersonModel p in person)
            {
                output += $"{p.Id}|";
            }

            output = output.Substring(0, output.Length - 1);
            return output;
        }
    }
}
