
namespace FinanceApp.Client
{
    public partial class NewStorePopup
    {
        public NewStorePopup()
        {
            InitializeComponent();
        }

        private void OnSaveClicked(object sender, EventArgs e)
        {
            string storeName = StoreNameEntry.Text;
            if (!string.IsNullOrWhiteSpace(storeName))
            {
                Close(storeName);
            }
        }

        private void OnCancelClicked(object sender, EventArgs e)
        {
            Close(null);
        }
    }
}