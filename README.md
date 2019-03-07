# Usage
1. Point project reference to `Starcounter.BluestarFFI` to a version that contain [this commit](https://github.com/Starcounter/Starcounter.BluestarFFI/commit/00746026845de44907783554f9fd9d58caff3332) 
2. Make sure new CLI tooling is in the environment (i.e. produced by [this PR](https://github.com/Starcounter/level0/pull/1441)).
3. Run the project like `dotnet run <instance> <store location path> <current directory>`, all being optional. Running just `dotnet run` will resolve default instance in default location.
