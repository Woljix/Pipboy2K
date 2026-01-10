using System;
using System.Linq;
using Nuke.Common;
using Nuke.Common.CI;
using Nuke.Common.Execution;
using Nuke.Common.IO;
using Nuke.Common.ProjectModel;
using Nuke.Common.Tooling;
using Nuke.Common.Tools.Git;
using Nuke.Common.Tools.DotNet;
using Nuke.Common.Utilities.Collections;
using static Nuke.Common.EnvironmentInfo;
using static Nuke.Common.IO.PathConstruction;
using static Nuke.Common.Tools.DotNet.DotNetTasks;
using static Nuke.Common.Tools.Git.GitTasks;
using System.Collections.Generic;
using System.Runtime.InteropServices;

class Build : NukeBuild
{
    public static int Main() => Execute<Build>(x => x.Compile);

    [Solution] readonly Solution Solution;

    [Parameter("Configuration to build - Default is 'Debug' (local) or 'Release' (server)")]
    readonly Configuration Configuration = IsLocalBuild ? Configuration.Debug : Configuration.Release;

    [Parameter("Target platform fro the build")]
    readonly string[] Runtimes;

    IEnumerable<string> TargetRuntimes => Runtimes ?? new[] { GetHostRuntimeIdentifier() };

    AbsolutePath RaylibSource => RootDirectory / "external" / "raylib";
    AbsolutePath RaylibNativeArtifact => RaylibSource / "src" / "libraylib.so";

    AbsolutePath OutputDirectory => RootDirectory / "output";

    Target BuildRaylib => _ => _
    .OnlyWhenStatic(() => TargetRuntimes.Contains("linux-arm64"))
    .Executes(() =>
    {
        Serilog.Log.Information("Preparing Raylib...");

        if (!RaylibSource.DirectoryExists())
            Git($"clone --branch 5.5 --single-branch --depth 1 https://github.com/raysan5/raylib.git {RaylibSource}");

        Serilog.Log.Information("Building Raylib 5.5 for ARM64...");
        ProcessTasks.StartProcess("make", "PLATFORM=PLATFORM_DRM RAYLIB_LIBTYPE=SHARED", RaylibSource / "src")
            .AssertZeroExitCode();
    });

    Target Publish => _ => _
        .DependsOn(BuildRaylib)
        .Executes(() =>
        {
            OutputDirectory.CreateOrCleanDirectory();

            var project = Solution.GetProject("Pipboy2K");

            foreach (var rid in TargetRuntimes)
            {
                Serilog.Log.Information($"Publishing '{project.Name}' for '{rid}'...");

                var runOutput = OutputDirectory / rid;

                DotNetPublish(s => s
                    .SetProject(project)
                    .SetConfiguration(Configuration.Release)
                    .SetRuntime(rid)
                    .SetOutput(runOutput));

                if (rid == "linux-arm64")
                {
                    Serilog.Log.Information($"Copying custom Raylib library to {runOutput}");
                    RaylibNativeArtifact.Copy(
                        runOutput / "libraylib.so",
                        ExistsPolicy.FileOverwrite);
                }
            }
        });

    Target Compile => _ => _
        .DependsOn(BuildRaylib)
        .Executes(() =>
        {
            Project project = Solution.GetProject("Pipboy2K");

            DotNetBuild(s => s
                .SetProjectFile(project)
                .SetConfiguration(Configuration));
        });

    string GetHostRuntimeIdentifier()
    {
        // A. Determine Architecture (x64, arm64)
        var arch = RuntimeInformation.ProcessArchitecture switch
        {
            Architecture.X64 => "x64",
            Architecture.Arm64 => "arm64",
            _ => throw new System.PlatformNotSupportedException("Architecture not supported")
        };

        // B. Determine OS (win, linux, osx)
        if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            return $"win-{arch}";

        if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
            return $"linux-{arch}";

        if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
            return $"osx-{arch}";

        throw new System.PlatformNotSupportedException("Operating system not supported");
    }

    // Target Clean => _ => _
    //     .Before(Restore)
    //     .Executes(() =>
    //     {

    //     });

    // Target Restore => _ => _
    //     .Executes(() =>
    //     {
    //     });

    // Target Run => _ => _
    //     .Executes(() =>
    //     {
    //         DotNetRun();
    //     });



}
