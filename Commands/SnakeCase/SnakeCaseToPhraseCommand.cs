using Microsoft.VisualStudio.Shell.Interop;
using System.ComponentModel.Design;
using System.Linq;
using System.Text;

namespace PhraseToMethod.Commands.SnakeCase;

internal sealed class SnakeCaseToPhraseCommand
{

    private readonly Package package;

    private SnakeCaseToPhraseCommand(Package package)
    {
        this.package = package ?? throw new ArgumentNullException("package");

        OleMenuCommandService commandService = this.ServiceProvider.GetService(typeof(IMenuCommandService)) as OleMenuCommandService;
        if (commandService != null)
        {
            var menuCommandID = new CommandID(PackageGuids.SnakeCaseToPhraseCommandSet, PackageIds.SnakeToPhraseId);
            var menuItem = new MenuCommand(this.MenuItemCallback, menuCommandID);
            commandService.AddCommand(menuItem);
        }
    }

    public static SnakeCaseToPhraseCommand Instance
    {
        get;
        private set;
    }

    private IServiceProvider ServiceProvider
    {
        get
        {
            return this.package;
        }
    }

    public static void Initialize(Package package)
    {
        Instance = new SnakeCaseToPhraseCommand(package);
    }

    private async void MenuItemCallback(object sender, EventArgs e)
    {
        var docView = await VS.Documents.GetActiveDocumentViewAsync();

        var selection = docView?.TextView.Selection.SelectedSpans.FirstOrDefault();

        if (selection.HasValue)
        {
            var value = selection.Value.Snapshot.GetText(selection.Value.Span);
            if (value.Contains(" "))
                VsShellUtilities.ShowMessageBox(
                     this.ServiceProvider,
                     "Invalid input",
                     "Error",
                     OLEMSGICON.OLEMSGICON_WARNING,
                     OLEMSGBUTTON.OLEMSGBUTTON_OK,
                     OLEMSGDEFBUTTON.OLEMSGDEFBUTTON_FIRST);
            else
                docView?.TextBuffer.Replace(selection.Value, SnakeCaseToPhraseConverter(value));

        }
    }

    private string SnakeCaseToPhraseConverter(string input)
    {

        StringBuilder strBuilder = new();
        for (int i = 0; i < input.Length; i++)
        {
            if (input[i] == '_')
                strBuilder.Append($" ");
            else if (char.IsUpper(input[i]))
                strBuilder.Append(input[i].ToString().ToLower());
            else
                strBuilder.Append(input[i]);
        }
        return strBuilder.ToString();
    }
}
