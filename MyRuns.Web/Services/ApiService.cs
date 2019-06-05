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
                var newActivites = client.Activities.GetAthleteActivities(page, 200).Result;
                if (newActivites.Count == 0)
                {
                    break;
                }

                activities.AddRange(newActivites);
                page++;

            }

            viewModel.AddRange(activities
                    .Where(r => FilterDistance(r.Distance, distanceType))
                    .OrderBy(r=>r.ElapsedTime)
                    .Select(r => new ActivityViewModel(r)));

            return viewModel;
        }

        private bool FilterDistance(float distance, DistanceType distanceType)
        {
            switch (distanceType)
            {
                case DistanceType.FiveKilometers:
                    return distance > 4500 && distance < 5500;
                case DistanceType.TenKilometers:
                    return distance > 9500 && distance < 10500;
                case DistanceType.HalfMarathon:
                    return distance > 210500 && distance < 211500;
                case DistanceType.Marathon:
                    return distance > 420500 && distance < 422500;
                default:
                    return false;
            }

        }
    }
}
