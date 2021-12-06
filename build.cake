Task("Build all")
    .Does(() =>
    { 
        var settings = new DotNetBuildSettings
        {
            Configuration = "Release",
        };

        DotNetBuild(".", settings);
    });

Task("Run all")
    .DoesForEach(GetFiles("./src/**/day*.csproj").OrderBy(x => x.ToString()), file =>
    {
        var taskName = $"{file.GetFilenameWithoutExtension()}";

        Task(taskName)
            .Does(c =>
            {
                c.Environment.WorkingDirectory = file.GetDirectory();
                DotNetExecute($"bin/Release/net6.0/{taskName}.dll", null);
            });

        RunTarget(taskName);
    });

RunTarget("Build all");
RunTarget("Run all");
