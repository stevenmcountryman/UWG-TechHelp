using MvvmHelpers;
using System.Collections.Generic;

namespace UWG_TechHelp.Controls
{
    public class Service : ObservableObject
    {
        public Service()
        {
        }

        public int recordID
        {
            get;
            set;
        }
        public string platform
        {
            get;
            set;
        }
        public bool isRunning
        {
            get;
            set;
        }
        public int priority
        {
            get;
            set;
        }
        public List<string> announcements
        {
            get; set;
        }

        public string status
        {
            get
            {
                if (this.isRunning && (this.announcements == null || this.announcements.Count == 0))
                {
                    return "No Issues";

                }
                else if (this.isRunning)
                {
                    return "Annoucements";
                }
                else
                {
                    return "View Issues";
                }
            }
        }

        public static Service ParseService(string service, List<string> allAnnouncements)
        {
            int recordID;
            var platform = "";
            int priority;
            bool isRunning = false;
            var announcements = new List<string>();

            var serviceInfo = service.Split(',');
            recordID = int.Parse(serviceInfo[0].Substring(serviceInfo[0].IndexOf(':') + 1));
            platform = serviceInfo[1].Substring(serviceInfo[1].IndexOf(':') + 1);

            var status = serviceInfo[2].Substring(serviceInfo[2].IndexOf(':') + 1);
            if (status == "Green") isRunning = true;
            else isRunning = false;

            priority = int.Parse(serviceInfo[3].Substring(serviceInfo[3].IndexOf(':') + 1));

            try
            {
                foreach (var announcement in allAnnouncements)
                {
                    if (announcement.Contains("affected_service_id:" + recordID + ",") && !announcement.Contains("current_status:Archived"))
                    {
                        var ann = announcement.Split(',')[3];
                        announcements.Add(ann.Substring(ann.IndexOf(':') + 1));
                    }
                }
            }
            catch
            {
            }

            return new Service()
            {
                recordID = recordID,
                platform = platform,
                priority = priority,
                isRunning = isRunning,
                announcements = announcements
            };
        }
    }
}
