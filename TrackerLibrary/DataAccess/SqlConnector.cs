using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrackerLibrary.Models;
	//@PlaceNumber int, 
	//@PlaceName nvarchar(50), 
	//@PrizeAmount money,
 //   @PrizePercentage float,
 //   @Id int = 0 output

namespace TrackerLibrary.DataAccess
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
            using (IDbConnection connection = new System.Data.SqlClient.SqlConnection(GlobalConfig.CnnString("NCAATourney")) )
            {
                var p = new DynamicParameters();
                p.Add("@PlaceNumber", model.PlaceNumber);
                p.Add("@PlaceName", model.PlaceName);
                p.Add("@PrizeAmount", model.PriceAmount);
                p.Add("@PrizePercentage", model.PricePercentage);
                p.Add("@Id", 0, dbType: DbType.Int32, direction: ParameterDirection.Output );

                connection.Execute("dbo.spPrizes_Insert", p, commandType: CommandType.StoredProcedure);

                model.Id = p.Get<int>("@Id");

                return model;
            }
        }
    }
}
