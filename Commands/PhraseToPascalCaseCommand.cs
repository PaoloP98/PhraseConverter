using System.ComponentModel.Design;
using System.Linq;
using System.Text.RegularExpressions;

namespace PhraseToMethod
{
	//[Command(PackageIds.PhraseToMethodCommand)]
	//internal sealed class PhraseToMethodCommand : BaseCommand<PhraseToMethodCommand>
	//{
	//	protected override async Task ExecuteAsync(OleMenuCmdEventArgs e)
	//	{
	//		var docView = await VS.Documents.GetActiveDocumentViewAsync();
	//		var selection = docView?.TextView.Selection.SelectedSpans.FirstOrDefault();

	//		if (selection.HasValue)
	//		{
	//			docView.TextBuffer.Replace(selection.Value, "da frase a metodo");
	//		}
	//	}
	//}

	//[Command(PackageIds.PhraseToPascalId)]
	//internal sealed class PhraseToMethodCommand : BaseCommand<PhraseToMethodCommand>
	//{
	//	protected override async Task ExecuteAsync(OleMenuCmdEventArgs e)
	//	{
	//		var docView = await VS.Documents.GetActiveDocumentViewAsync();
	//		var selection = docView?.TextView.Selection.SelectedSpans.FirstOrDefault();

	//		if (selection.HasValue)
	//		{
	//			docView.TextBuffer.Replace(selection.Value, RemoveWhiteSpaceCapitalize(selection.Value.Snapshot.GetText(selection.Value.Span)));
	//		}
	//	}

	//	private string RemoveWhiteSpaceCapitalize(string input)
	//	{
	//		var splitInput = Regex.Replace(input, @"\s+", " ")
	//		.Trim()
	//		.Split(' ')
	//		.Select(word => word[0].ToString().ToUpper() + word.Substring(1, word.Length - 1));


	//		return string.Join("", splitInput);
	//	}
	//}

	internal sealed class PhraseToPascalCase
	{

		private readonly AsyncPackage package;

		private PhraseToPascalCase(AsyncPackage package)
		{
			this.package = package ?? throw new ArgumentNullException("package");

			OleMenuCommandService commandService = this.ServiceProvider.GetService(typeof(IMenuCommandService)) as OleMenuCommandService;
			if (commandService != null)
			{
				var menuCommandID = new CommandID(PackageGuids.PhraseToPascaleCaseCommandSet, PackageIds.PhraseToPascalId);
				var menuItem = new MenuCommand(this.MenuItemCallback, menuCommandID);
				commandService.AddCommand(menuItem);
			}
		}

		public static PhraseToPascalCase Instance
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

		public static void Initialize(AsyncPackage package)
		{
			Instance = new PhraseToPascalCase(package);
		}

		private async void MenuItemCallback(object sender, EventArgs e)
		{
			var docView = await VS.Documents.GetActiveDocumentViewAsync();
			
			var selection = docView?.TextView.Selection.SelectedSpans.FirstOrDefault();

			if (selection.HasValue)
			{
				docView?.TextBuffer.Replace(selection.Value, RemoveWhiteSpaceCapitalize(selection.Value.Snapshot.GetText(selection.Value.Span)));
			}
		}

		private string RemoveWhiteSpaceCapitalize(string input)
		{
			var splitInput = Regex.Replace(input, @"\s+", " ")
			.Trim()
			.Split(' ')
			.Select(word => word[0].ToString().ToUpper() + word.Substring(1, word.Length - 1));

			return string.Join("", splitInput);
		}
	}
}
