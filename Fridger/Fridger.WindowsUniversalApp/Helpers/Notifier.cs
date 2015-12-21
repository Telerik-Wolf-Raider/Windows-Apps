namespace Fridger.WindowsUniversalApp.Helpers
{
    using System;
    using System.Threading.Tasks;
    using Windows.UI.Popups;

    public class Notifier
    {
        public static async void Notify(string message)
        {
            var dialog = new MessageDialog(message);
            dialog.Commands.Add(new UICommand("OK"));
            await dialog.ShowAsync();
        }

        public static async Task<string> Ask(string message, string commandLabel)
        {
            var dialog = new MessageDialog(message);
            dialog.Commands.Add(new UICommand(commandLabel));
            dialog.Commands.Add(new UICommand("Cancel"));
            var result = await dialog.ShowAsync();
            return result.Label;
        }
    }
}
