﻿using System;
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
using BlAPI;
using BO;

namespace PL
{
    /// <summary>
    /// Interaction logic for UpdateBus.xaml
    /// </summary>
    public partial class UpdateBus : Window
    {
        IBL bl;
        BOBus bus = new BOBus();
        public UpdateBus(IBL ibl, BOBus b)
        {
            bl = ibl;
            bus = b;
            InitializeComponent();
            _LicenseNumber.DataContext = b.LicenseNumber;
            _LicensingDate.SelectedDate = b.LicensingDate;
            _DateCare.SelectedDate = b.DateOfLastCare;
            _KmCare.Text = b.KmFromLastCare.ToString();
            _KmRefuel.Text = b.KmFromLastRefuel.ToString();
            _Kilometerage.Text = b.Kilometerage.ToString();
            _DName.Text = b.Driver;

        }

        private void Updating_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                BOBus b = new BOBus()
                {
                    IsExist = true,
                    LicenseNumber = bus.LicenseNumber,
                    LicensingDate = bus.LicensingDate,
                    DateOfLastCare = DateTime.Parse(_DateCare.ToString()),
                    KmFromLastCare = int.Parse(_KmCare.Text),
                    KmFromLastRefuel = int.Parse(_KmRefuel.Text),
                    Kilometerage = int.Parse(_Kilometerage.Text),
                    Driver = _DName.Text
                };
                bl.UpdateBus(b);
                Close();
            }
            catch (BLException ex)
            {
                MessageBox.Show(ex.Message + "\nEdit and try again");
            }
            catch
            {
                MessageBox.Show("ERROR!\n" + "Missing input" + "\nEdit and try again");
            }
        }

        private void TextBox_OnlyNumbers_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            TextBox text = sender as TextBox;
            if (text == null) return;
            if (e == null) return;

            //allow list of system keys (add other key here if you want to allow)
            if (e.Key == Key.Tab || e.Key == Key.Enter || e.Key == Key.Return || e.Key == Key.Escape || e.Key == Key.Back || e.Key == Key.Delete ||
                e.Key == Key.CapsLock || e.Key == Key.LeftShift || e.Key == Key.Home
             || e.Key == Key.End || e.Key == Key.Insert || e.Key == Key.Down || e.Key == Key.Right)
                return;

            char c = (char)KeyInterop.VirtualKeyFromKey(e.Key);

            //allow control system keys
            if (Char.IsControl(c)) return;

            if (Char.IsDigit(c)) //allow digits (without Shift or Alt)
                if (!(Keyboard.IsKeyDown(Key.LeftShift) || Keyboard.IsKeyDown(Key.RightShift) || Keyboard.IsKeyDown(Key.RightAlt)))
                    return; //let this key be written inside the textbox

            //forbid letters and signs (#,$, %, ...)
            e.Handled = true; //ignore this key. mark event as handled, will not be routed to other controls
            return;
        } //checking if the input contains digits only
    }
}
