using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using Xamarin.Forms;

namespace MyBooks
{
    // Learn more about making custom code visible in the Xamarin.Forms previewer
    // by visiting https://aka.ms/xamarinforms-previewer
    [DesignTimeVisible(false)]
    public partial class MainPage : ContentPage
    {
        private ObservableCollection<Book> Books;
        public MainPage()
        {
            InitializeComponent();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            using(SQLite.SQLiteConnection conn = new SQLite.SQLiteConnection(App.DB_PATH))
            {
                conn.CreateTable<Book>();
                Books = new ObservableCollection<Book>(conn.Table<Book>());
                BooksListView.ItemsSource = Books;
            }

        }

        private void ToolbarItem_Activated(object sender, EventArgs e)
        {
            Navigation.PushAsync(new NewBookPage());
        }

        private async void BooksListView_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            Book selectedBook = Books[e.SelectedItemIndex];
            string action = await DisplayActionSheet("Edit book: " + selectedBook.Name, "Cancel", null, "Delete", "Edit info");
            if (action == "Edit info")
                Navigation.PushAsync(new NewBookPage(selectedBook));
            else if (action == "Delete")
                DeleteBook(selectedBook);
        }

        private void DeleteBook(Book book)
        {
            using(SQLite.SQLiteConnection conn = new SQLite.SQLiteConnection(App.DB_PATH))
            {
                conn.CreateTable<Book>();
                int rows = conn.Delete(book);
                Books.Remove(book);
                DisplayAlert("Book deleted!", "Book " + book.Name + " has been deleted!", "Ok");
            }
        }

    }
}
