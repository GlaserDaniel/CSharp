using Common.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Collections;
using MimeKit;

namespace Common.ViewModel
{
    public class EmailViewModel : INotifyPropertyChanged, IComparable<EmailViewModel>, INotifyDataErrorInfo
    {
        private readonly Dictionary<string, List<string>> _errors = new Dictionary<string, List<string>>();

        private int _id;
        private string _sender;
        private List<string> _receiver;
        private string _subject;
        private string _message;
        private DateTime _dateTime;
        private bool _isRead;
        private bool _isHtml;
        private List<string> _attachments;

        public event PropertyChangedEventHandler PropertyChanged;
        public event EventHandler<DataErrorsChangedEventArgs> ErrorsChanged;

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
            Attachments = new List<string>();
        }

        public EmailViewModel(Email email)
        {
            Id = email.Id;
            Sender = email.Sender;
            Receivers = email.Receiver;
            Subject = email.Subject;
            Message = email.Message;
            DateTime = email.DateTime;
            IsRead = email.IsRead;
            IsHtml = email.IsHtml;
            Attachments = email.Attachments;
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

        public List<string> Attachments
        {
            get
            {
                return _attachments;
            }
            set
            {
                if (_attachments == value) return;
                _attachments = value;
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

        public bool HasErrors
        {
            get
            {
                return _errors.Count > 0;
            }
        }

        public IEnumerable GetErrors(string propertyName)
        {
            if (string.IsNullOrEmpty(propertyName) ||
                !_errors.ContainsKey(propertyName)) return null;
            return _errors[propertyName];
        }

        public void RaiseErrorsChanged(string propertyName)
        {
            ErrorsChanged?.Invoke(this, new DataErrorsChangedEventArgs(propertyName));
        }

        public void AddError(string propertyName, string error, bool isWarning)
        {
            if (!_errors.ContainsKey(propertyName))
                _errors[propertyName] = new List<string>();

            if (!_errors[propertyName].Contains(error))
            {
                if (isWarning) _errors[propertyName].Add(error);
                else _errors[propertyName].Insert(0, error);
                RaiseErrorsChanged(propertyName);
            }
        }

        public void RemoveError(string propertyName, string error)
        {
            if (_errors.ContainsKey(propertyName) &&
                _errors[propertyName].Contains(error))
            {
                _errors[propertyName].Remove(error);
                if (_errors[propertyName].Count == 0) _errors.Remove(propertyName);
                RaiseErrorsChanged(propertyName);
            }
        }
    }
}
