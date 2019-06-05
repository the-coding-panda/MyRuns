using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;

namespace MyRuns.Web.Models
{
    public class HomeViewModel
    {
        public HomeViewModel(bool isAuthenticated)
        {
            IsAuthenticated = isAuthenticated;
            Activities = new List<ActivityViewModel>();
        }

        public bool IsAuthenticated { get; private set; }

        public List<ActivityViewModel> Activities { get; set; }
    }
}
