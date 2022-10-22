
namespace BetBookGamingMobile.ViewControls;

public partial class WagerSelectionButtonControl : ContentView
{
    public WagerSelectionButtonControl()
    {
        InitializeComponent();
    }

    public static readonly BindableProperty CustomCommandProperty = BindableProperty.Create(
            nameof(CustomCommand), 
            typeof(ICommand), 
            typeof(WagerSelectionButtonControl), 
            propertyChanged: (bindable, oldValue, newValue) =>
            {
                var control = (WagerSelectionButtonControl)bindable;

                control.WagerSelectionButton.Command = (ICommand)newValue;
            });

    public static readonly BindableProperty CustomCommandParameterProperty = BindableProperty.Create(
            nameof(CustomCommandParameter), 
            typeof(string), 
            typeof(WagerSelectionButtonControl), 
            propertyChanged: (bindable, oldValue, newValue) =>
            {
                var control = (WagerSelectionButtonControl)bindable;
                control.WagerSelectionButton.CommandParameter = newValue as string;
            });

    public static readonly BindableProperty CustomBackgroundProperty = BindableProperty.Create(
            nameof(CustomBackground),
            typeof(Brush),
            typeof(WagerSelectionButtonControl),
            propertyChanged: (bindable, oldValue, newValue) =>
            {
                var control = (WagerSelectionButtonControl)bindable;
                control.WagerSelectionButton.Background = newValue as Brush;
            });

    public static readonly BindableProperty CustomTextProperty = BindableProperty.Create(
            nameof(CustomText), 
            typeof(string), 
            typeof(WagerSelectionButtonControl), 
            propertyChanged: (bindable, oldValue, newValue) =>
            {
                var control = (WagerSelectionButtonControl)bindable;
                control.WagerSelectionButton.Text = newValue as string;
            });

    public ICommand CustomCommand 
    { 
        get => GetValue(CustomCommandProperty) as ICommand; 
        set => SetValue(CustomCommandProperty, value); 
    }

    public string CustomCommandParameter 
    { 
        get => GetValue(CustomCommandParameterProperty) as string; 
        set => SetValue(CustomCommandParameterProperty, value); 
    }

    public Brush CustomBackground
    {
        get => GetValue(CustomBackgroundProperty) as Brush;
        set => SetValue(CustomBackgroundProperty, value);
    }

    public string CustomText
    { 
        get => GetValue(CustomTextProperty) as string; 
        set => SetValue(CustomTextProperty, value); 
    }
}

