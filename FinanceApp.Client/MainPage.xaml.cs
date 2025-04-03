using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Maui.Controls;
using FinanceApp.Lib.Dtos;
using System.Text.Json;
using System.Net.Http.Json;
using FinanceApp.Lib.Dtos;
using CommunityToolkit.Maui.Views;

namespace FinanceApp.Client
{
    public partial class MainPage : ContentPage
    {
        private readonly HttpClient _httpClient;
        private List<ProductEntryFields> productFieldsList = new();
        private Picker _categoryPicker;
        private Picker _subcategoryPicker;
        public Grid dynamicGrid2 { get; set; }
        public MainPage()
        {
            InitializeComponent();
            _httpClient = new HttpClient();
            LoadStoresIntoPickerAsync();
            _categoryPicker = new Picker() { Title = "Category" };
            _subcategoryPicker = new Picker() { Title = "Subcategory" };
        }
    
        private void OnStoreSelected(object sender, EventArgs e)
        {
            string selectedStore = storePicker.SelectedItem as string;
        }
        private void OnDateSelected(object sender, DateChangedEventArgs e)
        {
            DateTime selectedDate = e.NewDate;
            DisplayAlert("Selected", $"Selected date: {selectedDate.ToShortDateString()}", "OK");
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

            LoadCategoriesIntoPickerAsync();
            _categoryPicker.SelectedIndexChanged += CategoryPicker_SelectedIndexChanged;
            Grid.SetRow(_categoryPicker, 1);
            Grid.SetColumn(_categoryPicker, 1);
            dynamicGrid2.Children.Add(_categoryPicker);

            Grid.SetRow(_subcategoryPicker, 1);
            Grid.SetColumn(_subcategoryPicker, 2);
            Grid.SetColumnSpan(_subcategoryPicker, 2);
            dynamicGrid2.Children.Add(_subcategoryPicker);

            VerticalStackLayout stackLayout = this.FindByName<VerticalStackLayout>("MyStackLayout");
            productsLayout.Children.Add(dynamicGrid1);
            productsLayout.Children.Add(dynamicGrid2);

            var productFields = new ProductEntryFields
            {
                Quantity = quantity,
                ProductName = productName,
                Amount = amount,
                Discount = discount,
                Category = _categoryPicker,
                Subcategory = _subcategoryPicker
            };

            productFieldsList.Add(productFields);
        }
    
        private void OnAddProductClicked(object sender, EventArgs e)
        {
            AddRowToGrid();
        }
        
        private async void CategoryPicker_SelectedIndexChanged(object sender, EventArgs e)
        {
            var picker = (Picker)sender;
            string selectedCategory = picker.SelectedItem as string;

            if (selectedCategory != null)
            {
                await LoadSubcategoriesByCategoriesIntoPickerAsync(selectedCategory);
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
            productsLayout.Children.Clear();
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
            //var response = await _httpClient.GetAsync("https://financeapp-gvaxa5fravg5grf2.ukwest-01.azurewebsites.net/api/stores/get-stores");
            var response = await _httpClient.GetAsync("http://localhost:5133/api/stores/get-stores");

            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                var storeList = JsonSerializer.Deserialize<List<StoreDto>>(content, new JsonSerializerOptions
                {
                    PropertyNamingPolicy = JsonNamingPolicy.CamelCase
                });
                if(storeList != null)
                {
                    storePicker.ItemsSource = storeList.Select(s => s.StoreDtoName).ToList();
                }
            }
            else
            {
                await DisplayAlert("Error during load stores", response.ReasonPhrase, "OK");
            }
        }
        private async void OnAddStoreClicked(object sender, EventArgs e)
        {
            await CreateStoreAsync();
        }
        private async Task CreateStoreAsync()
        {
            var popup = new NewStorePopup();
            var result = await this.ShowPopupAsync(popup);

            if (result is string storeName)
            {
                try
                {
                    StoreDto newStore = new StoreDto { StoreDtoName = storeName };
                    //var response = await _httpClient.PostAsync("https://financeapp-gvaxa5fravg5grf2.ukwest-01.azurewebsites.net/api/receipts/add-store", jsonContent);
                    var response = await _httpClient.PostAsJsonAsync("http://localhost:5133/api/receipts/add-store", newStore);
                    if (response.IsSuccessStatusCode)
                    {
                        await DisplayAlert("Added", $"Store '{storeName}' Added!", "OK");
                        await LoadStoresIntoPickerAsync();
                    }
                    else
                    {
                        await DisplayAlert("Error", response.ReasonPhrase, "OK");
                    }
                }
                catch (Exception ex)
                {
                    await DisplayAlert("Error", ex.Message, "OK");
                }
            }
        }
        private async void OnAddCategoryClicked(object sender, EventArgs e)
        {
            
        }
        private async void OnAddSubcategoryClicked(object sender, EventArgs e)
        {

        }
        private async Task LoadCategoriesIntoPickerAsync()
        {
            //var response = await _httpClient.GetAsync("https://financeapp-gvaxa5fravg5grf2.ukwest-01.azurewebsites.net/api/categories/get-categories");
            var response = await _httpClient.GetAsync("http://localhost:5133/api/categories/get-categories");
            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                var categoryList = JsonSerializer.Deserialize<List<CategoryDto>>(content, new JsonSerializerOptions
                {
                    PropertyNamingPolicy = JsonNamingPolicy.CamelCase
                });
                if (categoryList != null)
                {
                    _categoryPicker.ItemsSource = categoryList.Select(c => c.CategoryDtoName).ToList();
                }
            }
            else
            {
                await DisplayAlert("Error during load categories", response.ReasonPhrase, "OK");
            }
        }
        private async Task LoadSubcategoriesByCategoriesIntoPickerAsync(string categoryName)
        {
            //var response = await _httpClient.GetAsync("https://financeapp-gvaxa5fravg5grf2.ukwest-01.azurewebsites.net/api/subcategories/get-subcategories-by-category-name?categoryName={categoryName}");
            var response = await _httpClient.GetAsync($"http://localhost:5133/api/subcategories/get-subcategories-by-category-name?categoryName={categoryName}");
            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                var subcategoryList = JsonSerializer.Deserialize<List<SubcategoryDto>>(content, new JsonSerializerOptions
                {
                    PropertyNamingPolicy = JsonNamingPolicy.CamelCase
                });
                if (subcategoryList != null)
                {
                    _subcategoryPicker.ItemsSource = subcategoryList.Select(s => s.SubcategoryDtoName).ToList();
                }
            }
            else
            {
                string responseContent = await response.Content.ReadAsStringAsync();
                await DisplayAlert("Error during load subcategories", response.ReasonPhrase + responseContent, "OK");
            }
        }
    }
}


