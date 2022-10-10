
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

    public static readonly BindableProperty CustomBackgroundColorProperty = BindableProperty.Create(
            nameof(CustomBackgroundColor),
            typeof(Color),
            typeof(WagerSelectionButtonControl),
            propertyChanged: (bindable, oldValue, newValue) =>
            {
                var control = (WagerSelectionButtonControl)bindable;
                control.WagerSelectionButton.BackgroundColor = newValue as Color;
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

    public Color CustomBackgroundColor
    {
        get => GetValue(CustomBackgroundColorProperty) as Color;
        set => SetValue(CustomBackgroundColorProperty, value);
    }

    public string CustomText
    { 
        get => GetValue(CustomTextProperty) as string; 
        set => SetValue(CustomTextProperty, value); 
    }
}

