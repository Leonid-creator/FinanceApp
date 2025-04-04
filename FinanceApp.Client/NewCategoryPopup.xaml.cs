
namespace FinanceApp.Client
{
    public partial class NewCategoryPopup
    {
        public NewCategoryPopup()
        {
            InitializeComponent();
        }

        private void OnSaveClicked(object sender, EventArgs e)
        {
            string categoryName = CategoryNameEntry.Text;
            if (!string.IsNullOrWhiteSpace(categoryName))
            {
                Close(categoryName);
            }
        }

        private void OnCancelClicked(object sender, EventArgs e)
        {
            Close(null);
        }
    }
}