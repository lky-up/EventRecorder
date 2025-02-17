using System.Text;

namespace Timeline;
public partial class AboutPage : ContentPage
{
	public AboutPage()
	{
		InitializeComponent();
        
    }

    protected override void OnAppearing()
    {
        string PreString = Preferences.Get("PreferenceString", "");
        base.OnAppearing();
        PreStr.Text = $"当前的偏好列表为:{PreString}。这些偏好将会按照顺序显示到小组件内。最多显示4个。";
    }


    private async void CleanAllData(object? sender, EventArgs e)
    {
		await App.Database.ClearDatabaseAsync();
		Preferences.Clear();
        PreStr.Text = $"当前的偏好列表为:。这些偏好将会按照顺序显示到小组件内。最多显示4个。";
        await DisplayAlert("成功", "数据已全部清空", "确定"); // 弹出警告对话框
    }
}