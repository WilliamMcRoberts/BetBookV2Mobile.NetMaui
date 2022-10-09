namespace BetBookGamingMobile.Controls;

public partial class BettorParleyBetsCollectionViewControl : ContentView
{
	public BettorParleyBetsCollectionViewControl()
	{
		InitializeComponent();
	}

    public static readonly BindableProperty CustomItemSourceProperty = BindableProperty.Create(
        nameof(CustomItemSource), 
        typeof(ObservableCollection<ParleyBetSlipModel>), 
        typeof(BettorParleyBetsCollectionViewControl), 
        propertyChanged: (bindable, oldValue, newValue) =>
        {
            var control = (BettorParleyBetsCollectionViewControl)bindable;

            control.BettorParleyBetsCollectionView.ItemsSource = newValue as ObservableCollection<ParleyBetSlipModel>;
        });

    public ObservableCollection<ParleyBetSlipModel> CustomItemSource
    {
        get => GetValue(CustomItemSourceProperty) as ObservableCollection<ParleyBetSlipModel>;
        set => SetValue(CustomItemSourceProperty, value);
    }
}