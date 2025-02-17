using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Text;
using System.Xml.Linq;

namespace Timeline;

public class Record
{
    public string Name { get; set; }
    public int ID { get; set; }
}
public partial class AnalyzePage : ContentPage
{
    public ObservableCollection<Record> Records { get; set; }
    public ObservableCollection<string> RecordNames { get; set; }
    public AnalyzePage()
	{
		InitializeComponent();
    }


    protected override void OnAppearing()
    {
        base.OnAppearing();
        LoadTimeEvents();
    }

    private async void LoadTimeEvents()
    {
        Records = new ObservableCollection<Record>();
        RecordNames = new ObservableCollection<string>();
        var timeEvents = await App.Database.GetTimeEventsAsync();
        foreach (var timeEvent in timeEvents)
        {
            Records.Add(new Record()
            {
                Name = timeEvent.Name,
                ID = timeEvent.Id
            });
        }
        RecordNames = new ObservableCollection<string>(Records.OrderByDescending(r => r.Name).Select(r => r.Name));
        EventPicker.ItemsSource = RecordNames;
        EventPicker.SelectedIndexChanged += EventPicker_SelectedIndexChanged;
        RecordsLabel.Text="";

    }

    private async void EventPicker_SelectedIndexChanged(object? sender, EventArgs e)
    {
        var selectedItem = EventPicker.SelectedItem as string;
        Record selectRecorder = Records.FirstOrDefault(r => r.Name == selectedItem);
        if (selectRecorder != null)
        {
            var timeEventDatas = await App.Database.GetTimeEventDataAsync(selectRecorder.ID);
            StringBuilder displayString = new StringBuilder();

            if (timeEventDatas != null)
            {
                foreach (TimeEventData item in timeEventDatas)
                {
                    displayString.AppendLine("触发时间: " + item.TimeCreated.ToString("yyyy-MM-dd HH:mm:ss") + " 备注: " + item.ExtraData?.ToString());
                }
            }
            RecordsLabel.Text = displayString.ToString();
        }
    }
}