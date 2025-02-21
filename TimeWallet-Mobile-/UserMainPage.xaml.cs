using Microcharts;
using SkiaSharp;
using System.Windows.Input;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;
using Microcharts;
using Microsoft.Maui.Controls;
using SkiaSharp;
using TimeWallet_Mobile_.Data.API_service;
using TimeWallet_Mobile_.Data.DTO_s.Json;
using TimeWallet_Mobile_.Data.Models;


namespace TimeWallet_Mobile_;

public partial class UserMainPage : ContentPage
{
    private readonly ApiService _apiService = new ApiService();
    private List<ChartEntry> lastTenExpensesEntries = new List<ChartEntry>();
    private string _email;

    public UserMainPage()
    {
        InitializeComponent();
        LoadData();
    }

    private async void LoadData()
    {
        try
        {
            // Fetch the email
            _email = await GetEmailAsync();
            if (string.IsNullOrEmpty(_email))
            {
                //Console.WriteLine("Email not found.");
                return;
            }

            // Fetch the entries
            var entries = await GetEntriesAsync(_email);
            if (entries == null || entries.Count == 0)
            {
                //Console.WriteLine("No entries found.");
                return;
            }

            // Add entries to the chart
            AddChartEntries(entries);

            // Initialize the chart
            LastTenExpensesChart.Chart = new LineChart
            {
                Entries = lastTenExpensesEntries,
                LabelOrientation = Orientation.Horizontal, // Adjust label orientation
                ValueLabelOrientation = Orientation.Horizontal, // Adjust value label orientation
                LabelTextSize = 12 
            };
        }
        catch (System.Exception ex)
        {
            //Console.WriteLine($"Error loading data: {ex.Message}");
        }
    }

    private void AddChartEntries(List<Elements> entries)
    {
        foreach (var element in entries)
        {
            var entry = new ChartEntry((float)element.Amount)
            {
                Label = $"{element.Name}\n({element.Budgets.Name})",
                ValueLabel = element.Amount.ToString(),
                Color = SKColor.Parse("#a8d09a")
            };
            lastTenExpensesEntries.Add(entry);
        }
    }

    public async Task<string> GetEmailAsync()
    {
        try
        {
            return "memo@gmail.com";
        }
        catch (System.Exception ex)
        {
            //Console.WriteLine($"Error retrieving email: {ex.Message}");
            return null;
        }
    }

    public async Task<List<Elements>> GetEntriesAsync(string email)
    {
        try
        {
            return await _apiService.GetLastElementsAsync(email);
        }
        catch (System.Exception ex)
        {
            //Console.WriteLine($"Error retrieving entries: {ex.Message}");
            return null;
        }
    }
}