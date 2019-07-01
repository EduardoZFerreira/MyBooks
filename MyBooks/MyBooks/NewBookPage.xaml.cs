using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MyBooks
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class NewBookPage : ContentPage
    {
        private Book Book = new Book();
        public NewBookPage()
        {
            InitializeComponent();
        }

        public NewBookPage(Book book)
        {
            InitializeComponent();
            SetBook(book);
            SetFieldsForEdit();            
        }

        private void SetFieldsForEdit()
        {
            nameEntry.Text = Book.Name;
            authorEntry.Text = Book.Author;
        }

        private void SetBook(Book book)
        {
            Book = book;
        }

        private void Button_Clicked(object sender, EventArgs e)
        {
            SetBook(new Book() { Id = Book.Id, Name  = nameEntry.Text,  Author = authorEntry.Text });
            Save();
        }

        private void Save()
        {
            try
            {
                using (SQLite.SQLiteConnection conn = new SQLite.SQLiteConnection(App.DB_PATH))
                {
                    conn.CreateTable<Book>();
                    string msg = Book.Id > 0 ? "updated" : "created";
                    int rowNumber = Book.Id > 0 ? conn.Update(Book) : conn.Insert(Book);
                    DisplayAlert("Book " + msg + "!", "", "Ok");
                    Navigation.PushAsync(new MainPage());
                }
            }
            catch(Exception e)
            {
                DisplayAlert("There was an error: " + e.Message, "ERROR", "Ok");
            }
        }

    }
}