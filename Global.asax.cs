using DAL;
using EmailHandling;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Timers;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace WebApplication
{
    public class MvcApplication : System.Web.HttpApplication
    {
        private static Timer appTimer;

        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            var culture = new CultureInfo("fr-FR");
            CultureInfo.DefaultThreadCurrentCulture = culture;
            CultureInfo.DefaultThreadCurrentUICulture = culture;

            // DB.Users.ResetAllUsersOnlineStatus(); obselete

            appTimer = new Timer();
            appTimer.Interval = 10000000; // 10 second in milliseconds
            appTimer.Elapsed += new ElapsedEventHandler(OnTimerElapsed);
            appTimer.Enabled = true; // Start the timer

            // cleaning
            foreach (var login in DB.Logins.ToList().Copy())
            {
                if (login.User == null) DB.Logins.Delete(login.Id);
            }
            foreach (var uvEmail in DB.UnverifiedEmails.ToList().Copy())
            {
                if (uvEmail.User == null) DB.UnverifiedEmails.Delete(uvEmail.Id);
            }
            foreach (RenewPasswordCommand renewPC in DB.RenewPasswordCommands.ToList().Copy())
            {
                if (renewPC.User == null) DB.RenewPasswordCommands.Delete(renewPC.Id);
            }
            foreach (Models.Event @event in DB.Events.ToList().Copy())
            {
                if (@event.User == null) DB.RenewPasswordCommands.Delete(@event.Id);
            }
            foreach (var notification in DB.Notifications.ToList().Copy())
            {
                if (notification.User == null || notification.User == null) DB.Notifications.Delete(notification.Id);
            }
        }

        private static void OnTimerElapsed(object sender, ElapsedEventArgs e)
        {
            // Your cyclic function code goes here
            // Be careful with threading, as this runs on a thread pool thread
            System.Diagnostics.Debug.WriteLine("Cyclic function ran at: " + DateTime.Now);
            
        }
        protected void Session_Start()
        {
            // do session intialisations
        }
        protected void Session_End()
        {
            var connectedUser = Models.User.ConnectedUser;
            if (connectedUser != null)
                connectedUser.Online = false;
        }
        protected void Application_End(object sender, EventArgs e)
        {
            if (appTimer != null)
            {
                appTimer.Enabled = false;
                appTimer.Dispose();
            }
        }
    }
}
