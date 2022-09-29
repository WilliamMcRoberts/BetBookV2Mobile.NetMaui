
using BetBookGamingMobile.Dto;
using BetBookGamingMobile.Models;
using BetBookGamingMobile.StateManagement;
using BetBookGamingMobile.ViewModels;
using Microsoft.Maui.Controls;

namespace BetBookGamingMobile.Views;

public partial class GameDetailsPage : ContentPage
{
    private readonly GameDetailsViewModel _viewModel;

    public GameDetailsPage(GameDetailsViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = _viewModel = viewModel;
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();
        (_viewModel.BetSlip, _viewModel.ButtonColorState) =
            _viewModel.GetButtonColorAndBetSlipStates();
        _viewModel.ButtonTextState = _viewModel.GetButtonTextState();
    }
}