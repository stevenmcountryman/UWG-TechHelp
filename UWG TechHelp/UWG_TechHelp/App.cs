using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UWG_TechHelp.Views;
using Xamarin.Forms;

namespace UWG_TechHelp
{
    public class App : Application
    {
        public App()
        {
            var navPage =
                    new NavigationPage(
                        new GlanceView_Page()
                        {
                            Title = "UWG TechHelp"                            
                        })
                    {
                        BarBackgroundColor = Color.FromHex("547799"),
                        BarTextColor = Color.White
                    };

            MainPage = navPage;
        }

        protected override void OnStart()
        {
            // Handle when your app starts
        }

        protected override void OnSleep()
        {
            // Handle when your app sleeps
        }

        protected override void OnResume()
        {
            // Handle when your app resumes
        }
    }
}
