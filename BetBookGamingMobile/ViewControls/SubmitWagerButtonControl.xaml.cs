namespace BetBookGamingMobile.ViewControls;

public partial class SubmitWagerButtonControl : ContentView
{
	public SubmitWagerButtonControl()
	{
		InitializeComponent();
	}

    public static readonly BindableProperty CustomCommandProperty = BindableProperty.Create(
            nameof(CustomCommand),
            typeof(ICommand),
            typeof(SubmitWagerButtonControl),
            propertyChanged: (bindable, oldValue, newValue) =>
            {
                var control = (SubmitWagerButtonControl)bindable;

                control.SubmitWagerButton.Command = (ICommand)newValue;
            });

    public static readonly BindableProperty CustomTextProperty = BindableProperty.Create(
            nameof(CustomTextProperty),
            typeof(string),
            typeof(SubmitWagerButtonControl),
            propertyChanged: (bindable, oldValue, newValue) =>
            {
                var control = (SubmitWagerButtonControl)bindable;
                control.SubmitWagerButton.Text = newValue as string;
            });

    public ICommand CustomCommand
    {
        get => GetValue(CustomCommandProperty) as ICommand;
        set => SetValue(CustomCommandProperty, value);
    }

    public string CustomText
    {
        get => GetValue(CustomTextProperty) as string;
        set => SetValue(CustomTextProperty, value);
    }
}