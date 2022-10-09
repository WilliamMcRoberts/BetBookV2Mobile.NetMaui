
using System.Windows.Input;

namespace BetBookGamingMobile.Controls;

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

    public ObservableCollection<SingleBetModel> CustomItemSource
    {
        get => GetValue(CustomItemSourceProperty) as ObservableCollection<SingleBetModel>;
        set => SetValue(CustomItemSourceProperty, value);
    }
}