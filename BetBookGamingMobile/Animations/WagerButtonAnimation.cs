namespace BetBookGamingMobile.Animations;

class WagerButtonAnimation : BaseAnimation
{
    public override async Task Animate(VisualElement view)
    {
        await view.ScaleTo(1.1, Length, Easing);
        await view.ScaleTo(.9, Length, Easing);
        await view.ScaleTo(1, Length, Easing);
    }
}
