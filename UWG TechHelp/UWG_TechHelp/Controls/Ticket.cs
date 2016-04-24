using MvvmHelpers;
using System;
using System.Collections.Generic;
using System.Linq;

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

        public static Ticket ParseTicket(string[] ticketData)
        {
            string building = "";
            string firstName = "";
            string lastName = "";
            string[] oldDescriptions = new string[10];
            string description = "";
            string[] assignees = new string[10];
            string ticketnum = "";
            string urgency = "";
            string status = "";
            string title = "";
            string lastUpdate = "";
            string roomNumber = "";
            foreach (string key in ticketData)
            {
                if (key.Contains("Building:")) building = key.Substring(key.IndexOf(":") + 1).Replace("__b", " ");
                else if (key.Contains("FirstName:")) firstName = key.Substring(key.IndexOf(":") + 1);
                else if (key.Contains("LastName:")) lastName = key.Substring(key.IndexOf(":") + 1);
                else if (key.Contains("Descriptions:"))
                {
                    oldDescriptions = key.Substring(key.IndexOf(":") + 1).Replace("&nbsp;", "\n").Replace("\r", "").Replace("\n\n", "\n").Replace("<br />", "\n").Replace("<! ", "<!").Split(new string[] { "<!" }, StringSplitOptions.None);
                    for (int i = 1; i < oldDescriptions.Count(); i++)
                    {
                        oldDescriptions[i] = oldDescriptions[i].Replace("\n\n\n", "\n").Replace("\n\n", "\n").Replace("--/*SC*/ ", "").Replace(" /*EC*/-->", ":").Replace(" >", ":");
                        oldDescriptions[i] = oldDescriptions[i].Substring(0, 19) + " - " + oldDescriptions[i].Substring(19);
                    }
                    oldDescriptions = oldDescriptions.Reverse<string>().ToArray<string>();
                }
                else if (key.Contains("Assignees:")) assignees = key.Substring(key.IndexOf(":") + 1).Split(' ');
                else if (key.Contains("Description:"))
                {
                    description = key.Substring(key.IndexOf("\n") + 2).Replace("&nbsp;", "\n").Replace("\r", "").Replace("\n\n", "\n").Replace("<br />", "\n");
                    while (description.Length > 0 && description.Substring(0, 1) == "\n") description = description.Substring(1);
                }
                else if (key.Contains("Issue Number:")) ticketnum = key.Substring(key.IndexOf(":") + 1);
                else if (key.Contains("Urgency:")) urgency = key.Substring(key.IndexOf(":") + 1).Replace("__f", " ");
                else if (key.Contains("Status:")) status = key.Substring(key.IndexOf(":") + 1).Replace("__b", " ");
                else if (key.Contains("Title:")) title = key.Substring(key.IndexOf(":") + 1);
                else if (key.Contains("LastUpdated:")) lastUpdate = key.Substring(key.IndexOf(":") + 1);
                else if (key.Contains("Room:")) roomNumber = key.Substring(key.IndexOf(":") + 1);
            }
            return new Ticket()
            {
                firstName = firstName,
                lastName = lastName,
                roomNumber = roomNumber,
                building = building,
                lastUpdated = lastUpdate,
                desc = description,
                oldDescs = oldDescriptions,
                status = status,
                title = title,
                ticketNum = ticketnum,
                priority = urgency,
                assignees = assignees.ToList()
            };
        }
    }
}
