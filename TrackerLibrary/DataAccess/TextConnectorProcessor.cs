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

        public static void SavetoPrizeFile(this List<PriceModel> models, string filename)
        {
            List<string> lines = new List<string>();

            foreach (PriceModel p in models)
            {
                lines.Add($"{p.Id},{p.PlaceNumber},{p.PlaceName},{p.PriceAmount},{p.PricePercentage}");
            }

            File.WriteAllLines(filename.FullFilePath(), lines);
        }
    }
}
