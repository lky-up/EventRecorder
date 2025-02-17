namespace Timeline;

public partial class CreateNewEventPage : ContentPage
{
    public event Action OnSubmit; // �����¼�

    public CreateNewEventPage()
	{
		InitializeComponent();
	}

    private async void OnSubmitClicked(object sender, EventArgs e)
    {
        // ��ȡ�����ֵ
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
            await DisplayAlert("�ύʧ��", "$\"�¼�����: {name} �ظ�", "ȷ��");
            return;
        }

        var insertResult = await App.Database.SaveTimeEventAsync(te);

        if (insertResult > 0)
        {
            await DisplayAlert("�ύ�ɹ�", $"�¼�����: {name}\nĬ��ֵ: {tmpEventValue}", "ȷ��");
        }
        else
        {
            await DisplayAlert("�ύʧ��", "", "ȷ��");
            return;
        }
        // �رնԻ���
        OnSubmit();
        await Navigation.PopModalAsync();
    }

    private async void OnCancelClicked(object sender, EventArgs e)
    {
        // �رնԻ���
        await Navigation.PopModalAsync();
    }
}