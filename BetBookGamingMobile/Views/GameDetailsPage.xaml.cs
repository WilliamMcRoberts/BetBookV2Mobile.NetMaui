
using BetBookGamingMobile.ViewModels;
using Microsoft.Maui.Controls;

namespace BetBookGamingMobile.Views;

public partial class GameDetailsPage : ContentPage
{

    public GameDetailsPage(GameDetailsViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }
}