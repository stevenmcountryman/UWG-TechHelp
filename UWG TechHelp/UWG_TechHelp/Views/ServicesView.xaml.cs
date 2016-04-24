using UWG_TechHelp.Models;
using Xamarin.Forms;

namespace UWG_TechHelp.Views
{
    public partial class ServicesView : ContentPage
    {
        protected ServicesViewModel ViewModel => BindingContext as ServicesViewModel;

        public ServicesView()
        {
            InitializeComponent();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            ViewModel.LoadServicesCommand.Execute(null);
        }

        /// <summary>
        /// The action to take when a list item is tapped.
        /// </summary>
        /// <param name="sender"> The sender.</param>
        /// <param name="e">The ItemTappedEventArgs</param>
        void ItemTapped(object sender, ItemTappedEventArgs e)
        {
        }
    }
}
