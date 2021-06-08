using Nuke.Common;
using Nuke.Common.CI;
using Nuke.Common.Execution;
using Nuke.Common.Git;
using Nuke.Common.ProjectModel;
using Nuke.Common.Tooling;
using Nuke.Common.Tools.DotNet;
using static Nuke.Common.Tools.DotNet.DotNetTasks;

[CheckBuildProjectConfigurations]
[ShutdownDotNetAfterServerBuild]
//[GitHubActions("build-and-test", GitHubActionsImage.WindowsLatest, OnPushBranches = new[] { "master" })]
class Build : NukeBuild
{
    public static int Main() => Execute<Build>(x => x.FunctionalTest);

    [Parameter("MyParameter Cezary Walenciuk")]
    readonly string MyParameter;

    [Parameter("Configuration to build - Default is 'Debug' (local) or 'Release' (server)")]
    readonly Configuration Configuration = IsLocalBuild ? Configuration.Debug : Configuration.Release;






    [Solution] readonly Solution Solution;
    [GitRepository] readonly GitRepository GitRepository;


    [PathExecutable]
    readonly Tool Git;

    Target Clean => _ => _
        .Before(Restore)
        .Executes(() =>
        {
            Git("status");
        });



    Target UnitTest => _ => _
   .DependsOn(Compile)
   .Executes(() =>
   {
       DotNetTest(s => s.SetProjectFile(RootDirectory / "GamesAPI.UnitTest")
       .EnableNoRestore()
       .EnableNoBuild());
   });




    private IProcess _ApiProcess;
    Target StartAPI => _ => _
    .Executes(() =>
    {
        _ApiProcess = ProcessTasks.StartProcess("dotnet", "run", RootDirectory / "GamesAPI");
    });

    Target StopAPI => _ => _
   .Executes(() =>
   {
       _ApiProcess.Kill();
   });

    Target FunctionalTest => _ => _
    .DependsOn(StartAPI, UnitTest)
    .Triggers(StopAPI)
    .Executes(() =>
    {
        DotNetTest(s => s.SetProjectFile(RootDirectory / "GamesAPI.FunctionalTest")
        .EnableNoRestore()
        .EnableNoBuild());
    });

    Target Restore => _ => _
        .Executes(() =>
        {
            DotNetRestore(s => s
                .SetProjectFile(Solution));
        });

    Target Compile => _ => _
        .DependsOn(Restore)
        .Executes(() =>
        {
            DotNetBuild(s => s
                .SetProjectFile(Solution)
                .SetConfiguration(Configuration)
                .EnableNoRestore());
        });

}
