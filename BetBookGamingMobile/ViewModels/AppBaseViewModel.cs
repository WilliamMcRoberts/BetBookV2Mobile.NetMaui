
namespace BetBookGamingMobile.ViewModels;

public partial class AppBaseViewModel : BaseViewModel
{
    public INavigation NavigationService { get; set; }
    public Page PageService { get; set; }

    protected IApiService _apiService { get; set; }

    public AppBaseViewModel(IApiService apiService) : base()
    {
        _apiService = apiService;
    }

    [RelayCommand]
    private async Task NavigateBack() =>
        await NavigationService.PopAsync();

    [RelayCommand]
    private async Task CloseModal() =>
        await NavigationService.PopModalAsync();
}
