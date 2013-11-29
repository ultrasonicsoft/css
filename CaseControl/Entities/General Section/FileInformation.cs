using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

namespace CaseControl
{
    public class ClientFileInformation
    {
        private string firstName;
        private string lastName;
        private string fileID;

        private string injuryNoteNumber;
        private string createdDate;
        private string description;

        private string miscNoteNumber;
        private string miscCreatedDate;
        private string miscDescription;

        public string FirstName
        {
            get { return firstName; }
            set
            {
                firstName = value;
                NotifyPropertyChanged("FirstName");
            }
        }

        public string LastName
        {
            get { return lastName; }
            set
            {
                lastName = value;
                NotifyPropertyChanged("LastName");
            }
        }
        public string FileID
        {
            get { return fileID; }
            set
            {
                fileID = value;
                NotifyPropertyChanged("FileID");
            }
        }

        public string InjuryNoteNumber
        {
            get { return injuryNoteNumber; }
            set
            {
                injuryNoteNumber = value;
                NotifyPropertyChanged("InjuryNoteNumber");
            }
        }

        public string CreatedDate
        {
            get { return createdDate; }
            set
            {
                createdDate = value;
                NotifyPropertyChanged("CreatedDate");
            }
        }
        public string ModifiedDate { get; set; }
        public string Description
        {
            get { return description; }
            set
            {
                description = value;
                NotifyPropertyChanged("Description");
            }
        }

        public string MiscNoteNumber
        {
            get { return miscNoteNumber; }
            set
            {
                miscNoteNumber = value;
                NotifyPropertyChanged("MiscNoteNumber");
            }
        }

        public string MiscCreatedDate
        {
            get { return miscCreatedDate; }
            set
            {
                miscCreatedDate = value;
                NotifyPropertyChanged("MiscCreatedDate");
            }
        }
        public string MiscDescription
        {
            get { return miscDescription; }
            set
            {
                miscDescription = value;
                NotifyPropertyChanged("MiscDescription");
            }
        }

        public string Status { get; set; }

        public string ClientCreatedOn { get; set; }

        #region INotifyPropertyChanged Members

        public event PropertyChangedEventHandler PropertyChanged;

        #endregion

        #region Private Helpers

        private void NotifyPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        #endregion
    }
}
