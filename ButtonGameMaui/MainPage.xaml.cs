using Microsoft.Maui.Controls;
using System;

namespace ButtonGameMaui
{
    public partial class MainPage : ContentPage
    {
        private int size;
        private Button[,] buttons;

        public MainPage()
        {
            InitializeComponent();
            InitializeGame();
        }

        private void InitializeGame()
        {
            GetBoardSizeFromUser();
        }

        private void GetBoardSizeFromUser()
        {
            var entry = new Entry
            {
                Placeholder = "Podaj nieparzysty rozmiar planszy (min. 3)",
                Keyboard = Keyboard.Numeric
            };

            var confirmButton = new Button
            {
                Text = "Potwierdź",
                Command = new Command(() =>
                {
                    try
                    {
                        int inputSize = Convert.ToInt32(entry.Text);

                        if (inputSize < 3 || inputSize % 2 == 0)
                            throw new Exception("Rozmiar planszy musi być liczbą nieparzystą większą niż 2.");

                        size = inputSize;
                        InitializeGameContent();
                    }
                    catch (Exception ex)
                    {
                        DisplayAlert("Błąd", ex.Message, "OK");
                    }
                })
            };
            var stackLayout = new StackLayout
            {
                Children = { entry, confirmButton }
            };
            Content = stackLayout;
        }
        private void InitializeGameContent()
        {
            buttons = new Button[size, size];
            var grid = new Grid();
            for (int i = 0; i < size; i++)
            {
                grid.RowDefinitions.Add(new RowDefinition());
                grid.ColumnDefinitions.Add(new ColumnDefinition());
            }
            for (int i = 0; i < size; i++)
            {
                for (int j = 0; j < size; j++)
                {
                    buttons[i, j] = new Button
                    {
                        Text = "O",
                        CommandParameter = (i, j),
                        BackgroundColor = Color.FromArgb("#FF000000")
                    };
                    buttons[i, j].Clicked += Button_Clicked;
                    grid.Children.Add(buttons[i, j]);
                    Grid.SetRow(buttons[i, j], i);
                    Grid.SetColumn(buttons[i, j], j);
                }
            }
            int middle = size / 2;
            PressButton(middle, middle);
            Content = grid;
        }
        private void Button_Clicked(object sender, EventArgs e)
        {
            var button = (Button)sender;
            var position = (ValueTuple<int, int>)button.CommandParameter;
            int row = position.Item1;
            int col = position.Item2;
            PressButton(row, col);
            PressButton(row - 1, col);
            PressButton(row + 1, col);
            PressButton(row, col - 1);
            PressButton(row, col + 1);
            CheckGameStatus();
        }
        private void PressButton(int row, int col)
        {
            if (row >= 0 && row < size && col >= 0 && col < size)
            {
                if (buttons[row, col].Text == "O")
                {
                    buttons[row, col].Text = "X";
                    buttons[row, col].BackgroundColor = Color.FromArgb("#FFFFFFFF");
                }
                else
                {
                    buttons[row, col].Text = "O";
                    buttons[row, col].BackgroundColor = Color.FromArgb("#FF000000");
                }
            }
        }
        private void CheckGameStatus()
        {
            bool allButtonsPressed = true;
            foreach (var button in buttons)
            {
                if (button.Text == "X")
                {
                    allButtonsPressed = false;
                    break;
                }
            }

            if (allButtonsPressed)
            {
                DisplayAlert("Gratulacje!", "Wszystkie przyciski zostały zgaszone!", "OK");
            }
        }

    }
}
