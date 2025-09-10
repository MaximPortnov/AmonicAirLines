using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace AmonicAirLines.forXaml
{
    public static class GridHelpers
    {
        public static readonly DependencyProperty GridConfigProperty =
            DependencyProperty.RegisterAttached(
                "GridConfig",
                typeof(string),
                typeof(GridHelpers),
                new PropertyMetadata(default(string), OnGridConfigChanged));

        public static void SetGridConfig(UIElement element, string value)
        {
            element.SetValue(GridConfigProperty, value);
        }

        public static string GetGridConfig(UIElement element)
        {
            return (string)element.GetValue(GridConfigProperty);
        }

        private static void OnGridConfigChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is UIElement element && e.NewValue is string config)
            {
                var parameters = config.Split(',');
                if (parameters.Length == 4)
                {
                    Grid.SetColumn(element, int.Parse(parameters[0]));
                    Grid.SetRow(element, int.Parse(parameters[1]));
                    Grid.SetColumnSpan(element, int.Parse(parameters[2]));
                    Grid.SetRowSpan(element, int.Parse(parameters[3]));
                }

            }
        }
    }
    public static class Rack
    {
        public static readonly DependencyProperty RackRowsProperty = DependencyProperty.RegisterAttached(
            "RackRows", typeof(string), typeof(Rack), new PropertyMetadata("", OnRackRowsChanged));
        public static readonly DependencyProperty RackColumnsProperty = DependencyProperty.RegisterAttached(
            "RackColumns", typeof(string), typeof(Rack), new PropertyMetadata("", OnRackColumnsChanged));

        public static void SetRackRows(UIElement element, string value)
        {
            element.SetValue(RackRowsProperty, value);
        }

        public static string GetRackRows(UIElement element)
        {
            return (string)element.GetValue(RackRowsProperty);
        }

        public static void SetRackColumns(UIElement element, string value)
        {
            element.SetValue(RackColumnsProperty, value);
        }

        public static string GetRackColumns(UIElement element)
        {
            return (string)element.GetValue(RackColumnsProperty);
        }

        private static void OnRackRowsChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is Grid grid)
            {
                grid.RowDefinitions.Clear();
                string rows = e.NewValue as string;
                Console.WriteLine(rows);
                foreach (var row in rows.Split(' '))
                {
                    Console.WriteLine(row);
                    if (row == "Auto")
                    {
                        grid.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
                    }
                    else if (row.EndsWith("*"))
                    {
                        if (!double.TryParse(row.TrimEnd('*'), out double factor))
                        {
                            factor = 1.0;
                        }
                        grid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(factor, GridUnitType.Star) });
                    }
                    else
                    {
                        grid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(double.Parse(row)) });
                    }
                }
            }
        }
        private static void OnRackColumnsChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is Grid grid)
            {
                grid.ColumnDefinitions.Clear();
                string columns = e.NewValue as string;
                foreach (var column in columns.Split(' '))
                {
                    if (column == "Auto")
                    {
                        grid.ColumnDefinitions.Add(new ColumnDefinition { Width = GridLength.Auto });
                    }
                    else if (column.EndsWith("*"))
                    {
                        if (!double.TryParse(column.TrimEnd('*'), out double factor))
                        {
                            factor = 1.0;
                        }
                        grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(factor, GridUnitType.Star) });
                    }
                    else
                    {
                        grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(double.Parse(column)) });
                    }
                }
            }
        }
    }
}
