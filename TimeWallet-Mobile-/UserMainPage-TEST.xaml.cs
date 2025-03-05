using Microcharts;
using Microcharts.Maui;
using SkiaSharp;
using System.Xml;
using System.Xml.Serialization;
using TimeWallet_Mobile_.Data.API_service;
using TimeWallet_Mobile_.Data.Models;
using TimeWallet_Mobile_.Data.Translation;

namespace TimeWallet_Mobile_;

public partial class UserMainPage_TEST : ContentPage
{
    private char _chartType = 'D';

    private ApiService _apiService;

    private ChartEntry[] _entries;
    private List<ChartEntry> _currentEntries;
    private Translations _translations;
	    
	public UserMainPage_TEST()
	{
		InitializeComponent();
        //Shell.SetTabBarIsVisible(this, true);
        _apiService = new ApiService();
        AddEntries();
        ButtonsCalibration();

	}

    private async void AddEntries()
    {
        string email = await SecureStorage.GetAsync("UserEmail");
        List<Elements> entries = await _apiService.GetLastElementsAsync(email);
        List<ChartEntry> entriesToDisplay = new List<ChartEntry>();
        _entries = new ChartEntry[entries.Count];
        foreach (Elements e in entries)
        {
            ChartEntry chartEntry = new ChartEntry((float)e.Amount)
            {
                Label = e.Name,
                ValueLabel = e.Amount.ToString(),
                Color = GetRandomColor()
            };
            entriesToDisplay.Add(chartEntry);
        }
        _entries = entriesToDisplay.ToArray();
        _currentEntries = _entries.ToList();
        LastTenExpensesChart.Chart = new DonutChart
        {
            Entries = _entries,
            BackgroundColor = SkiaSharp.SKColor.Parse("#e1f2d9"),
            //LabelOrientation = Orientation.Horizontal, // Adjust label orientation
            //ValueLabelOrientation = Orientation.Horizontal, // Adjust value label orientation
            LabelTextSize = 32
        };
        ElementsCheck();
    }

    public static SKColor GetRandomColor()
    {
        Random rand = new Random();
        byte r = (byte)rand.Next(256); // Random Red (0-255)
        byte g = (byte)rand.Next(256); // Random Green (0-255)
        byte b = (byte)rand.Next(256); // Random Blue (0-255)

        return new SKColor(r, g, b);
    }

    private async void ElementsCheck()
    {
        if(_entries != null)
        {
            if (_entries.Count() < 10)
            {
                tenItems_btn.IsVisible = false;
            }
            if (_entries.Count() < 5)
            {
                fiveItems_btn.IsVisible = false;
            }
            if (_entries.Count() < 3)
            {
                threeItems_btn.IsVisible = false;
            }
        }
        else
        {
            tenItems_btn.IsVisible = false;
            fiveItems_btn.IsVisible = false;
            threeItems_btn.IsVisible = false;
        }
        
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
        _currentEntries = _entries.TakeLast(3).ToList();
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
        _currentEntries = _entries.TakeLast(5).ToList();
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
        _currentEntries = _entries.TakeLast(10).ToList ();
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
        LastTenExpensesChart.Chart = new BarChart { Entries = _currentEntries, LabelTextSize = 32, BackgroundColor = SkiaSharp.SKColor.Parse("#e1f2d9") };
    }
    private void makeLineChart()
    {
        LastTenExpensesChart.Chart = new LineChart { Entries = _currentEntries, LabelTextSize = 32, BackgroundColor = SkiaSharp.SKColor.Parse("#e1f2d9") };
    }
    private void makeDonutChart()
    {
        LastTenExpensesChart.Chart = new DonutChart { Entries = _currentEntries, LabelTextSize = 32, BackgroundColor = SkiaSharp.SKColor.Parse("#e1f2d9") };
    }

    private async void Button_Clicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new CameraPage(false));
    }

    private async void Button_Clicked_1(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new AddElementPage());
    }

    private async void Button_Clicked_2(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new AddBudgetPage());
    }

    private void Refresh_btn_Clicked(object sender, EventArgs e)
    {
        OnAppearing();
        AddEntries();
        ButtonsCalibration();
    }

    private async void ButtonsCalibration()
    {
        string theme = await SecureStorage.GetAsync("Theme");
        if (theme == null)
        {
            await DisplayAlert("Atention", "Error", "Ok");
        }
        else if (theme == "light")
        {
            this.BackgroundColor = Color.FromArgb("#e0f2d8");
        }
        else
        {
            this.BackgroundColor = Color.FromArgb("#a9d494");
        }

        string language = await SecureStorage.GetAsync("language");

        if (language == null)
        {
            await DisplayAlert("Atention", "Error occured! Try again later.", "Ok");
            await Navigation.PopAsync();
        }
        else if (language == "en")
        {
            _translations = new Translations("en");
        }
        else
        {
            _translations = new Translations("bg");
        }
        SetText();
    }

    private void SetText()
    {
        MainTextLabel.Text = _translations.MainPageMainText;
        AddBudgetBtn.Text = _translations.MainPageBudgetButtonText;
        AddExpenseBtn.Text = _translations.MainPageElementButtonText;
        CameraBtn.Text = _translations.MainPageCameraButtonText;
    }
    protected override async void OnAppearing()
    {
        base.OnAppearing();
        AddEntries();
        ButtonsCalibration();
        // Refresh data when the page appears
    }
}