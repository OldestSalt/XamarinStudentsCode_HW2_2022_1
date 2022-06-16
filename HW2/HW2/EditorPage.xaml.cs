using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using System.Collections.Generic;

namespace HW2 {
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class EditorPage : ContentPage {
        private List<string> stateArray;
        private int currentState, lastState;
        private bool userChanged;
        public Note note;
        public EditorPage(Note note = null) {
            InitializeComponent();
            if (note != null) {
                this.note = note;
                editor.SetBinding(Editor.TextProperty, new Binding { Source = this.note, Path = "full_text", Mode = BindingMode.TwoWay });
            }

            stateArray = new List<string>();
            currentState = 0;
            lastState = 0;
            userChanged = true;

            stateArray.Add(editor.Text);

        }

        protected override void OnAppearing() {
            base.OnAppearing();
            editor.Focus();
        }



        private void Button_Clicked(object sender, EventArgs e) {
            note = new Note() { full_text = editor.Text, creation_data = DateTime.Now.ToString("hh:mm, dd/MM/yyyy") };
            Navigation.PopAsync();
        }

        private void editor_TextChanged(object sender, TextChangedEventArgs e) {
            count.Text = $"{editor.Text.Length} символов";
            if (stateArray != null) {
                if (userChanged) {
                    try {
                        stateArray[++currentState] = editor.Text;
                        lastState = currentState;
                    }
                    catch {
                        stateArray.Add(editor.Text);
                        ++lastState;
                    }
                }
                else {
                    userChanged = true;
                }
            }
        }

        private void Undo(object sender, EventArgs e) {
            if (currentState > 0) {
                currentState--;
                userChanged = false;
                editor.Text = stateArray[currentState];
            }
        }
        private void Redo(object sender, EventArgs e) {
            if (currentState < lastState) {
                currentState++;
                userChanged = false;
                editor.Text = stateArray[currentState];
            }
        }
    }
}