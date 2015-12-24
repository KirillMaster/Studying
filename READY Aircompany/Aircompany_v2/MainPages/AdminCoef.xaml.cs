using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Airport
{
    /// <summary>
    /// Логика взаимодействия для AdminCoef.xaml
    /// </summary>
    public partial class AdminCoef 
    {
        private bool closed;

        public bool Exited
        {
            get { return closed; }
        }
        public AdminCoef()
        {
            InitializeComponent();
        }

        private void ButtonSave_Click(object sender, RoutedEventArgs e)
        {
            closed = true;
            this.Close();

        }

        private void ButtonExit_Click(object sender, RoutedEventArgs e)
        {
            closed = true;
            this.Close();
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            closed = true;
        }
    }
}
