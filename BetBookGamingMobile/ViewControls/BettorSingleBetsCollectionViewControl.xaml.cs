

namespace BetBookGamingMobile.ViewControls;

public partial class BettorSingleBetsCollectionViewControl : ContentView
{
	public BettorSingleBetsCollectionViewControl()
	{
		InitializeComponent();
	}

    public static readonly BindableProperty CustomItemSourceProperty = BindableProperty.Create(
            nameof(CustomItemSource), 
            typeof(ObservableCollection<SingleBetModel>), 
            typeof(BettorSingleBetsCollectionViewControl),
            propertyChanged: (bindable, oldValue, newValue) =>
            {
                var control = (BettorSingleBetsCollectionViewControl)bindable;

                control.BettorSingleBetsCollectionView.ItemsSource = newValue as ObservableCollection<SingleBetModel>;
            });

    public static readonly BindableProperty CustomTextProperty = BindableProperty.Create(
            nameof(CustomText),
            typeof(string),
            typeof(BettorSingleBetsCollectionViewControl),
            propertyChanged: (bindable, oldValue, newValue) =>
            {
                var control = (BettorSingleBetsCollectionViewControl)bindable;
                control.BetListLabel.Text = newValue as string;
            });

    public static readonly BindableProperty CustomIsVisibleProperty = BindableProperty.Create(
            nameof(CustomIsVisible),
            typeof(bool),
            typeof(BettorSingleBetsCollectionViewControl),
            propertyChanged: (bindable, oldValue, newValue) =>
            {
                var control = (BettorSingleBetsCollectionViewControl)bindable;
                control.MySingleBetsContentView.IsVisible = (bool)newValue;
            });

    public ObservableCollection<SingleBetModel> CustomItemSource
    {
        get => GetValue(CustomItemSourceProperty) as ObservableCollection<SingleBetModel>;
        set => SetValue(CustomItemSourceProperty, value);
    }

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
}