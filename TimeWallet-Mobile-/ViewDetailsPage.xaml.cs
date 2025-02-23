

namespace TimeWallet_Mobile_;

public partial class ViewDetailsPage : ContentPage
{

	private Grid tableGrid;


	public ViewDetailsPage()
	{
		InitializeComponent();
	}

	private void AddGrid()
	{
        tableGrid = new Grid
        {
            Padding = 10,
            ColumnSpacing = 5,
            RowSpacing = 5
        };
        tableGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = GridLength.Auto });
        tableGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star)});
        tableGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = GridLength.Auto });

        Label label = new Label() { Text="1" };
        Grid.SetRow(label, 0);
        Grid.SetColumn(label, 1);

        Label label1 = new Label() { Text = "Row 2, Column 2" };
        Grid.SetRow(label, 2);
        Grid.SetColumn(label, 2);

        Label label2 = new Label() { Text = "Row 2, Column 2" };
        Grid.SetRow(label, 2);
        Grid.SetColumn(label, 2);
    }
}