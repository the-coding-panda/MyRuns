using Microsoft.Extensions.Caching.Memory;
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
        private readonly IMemoryCache _cache;

        public ApiService(IMemoryCache cache)
        {
            _cache = cache;
        }
        
        public IList<ActivityViewModel> GetActivities(Authenticator authenticator, DistanceType distanceType, bool includeTreadmill)
        {
            var viewModel = new List<ActivityViewModel>();

            var client = new Client(authenticator);
            List<ActivitySummary> activities = GetActivities(client);

            viewModel.AddRange(activities
                    .Where(r => FilterDistance(r.Distance, distanceType))
                    .Where(r=>r.Trainer == includeTreadmill)
                    .OrderBy(r => r.ElapsedTime)
                    .Take(10)
                    .Select(r => new ActivityViewModel(r)));

            return viewModel;
        }

        private List<ActivitySummary> GetActivities(Client client)
        {
            if (_cache.TryGetValue($"Activities", out List<ActivitySummary> cachedActivities))
            {
                return cachedActivities;
            }

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
            _cache.Set<List<ActivitySummary>>("Activities", activities);
            return activities;
        }

        private bool FilterDistance(float distance, DistanceType distanceType)
        {
            switch (distanceType)
            {
                case DistanceType.FiveKilometers:
                    return distance > 4800 && distance < 5500;
                case DistanceType.TenKilometers:
                    return distance > 9800 && distance < 10500;
                case DistanceType.HalfMarathon:
                    return distance > 21000 && distance < 21500;
                case DistanceType.Marathon:
                    return distance > 42000 && distance < 42250;
                default:
                    return false;
            }

        }
    }
}
