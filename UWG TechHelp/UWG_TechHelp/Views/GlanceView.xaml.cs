using UWG_TechHelp.Models;
using Xamarin.Forms;

namespace UWG_TechHelp.Views
{
    public partial class GlanceView : TabbedPage
    {
        public GlanceView()
        {
            InitializeComponent();

            var ticketsView = new TicketsView()
            {
                BindingContext = new TicketsViewModel(),
                Title = "Tickets"
            };
            this.Children.Add(ticketsView);
            
            var servicesView = new ServicesView()
            {
                BindingContext = new ServicesViewModel(),
                Title = "Services"
            };
            this.Children.Add(servicesView);
        }
    }
}
