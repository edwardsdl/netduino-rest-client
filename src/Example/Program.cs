using System;
using AngrySquirrel.Netduino.RestClient;

namespace Example
{
    /// <summary>
    /// Represents an example project showing how to use the <see cref="RestClient" /> library
    /// </summary>
    public class Program
    {
        #region Public Methods and Operators

        /// <summary>
        /// Program entry point
        /// </summary>
        public static void Main()
        {
            var restClient = new RestClient("stnapi", 8360);
            restClient.Post("/slideruns", "{ \"SnaggerId\": \"416\", \"OccurredOn\": \"" + DateTime.Now + "\", \"TimeInMs\": \"5000\" }");
        }

        #endregion
    }
}