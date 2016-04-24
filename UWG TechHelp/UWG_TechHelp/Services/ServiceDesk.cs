using MvvmHelpers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using UWG_TechHelp.Controls;

namespace UWG_TechHelp.Resources
{
    public class ServiceDesk
    {
        /// <summary>
        /// Creates a new helpdesk ticket with specified user ID, title, and body
        /// </summary>
        /// <param name="userID">The user ID of the submitter</param>
        /// <param name="title">The title of the ticket</param>
        /// <param name="emailBody">The body of the ticket</param>
        /// <returns>Returns the ticket number if the ticket was created successfully. Otherwise returns null</returns>
        public static async Task<string> createIssue(string userID, string title, string emailBody)
        {
            try
            {
                var result = await getHTTPRequestResult("https://techhelp.westga.edu?createTicket&UserID=" + userID + "&Title=" + title + "&Desc=" + emailBody);
                var ticketNum = result.Substring(result.IndexOf("<span id=\"MainContent_Msg\">") + 27, 6);
                return ticketNum;
            }
            catch
            {
                return null;
            }
        }
        /// <summary>
        /// Edits an existing helpdesk ticket by way of its ticket number
        /// </summary>
        /// <param name="issueID">The ticket number to edit</param>
        /// <param name="desc">The new message for the ticket</param>
        /// <param name="status">The new status of the ticket</param>
        /// <param name="submitter">The submitter of the edit</param>
        /// <param name="assignees">The new assignees of the ticket</param>
        /// <returns>Returns a success message if the reply completed, otherwise returns a failure message</returns>
        public static async Task<string> editTicket(string issueID, string desc, string status, string submitter, string assignees)
        {
            try
            {
                var request = (HttpWebRequest)WebRequest.Create("https://techhelp.westga.edu?editTicket&IssueID=" + issueID + "&Desc=" + desc + "&Status=" + status + "&Submitter=" + submitter + "&Assignees=" + assignees);
                var response = (HttpWebResponse)(await request.GetResponseAsync());
                return "success";
            }
            catch
            {
                return "failed";
            }
        }
        /// <summary>
        /// Replies to an existing helpdesk ticket by way of its ticket number. (Similar to editing except defaulted to "Customer Responded")
        /// </summary>
        /// <param name="issueID">The ticket number to edit</param>
        /// <param name="desc">The new message of the ticket</param>
        /// <param name="submitter">The submitter of the reply</param>
        /// <returns>Returns a success message if the reply completed, otherwise returns a failure message</returns>
        public static async Task<string> replyTicket(string issueID, string desc, string submitter)
        {
            try
            {
                var request = (HttpWebRequest)WebRequest.Create("https://techhelp.westga.edu?editTicket&IssueID=" + issueID + "&Desc=" + desc + "&Status=Customer Responded" + "&Submitter=" + submitter);
                var response = (HttpWebResponse)(await request.GetResponseAsync());
                return "success";
            }
            catch
            {
                return "failed";
            }
        }
        /// <summary>
        /// Retrieves a list of all tickets assigned to or submitted by the specified user ID and group ID
        /// </summary>
        /// <param name="userID">The user ID that tickets are assigned to or submitted by</param>
        /// <param name="groupID">The group that tickets are assigned to</param>
        /// <returns>List of Tickets</returns>
        public static async Task<List<Ticket>> getAllTickets(string userID, string groupID)
        {
            try
            {
                var result = await getHTTPRequestResult("https://techhelp.westga.edu?getTickets&UserID=" + userID + "&GroupID=" + groupID.Replace(" ", "__b"));
                var details = result.Substring(result.IndexOf("<span id=\"MainContent_Msg\">") + 27).Replace("</b>", "").Replace("<br id=ticketbreak>", "NEWTICKET").Replace("<p>", "").Replace("</p>", "").Replace("<b>", "").Replace("&#58;", ":").Replace("&#34;", "\"").Replace("&#60;", "<").Replace("&#62;", ">").Replace("&#39;", "'").Replace("<br>", "&");
                var allDetails = details.Remove(details.IndexOf("&NEWTICKET</span></h2>")).Split(new string[] { "&NEWTICKET" }, StringSplitOptions.None);
                var allTickets = new List<Ticket>();
                if (allDetails != null && allDetails.Count() > 0)
                {
                    foreach (string ticket in allDetails)
                    {
                        var ticketData = ticket.Split(new string[] { "&KEY" }, StringSplitOptions.None);
                        Ticket newTicket = Ticket.ParseTicket(ticketData);
                        allTickets.Add(newTicket);
                    }
                    return allTickets;
                }
                else return null;
            }
            catch
            {
                return null;
            }
        }
        /// <summary>
        /// Retrieves a list of all tickets assigned to or submitted by the specified user ID and group ID
        /// </summary>
        /// <param name="userID">The user ID that tickets are assigned to or submitted by</param>
        /// <param name="groupID">The group that tickets are assigned to</param>
        /// <returns>List of Tickets</returns>
        public static async Task<List<Ticket>> getReport(string userID, string groupID, string start, string end)
        {
            try
            {
                var result = await getHTTPRequestResult("https://techhelp.westga.edu?getReport&UserID=" + userID + "&GroupID=" + groupID.Replace(" ", "__b") + "&start=" + start + "&end=" + end);
                var details = result.Substring(result.IndexOf("<span id=\"MainContent_Msg\">") + 27).Replace("</b>", "").Replace("<br id=ticketbreak>", "NEWTICKET").Replace("<p>", "").Replace("</p>", "").Replace("<b>", "").Replace("&#58;", ":").Replace("&#34;", "\"").Replace("&#60;", "<").Replace("&#62;", ">").Replace("&#39;", "'").Replace("<br>", "&");
                var allDetails = details.Remove(details.IndexOf("&NEWTICKET</span></h2>")).Split(new string[] { "&NEWTICKET" }, StringSplitOptions.None);
                var allTickets = new List<Ticket>();
                if (allDetails != null && allDetails.Count() > 0)
                {
                    foreach (string ticket in allDetails)
                    {
                        var keys = ticket.Split(new string[] { "&KEY" }, StringSplitOptions.None);
                        if (keys.Count() == 2)
                        {
                            string building = "";
                            string urgency = "";
                            foreach (string key in keys)
                            {
                                if (key.Contains("Building:")) building = key.Substring(key.IndexOf(":") + 1).Replace("__b", " ");
                                else if (key.Contains("Urgency:")) urgency = key.Substring(key.IndexOf(":") + 1).Replace("__f", " ");
                            }
                            var newTicket = new Ticket()
                            {
                                building = building,
                                priority = urgency
                            };
                            allTickets.Add(newTicket);
                        }
                    }
                    return allTickets;
                }
                else return null;
            }
            catch
            {
                return null;
            }
        }
        /// <summary>
        /// Gets information of the specified user ID
        /// </summary>
        /// <param name="userID"></param>
        /// <returns>List of user info such as: First Name, Last Name, Building, Room, Phone Number, etc.</returns>
        public static async Task<string[]> getUserInfo(string userID)
        {
            try
            {
                var result = await getHTTPRequestResult("https://techhelp.westga.edu?getUser&UserID=" + userID);
                var details = result.Substring(result.IndexOf("<span id=\"MainContent_Msg\">") + 27).Replace("</b>", "").Replace("<br id=ticketbreak>", "NEWTICKET").Replace("<p>", "").Replace("</p>", "").Replace("<b>", "").Replace("&#58;", ":").Replace("&#34;", "\"").Replace("&#60;", "<").Replace("&#62;", ">").Replace("&#39;", "'").Replace("<br>", "&");
                var allDetails = details.Remove(details.IndexOf("&NEWTICKET</span></h2>")).Split('&');
                return allDetails;
            }
            catch
            {
                return null;
            }
        }
        /// <summary>
        /// Gets a list of all agents in Numara
        /// </summary>
        /// <returns>List of agent user IDs</returns>
        public static async Task<List<Agent>> getAllAgents()
        {
            try
            {
                string agentURL = "https://docs.google.com/spreadsheets/d/1Ys_Mxgukp_m2aUe6ZTOwoYUfVYXe7lFlaWPUJBwncG0/pub?gid=0&single=true&output=csv"; // REPLACE THIS WITH YOUR URL
                var agentResult = await getHTTPRequestResult(agentURL);

                string[] agents = agentResult.Split(new string[] { "\r\n" }, StringSplitOptions.None);
                List<Agent> agentList = new List<Agent>();
                foreach (string agent in agents)
                {
                    string[] agentInfo = agent.Split(',');
                    Agent newAgent = new Agent()
                    {
                        UserName = agentInfo[0],
                        LastName = agentInfo[1],
                        FirstName = agentInfo[2]
                    };
                    agentList.Add(newAgent);
                }
                return agentList;
            }
            catch
            {
                return null;
            }
        }
        /// <summary>
        /// Gets a list of all groups in Numara
        /// </summary>
        /// <returns>List of group names</returns>
        public static async Task<List<Group>> getAllGroups()
        {
            try
            {
                string groupURL = "https://docs.google.com/spreadsheets/d/1Ys_Mxgukp_m2aUe6ZTOwoYUfVYXe7lFlaWPUJBwncG0/pub?gid=535266510&single=true&output=csv"; // REPLACE THIS WITH YOUR URL
                var groupResult = await getHTTPRequestResult(groupURL);

                string[] groups = groupResult.Split(new string[] { "\r\n" }, StringSplitOptions.None);
                List<Group> groupList = new List<Group>();
                foreach (string group in groups)
                {
                    Group newGroup = new Group()
                    {
                        Name = group
                    };
                    groupList.Add(newGroup);
                }
                return groupList;
            }
            catch
            {
                return null;
            }
        }
        /// <summary>
        /// Gets details of a ticket via the specified ticket number
        /// </summary>
        /// <param name="issueID">The ticket number</param>
        /// <returns>Ticket information such as: Submitter, last update time, latest message, etc.</returns>
        public static async Task<string> getIssueDetails(string issueID)
        {
            try
            {
                var result = await getHTTPRequestResult("https://techhelp.westga.edu?queryTicket&IssueID=" + issueID);
                var details = result.Substring(result.IndexOf("<span id=\"MainContent_Msg\">") + 27).Replace("</b>", "").Replace("<b>", "").Replace("&#58;", ":").Replace("<br>", "&");
                details = details.Remove(details.IndexOf("&</span></h2>"));
                return details;
            }
            catch
            {
                return null;
            }
        }
        /// <summary>
        /// Gets all UWG service announcements
        /// </summary>
        /// <returns>Returns a list of all announcements related to current UWG services</returns>
        public static async Task<List<string>> getAllServiceStatusAnnouncements()
        {
            try
            {
                var result = await getHTTPRequestResult("https://apps.westga.edu/sd/share/get_announcements.php");
                var statusJSON = result.Replace("[", "").Replace("]", "").Replace("\"", "").Replace("\\", "").Replace("}", "");
                var allAnnouncements = statusJSON.Split('{').ToList<string>();
                return allAnnouncements;
            }
            catch
            {
                return null;
            }
        }
        /// <summary>
        /// Gets all current UWG service statuses, whether they are running or not
        /// </summary>
        /// <returns>Returns a list of all services and their statuses</returns>
        public static async Task<List<Service>> getServices(List<string> statuses)
        {
            try
            {
                var result = await getHTTPRequestResult("https://apps.westga.edu/sd/share/get_statuses.php");
                var statusJSON = result.Replace("[", "").Replace("]", "").Replace("\"", "").Replace("\\", "").Replace("}", "");
                var services = statusJSON.Split('{').ToList<string>();
                services.Remove("");

                var allServices = new List<Service>();
                if (services != null && services.Count() > 0)
                {
                    foreach (string service in services)
                    {
                        Service newService = Service.ParseService(service, statuses);
                        allServices.Add(newService);
                    }
                    return allServices;
                }
                else return null;
            }
            catch
            {
                return null;
            }
        }

        private static async Task<string> getHTTPRequestResult(string URL)
        {
            var request = (HttpWebRequest)WebRequest.Create(URL);
            var response = (HttpWebResponse)(await request.GetResponseAsync());
            string result = String.Empty;
            using (var reader = new StreamReader(response.GetResponseStream()))
            {
                result = reader.ReadToEnd();
            }
            return result;
        }
    }
}
