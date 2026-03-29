using System;
using System.Windows;
using System.Windows.Controls;

namespace VSIXProject1
{
    public class MyToolWindowControl : UserControl
    {
        private static readonly Random Rng = new Random();
        private readonly TextBlock _contentText;

        private static readonly string[] Phrases =
        {
            "The quick brown fox jumps over the lazy dog.",
            "Visual Studio extensions are powerful tools.",
            "Hello from your custom tool window!",
            "WPF makes building UI straightforward.",
            "Random fact: honey never spoils.",
            "Did you know? The first computer bug was an actual moth.",
            "Keep calm and write extensions.",
        };

        public MyToolWindowControl()
        {
            _contentText = new TextBlock
            {
                TextWrapping = TextWrapping.Wrap,
                FontSize = 14,
                Margin = new Thickness(0, 0, 0, 12)
            };

            var button = new Button
            {
                Content = "Generate New Content",
                Padding = new Thickness(8, 4, 8, 4),
                HorizontalAlignment = HorizontalAlignment.Left
            };
            button.Click += (s, e) => GenerateContent();

            var stack = new StackPanel { Margin = new Thickness(16) };
            stack.Children.Add(new TextBlock
            {
                Text = "My Tool Window",
                FontSize = 20,
                FontWeight = FontWeights.Bold,
                Margin = new Thickness(0, 0, 0, 12)
            });
            stack.Children.Add(_contentText);
            stack.Children.Add(button);

            Content = stack;
            GenerateContent();
        }

        private void GenerateContent()
        {
            _contentText.Text = Phrases[Rng.Next(Phrases.Length)]
                + $"\n\nGenerated at {DateTime.Now:HH:mm:ss}";
        }
    }
}
