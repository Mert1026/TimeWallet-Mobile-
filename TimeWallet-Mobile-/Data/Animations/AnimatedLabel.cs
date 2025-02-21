using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TimeWallet_Mobile_.Data.Animations
{
    public class AnimatedLabel : Label
    {
        public static readonly BindableProperty TargetValueProperty =
            BindableProperty.Create(nameof(TargetValue), typeof(decimal), typeof(AnimatedLabel), default(decimal), propertyChanged: OnTargetValueChanged);

        public decimal TargetValue
        {
            get => (decimal)GetValue(TargetValueProperty);
            set => SetValue(TargetValueProperty, value);
        }

        // Keep the method signature as 'void' for the propertyChanged callback
        private static void OnTargetValueChanged(BindableObject bindable, object oldValue, object newValue)
        {
            if (bindable is AnimatedLabel label && newValue is decimal newAmount)
            {
                // Run the async method in a background thread
                Task.Run(async () =>
                {
                    try
                    {
                        await label.AnimateValueAsync(newAmount);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Error during animation: {ex.Message}");
                    }
                });
            }
        }

        private async Task AnimateValueAsync(decimal target)
        {
            decimal start = 0;
            int duration = 1500; // Animation duration in milliseconds
            int steps = 60; // Number of animation steps
            decimal increment = (target - start) / steps;

            for (int i = 0; i <= steps; i++)
            {
                this.Text = $"{Math.Round(start, 2):N2}"; // Format to 2 decimal places
                start += increment;
                await Task.Delay(duration / steps);
            }

            this.Text = $"{target:N2}"; // Ensure it ends at the exact target value
        }
    }
}


