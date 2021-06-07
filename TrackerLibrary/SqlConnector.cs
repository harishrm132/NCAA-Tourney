using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrackerLibrary
{
    public class SqlConnector : IDataConnection
    {
        //TODO - Make method actually save to the dataase
        /// <summary>
        /// Saves a prize to the database
        /// </summary>
        /// <param name="model"></param>
        /// <returns>The Prize Information. including the ID</returns>
        public PriceModel CreatePrize(PriceModel model)
        {
            model.Id = 1;

            return model;
        }
    }
}
