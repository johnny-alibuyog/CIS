using System.Windows;
using System.Windows.Input;

namespace CIS.UI.Utilities.Behaviors;

public static class KeyBindingBehavior
{
    // Command
    public static readonly DependencyProperty CommandProperty = DependencyProperty.RegisterAttached("Command", typeof(ICommand), typeof(KeyBindingBehavior), new PropertyMetadata(null));
    public static void SetCommand(UIElement element, ICommand value) => element.SetValue(CommandProperty, value);
    public static ICommand GetCommand(UIElement element) => (ICommand)element.GetValue(CommandProperty);

    // CommandParameter
    public static readonly DependencyProperty CommandParameterProperty = DependencyProperty.RegisterAttached("CommandParameter", typeof(object), typeof(KeyBindingBehavior), new PropertyMetadata(null));
    public static void SetCommandParameter(UIElement element, object value) => element.SetValue(CommandParameterProperty, value);
    public static object GetCommandParameter(UIElement element) => element.GetValue(CommandParameterProperty);

    // Key
    public static readonly DependencyProperty KeyProperty = DependencyProperty.RegisterAttached("Key", typeof(Key), typeof(KeyBindingBehavior), new PropertyMetadata(Key.None, OnKeyChanged));
    public static void SetKey(UIElement element, Key value) => element.SetValue(KeyProperty, value);
    public static Key GetKey(UIElement element) => (Key)element.GetValue(KeyProperty);

    private static void OnKeyChanged(DependencyObject dependency, DependencyPropertyChangedEventArgs args)
    {
        if (dependency is not UIElement element)
            return;
        
        element.PreviewKeyDown -= OnPreviewKeyDown; // Detach to avoid memory leaks and duplicate subscriptions
        element.PreviewKeyDown += OnPreviewKeyDown;
    }

    private static void OnPreviewKeyDown(object sender, KeyEventArgs args)
    {
        var element = sender as UIElement;
        var key = GetKey(element);

        if (args.Key != key)
            return;

        var command = GetCommand(element);
        var commandParameter = GetCommandParameter(element);

        if (command == null || !command.CanExecute(commandParameter))
            return;
        
        command.Execute(commandParameter);
    }
}
