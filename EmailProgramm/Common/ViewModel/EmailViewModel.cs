﻿using Common.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Common.ViewModel
{
    public class EmailViewModel : INotifyPropertyChanged, IComparable<EmailViewModel>
    {
        private int _id;
        private string _sender;
        private List<string> _receiver;
        private string _subject;
        private string _message;
        private DateTime _dateTime;
        private bool _isRead;
        private bool _isHtml;
        private string _fileURI;

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public int CompareTo(EmailViewModel other)
        {
            return Id.CompareTo(other.Id);
        }

        public EmailViewModel()
        {
            Receivers = new List<string>();
        }

        public EmailViewModel(Email email)
        {
            this.Id = email.Id;
            this.Sender = email.Sender;
            this.Receivers = email.Receiver;
            this.Subject = email.Subject;
            this.Message = email.Message;
            this.DateTime = email.DateTime;
            this.IsRead = email.IsRead;
            this.IsHtml = email.IsHtml;
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

        public List<string> Receivers
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

        public bool IsHtml
        {
            get
            {
                return _isHtml;
            }
            set
            {
                if (_isHtml == value) return;
                _isHtml = value;
                OnPropertyChanged();
            }
        }

        public String DateTimeString
        {
            get
            {
                return DateTime.ToString();
            }
        }

        public string ReceiversString
        {
            get
            {
                string result = "";
                if (Receivers.Count > 0)
                {
                    result += Receivers[0].ToString();
                    for (int i = 1; i < Receivers.Count; i++)
                    {
                        result += ", " + Receivers[i].ToString();
                    }
                }

                return result.ToString();
            }
        }
    }
}
