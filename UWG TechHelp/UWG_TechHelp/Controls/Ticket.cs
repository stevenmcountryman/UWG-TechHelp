using MvvmHelpers;
using System;
using System.Collections.Generic;
using UWG_TechHelp.Resources;
using Windows.UI.Xaml.Controls;

namespace UWG_TechHelp.Controls
{
    public class Ticket : ObservableObject
    {
        public Ticket()
        {
        }

        public string ticketNum { get; set; }
        public string ticketString { get { return "# " + ticketNum; } }

        public string firstName { get; set; }
        public string lastName { get; set; }
        
        public string DisplayName => $"{lastName}, {firstName}";
        public string name { get { return firstName + " " + lastName; } }

        public string roomNumber { get; set; }
        public string building { get; set; }
        public string location { get { return building + " " + roomNumber; } }

        public string lastUpdated { get; set; }
        public string lastUpdateTime {
            get
            {
                DateTime lastUpdateTime;
                if (DateTime.TryParse(lastUpdated, out lastUpdateTime))
                {
                    //return AppResources.getTimePassed(lastUpdateTime);
                }
                return "Unknown";
            }
        }

        public string title { get; set; }

        public string desc { get; set; }

        public string[] oldDescs { get; set; }

        public List<string> assignees { get; set; }
        public List<Agent> agents { get; set; }
        public List<Group> groups { get; set; }

        public string status { get; set; }

        public string priority;
        public string urgency { get { return getUrgency(); } }

        public string getUrgency()
        {
            switch (this.priority)
            {
                case "1":
                    return "Critical";
                case "2":
                    return "High";
                case "3":
                    return "Medium";
                case "4":
                    return "Normal";
                case "5":
                    return "Project";
                case "6":
                    return "Consultation";
                case "7":
                    return "Copyright";
                default:
                    return "Other";
            }
        }
    }
}
