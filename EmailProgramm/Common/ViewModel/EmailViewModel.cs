using Common.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Common.ViewModel
{
    public class EmailViewModel : INotifyPropertyChanged
    {
        private int _id;
        private string _sender;
        private List<string> _receiver;
        private string _subject;
        private string _message;
        private DateTime _dateTime;
        private bool _isRead;
        private string _fileURI;

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public EmailViewModel()
        {
            Receiver = new List<string>();
        }

        public EmailViewModel(Email email)
        {
            this.Id = email.Id;
            this.Sender = email.Sender;
            this.Receiver = email.Receiver;
            this.Subject = email.Subject;
            this.Message = email.Message;
            this.DateTime = email.DateTime;
            this.IsRead = email.IsRead;
            this.FileURI = email.FileURI;
        }

        public int Id
        {
            get
            {
                return _id;
            }
            set
            {
                if (_id == value) return;
                _id = value;
                OnPropertyChanged();
            }
        }

        public string Sender
        {
            get
            {
                return _sender;
            }
            set
            {
                if (_sender == value) return;
                _sender = value;
                OnPropertyChanged();
            }
        }

        public List<string> Receiver
        {
            get
            {
                return _receiver;
            }
            set
            {
                if (_receiver == value) return;
                _receiver = value;
                OnPropertyChanged();
            }
        }

        public string Subject
        {
            get
            {
                return _subject;
            }
            set
            {
                if (_subject == value) return;
                _subject = value;
                OnPropertyChanged();
            }
        }

        public string Message
        {
            get
            {
                return _message;
            }
            set
            {
                if (_message == value) return;
                _message = value;
                OnPropertyChanged();
            }
        }

        public DateTime DateTime
        {
            get
            {
                return _dateTime;
            }
            set
            {
                if (_dateTime == value) return;
                _dateTime = value;
                OnPropertyChanged();
            }
        }

        public bool IsRead
        {
            get
            {
                return _isRead;
            }
            set
            {
                if (_isRead == value) return;
                _isRead = value;
                OnPropertyChanged();
            }
        }

        public string FileURI
        {
            get
            {
                return _fileURI;
            }
            set
            {
                if (_fileURI == value) return;
                _fileURI = value;
                OnPropertyChanged();
            }
        }
    }
}
