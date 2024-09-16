using System;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

namespace CertificationAnswers
{
    public class TotalGoals
    {
        //private static readonly HttpClient client = new HttpClient();
        private HttpClient client;

        public TotalGoals(HttpClient client)
        {
            this.client = client;
        }

        public async Task<int> GetGoalsForTeam(string team, int year, string teamPosition)
        {
            int goals = 0;
            int currentPage = 1;
            int totalPages;

            do
            {
                string url = $"https://jsonmock.hackerrank.com/api/football_matches?year={year}&{teamPosition}={team}&page={currentPage}";
                string responseBody = await client.GetStringAsync(url);
                var jsonResponse = JObject.Parse(responseBody);

                totalPages = jsonResponse["total_pages"].Value<int>();
                int perPage = jsonResponse["per_page"].Value<int>();

                var data = jsonResponse["data"] as JArray;
                if (data != null)
                {
                    foreach (var match in data)
                    {
                        string goalsKey = $"{teamPosition}goals";
                        if (match[goalsKey] != null && int.TryParse(match[goalsKey].ToString(), out int matchGoals))
                        {
                            goals += matchGoals;
                        }
                    }
                }

                currentPage++;
            } while (currentPage <= totalPages);

            return goals;
        }


    }
}