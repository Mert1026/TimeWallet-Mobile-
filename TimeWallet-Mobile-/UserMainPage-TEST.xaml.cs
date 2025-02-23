using Microcharts;
using Microcharts.Maui;

namespace TimeWallet_Mobile_;

public partial class UserMainPage_TEST : ContentPage
{
    private char _chartType = 'D';

    private ChartEntry[] _entriesToAdd;
    ChartEntry[] _entries = new[]
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
            Color = SkiaSharp.SKColor.Parse("#18e7a2")
        },
        new ChartEntry(10)
        {
            Label = "Test2",
            ValueLabel = "10",
            Color = SkiaSharp.SKColor.Parse("#d32c6b")
        },
        new ChartEntry(10)
        {
            Label = "Test2",
            ValueLabel = "10",
            Color = SkiaSharp.SKColor.Parse("#426f32")
        },

    };
	    
	public UserMainPage_TEST()
	{
		InitializeComponent();
        _entriesToAdd = _entries;
        LastTenExpensesChart.Chart = new DonutChart
        {
            Entries = _entries,
            //LabelOrientation = Orientation.Horizontal, // Adjust label orientation
            //ValueLabelOrientation = Orientation.Horizontal, // Adjust value label orientation
            LabelTextSize = 32
        };
	}

    private void donutChart_btn_Clicked(object sender, EventArgs e)
    {
        makeDonutChart();
        _chartType = 'D';
    }

    private void lineChart_btn_Clicked(object sender, EventArgs e)
    {
        makeLineChart();
        _chartType = 'L';
    }

    private void barChart_btn_Clicked(object sender, EventArgs e)
    {
        makeBarChart();
        _chartType = 'B';
    }

    private void threeItems_btn_Clicked(object sender, EventArgs e)
    {
        _entriesToAdd = _entries.TakeLast(3).ToArray();
        if (_chartType == 'D')
        {
            makeDonutChart();
        }
        else if(_chartType == 'L')
        {
            makeLineChart();
        }
        else if (_chartType == 'B')
        {
            makeBarChart();
        }

    }

    private void fiveItems_btn_Clicked(object sender, EventArgs e)
    {
        _entriesToAdd = _entries.TakeLast(5).ToArray();
        if (_chartType == 'D')
        {
            makeDonutChart();
        }
        else if (_chartType == 'L')
        {
            makeLineChart();
        }
        else if (_chartType == 'B')
        {
            makeBarChart();
        }
    }

    private void tenItems_btn_Clicked(object sender, EventArgs e)
    {
        _entriesToAdd = _entries.TakeLast(10).ToArray();
        if (_chartType == 'D')
        {
            makeDonutChart();
        }
        else if (_chartType == 'L')
        {
            makeLineChart();
        }
        else if (_chartType == 'B')
        {
            makeBarChart();
        }
    }

    private void makeBarChart()
    {
        LastTenExpensesChart.Chart = new BarChart { Entries = _entriesToAdd, LabelTextSize = 32 };
    }
    private void makeLineChart()
    {
        LastTenExpensesChart.Chart = new LineChart { Entries = _entriesToAdd, LabelTextSize = 32 };
    }
    private void makeDonutChart()
    {
        LastTenExpensesChart.Chart = new DonutChart { Entries = _entriesToAdd, LabelTextSize = 32 };
    }

}