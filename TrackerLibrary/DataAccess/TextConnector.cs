using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrackerLibrary.Models;
using TrackerLibrary.DataAccess.TextHelpers;

namespace TrackerLibrary.DataAccess
{
    public class TextConnector : IDataConnection
    {
        private const string PrizesFile = "PrizeModels.csv";
        //TODO - Make method actually save to the text dataase 
        public PriceModel CreatePrize(PriceModel model)
        {
            //Load the text file and Convert text to list<prize model>
            List<PriceModel> prizes = PrizesFile.FullFilePath().LoadFile().ConverttoPrizeModel();

            //Find the Max ID 
            int currentId = 1;
            if(prizes.Count > 0) { currentId = prizes.OrderByDescending(x => x.Id).First().Id + 1; }
            model.Id = currentId;
            
            //Add new record new ID - max + 1
            prizes.Add(model);

            //Convert prizes to list<string> and Save list of string to text file
            prizes.SavetoPrizeFile(PrizesFile);

            return model;
        }
    }
}
