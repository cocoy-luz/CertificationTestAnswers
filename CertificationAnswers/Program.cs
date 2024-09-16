using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace CertificationAnswers
{
    public class Program
    {

        private static readonly HttpClient client = new HttpClient();
       
        public static async Task Main(string[] args)
        {
            

            string team = "Barcelona";
            int year = 2011;
            int goalsInTotal = await GetTotalGoals(team, year);
            Console.WriteLine($"Total goals for {team} in {year}: {goalsInTotal}");

        }

        public static async Task<int> GetTotalGoals(string team, int year)
        {
            TotalGoals totalGoals = new TotalGoals(client);

            int goalsInTotal = 0;

            goalsInTotal += await totalGoals.GetGoalsForTeam(team, year, "team1");
            goalsInTotal += await totalGoals.GetGoalsForTeam(team, year, "team2");

            return goalsInTotal;
        }

       
    }
}
