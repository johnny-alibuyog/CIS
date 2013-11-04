using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace CIS.UI.Utilities.Controls
{
    public static class PasswordService
    {
        public static readonly DependencyProperty PasswordProperty =
            DependencyProperty.RegisterAttached("Password", typeof(string), typeof(PasswordService),
            new FrameworkPropertyMetadata(string.Empty, OnPasswordPropertyChanged));

        public static readonly DependencyProperty AttachProperty =
            DependencyProperty.RegisterAttached("Attach", typeof(bool), typeof(PasswordService),
            new PropertyMetadata(false, Attach));

        private static readonly DependencyProperty IsUpdatingProperty =
           DependencyProperty.RegisterAttached("IsUpdating", typeof(bool), typeof(PasswordService));

        public static void SetAttach(DependencyObject dependencyObject, bool value)
        {
            dependencyObject.SetValue(AttachProperty, value);
        }

        public static bool GetAttach(DependencyObject dependencyObject)
        {
            return (bool)dependencyObject.GetValue(AttachProperty);
        }

        public static string GetPassword(DependencyObject dependencyObject)
        {
            return (string)dependencyObject.GetValue(PasswordProperty);
        }

        public static void SetPassword(DependencyObject dependencyObject, string value)
        {
            dependencyObject.SetValue(PasswordProperty, value);
        }

        private static bool GetIsUpdating(DependencyObject dependencyObject)
        {
            return (bool)dependencyObject.GetValue(IsUpdatingProperty);
        }

        private static void SetIsUpdating(DependencyObject dependencyObject, bool value)
        {
            dependencyObject.SetValue(IsUpdatingProperty, value);
        }

        private static void OnPasswordPropertyChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            PasswordBox passwordBox = sender as PasswordBox;
            passwordBox.PasswordChanged -= PasswordChanged;

            if (!(bool)GetIsUpdating(passwordBox))
                passwordBox.Password = (string)e.NewValue;

            passwordBox.PasswordChanged += PasswordChanged;
        }

        private static void Attach(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            var passwordBox = sender as PasswordBox;

            if (passwordBox == null)
                return;

            if ((bool)e.OldValue)
                passwordBox.PasswordChanged -= PasswordChanged;

            if ((bool)e.NewValue)
                passwordBox.PasswordChanged += PasswordChanged;
        }

        private static void PasswordChanged(object sender, RoutedEventArgs e)
        {
            var passwordBox = sender as PasswordBox;
            SetIsUpdating(passwordBox, true);
            SetPassword(passwordBox, passwordBox.Password);
            SetIsUpdating(passwordBox, false);
        }
    }
}
