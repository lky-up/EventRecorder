
using System.Text;

namespace Timeline
{
    public partial class MainPage : ContentPage
    {
        int EventCountInDatabase { get; set; }
        StringBuilder PrefrenceList { get; set; }
        StringBuilder PrefrenceIDList { get; set; }

        public MainPage()
        {
            InitializeComponent();
        }

        protected override void OnAppearing()
        {
            PrefrenceList = new StringBuilder();
            PrefrenceIDList = new StringBuilder();
            base.OnAppearing();
            LoadTimeEvents();
        }

        private async void LoadTimeEvents()
        {
            DynamicGrid.RowDefinitions.Clear(); // 清空之前的行定义
            DynamicGrid.Children.Clear(); // 清空之前的控件

            var timeEvents = await App.Database.GetTimeEventsAsync();
            // 处理数据，例如显示在 ListView 中
            int colNum = DynamicGrid.ColumnDefinitions.Count;
            EventCountInDatabase = timeEvents.Count;

            for (int k = 0; k < ((timeEvents.Count+2)/colNum)+1; k++)
            {
                DynamicGrid.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
            }

            int i = 0;
            foreach (var timeEvent in timeEvents)
            {
                if (timeEvent.IsPreference)
                {
                    PrefrenceList.Append(timeEvent.Name+"|");
                    PrefrenceIDList.Append(timeEvent.Id.ToString() + "|");
                }
                Button btn = new Button
                {
                    Text = timeEvent.Name,
                    CommandParameter = timeEvent.Id,
                    HorizontalOptions = LayoutOptions.Center,
                    VerticalOptions = LayoutOptions.Center,
                    BackgroundColor = Colors.LightGray, // 可选：设置背景颜色以便于查看
                    Margin = new Thickness(5), // 可选：设置边距
                };
                btn.Clicked += ProcessEvent;
                DynamicGrid.Children.Add(btn);
                DynamicGrid.SetRow(btn, i / colNum);
                DynamicGrid.SetColumn(btn, i % colNum);
                i += 1;
            }

            Button addEventButton = new Button
            {
                Text = "添加",
                CommandParameter = -1,
                HorizontalOptions = LayoutOptions.Center,
                VerticalOptions = LayoutOptions.Center,
                BackgroundColor = Colors.LightGray, // 可选：设置背景颜色以便于查看
                Margin = new Thickness(5), // 可选：设置边距
            };
            addEventButton.Clicked += ProcessEvent;
            DynamicGrid.Children.Add(addEventButton);
            DynamicGrid.SetRow(addEventButton, i / colNum);
            DynamicGrid.SetColumn(addEventButton, i % colNum);
            i += 1;

            Button editEventButton = new Button
            {
                Text = "编辑",
                CommandParameter = -2,
                HorizontalOptions = LayoutOptions.Center,
                VerticalOptions = LayoutOptions.Center,
                BackgroundColor = Colors.LightGray, // 可选：设置背景颜色以便于查看
                Margin = new Thickness(5), // 可选：设置边距
            };
            editEventButton.Clicked += EditEvent;
            DynamicGrid.Children.Add(editEventButton);
            DynamicGrid.SetRow(editEventButton, i / colNum);
            DynamicGrid.SetColumn(editEventButton, i % colNum);
            i += 1;

            if (PrefrenceList.Length > 0)
            {
                Preferences.Set("PreferenceString", PrefrenceList.ToString().Substring(0, PrefrenceList.ToString().Length - 1));
                Preferences.Set("PrefrenceIDString", PrefrenceIDList.ToString().Substring(0, PrefrenceIDList.ToString().Length - 1));
            }
            else
            {
                Preferences.Set("PreferenceString", PrefrenceList.ToString());
                Preferences.Set("PrefrenceIDString", PrefrenceIDList.ToString());
            }
            

        }

        private async void EditEvent(object? sender, EventArgs e)
        {
            var eventList = await App.Database.GetTimeEventsAsync();
            if (eventList.Count <= 0)
            {
                OnShowMessageClicked("还未创建任何事件");
            }
            else
            {
                EditEventPage editFormPage = new EditEventPage();
                editFormPage.OnEditSubmit += EditFormPage_OnEditSubmit; ;
                await Navigation.PushModalAsync(editFormPage);
            }
        }

        private void EditFormPage_OnEditSubmit(string obj)
        {
            OnShowMessageClicked(obj);
            LoadTimeEvents();
        }

        private async void ProcessEvent(object? sender, EventArgs e)
        {
            Button buttongSender = sender as Button;
            if (buttongSender != null)
            {
                int eventID = Convert.ToInt32(buttongSender.CommandParameter);
                if (eventID > 0)
                {
                    //public int TimeEventId { get; set; } // 外键，指向 TimeEvent
                    //public DateTime TimeCreated { get; set; }

                    //public string? ExtraData { get; set; }
                    var CurrentEvent = await App.Database.GetTimeEventsAsync(eventID);
                    if (CurrentEvent.Count > 0)
                    {
                        TimeEventData timeEventData = new TimeEventData();
                        timeEventData.TimeEventId = eventID;
                        timeEventData.TimeCreated = DateTime.Now;
                        if (CurrentEvent.FirstOrDefault().DefaultValue != null)
                        {
                            timeEventData.ExtraData = CurrentEvent.FirstOrDefault().DefaultValue.ToString();
                        }
                        var tmpEventsData = await App.Database.SaveTimeEventDataAsync(timeEventData);
                        if (tmpEventsData > 0)
                        {
                            OnShowMessageClicked("记录成功-"+CurrentEvent.FirstOrDefault().Name + "! ");
                        }
                        else
                        {
                            OnShowMessageClicked("记录失败-"+CurrentEvent.FirstOrDefault().Name + "! ");
                        }
                    }
                    else
                    {
                        OnShowMessageClicked("未找到对应的事件!");
                    }
                }

                else
                {
                    CreateNewEventPage formPage = new CreateNewEventPage();
                    formPage.OnSubmit += FormPage_OnSubmitAsync;
                    await Navigation.PushModalAsync(formPage);
                    
                }
            }
        }

        private async void FormPage_OnSubmitAsync()
        {
            var timeEvents = await App.Database.GetTimeEventsAsync();
            if (timeEvents.Count != EventCountInDatabase)
            {
                LoadTimeEvents();
            }
        }

        private async void OnShowMessageClicked(string message)
        {
            // 设置消息文本并显示
            MessageLabel.Text = message+"当前时间: "+DateTime.Now.ToString("yyyy-MM-dd HH:m:ss");
            MessageLabel.IsVisible = true;

            // 等待 1 秒
            await Task.Delay(1000);

            // 隐藏消息
            MessageLabel.IsVisible = false;
        }

    }

}
