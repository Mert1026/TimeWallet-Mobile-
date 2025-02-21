using Microcharts;
using Microcharts.Maui;

namespace TimeWallet_Mobile_;

public partial class UserMainPage_TEST : ContentPage
{
    ChartEntry[] entries = new[]
    {
        new ChartEntry(100)
        {
            Label = "Test1",
            ValueLabel = "100",
            Color = SkiaSharp.SKColor.Parse("#2c3e50")
        },
        new ChartEntry(10)
        {
            Label = "Test2",
            ValueLabel = "10",
            Color = SkiaSharp.SKColor.Parse("#2c3e50")
        },
        new ChartEntry(170)
        {
            Label = "Test3",
            ValueLabel = "170",
            Color = SkiaSharp.SKColor.Parse("#2c3e50")
        },
        new ChartEntry(10)
        {
            Label = "Test2",
            ValueLabel = "10",
            Color = SkiaSharp.SKColor.Parse("#2c3e50")
        },
        new ChartEntry(10)
        {
            Label = "Test2",
            ValueLabel = "10",
            Color = SkiaSharp.SKColor.Parse("#2c3e50")
        },
        new ChartEntry(10)
        {
            Label = "Test2",
            ValueLabel = "10",
            Color = SkiaSharp.SKColor.Parse("#2c3e50")
        },
        new ChartEntry(10)
        {
            Label = "Test2",
            ValueLabel = "10",
            Color = SkiaSharp.SKColor.Parse("#2c3e50")
        },

    };
	    
	public UserMainPage_TEST()
	{
		InitializeComponent();
        LastTenExpensesChart.Chart = new LineChart
        {
            Entries = entries,
            LabelOrientation = Orientation.Horizontal, // Adjust label orientation
            ValueLabelOrientation = Orientation.Horizontal, // Adjust value label orientation
            LabelTextSize = 32
        };
	}
}