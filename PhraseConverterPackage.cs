global using Community.VisualStudio.Toolkit;

global using Microsoft.VisualStudio.Shell;

global using System;

global using Task = System.Threading.Tasks.Task;
using PhraseToMethod.Commands.SnakeCase;
using System.Runtime.InteropServices;
using System.Threading;

namespace PhraseToMethod
{
	[PackageRegistration(UseManagedResourcesOnly = true, AllowsBackgroundLoading = true)]
	[InstalledProductRegistration(Vsix.Name, Vsix.Description, Vsix.Version)]
	[ProvideMenuResource("Menus.ctmenu", 1)]
	[Guid(PackageGuids.PhraseToMethodString)]
	public sealed class PhraseConverterPackage : ToolkitPackage
	{
		protected override async Task InitializeAsync(CancellationToken cancellationToken, IProgress<ServiceProgressData> progress)
		{
			await this.RegisterCommandsAsync();
			PascalCaseToPhrase.Initialize(this);
			PhraseToPascalCase.Initialize(this);
			SnakeCaseToPhraseCommand.Initialize(this);
			PhraseToSnakeCaseCommand.Initialize(this);
		}
	}
}