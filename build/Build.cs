using System.Linq;

using Nuke.Common;
using Nuke.Common.IO;
using Nuke.Common.Git;
using Nuke.Common.ProjectModel;
using Nuke.Common.Tools.DotNet;
using Nuke.Common.Tools.GitVersion;
using Nuke.Common.Utilities.Collections;
using Nuke.Common.CI.GitHubActions;

using static Nuke.Common.IO.FileSystemTasks;
using static Nuke.Common.IO.PathConstruction;
using static Nuke.Common.Tools.DotNet.DotNetTasks;
using System.IO;
using System.Threading.Tasks;
using Nuke.Common.Tools.GitHub;
using Nuke.Common.ChangeLog;
using System;
using ParameterAttribute = Nuke.Common.ParameterAttribute;
using Microsoft.Build.Tasks;
using Nuke.Common.Tools.MSBuild;
using Nuke.Common.Utilities;
using Nuke.Common.Tools.Git;
using LibGit2Sharp;




[GitHubActions("continuous",
GitHubActionsImage.WindowsLatest,
AutoGenerate = false,
FetchDepth = 0,
	OnPushBranches = new[]
	{
		"development"

	},
	OnPullRequestBranches = new[]
	{
		"development"
	},
	InvokedTargets = new[]
	{
		nameof(Publish),
	},
	EnableGitHubToken = true,
	CacheKeyFiles = new[] { "**/global.json", "**/*.csproj" },
	CacheIncludePatterns = new[] { ".nuke/temp", "~/.nuget/packages" },
	CacheExcludePatterns = new string[0]

)]
class Build : NukeBuild
{


	public static int Main() => Execute<Build>(x => x.Publish);


	//[Parameter("MyGet Feed Url for Public Access of Pre Releases")]
	//readonly string MyGetNugetFeed;
	//[Parameter("MyGet Api Key"), Secret]
	//readonly string MyGetApiKey;

	//[Parameter("Nuget Feed Url for Public Access of Pre Releases")]
	//readonly string NugetFeed;
	//[Parameter("Nuget Api Key"), Secret]
	//readonly string NuGetApiKey;

	//[Parameter("Copyright Details")]
	//readonly string Copyright;

	//[Parameter("Artifacts Type")]
	//readonly string ArtifactsType;

	//[Parameter("Excluded Artifacts Type")]
	//readonly string ExcludedArtifactsType;

	//[GitVersion]
	//readonly GitVersion GitVersion;

	//[GitRepository]
	//readonly GitRepository GitRepository;

	[Solution(GenerateProjects = true)]
	readonly Solution Solution;

	static GitHubActions GitHubActions => GitHubActions.Instance;
	static AbsolutePath ArtifactsDirectory => RootDirectory / ".artifacts";

	static readonly string PackageContentType = "application/octet-stream";
	static string ChangeLogFile => RootDirectory / "CHANGELOG.md";

	readonly Configuration Configuration = IsLocalBuild ? Configuration.Debug : Configuration.Release;

	Target Clean => _ => _
		.Before(Restore)
		.Executes(() =>
		{

		});

	Target Restore => _ => _
		.Executes(() =>
		{
			DotNetRestore(x =>
				x.SetProjectFile(Solution.Platfroms.HolyClient_Desktop));
		});

	Target Compile => _ => _
		.DependsOn(Restore)
		.Executes(() =>
		{
			DotNetBuild(x =>
				x.SetProjectFile(Solution.Platfroms.HolyClient_Desktop)
				.EnableNoRestore());
		});


	readonly AbsolutePath SetupExe = ArtifactsDirectory / "setup.exe";
	readonly AbsolutePath HolyClient_Application = ArtifactsDirectory / "HolyClient.Desktop.application";
	readonly AbsolutePath ApplicationFiles = ArtifactsDirectory / "Application Files";

	readonly AbsolutePath NuGetDirectory = RootDirectory / "NuGetArtifacts";


	readonly AbsolutePath ClickOnceArtifacts = RootDirectory / "ClickOnceArtifacts";



	Target Publish => _ => _
		.DependsOn(Compile)
		.Produces(ArtifactsDirectory / "*.exe")
		.Executes(() =>
		{



			DotNetPack(x => x
				.SetProject(Solution.McProtoNet.McProtoNet)
				.SetOutputDirectory(NuGetDirectory));

			DotNetPublish(x => x
				.SetProject(Solution.Platfroms.HolyClient_Desktop)
				.EnableNoRestore()
				.EnableNoBuild()
				.SetProperty("DebugType", "None")
				.SetProperty("DebugSymbols", "False")
				.SetPublishProfile("FolderProfile")
				.SetProperty("PublishDir", ArtifactsDirectory));


		});

	Target Pack => _ => _		
		.Executes(() =>
		{

			DotNetRestore(x => x
				.SetProjectFile(Solution.ProxyLib.QuickProxyNet));

			DotNetBuild(x => x
				.EnableNoRestore()
				.SetProjectFile(Solution.ProxyLib.QuickProxyNet));

			DotNetPack(x => x
				.EnableNoRestore()
				.EnableNoBuild()
				.SetProject(Solution.ProxyLib.QuickProxyNet)
				.SetOutputDirectory(NuGetDirectory));


		});



}
