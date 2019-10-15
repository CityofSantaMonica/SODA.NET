# Deploying a new release

Deployment is hard. There are too many manual steps. Let's at least enumerate
them so everyone is on the same page.

Assuming all changes for the new release are commited, tests have been run and
are passing, and you're happy with the state of things...

1. Create a branch named `release`
1. Update [`ReleaseNotes.md`](ReleaseNotes.md), following the existing format
1. Bump the version number in [`Directory.build.props`](Directory.build.props) and [`appveyor.yml`](appveyor.yml)
1. Create the NuGet packages (in the local solution root) for the new version: `dotnet pack --output ..\`
1. Test the NuGet packages! [How to install NuGet package locally](http://stackoverflow.com/questions/10240029/how-to-install-a-nuget-package-nupkg-file-locally)
1. Push the `release` branch to GitHub and create a pull request
1. If the build succeeds, accept the pull request
1. Create and push a tag from the new HEAD `git tag v#.#.#` and `git push --tags`
1. [Create a new release](https://help.github.com/articles/creating-releases) using the tag
you just created and pasting in the release notes you just wrote up. Attach a copy of the latest `.nupkg` files generated above.
1. Push the new packages up to NuGet `dotnet nuget push`
