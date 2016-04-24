using MvvmHelpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UWG_TechHelp.Controls;
using UWG_TechHelp.Resources;
using Xamarin.Forms;

namespace UWG_TechHelp.Models
{
    public class ServicesViewModel : BaseNavigationViewModel
    {
        public ServicesViewModel()
        {
        }

        ObservableRangeCollection<Service> _Services;

        Command _LoadServicesCommand;

        Command _RefreshServicesCommand;

        public ObservableRangeCollection<Service> Services
        {
            get { return _Services ?? (_Services = new ObservableRangeCollection<Service>()); }
            set
            {
                _Services = value;
                OnPropertyChanged("Tickets");
            }
        }

        /// <summary>
        /// Command to load acquaintances
        /// </summary>
        public Command LoadServicesCommand
        {
            get { return _LoadServicesCommand ?? (_LoadServicesCommand = new Command(async () => await ExecuteLoadServicesCommand())); }
        }

        async Task ExecuteLoadServicesCommand()
        {
            if (Services.Count < 1)
            {
                LoadServicesCommand.ChangeCanExecute();

                await FetchServices();

                LoadServicesCommand.ChangeCanExecute();
            }
        }

        public Command RefreshServicesCommand
        {
            get
            {
                return _RefreshServicesCommand ?? (_RefreshServicesCommand = new Command(async () => await ExecuteRefreshServicesCommandCommand()));
            }
        }

        async Task ExecuteRefreshServicesCommandCommand()
        {
            RefreshServicesCommand.ChangeCanExecute();

            await FetchServices();

            RefreshServicesCommand.ChangeCanExecute();
        }

        async Task FetchServices()
        {
            IsBusy = true;

            var statuses = await ServiceDesk.getAllServiceStatusAnnouncements();
            var services = await ServiceDesk.getServices(statuses);

            if (services != null)
            {
                Services.Clear();

                Services.AddRange(services);
            }

            IsBusy = false;
        }
    }
}
