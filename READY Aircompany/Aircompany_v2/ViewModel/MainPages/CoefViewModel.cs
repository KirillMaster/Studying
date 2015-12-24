using Airport.ViewModel;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace Airport.ViewModel
{
    public class CoefViewModel : INotifyPropertyChanged
    {
        #region Implement INotyfyPropertyChanged members

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        #endregion

        #region Variable Declaration
        private float milesCount;
        private float bussinesClass;
        private float economClass;
        private float dateOrder;
        private bool isButtonSaveEnabled;
        private bool isMilesCountEnabled;
        private bool isBussinesClassEnabled;
        private bool isEconomClassEnabled;
        private bool isDateOrderEnabled;
        #endregion

        #region Properties
        public float MilesCount
        {
            get { return milesCount; }
            set
            {
                milesCount = value;
                OnPropertyChanged("MilesCount");
            }
        }
        public float BussinesClass
        {
            get { return bussinesClass; }
            set
            {
                bussinesClass = value;
                OnPropertyChanged("BussinesClass");
            }
        }
        public float EconomClass
        {
            get { return economClass; }
            set
            {
                economClass = value;
                OnPropertyChanged("EconomClass");
            }
        }
        public float DateOrder
        {
            get { return dateOrder; }
            set
            {
                dateOrder = value;
                OnPropertyChanged("DateOrder");
            }
        }
        public bool IsButtonSaveEnabled
        {
            get { return isButtonSaveEnabled; }
            set
            {
                isButtonSaveEnabled = value;
                OnPropertyChanged("IsButtonSaveEnabled");
            }
        }
        public bool IsMilesCountEnabled
        {
            get { return isMilesCountEnabled; }
            set
            {
                isMilesCountEnabled = value;
                OnPropertyChanged("IsMilesCountEnabled");
            }
        }
        public bool IsBussinesClassEnabled
        {
            get { return isBussinesClassEnabled; }
            set
            {
                isBussinesClassEnabled = value;
                OnPropertyChanged("IsBussinesClassEnabled");
            }
        }
        public bool IsEconomClassEnabled
        {
            get { return isEconomClassEnabled; }
            set
            {
                isEconomClassEnabled = value;
                OnPropertyChanged("IsEconomClassEnabled");
            }
        }
        public bool IsDateOrderEnabled
        {
            get { return isDateOrderEnabled; }
            set
            {
                isDateOrderEnabled = value;
                OnPropertyChanged("IsDateOrderEnabled");
            }
        }
        #endregion

        #region Command Declaration
        public ICommand CheckDigits { get; set; }
        public ICommand OnClickSave { get; set; }
        public ICommand OnClickExit { get; set; }
        #endregion

        public CoefViewModel()
        {
            OnClickSave = new Command(arg => ButtonSaveClicked(arg));
            OnClickExit = new Command(arg => ButtonExitClicked(arg));
            CheckDigits = new Command(arg => Check.OnlyDigits(arg));
            PermanentCoefficientsLogic logic = new PermanentCoefficientsLogic();
            logic.SelectAll();
            if(logic.PermanentCoefficientss.Count!=0)
            {
                MilesCount = (float)logic.PermanentCoefficientss[0].mileCoefficient;
                BussinesClass = (float)logic.PermanentCoefficientss[0].BusinessClassCoefficient;
                EconomClass = (float)logic.PermanentCoefficientss[0].economClassCoefficient;
                DateOrder = (float)logic.PermanentCoefficientss[0].buyDateCoefficient;

            }
            if (MainViewModel.Admin)
            {
                OpenAccess();
            }
            else CloseAccess();

        }

        private void OpenAccess()
        {
            IsBussinesClassEnabled = true;
            IsButtonSaveEnabled = true;
            IsDateOrderEnabled = true;
            IsEconomClassEnabled = true;
            IsMilesCountEnabled = true;
        }

        private void CloseAccess()
        {
            IsBussinesClassEnabled = false;
            IsButtonSaveEnabled = false;
            IsDateOrderEnabled = false;
            IsEconomClassEnabled = false;
            IsMilesCountEnabled = false;
        }


        private void Validation()
        {
            Check.AllStringInput(MilesCount.ToString(), BussinesClass.ToString(), EconomClass.ToString(), DateOrder.ToString());
        }

        private void ButtonSaveClicked(object window)
        {
            try
            {
                Validation();
                ButtonExitClicked(window);
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message, "Ошибка!", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            PermanentCoefficientsLogic logic = new PermanentCoefficientsLogic();
            logic.SelectAll();
            logic.Information = new PermanentCoefficientInfo(0, (float)MilesCount, (float)EconomClass, (float)BussinesClass, (float)DateOrder);
            if (logic.PermanentCoefficientss.Count == 0)
            {

                logic.Insert();
            }
            else
            {
                logic.DeleteByPrimaryKey();
                logic.Insert();
            }
        }

        private void ButtonExitClicked(object window)
        {
            Window CurrentWindow = window as Window;
            CurrentWindow.Close();
        }
    }
}
