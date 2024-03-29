public record BuildPaths(
    BuildDirectories Directories,
    FilePath SignClientPath,
    FilePath SignFilterPath
)
{
    public static BuildPaths GetPaths(
        ICakeContext context,
        string configuration,
        string semVersion
        )
    {
        if (context == null)
        {
            throw new ArgumentNullException("context");
        }
        if (string.IsNullOrEmpty(configuration))
        {
            throw new ArgumentNullException("configuration");
        }
        if (string.IsNullOrEmpty(semVersion))
        {
            throw new ArgumentNullException("semVersion");
        }

        var artifactsDir = (DirectoryPath)(context.Directory("./artifacts") + context.Directory("v" + semVersion));
        var testResultsDir = artifactsDir.Combine("test-results");
        var nugetRoot = artifactsDir.Combine("nuget");

        var testCoverageOutputFilePath = testResultsDir.CombineWithFilePath("OpenCover.xml");

        var integrationTestsBin = context.MakeAbsolute(context.Directory("./tests/integration/tools"));
        var integrationTestsBinTool = integrationTestsBin.Combine("Cake.Tool");

        // Directories
        var buildDirectories = new BuildDirectories(
            artifactsDir,
            testResultsDir,
            nugetRoot,
            integrationTestsBinTool);

        var signClientPath = context.Tools.Resolve("sign.exe")
                                ?? context.Tools.Resolve("sign")
                                ?? (
                                    context.IsRunningOnWindows()
                                        ? throw new Exception("Failed to locate sign tool")
                                        : null
                                    );

        var signFilterPath = context.MakeAbsolute(context.File("./build/signclient.filter"));

        return new BuildPaths(
            Directories: buildDirectories,
            SignClientPath: signClientPath,
            SignFilterPath: signFilterPath
        );
    }
}

public record BuildDirectories(
        DirectoryPath Artifacts,
        DirectoryPath TestResults,
        DirectoryPath NuGetRoot,
        DirectoryPath IntegrationTestsBinTool
        )
{
    public ICollection<DirectoryPath> ToClean { get; } = new[] {
            Artifacts,
            TestResults,
            NuGetRoot,
            IntegrationTestsBinTool
        };
}