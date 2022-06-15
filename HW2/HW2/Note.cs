using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;

namespace HW2 {
    public class Note : INotifyPropertyChanged {

        private static int idCounter = 0;

        public int id { get; }
        public string visible_text { get => UIFixerSuperUtilsBazuka.Shorten(ft, 120, 3); }

        private string ft = "";
        public string full_text {
            get {
                return ft;
            }
            set {
                if (ft != value) {
                    ft = value;
                    OnPropertyChanged("full_text");
                    OnPropertyChanged("text_count");
                }
            }
        }
        public string text_count { get => $"{ft.Length} символов"; }
        public string creation_data { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged(string property = "") {
            if (PropertyChanged != null) {
                PropertyChanged(this, new PropertyChangedEventArgs(property));
            }
        }
        public Note() {
            id = ++idCounter;
        }
    }
}
