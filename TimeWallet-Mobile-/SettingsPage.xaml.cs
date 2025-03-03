namespace TimeWallet_Mobile_;

public partial class SettingsPage : ContentPage
{
	public SettingsPage()
	{
		InitializeComponent();
	}
    private void OnPickerSelectedIndexChanged(object sender, EventArgs e)
    {
        var picker = (Picker)sender;
        string selectedCategory = (string)picker.SelectedItem;
        DisplayAlert("Selection", $"You selected: {selectedCategory}", "OK");
    }

    private void MainButtonLabel_Clicked(object sender, EventArgs e)
    {

    }

    private void Switch_Toggled(object sender, ToggledEventArgs e)
    {
        SwitchLabel.Text = e.Value ? "Enabled ✅" : "Disabled ❌";
    }

}