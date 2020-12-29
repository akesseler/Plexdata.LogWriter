
## Overview

The help file project named ``Plexdata.LogWriter.help.shfbproj`` has been created using [Sandcastle Help File Builder](https://ewsoftware.github.io/SHFB/html/bd1ddb51-1c4f-434f-bb1a-ce2135d3a909.htm) version v2018.12.10.0.

## Building the Help

Usually, there should be no need to change this file because of the CHM help file is automatically created during release build of the project sources via ``MSBuild.exe``. But if you like, you may download _Sandcastle Help File Builder_ from [https://github.com/EWSoftware/SHFB/releases](https://github.com/EWSoftware/SHFB/releases) and modify the help project fitting your own needs.

For example you would like to create an HTML version of the project API documentation. In such a case just download and install the _Sandcastle Help File Builder_ as mentioned above. Thereafter, follow the steps below. See also section **Additional Conditions** for more requirements.

* As first, you should create a copy of the help file project ``Plexdata.LogWriter.help.shfbproj`` and rename it, for instance into ``Plexdata.LogWriter.help-HTML.shfbproj``. 
* As next, open this new file with the _Sandcastle Help File Builder_.
* Thereafter, open tab _Project Properties_ and move to section _Build_. 
* Now un-tick the _HTML help 1 (chm)_ setting and tick the setting _Website (HTML/ASP.NET)_ instead.
* Then you should change the output path accordingly. For this purpose move to section _Paths_ and modify the value of box _Help content output path_. For example you could change this path into ``html\``. 
* You may also need to adjust the path in _HTML Help 1 compiler path_, but only if you did not install the program into its default directory. 
* Finally, rebuild the whole help.

After a successful build you will find the result inside the project file’s sub-folder you named above.

### Additional Conditions

Ensure the utilities _HTML Help Workshop_ and _Microsoft Build Tools 2015_ are installed on your system. If not, you should download and install them. Below please find the download links of these programs.

* _HTML Help Workshop_: https://www.microsoft.com/en-us/download/details.aspx?id=21138
* _MS Build Tools 2015_: https://www.microsoft.com/en-us/download/details.aspx?id=48159
