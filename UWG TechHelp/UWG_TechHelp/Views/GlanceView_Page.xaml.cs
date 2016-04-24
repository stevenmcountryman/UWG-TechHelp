using UWG_TechHelp.Models;
using Xamarin.Forms;

namespace UWG_TechHelp.Views
{
    public partial class GlanceView_Page : TabbedPage
    {
        public GlanceView_Page()
        {
            InitializeComponent();

            var navPage = new TicketsView()
            {
                BindingContext = new TicketsViewModel(),
                Title = "Tickets"
            };

            this.Children.Add(navPage);
            this.Children.Add(new ContentPage()
            {
                Title = "Services"
            });
        }
    }
}
