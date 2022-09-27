
using AndroidX.Lifecycle;
using BetBookGamingMobile.Dto;
using BetBookGamingMobile.Models;
using BetBookGamingMobile.StateManagement;
using BetBookGamingMobile.ViewModels;
using Microsoft.Maui.Controls;

namespace BetBookGamingMobile.Views;

public partial class GameDetailsPage : ContentPage
{
    private readonly GameDetailsViewModel _viewModel;
    private readonly BetSlipState _betSlipState;

    public GameDetailsPage(GameDetailsViewModel viewModel, BetSlipState betSlipState)
    {
        InitializeComponent();
        BindingContext = viewModel;
        _viewModel = viewModel;
        _betSlipState = betSlipState;
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();
        _viewModel.BetSlip = _betSlipState.GetBetSlipState();
        _viewModel.ButtonColorState = _betSlipState.GetButtonColorState(_viewModel.GameDto);
    }

}