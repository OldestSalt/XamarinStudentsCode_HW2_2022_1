using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace HW2 {
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class EditorPage : ContentPage {
        public Note note;
        public EditorPage(Note note = null) {
            InitializeComponent();
            if (note != null) {
                this.note = note;
            }
            else {
                this.note = new Note() { full_text = "", creation_data = DateTime.Now.ToString("dd/MM/yyyy") };
            }
            editor.SetBinding(Editor.TextProperty, new Binding { Source = this.note.full_text, Mode = BindingMode.OneWayToSource });
            count.SetBinding(Label.TextProperty, new Binding { Source = this.note.text_count }); //почему-то не обновляется, призываю индусов \o/
            editor.Text = this.note.full_text;

        }

        protected override void OnAppearing() {
            base.OnAppearing();
            editor.Focus();
        }

        private void Editor_TextChanged(object sender, TextChangedEventArgs e) {
            note.full_text = e.NewTextValue;
        }

        private void Button_Clicked(object sender, EventArgs e) {
            Navigation.PopAsync();
        }
    }
}