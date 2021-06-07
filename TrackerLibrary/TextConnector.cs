using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrackerLibrary
{
    public class TextConnector : IDataConnection
    {
        //TODO - Make method actually save to the text dataase 
        public PriceModel CreatePrize(PriceModel model)
        {
            model.Id = 1;

            return model;
        }
    }
}
