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
        PreStr.Text = $"��ǰ��ƫ���б�Ϊ:{PreString}����Щƫ�ý��ᰴ��˳����ʾ��С����ڡ������ʾ4����";
    }


    private async void CleanAllData(object? sender, EventArgs e)
    {
		await App.Database.ClearDatabaseAsync();
		Preferences.Clear();
        PreStr.Text = $"��ǰ��ƫ���б�Ϊ:����Щƫ�ý��ᰴ��˳����ʾ��С����ڡ������ʾ4����";
        await DisplayAlert("�ɹ�", "������ȫ�����", "ȷ��"); // ��������Ի���
    }
}