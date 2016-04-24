using MvvmHelpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UWG_TechHelp.Controls;
using UWG_TechHelp.Models;
using UWG_TechHelp.Resources;
using Xamarin.Forms;

namespace UWG_TechHelp.Views
{
    public partial class TicketsView : ContentPage
    {
        protected TicketsViewModel ViewModel => BindingContext as TicketsViewModel;

        public TicketsView()
        {
            InitializeComponent();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            ViewModel.LoadTicketsCommand.Execute(null);
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
