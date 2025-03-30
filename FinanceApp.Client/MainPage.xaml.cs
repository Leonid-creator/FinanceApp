using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Maui.Controls;
using FinanceApp.Lib.Dtos;
using System.Text.Json;
//using Android.App.AppSearch;
//using Android.App;

namespace FinanceApp.Client
{
    public partial class MainPage : ContentPage
    {
        private readonly HttpClient _httpClient;
        private List<ProductEntryFields> productFieldsList = new();
        public Grid dynamicGrid2 { get; set; }
        public MainPage()
        {
            InitializeComponent();
            _httpClient = new HttpClient();
            LoadStoresIntoPickerAsync();
        }
    
        private void OnStoreSelected(object sender, EventArgs e)
        {
            string selectedStore = storePicker.SelectedItem as string;
        }
        private void OnDateSelected(object sender, DateChangedEventArgs e)
        {
            DateTime selectedDate = e.NewDate;
            DisplayAlert("Выбрано", $"Выбрана дата: {selectedDate.ToShortDateString()}", "OK");
        }
    
        private void AddRowToGrid()
        {
            Grid dynamicGrid1 = new Grid { Margin = new Thickness(0) };
            dynamicGrid2 = new Grid { Margin = new Thickness(0, 0, 0, 15) };
    
            dynamicGrid1.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
    
            dynamicGrid1.ColumnDefinitions.Add(new ColumnDefinition { Width = 40 });
            dynamicGrid1.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
            dynamicGrid1.ColumnDefinitions.Add(new ColumnDefinition { Width = 60 });
            dynamicGrid1.ColumnDefinitions.Add(new ColumnDefinition { Width = GridLength.Auto });
    
            dynamicGrid2.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Star) });
    
            dynamicGrid2.ColumnDefinitions.Add(new ColumnDefinition { Width = 50 });
            dynamicGrid2.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
            dynamicGrid2.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
    
            Entry quantity = new Entry { Placeholder = "Q-ty", Keyboard = Keyboard.Numeric, Text = "1"};
            Grid.SetRow(quantity, 0);
            Grid.SetColumn(quantity, 0);
            dynamicGrid1.Children.Add(quantity);
    
            Entry productName = new Entry { Placeholder = "Prod. name"};
            Grid.SetRow(productName, 0);
            Grid.SetColumn(productName, 1);
            dynamicGrid1.Children.Add(productName);
    
            Entry amount = new Entry { Placeholder = "Price", Keyboard = Keyboard.Numeric };
            Grid.SetRow(amount, 0);
            Grid.SetColumn(amount, 2);
            dynamicGrid1.Children.Add(amount);
    
            Button removeProductBTN = new Button { Text = "X", WidthRequest = 40, HeightRequest = 40, Margin = new Thickness(10, 0, 0, 0) };
            Grid.SetRow(removeProductBTN, 0);
            Grid.SetColumn(removeProductBTN, 3);
            removeProductBTN.Clicked += RemoveProdRow;
            dynamicGrid1.Children.Add(removeProductBTN);
    
            Entry discount = new Entry { Placeholder = "Disc", Keyboard = Keyboard.Numeric, Text = "0" };
            Grid.SetRow(discount, 1);
            Grid.SetColumn(discount, 0);
            dynamicGrid2.Children.Add(discount);

            //Entry category = new Entry { Placeholder = "Category" };
            //Grid.SetRow(category, 1);
            //Grid.SetColumn(category, 1);
            //dynamicGrid2.Children.Add(category);

            Picker categoryPicker = new Picker { Title = "Category" };
            categoryPicker.ItemsSource = new List<string> { "Groceries", "Home Goods", "Utilities" };  // Список для Picker
            categoryPicker.SelectedIndexChanged += CategoryPicker_SelectedIndexChanged;
            Grid.SetRow(categoryPicker, 1);  // Устанавливаем строку
            Grid.SetColumn(categoryPicker, 1);  // Устанавливаем колонку
            dynamicGrid2.Children.Add(categoryPicker);

            //Entry subcategory = new Entry { Placeholder = "Subcategory" };
            //Grid.SetRow(subcategory, 1);
            //Grid.SetColumn(subcategory, 2);
            //Grid.SetColumnSpan(subcategory, 2);
            //dynamicGrid2.Children.Add(subcategory);

            Picker subcategoryPicker = new Picker { Title = "Subcategory" };
            //subcategoryPicker.ItemsSource = new List<string> { "Bread", "Milk", "Fruits", "Vegetables", "Seafood", "Herbs", "Meat and poultry" };
            Grid.SetRow(subcategoryPicker, 1);
            Grid.SetColumn(subcategoryPicker, 2);
            Grid.SetColumnSpan(subcategoryPicker, 2);
            dynamicGrid2.Children.Add(subcategoryPicker);

            VerticalStackLayout stackLayout = this.FindByName<VerticalStackLayout>("MyStackLayout");
            productsLayout.Children.Add(dynamicGrid1);
            productsLayout.Children.Add(dynamicGrid2);

            var productFields = new ProductEntryFields
            {
                Quantity = quantity,
                ProductName = productName,
                Amount = amount,
                Discount = discount,
                Category = categoryPicker,
                Subcategory = subcategoryPicker
            };

            productFieldsList.Add(productFields);
        }
    
        private void OnAddProductClicked(object sender, EventArgs e)
        {
            AddRowToGrid();
        }
        private Dictionary<string, List<string>> subcategoryLists = new Dictionary<string, List<string>>
        {
            { "Groceries", new List<string> { "Bread", "Milk", "Fruits", "Vegetables", "Seafood", "Herbs", "Meat and poultry" } },
            { "Home Goods", new List<string> { "Home decor", "Lighting", "Cleaning", "Storage"} },
            { "Utilities", new List<string> { "Electricity", "Gas", "Internet and phone" } }
        };
        private void CategoryPicker_SelectedIndexChanged(object sender, EventArgs e)
        {
            var picker = (Picker)sender;
            var selectedCategory = picker.SelectedItem as string;

            // Получаем список подкатегорий для выбранной категории
            if (selectedCategory != null && subcategoryLists.ContainsKey(selectedCategory))
            {
                var subcategoryPicker = (Picker)dynamicGrid2.Children.FirstOrDefault(x => x is Picker && dynamicGrid2.GetColumn(x) == 2);
                subcategoryPicker.ItemsSource = subcategoryLists[selectedCategory];
                subcategoryPicker.SelectedIndex = -1; // Сброс выбора
            }
        }

        private void RemoveProdRow(object sender, EventArgs e)
        {
            if (sender is Button button)
            {
                if (button.Parent is Grid dynamicGrid1)
                {
                    int index = productsLayout.Children.IndexOf(dynamicGrid1);
    
                    if (index != -1 && index + 1 < productsLayout.Children.Count)
                    {
                        productsLayout.Children.RemoveAt(index);     // first Grid
                        productsLayout.Children.RemoveAt(index);     // second Grid (second one will shift on first place)
                    }
                }
            }
        }

        private void RemoveAllProdRows()
        {
            // Удаляем все элементы из layout
            productsLayout.Children.Clear();

            // Если нужно, можно очистить список productFieldsList
            productFieldsList.Clear();
        }

        private async Task SendDataToApiAsync()
        {
            var tempReceipt = new
            {
                StoreName = storePicker.SelectedItem?.ToString(),
                DateTime =datePicker.Date,
                TotalAmount = Convert.ToDecimal(totalAmountField.Text),
                ReceiptDiscount = Convert.ToDecimal(receiptDiscountField.Text)
            };

            var tempDetails = new List<object>();

            foreach (var fields in productFieldsList)
            {
                if (decimal.TryParse(fields.Amount.Text, out var amount) &&
                    decimal.TryParse(fields.Discount.Text, out var discount) &&
                    int.TryParse(fields.Quantity.Text, out var quantity))
                {
                    tempDetails.Add(new
                    {
                        ProductName = fields.ProductName.Text,
                        Quantity = quantity,
                        Amount = amount,
                        Discount = discount,
                        Category = fields.Category.SelectedItem?.ToString(),
                        Subcategory = fields.Subcategory.SelectedItem?.ToString()
                    });
                }
            }

            var requestData = new
            {
                tempReceipt,
                tempDetails
            };
    
            var jsonContent = new StringContent(JsonSerializer.Serialize(requestData, new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase }), Encoding.UTF8, "application/json");
    
            //var response = await _httpClient.PostAsync("https://financeapp-gvaxa5fravg5grf2.ukwest-01.azurewebsites.net/api/receipts/add-test", jsonContent);
            var response = await _httpClient.PostAsync("http://localhost:5133/api/receipts/add-test", jsonContent);
    
            if (response.IsSuccessStatusCode)
            {
                Console.WriteLine("Receipt successfully added");
                RemoveAllProdRows();
                await Application.Current.MainPage.DisplayAlert("Success", "Receipt added", "ОК");
            }
            else
            {
                Console.WriteLine("Error: " + response.ReasonPhrase);
                await Application.Current.MainPage.DisplayAlert("Error", response.ReasonPhrase, "ОК");
            }
        }
    
        private async void OnProcessReceiptClicked(object sender, EventArgs e)
        {
            await SendDataToApiAsync();
        }

        private async Task GetProdFromApiAsync()
        {
            //var response = await _httpClient.GetAsync("https://financeapp-gvaxa5fravg5grf2.ukwest-01.azurewebsites.net/api/receipts/get-products");
            var response = await _httpClient.GetAsync("http://localhost:5133/api/receipts/get-products");

            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                List<string> products = JsonSerializer.Deserialize<List<string>>(content);
                productListView.ItemsSource = products;
            }
            else
            {
                Console.WriteLine("Error: " + response.ReasonPhrase);
            }
        }
        private async void OnGetProductsClicked(object sender, EventArgs e)
        {
            await GetProdFromApiAsync();
        }

        private async Task LoadStoresIntoPickerAsync()
        {
            var response = await _httpClient.GetAsync("http://localhost:5133/api/receipts/get-stores");

            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                var storeList = JsonSerializer.Deserialize<List<string>>(content);
                storePicker.ItemsSource = storeList;
            }
            else
            {
                Console.WriteLine("Ошибка: " + response.ReasonPhrase);
            }
        }
    }
}


