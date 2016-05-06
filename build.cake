//Addins
#addin Cake.VersionReader

//Arguments
var tools = "./tools";
var sln = "MAL.Net.sln";
var releaseFolder = "./MAL.NetSelfHosted/bin/Release";
var releaseBinary = "/MAL.NetSelfHosted.exe";

var target = Argument("target", "Build");
var buildType = Argument<string>("buildType", "develop");
var buildCounter = Argument<int>("buildCounter", 0);

var version = "0.0.0";
var ciVersion = "0.0.0-CI-00000";
var runningOnTeamCity = false;
var testSucceeded = true;

//Find out if we are running on a Build Server
Task("DiscoverBuildDetails")
	.Does(() =>
	{
		runningOnTeamCity = TeamCity.IsRunningOnTeamCity;
		Information("Running on TeamCity: " + runningOnTeamCity);
	});
	
Task ("Build")
.IsDependentOn("DiscoverBuildDetails")
	.Does (() => {
		NuGetRestore (sln);
		StartProcess("msbuild.exe", new ProcessSettings{ Arguments = sln + " /t:Build /p:Configuration=Release"});
		//DotNetBuild (sln, c => c.Configuration = "Release");
		var file = MakeAbsolute(Directory(releaseFolder)) + releaseBinary;
		version = GetVersionNumber(file);
		ciVersion = GetVersionNumberWithContinuesIntegrationNumberAppended(file, buildCounter);
		Information("Version: " + version);
		Information("CI Version: " + ciVersion);
		PushVersionToTeamcity(ciVersion);
	});
	
//Execute Unit tests
Task("UnitTest")
	.IsDependentOn("Build")
	.Does(() =>
	{
		StartBlock("Unit Testing");
		
		using(var process = StartAndReturnProcess(tools + "/NUnit.ConsoleRunner/tools/nunit3-console.exe", 
		new ProcessSettings { Arguments = 
		"\"./MAL.NetTests/bin/Release/MAL.NetTests.dll\" --teamcity --workers=1"}))
			{
				process.WaitForExit();
				Information("Exit Code {0}", process.GetExitCode());
				testSucceeded = false;
			};
		
		EndBlock("Unit Testing");
	});
	

Task("Default")
	.IsDependentOn("UnitTest");
	
RunTarget (target);

public void StartBlock(string blockName)
{
		if(runningOnTeamCity)
		{
			TeamCity.WriteStartBlock(blockName);
		}
}

public void StartBuildBlock(string blockName)
{
	if(runningOnTeamCity)
	{
		TeamCity.WriteStartBuildBlock(blockName);
	}
}

public void EndBlock(string blockName)
{
	if(runningOnTeamCity)
	{
		TeamCity.WriteEndBlock(blockName);
	}
}

public void EndBuildBlock(string blockName)
{
	if(runningOnTeamCity)
	{
		TeamCity.WriteEndBuildBlock(blockName);
	}
}

public void PushVersionToTeamcity(string version)
{
	if(runningOnTeamCity)
	{
		TeamCity.SetBuildNumber(version);
	}
}