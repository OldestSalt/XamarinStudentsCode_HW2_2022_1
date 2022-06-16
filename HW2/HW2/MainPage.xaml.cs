using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace HW2 {
    public partial class MainPage : ContentPage {
        public static List<Note> list_all = new List<Note>();
        ObservableCollection<Note> list_left = new ObservableCollection<Note>();
        ObservableCollection<Note> list_right = new ObservableCollection<Note>();
        public MainPage() {
            InitializeComponent();
            SaveSystem.Load(out list_all);
            BindableLayout.SetItemsSource(notes_left, list_left);
            BindableLayout.SetItemsSource(notes_right, list_right);
            
        }

        protected override void OnSizeAllocated(double width, double height) {
            base.OnSizeAllocated(width, height);
            Task.Run(() => {
                Device.BeginInvokeOnMainThread(() => {
                    SortNotes();
                });
            });
        }

        private void SortNotes() {
            list_left.Clear();
            list_right.Clear();

            foreach (var note in list_all) {
                if (notes_left.Height <= notes_right.Height || list_left.Count() == 0) {
                    list_left.Add(note);
                }
                else {
                    list_right.Add(note);
                }
                notes_left.ResolveLayoutChanges();
                notes_right.ResolveLayoutChanges();
            }
        }

        private void Button_Clicked(object sender, EventArgs e) {
            var editor = new EditorPage();

            editor.Disappearing += (__, _) => {
                if (editor.note != null) {
                    list_all.Add(editor.note);

                    Task.Run(() => {
                        Device.BeginInvokeOnMainThread(() => {
                            SortNotes();
                        });
                    });

                    SaveSystem.Save(list_all);
                }
            };
            Navigation.PushAsync(editor);
        }

        private void TapGestureRecognizer_Tapped(object sender, EventArgs e) {
            var editor = new EditorPage(list_all.FirstOrDefault(x => x.id == int.Parse((sender as Frame).ClassId)));

            editor.Disappearing += (__, _) => {

                Task.Run(() => {
                    Device.BeginInvokeOnMainThread(() => {
                        SortNotes();
                    });
                });

                SaveSystem.Save(list_all);

            };
            Navigation.PushAsync(editor);
        }

        private void PanGestureRecognizer_PanUpdated(object sender, PanUpdatedEventArgs e) {
            FramePanUpdated(sender, e, true);
        }

        private void PanGestureRecognizer_PanUpdated_1(object sender, PanUpdatedEventArgs e) {
            FramePanUpdated(sender, e, false);
        }

        private async void FramePanUpdated(object sender, PanUpdatedEventArgs e, bool isLeft) {
            if (isLeft && e.TotalX > 0 || !isLeft && e.TotalX < 0) return;

            scroll.InputTransparent = true;
            var frame = sender as Frame;
            double offset = frame.Width * 0.3;

            switch (e.StatusType) {
                case GestureStatus.Running:
                    frame.TranslationX = (isLeft ? -1 : 1) * Math.Min(offset + 1, Math.Abs(e.TotalX));
                    break;
                case GestureStatus.Completed:
                
                case GestureStatus.Canceled:
                    if (Math.Abs(frame.TranslationX) < offset) {
                        await frame.TranslateTo(0, 0, 250);
                    }
                    else {
                        frame.TranslationX = 0;
                        scroll.InputTransparent = false;
                        Note note = list_all.FirstOrDefault(u => u.id.ToString() == frame.ClassId);
                        bool res = await DisplayAlert("Are you sure?", "You will delete the note.", "Yes", "No", FlowDirection.RightToLeft);
                        if (res) {
                            list_all.Remove(note);
                            await Task.Run(() => {
                                Device.BeginInvokeOnMainThread(() => {
                                    SortNotes();
                                });
                            });
                            SaveSystem.Save(list_all);
                        };
                    }
                    break;
            }
        }
    }
}
