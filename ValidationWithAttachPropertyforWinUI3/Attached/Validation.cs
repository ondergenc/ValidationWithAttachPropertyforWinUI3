using CommunityToolkit.Mvvm.ComponentModel;
using Microsoft.UI.Xaml;
using System.Collections;
using System.Linq;
using ValidationWithAttachPropertyforWinUI3.Extensions;

namespace ValidationWithAttachPropertyforWinUI3.Attached
{
    public sealed class Validation : DependencyObject
    {
        public static readonly DependencyProperty ValidationProviderProperty
            = DependencyProperty.RegisterAttached("ValidationProvider", typeof(ObservableValidator),
                typeof(Validation), new(null, OnValidationProviderChanged));

        public static readonly DependencyProperty ValidationPropertyNameProperty
            = DependencyProperty.RegisterAttached("ValidationPropertyName", typeof(string),
                typeof(Validation), null);

        public static readonly DependencyProperty ErrorsProperty
            = DependencyProperty.RegisterAttached("Errors", typeof(IEnumerable),
                typeof(Validation), null);

        public static readonly DependencyProperty HasErrorsProperty
            = DependencyProperty.RegisterAttached("HasErrors", typeof(bool),
            typeof(Validation), null);

        public static bool GetHasErrors(DependencyObject obj)
            => (bool)obj.GetValue(HasErrorsProperty);

        public static void SetHasErrors(DependencyObject obj, bool value)
            => obj.SetValue(HasErrorsProperty, value);

        public static string GetValidationPropertyName(DependencyObject obj)
            => (string)obj.GetValue(ValidationPropertyNameProperty);
        public static void SetValidationPropertyName(DependencyObject obj, string value)
            => obj.SetValue(ValidationPropertyNameProperty, value);

        public static IEnumerable GetErrors(DependencyObject obj)
            => (IEnumerable)obj.GetValue(ErrorsProperty);
        public static void SetErrors(DependencyObject obj, IEnumerable errors)
            => obj.SetValue(ErrorsProperty, errors);


        public static ObservableValidator GetValidationProvider(DependencyObject obj)
            => (ObservableValidator)obj.GetValue(ValidationProviderProperty);
        public static void SetValidationProvider(DependencyObject obj, ObservableValidator value)
            => obj.SetValue(ValidationProviderProperty, value);

        private static void OnValidationProviderChanged(DependencyObject sender, DependencyPropertyChangedEventArgs args)
        {
            sender.SetValue(ErrorsProperty, null);
            if (args.NewValue is ObservableValidator info)
            {
                string propName = GetValidationPropertyName(sender);
                if (!string.IsNullOrEmpty(propName))
                {
                    info.ErrorsChanged += (source, eventArgs) =>
                    {
                        if (eventArgs.PropertyName == propName)
                        {
                            var errorsString = string.Join("\n", info.GetFormattedErrors(propName));
                            sender.SetValue(ErrorsProperty, errorsString);

                            var hasErrors = info.GetErrors(propName)?.Cast<object>().Any() ?? false;
                            sender.SetValue(HasErrorsProperty, hasErrors);
                        }
                    };

                    sender.SetValue(ErrorsProperty, info.GetFormattedErrors(propName));

                    var initialHasErrors = info.GetErrors(propName)?.Cast<object>().Any() ?? false;
                    sender.SetValue(HasErrorsProperty, initialHasErrors);
                }
            }
        }
    }
}
