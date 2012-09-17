using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Input;
using System.Windows;

namespace MyTestApp.ViewModels
{
    public class ClassViewModel
    {
        public class OpenCommand : ICommand
        {
            #region ICommand Members

            public bool CanExecute(object parameter)
            {
                return true;
            }

            public event EventHandler CanExecuteChanged;

            public void Execute(object parameter)
            {
                MessageBox.Show("Open");
            }

            #endregion
        }

        private static Random rnd = new Random();
        public ClassViewModel()
        {
            Properties = new List<string>();
            var max = rnd.Next(10);
            for (int i = 0; i < max; i++)
            {
                Properties.Add("Property " + (i + 1));
            }

            Open = new OpenCommand();
        }

        public string Name { get; set; }
        public bool IsAbstract { get; set; }
        public IList<string> Properties { get; private set; }

        public ICommand Open { get; private set; }
    }

}
