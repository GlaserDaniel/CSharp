﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Common.ViewModel
{
    public class AccountViewModel : INotifyPropertyChanged
    {
        private string _user;
        private string _email;
        private string _password;
        private bool _useImap;
        private string _imapPop3Server;
        private int _imapPop3Port;
        private string _smtpServer;
        private int _smtpPort;
        private ObservableCollection<Email> _emails;

        public event PropertyChangedEventHandler PropertyChanged;

        //[NotifyPropertyChangedInvocator] TODO
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public AccountViewModel()
        {

        }

        public AccountViewModel(Account account)
        {

        }

        public ObservableCollection<Email> Emails
        {
            get { return _emails; }
            set
            {
                if (_emails == value) return;
                _emails = value;
                OnPropertyChanged();
            }
        }

        public string User
        {
            get
            {
                return _user;
            }
            set
            {
                if (_user == value) return;
                _user = value;
                OnPropertyChanged();
            }
        }

        public string Email
        {
            get
            {
                return _email;
            }
            set
            {
                if (_email == value) return;
                _email = value;
                OnPropertyChanged();
            }
        }

        

        public string Password
        {
            get { return _password; }
            set
            {
                if (_password == value) return;
                _password = value;
                OnPropertyChanged();
            }
        }

        public string ImapPop3Server
        {
            get { return _imapPop3Server; }
            set
            {
                if (_imapPop3Server == value) return;
                _imapPop3Server = value;
                OnPropertyChanged();
            }
        }
        
        public string SmtpServer
        {
            get { return _smtpServer; }
            set
            {
                if (_smtpServer == value) return;
                _smtpServer = value;
                OnPropertyChanged();
            }
        }

        public bool UseImap
        {
            get { return _useImap; }
            set
            {
                if (_useImap == value) return;
                _useImap = value;
                OnPropertyChanged();
            }
        }

        public int ImapPop3Port
        {
            get { return _imapPop3Port; }
            set
            {
                if (_imapPop3Port == value) return;
                _imapPop3Port = value;
                OnPropertyChanged();
            }
        }

        public int SmtpPort
        {
            get { return _smtpPort; }
            set
            {
                if (_smtpPort == value) return;
                _smtpPort = value;
                OnPropertyChanged();
            }
        }
    }
}