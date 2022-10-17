namespace BetBookGamingMobile.ViewControls;

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

    public static readonly BindableProperty CustomTextProperty = BindableProperty.Create(
            nameof(CustomText),
            typeof(string),
            typeof(BettorParleyBetsCollectionViewControl),
            propertyChanged: (bindable, oldValue, newValue) =>
            {
                var control = (BettorParleyBetsCollectionViewControl)bindable;
                control.BetListLabel.Text = newValue as string;
            });

    public static readonly BindableProperty CustomIsVisibleProperty = BindableProperty.Create(
            nameof(CustomIsVisible),
            typeof(bool),
            typeof(BettorParleyBetsCollectionViewControl),
            propertyChanged: (bindable, oldValue, newValue) =>
            {
                var control = (BettorParleyBetsCollectionViewControl)bindable;
                control.MyParleyBetsContentView.IsVisible = (bool)newValue;
            });

    public string CustomText
    {
        get => GetValue(CustomTextProperty) as string;
        set => SetValue(CustomTextProperty, value);
    }

    public bool CustomIsVisible
    {
        get => (bool)GetValue(CustomIsVisibleProperty);
        set => SetValue(CustomIsVisibleProperty, value);
    }

    public ObservableCollection<ParleyBetSlipModel> CustomItemSource
    {
        get => GetValue(CustomItemSourceProperty) as ObservableCollection<ParleyBetSlipModel>;
        set => SetValue(CustomItemSourceProperty, value);
    }
}