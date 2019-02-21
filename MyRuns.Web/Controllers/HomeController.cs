using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using MyRuns.Web.Auth;
using MyRuns.Web.Models;
using StravaSharp;

namespace MyRuns.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly IHttpContextAccessor _context;
        private IConfiguration _configuration;

        public HomeController(IHttpContextAccessor httpContextAccessor, IConfiguration configuration)
        {
            _context = httpContextAccessor;
            _configuration = configuration;
        }

        public async Task<IActionResult> Index()
        {
            var authenticator = CreateAuthenticator();
            var viewModel = new HomeViewModel(authenticator.IsAuthenticated);
            if (authenticator.IsAuthenticated)
            {
                var client = new Client(authenticator);
                var activities = await client.Activities.GetAthleteActivities();
                foreach (var activity in activities)
                {
                    viewModel.Activities.Add(new ActivityViewModel(activity));
                }
            }
            return View(viewModel);
        }

        Authenticator CreateAuthenticator()
        {
            var redirectUrl = $"{Request.Scheme}://{Request.Host.Host}:{Request.Host.Port}/Home/Callback";
            var config = new RestSharp.Portable.OAuth2.Configuration.RuntimeClientConfiguration
            {
                IsEnabled = false,
                ClientId = _configuration["ClientId"],
                ClientSecret = _configuration["ClientSecret"],
                RedirectUri = redirectUrl,
                Scope = "view_private",
            };
            var client = new StravaClient(new RequestFactory(), config);

            return new Authenticator(_context, client);
        }

        public async Task<ActionResult> List()
        {
            var authenticator = CreateAuthenticator();
            var loginUri = await authenticator.GetLoginLinkUri();

            return Redirect(loginUri.AbsoluteUri);
        }

        public async Task<ActionResult> Callback()
        {
            var authenticator = CreateAuthenticator();
            await authenticator.OnPageLoaded(new Uri(Request.GetDisplayUrl()));
            return RedirectToAction("Index");
        }
        public IActionResult Privacy()
        {
            return View();
        }
    }
}
