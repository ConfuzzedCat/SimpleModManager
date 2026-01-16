# SimpleModManager

This program was meant to help me mod games ion linux, 
but should be cross-platform.

## WARNING!
### This is barely working. Has A ton of bugs. It won't remove files, (unless a mod overwrites a vanilla file).
### Although every thing should be fine to be used, you are very much on your own. No support nor accountability if stuff breaks.

## Usage:
compile it and run the cli(a tui) program from the ./bin/{Debug/Relalse} folder.
If you want it to call the nexus api, then after first run, a `.env` file will be created, 
where you can supply it your personal access token from [here](https://www.nexusmods.com/settings/api-keys).
### I only use the endpoint to get info for a mod. You don't need to give api key, as the program will try to figure out the version, id, etc. [see ApiClient.cs](./SimpleModManager/Api/ApiClient.cs)


For everyone that wants to help with this, don't.
Not to say no to help, but this needs to remade from scratch. 
I also don't really plan on making it work for everything. 
It is mainly made for me to mod some games on Linux.

### [Also see the library's readme](./SimpleModManager/README.md)