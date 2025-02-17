using Microsoft.Extensions.Logging;
using System.Collections.ObjectModel;
using System.Text;

namespace Timeline;

public partial class EditEventPage : ContentPage
{
    public event Action<string> OnEditSubmit;
    public ObservableCollection<string> RecordNames { get; set; }

    public ObservableCollection<TimeEvent> Records { get; set; }
    
    public EditEventPage()
	{
		InitializeComponent();
        LoadTimeEvents();
    }

    private async void LoadTimeEvents()
    {
        var timeEvents = await App.Database.GetTimeEventsAsync();

        Records = new ObservableCollection<TimeEvent>();

        foreach (var timeEvent in timeEvents)
        {
            Records.Add(new TimeEvent()
            {
                Name = timeEvent.Name,
                Id = timeEvent.Id,
                DefaultValue = timeEvent.DefaultValue,
                Description = timeEvent.Description,
                IsPreference = timeEvent.IsPreference,
            });
        }
        RecordNames = new ObservableCollection<string>(Records.OrderByDescending(r => r.Name).Select(r => r.Name));
        EventPicker.ItemsSource = RecordNames;
        EventPicker.SelectedIndexChanged += EventPicker_SelectedIndexChangedAsync;

    }

    private async void EventPicker_SelectedIndexChangedAsync(object? sender, EventArgs e)
    {
        var selectedItem = EventPicker.SelectedItem as string;
        TimeEvent selectRecorder = Records.FirstOrDefault(r => r.Name == selectedItem);
        if (selectRecorder != null)
        {
            EventID.Text= selectRecorder.Id.ToString();
            EventValue.Text = selectRecorder.DefaultValue;
            EventDescribe.Text = selectRecorder.Description;
            EventName.Text=selectRecorder.Name;
            PreferenceSwitch.IsToggled = selectRecorder.IsPreference;
        }
    }

    private async void OnSubmitClicked(object sender, EventArgs e)
    {
        // UpdateTimeEventAsync
        // �رնԻ���
        var result = await App.Database.UpdateTimeEventAsync(Convert.ToInt32(EventID.Text), EventDescribe.Text, EventValue.Text, PreferenceSwitch.IsToggled);
        if (result)
        {
            OnEditSubmit("����" + EventName.Text + "�ɹ�!");
        }
        else
        {
            OnEditSubmit("����" + EventName.Text + "ʧ��!");
        }
        await Navigation.PopModalAsync();
    }

    private async void OnCancelClicked(object sender, EventArgs e)
    {
        // �رնԻ���
        await Navigation.PopModalAsync();
    }

    private async void OnSubCommitClicked(object sender, EventArgs e)
    {
        // �رնԻ���
        await App.Database.UpdateTimeEventAsync(Convert.ToInt32(EventID.Text), EventDescribe.Text, EventValue.Text, PreferenceSwitch.IsToggled);
        TimeEventData timeEventData = new TimeEventData();

        timeEventData.TimeEventId = Convert.ToInt32(EventID.Text);
        timeEventData.TimeCreated = DateTime.Now;
        timeEventData.ExtraData = EventValue.Text;
        var result = await App.Database.SaveTimeEventDataAsync(timeEventData);

        if (result > 0)
        {
            OnEditSubmit("��¼�ɹ�-" + EventValue.Text + "! ");
        }
        else
        {
            OnEditSubmit("��¼ʧ��-" + EventValue.Text + "! ");
        }

        await Navigation.PopModalAsync();
    }

    private async void OnCommitClicked(object sender, EventArgs e)
    {
        // �رնԻ���
        TimeEventData timeEventData = new TimeEventData();

        timeEventData.TimeEventId = Convert.ToInt32(EventID.Text);
        timeEventData.TimeCreated = DateTime.Now;
        timeEventData.ExtraData = EventValue.Text;
        var result = await App.Database.SaveTimeEventDataAsync(timeEventData);

        if (result > 0)
        {
            OnEditSubmit("��¼�ɹ�-" + EventValue.Text + "! ");
        }
        else
        {
            OnEditSubmit("��¼ʧ��-" + EventValue.Text + "! ");
        }
        await Navigation.PopModalAsync();
    }

    private async void OnDeleteClicked(object sender, EventArgs e)
    {
        int eventId = Convert.ToInt32(EventID.Text);
        
        var result = await App.Database.DeleteTimeEventByIdAsync(eventId);

        if (result > 0)
        {
            OnEditSubmit("ɾ���ɹ�-" + EventValue.Text + "! ");
        }
        else
        {
            OnEditSubmit("ɾ��ʧ��-" + EventValue.Text + "! ");
        }
        await Navigation.PopModalAsync();
    }

}