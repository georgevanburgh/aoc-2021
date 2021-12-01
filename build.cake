Task("Run all")
    .DoesForEach(GetFiles("./src/**/day*.csproj").OrderBy(x => x.ToString()), file =>
    {
        var taskName = $"{file.GetFilenameWithoutExtension()}";

        Task(taskName)
            .Does(c =>
            {
                var settings = new DotNetRunSettings
                {
                    WorkingDirectory = file.GetDirectory(),
                    Configuration = "Release"
                };

                DotNetRun(file.ToString(), null, settings);
            });

        RunTarget(taskName);
    });

RunTarget("Run all");