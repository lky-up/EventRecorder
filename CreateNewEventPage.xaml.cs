namespace Timeline;

public partial class CreateNewEventPage : ContentPage
{
    public event Action OnSubmit; // 定义事件

    public CreateNewEventPage()
	{
		InitializeComponent();
	}

    private async void OnSubmitClicked(object sender, EventArgs e)
    {
        // 获取输入的值
        var name = EventName.Text.Trim();
        var tmpEventValue = EventValue.Text;
        var tmpDescribe = EventDescribe.Text;
        var tmpIsPrefernce = PreferenceSwitch.IsToggled;

        TimeEvent te = new TimeEvent();
        te.Name = name;
        te.DefaultValue = tmpEventValue;
        te.Description = tmpDescribe;
        te.IsPreference = tmpIsPrefernce;

        var allEvents = await App.Database.GetTimeEventsAsync();
        var duplicateEvent = allEvents.Where(data => data.Name == name);
        if (duplicateEvent.Count()>0)
        {
            await DisplayAlert("提交失败", "$\"事件名称: {name} 重复", "确定");
            return;
        }

        var insertResult = await App.Database.SaveTimeEventAsync(te);

        if (insertResult > 0)
        {
            await DisplayAlert("提交成功", $"事件名称: {name}\n默认值: {tmpEventValue}", "确定");
        }
        else
        {
            await DisplayAlert("提交失败", "", "确定");
            return;
        }
        // 关闭对话框
        OnSubmit();
        await Navigation.PopModalAsync();
    }

    private async void OnCancelClicked(object sender, EventArgs e)
    {
        // 关闭对话框
        await Navigation.PopModalAsync();
    }
}