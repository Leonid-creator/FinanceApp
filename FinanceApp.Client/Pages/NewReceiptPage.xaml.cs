using CommunityToolkit.Maui.Views;
using FinanceApp.Lib.Dtos;
using System.Net.Http.Json;
using System.Text.Json;
using System.Text;
using FinanceApp.Client.Api;

namespace FinanceApp.Client.Pages;

public partial class NewReceiptPage : ContentPage
{
    private readonly HttpClient _httpClient;
    private List<ProductEntryFields> productFieldsList = new();
    private Picker _categoryPicker;
    private Picker _subcategoryPicker;
    public Grid dynamicGrid1 { get; set; }
    public Grid dynamicGrid2 { get; set; }

    public NewReceiptPage()
    {
        InitializeComponent();
        _httpClient = new HttpClient();
        LoadStoresIntoPickerAsync();
        _categoryPicker = new Picker() { Title = "Category" };
        _subcategoryPicker = new Picker() { Title = "Subcategory" };
    }
    private void AddProductRowToGrid()
    {
        try
        {
            dynamicGrid1 = new Grid { Margin = new Thickness(0) };
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

            Entry quantity = new Entry { Placeholder = "Q-ty", Keyboard = Keyboard.Numeric};
            Grid.SetRow(quantity, 0);
            Grid.SetColumn(quantity, 0);
            dynamicGrid1.Children.Add(quantity);

            Entry productName = new Entry { Placeholder = "Prod. name" };
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

            Entry discount = new Entry { Placeholder = "Disc", Keyboard = Keyboard.Numeric};
            Grid.SetRow(discount, 0);
            Grid.SetColumn(discount, 0);
            dynamicGrid2.Children.Add(discount);

            Picker subcategoryPicker = new Picker { Title = "Subcategory" };
            subcategoryPicker.ItemsSource = _subcategoryPicker.ItemsSource;

            Picker categoryPicker = new Picker { Title = "Category" };
            LoadCategoriesIntoPickerAsync(categoryPicker);
            categoryPicker.SelectedIndexChanged += async (s, e) => await LoadSubcategoriesByCategoriesIntoPickerAsync(subcategoryPicker, categoryPicker.SelectedItem.ToString());
            Grid.SetRow(categoryPicker, 0);
            Grid.SetColumn(categoryPicker, 1);
            dynamicGrid2.Children.Add(categoryPicker);


            Grid.SetRow(subcategoryPicker, 0);
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
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
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
    private async Task SendReceiptToApiAsync()
    {
        var tempReceipt = new
        {
            StoreName = storePicker.SelectedItem?.ToString(),
            DateTime = datePicker.Date + timePicker.Time,
            TotalAmount = Convert.ToDecimal(totalAmountField.Text),
            ReceiptDiscount = Convert.ToDecimal(receiptDiscountField.Text)
        };
        //tempReceipt.DateTime = datePicker.Date + timePicker.Time;
        List<TempDetails> tempDetails = new List<TempDetails>();
        decimal sumOfPrices = 0;
        foreach (var fields in productFieldsList)
        {
            if (decimal.TryParse(fields.Amount.Text, out var amount) &&
                decimal.TryParse(fields.Discount.Text, out var discount) &&
                int.TryParse(fields.Quantity.Text, out var quantity))
            {
                tempDetails.Add(new TempDetails()
                {
                    ProductName = fields.ProductName.Text,
                    Quantity = quantity,
                    Amount = amount,
                    Discount = discount,
                    Category = fields.Category.SelectedItem?.ToString() ?? string.Empty,
                    Subcategory = fields.Subcategory.SelectedItem?.ToString() ?? string.Empty
                });
                sumOfPrices += (amount * quantity) + discount;
            }
        }
        decimal amountsDifference = Convert.ToDecimal(totalAmountField.Text) - Convert.ToDecimal(receiptDiscountField.Text) - sumOfPrices;
        if (amountsDifference != 0)
        {
            await Application.Current.MainPage.DisplayAlert("Error", $"The total price of the\nproducts does not equal\nthe total amount\nof the receipt!\n({amountsDifference})", "ОК");
        }
        else
        {
            var requestData = new
            {
                tempReceipt,
                tempDetails
            };

            var jsonContent = new StringContent(JsonSerializer.Serialize(requestData, new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase }), Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync(ApiEndpoints.BaseUrl + "receipts/add-full-receipt", jsonContent);

            if (response.IsSuccessStatusCode)
            {
                RemoveAllProdRows();
                await Application.Current.MainPage.DisplayAlert("Success", "Receipt added", "ОК");
            }
            else
            {
                var errorContent = await response.Content.ReadAsStringAsync();

                string errorMessage;

                try
                {
                    var errorObj = JsonSerializer.Deserialize<Dictionary<string, string>>(errorContent);
                    errorMessage = errorObj?["error"] ?? "Unknown error";
                }
                catch
                {
                    errorMessage = "Error parsing response from server.";
                }

                await Application.Current.MainPage.DisplayAlert("Error", errorMessage, "ОК");
            }
        }
    }
    private async Task GetProductsFromApiAsync()
    {
        var response = await _httpClient.GetAsync(ApiEndpoints.BaseUrl + "receipts/get-products");

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
    private async Task CreateStoreAsync()
    {
        var popup = new NewStorePopup();
        var result = await this.ShowPopupAsync(popup);

        if (result is string storeName)
        {
            try
            {
                StoreDto newStore = new StoreDto { StoreDtoName = storeName };
                var response = await _httpClient.PostAsJsonAsync(ApiEndpoints.BaseUrl + "stores/add-store", newStore);
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
    public async Task CreateCategoryAsync()
    {
        var popup = new NewCategoryPopup();
        var result = await this.ShowPopupAsync(popup);
        if (result is string categoryName)
        {
            try
            {
                CategoryDto newCategory = new CategoryDto { CategoryDtoName = categoryName };
                var response = await _httpClient.PostAsJsonAsync(ApiEndpoints.BaseUrl + "categories/add-category", newCategory);
                if (response.IsSuccessStatusCode)
                {
                    await DisplayAlert("Added", $"Category '{categoryName}' Added!", "OK");
                    //await LoadCategoriesIntoPickerAsync();
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
    public async Task CreateSubcategoryAsync()
    {
        await DisplayAlert("Error", "The function\nis not implemented", "OK");
    }
    private async Task LoadStoresIntoPickerAsync()
    {
        var response = await _httpClient.GetAsync(ApiEndpoints.BaseUrl + "stores/get-stores");

        if (response.IsSuccessStatusCode)
        {
            var content = await response.Content.ReadAsStringAsync();
            var storeList = JsonSerializer.Deserialize<List<StoreDto>>(content, new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            });
            if (storeList != null)
            {
                storePicker.ItemsSource = storeList.Select(s => s.StoreDtoName).ToList();
            }
        }
        else
        {
            await DisplayAlert("Error during load stores", response.ReasonPhrase, "OK");
        }
    }
    private async Task<Picker> LoadCategoriesIntoPickerAsync(Picker categoryPicker)
    {
        var response = await _httpClient.GetAsync(ApiEndpoints.BaseUrl + "categories/get-categories");
        if (response.IsSuccessStatusCode)
        {
            var content = await response.Content.ReadAsStringAsync();
            var categoryList = JsonSerializer.Deserialize<List<CategoryDto>>(content, new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            });
            if (categoryList != null)
            {
                categoryPicker.ItemsSource = categoryList.Select(c => c.CategoryDtoName).ToList();
            }
            return categoryPicker;
        }
        else
        {
            await DisplayAlert("Error during load categories", response.ReasonPhrase, "OK");
            throw new Exception("Failed to load categories");
        }
    }
    private async Task LoadSubcategoriesByCategoriesIntoPickerAsync(Picker subcategoryPicker, string categoryName)
    {
        var response = await _httpClient.GetAsync(ApiEndpoints.BaseUrl + $"subcategories/get-subcategories-by-category-name?categoryName={categoryName}");
        if (response.IsSuccessStatusCode)
        {
            var content = await response.Content.ReadAsStringAsync();
            var subcategoryList = JsonSerializer.Deserialize<List<SubcategoryDto>>(content, new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            });
            if (subcategoryList != null)
            {
                subcategoryPicker.ItemsSource = subcategoryList.Select(s => s.SubcategoryDtoName).ToList();
            }
        }
        else
        {
            string responseContent = await response.Content.ReadAsStringAsync();
            await DisplayAlert("Error during load subcategories", response.ReasonPhrase + responseContent, "OK");
        }
    }
    private void OnStoreSelected(object sender, EventArgs e)
    {
        string selectedStore = storePicker.SelectedItem as string;
    }
    //private void OnDateSelected(object sender, DateChangedEventArgs e)
    //{
    //    DateTime selectedDate = e.NewDate;
    //}
    //private void OnTimeSelected(object sender, TimeChangedEventArgs e)
    //{
    //    TimeSpan selectedTime = e.NewTime;
    //}
    private async void OnAddSubcategoryClicked(object sender, EventArgs e)
    {
        await CreateSubcategoryAsync();
    }
    private async void OnAddStoreClicked(object sender, EventArgs e)
    {
        await CreateStoreAsync();
    }
    private async void OnAddCategoryClicked(object sender, EventArgs e)
    {
        await CreateCategoryAsync();
    }
    private async void OnProcessReceiptClicked(object sender, EventArgs e)
    {
        await SendReceiptToApiAsync();
    }
    private void OnAddProductRowClicked(object sender, EventArgs e)
    {
        AddProductRowToGrid();
    }
    private async void OnGetProductsClicked(object sender, EventArgs e)
    {
        await GetProductsFromApiAsync();
    }
}