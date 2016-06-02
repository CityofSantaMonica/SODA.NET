#Deploying a new release

Deployment is hard. There are too many manual steps. Let's at least enumerate 
them so everyone is on the same page.

Assuming all changes for the new release are commited, tests have been run and
are passing, and you're happy with the state of things...

1. Create a branch named `release`
2. Update [`ReleaseNotes.md`](ReleaseNotes.md), following the existing format
3. Bump the version number in [`SolutionInfo.cs`](SolutionInfo.cs) and the `.nuspec` and `appveyor.yml` files (if required)
4. Push the branch to GitHub and create a pull request
5. Run `.\build CreatePackages` to create the NuGet packages (locally) for the new version
6. Test the NuGet packages! [How to install NuGet package locally](http://stackoverflow.com/questions/10240029/how-to-install-a-nuget-package-nupkg-file-locally)
7. If satisfied with the release, push the new packages up to NuGet
8. Accept the pull request
9. Create a tag `git tag v#.#.#`. For example, to create a tag for 1.0.0 `git tag v1.0.0`
10. [Create a new release](https://help.github.com/articles/creating-releases) using the tag
you just created and pasting in the release notes you just wrote up. Attach a copy of the latest `.nupkg` files generated above.
