rm *.nupkg
nuget pack .\FlockBuddy.nuspec -IncludeReferencedProjects -Prop Configuration=Release
cp *.nupkg C:\Projects\Nugets\