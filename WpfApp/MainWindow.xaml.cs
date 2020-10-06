using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using Exception = System.Exception;

namespace WpfApp
{
  /// <summary>
  /// Interaction logic for MainWindow.xaml
  /// </summary>
  public partial class MainWindow : Window
  {
    private List<BingoNumber> BingoNumbers { get; set; }

    public MainWindow()
    {
      InitializeComponent();
      BingoNumbers = new List<BingoNumber>();
    }

    private void Button_Click(object sender, RoutedEventArgs e)
    {
      Close();
    }

    private void DragMove(object sender, MouseButtonEventArgs e)
    {
      try
      {
        DragMove();
      }
      catch (Exception ex)
      {
        // don't do anything...
      }
    }
    
    private void BuildNumber(BingoNumber bingoNumber)
    {
      var border = new Border
      {
        Margin = new Thickness(CalculateMarginLeft(bingoNumber.Position), 0 ,0 ,0),
        Height = 100,
        Width = 100,
        BorderThickness = new Thickness(10),
        BorderBrush = bingoNumber.Position == 0 ? Brushes.Red : Brushes.Green,
        Background = Brushes.Transparent,
        CornerRadius = new CornerRadius(50),
        HorizontalAlignment = HorizontalAlignment.Left,
        Child = new TextBlock
        {
          Text = bingoNumber.Number.ToString(),
          HorizontalAlignment = HorizontalAlignment.Center,
          VerticalAlignment = VerticalAlignment.Center,
          FontFamily = new FontFamily("Arial Black"),
          FontSize = 50
        }
      };
      
      Grid.Children.Add(border);
    }

    private void removeLast()
    {
      if (BingoNumbers.Count != 0)
      {
        BingoNumbers.RemoveAt(BingoNumbers.Count -1);
        Refresh();
      }
    }

    private double CalculateMarginLeft(int position)
    {
      switch (position)
      {
        case 0:
          return 25;
        case 1:
          return 150;
        case 2:
          return 275;
        case 3:
          return 400;
        case 4:
          return 525;
        case 5:
          return 650;
        default:
          return 25;
      }
    }

    private void Button_Click_Clear(object sender, RoutedEventArgs e)
    {
      Clear();
    }

    private void Clear()
    {
      BingoNumbers.Clear();
      Grid.Children.Clear();
    }

    private void Refresh()
    {
      Grid.Children.Clear();

      var lastSix = BingoNumbers.TakeLast(6);
      List<BingoNumber> reversedList = new List<BingoNumber>(lastSix);
      reversedList.Reverse();

      int i = 0;
      foreach (var bingoNumber in reversedList)
      {
        bingoNumber.Position = i;
        BuildNumber(bingoNumber);
        i++;
      }
    }

    private void Button_Click_AddNumber(object sender, RoutedEventArgs e)
    {
      AddNumber();
    }

    private void AddNumber()
    {
      label.Visibility = Visibility.Hidden;
      if (int.TryParse(newNumber.Text, out int number))
      {
        BingoNumbers.Add(new BingoNumber(number));
        newNumber.Text = "";
        Refresh();
      }
      else
      {
        label.Visibility = Visibility.Visible;
      }
    }

    private void SetTextBoxFocus()
    {
      newNumber.Text = "";
      newNumber.Focus();
    }

    private void Button_Click_1(object sender, RoutedEventArgs e)
    {
      ToggleBackground();
    }

    private void ToggleBackground()
    {
      mainwin.Background = mainwin.Background == Brushes.Transparent ? Brushes.White : Brushes.Transparent;
    }

    private void Button_Click_2(object sender, RoutedEventArgs e)
    {
      removeLast();
    }

    private void mainwin_KeyUp(object sender, KeyEventArgs e)
    {
      if (e.Key == Key.F)
      {
        SetTextBoxFocus();
      }

      if (e.Key == Key.Enter || e.Key == Key.Return)
      {
        AddNumber();
        SetTextBoxFocus();
      }

      if (e.Key == Key.C)
      {
        Clear();
        SetTextBoxFocus();
      }

      if (e.Key == Key.R)
      {
        removeLast();
        SetTextBoxFocus();
      }

      if (e.Key == Key.T)
      {
        ToggleBackground();
        SetTextBoxFocus();
      }

      if (e.Key == Key.X)
      {
        Close();
      }
    }
  }

  public class BingoNumber
  {
    public BingoNumber(int number)
    {
      Number = number;
    }

    public int Number { get; set; }
    public int Position { get; set; }
  }
}
