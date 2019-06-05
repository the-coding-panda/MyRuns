using MyRuns.Web.Models;
using StravaSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyRuns.Web.Services
{
    public class ApiService
    {
        public IList<ActivityViewModel> GetActivities(Authenticator authenticator, DistanceType distanceType)
        {
            var viewModel = new List<ActivityViewModel>();

            var client = new Client(authenticator);

            var activities = new List<ActivitySummary>();

            int page = 1;

            while (true)
            {
                var newActivites = client.Activities.GetAthleteActivities(page, 50).Result;
                if (newActivites.Count == 0)
                {
                    break;
                }

                activities.AddRange(newActivites);
                page++;

            }

            viewModel.AddRange(activities
                    .Where(r => r.Distance > 9500 && r.Distance < 10500)
                    .OrderBy(r=>r.ElapsedTime)
                    .Select(r => new ActivityViewModel(r)));

            return viewModel;
        }
    }
}
