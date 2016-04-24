using MvvmHelpers;
using System.Threading.Tasks;
using UWG_TechHelp.Controls;
using UWG_TechHelp.Resources;
using Xamarin.Forms;

namespace UWG_TechHelp.Models
{
    public class TicketsViewModel : BaseNavigationViewModel
    {
        public TicketsViewModel()
        {
        }

        ObservableRangeCollection<Ticket> _Tickets;

        Command _LoadTicketsCommand;

        Command _RefreshTicketsCommand;

        public ObservableRangeCollection<Ticket> Tickets
        {
            get { return _Tickets ?? (_Tickets = new ObservableRangeCollection<Ticket>()); }
            set
            {
                _Tickets = value;
                OnPropertyChanged("Tickets");
            }
        }

        /// <summary>
        /// Command to load acquaintances
        /// </summary>
        public Command LoadTicketsCommand
        {
            get { return _LoadTicketsCommand ?? (_LoadTicketsCommand = new Command(async () => await ExecuteLoadAcquaintancesCommand())); }
        }

        async Task ExecuteLoadAcquaintancesCommand()
        {
            if (Tickets.Count < 1)
            {
                LoadTicketsCommand.ChangeCanExecute();

                await FetchTickets();

                LoadTicketsCommand.ChangeCanExecute();
            }
        }

        public Command RefreshAcquaintancesCommand
        {
            get
            {
                return _RefreshTicketsCommand ?? (_RefreshTicketsCommand = new Command(async () => await ExecuteRefreshTicketsCommandCommand()));
            }
        }

        async Task ExecuteRefreshTicketsCommandCommand()
        {
            RefreshAcquaintancesCommand.ChangeCanExecute();

            await FetchTickets();

            RefreshAcquaintancesCommand.ChangeCanExecute();
        }

        async Task FetchTickets()
        {
            IsBusy = true;

            var tickets = await ServiceDesk.getAllTickets("scountry", "CS COE");

            if (tickets != null)
            {
                Tickets.Clear();

                Tickets.AddRange(tickets);
            }

            IsBusy = false;
        }
    }
}
