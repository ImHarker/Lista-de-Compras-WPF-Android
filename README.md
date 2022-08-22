# Lista de Compras

This was a project made for an unit course where we had to create an Shopping List App using WPF and Xamarin for Windows and Android that keeps track of items to buy.
All the UI and categories names are in portuguese.
## Usage
### Desktop Standalone App for Win x64
Download the release [here](https://github.com/ImHarker/Lista-de-Compras-WPF-Android/releases/tag/v1.0.3) or you can [build it yourself](https://github.com/ImHarker/Lista-de-Compras-WPF-Android/edit/master/README.md#building-the-project).

[Dot Net Runtime for Desktop Apps](https://dotnet.microsoft.com/en-us/download/dotnet/thank-you/runtime-desktop-6.0.5-windows-x64-installer) needs to be installed to run the Win x64 version

The App will generate a file called ```'Listas.xml'``` at ```'C:\Users\<current_user>\AppData\Local'```.

The XML file contains all the data from the app.


### Android
You can archive and self sign the app to install it or you can run the app in debug mode or using Android Emulator (we used Pixel 2 with 4GB of RAM).
#### Warning
To build the android version of the app it is recommended to clone into ```C:/``` otherwise if there are non-ASCII chars or 255+ chars in your path it will error out

## Building the Project
The .sln file is located inside ```TrabalhoLab.WPF```.
You will need to build TrabalhoLab.Domain and after that you need to add a new dependency in all the modules ```TrabalhoLab.WPF TrabalhoLab.Xamarin TrabalhoLab.Xamarin.Android```. Browse into ```TrabalhoLab.Domain\bin\Debug\netstandard2.1``` folder where the ```TrabalhoLab.Domain.dll``` was created and select it.
Now you can build the solution normally and use  ```dotnet publish "Lista de Compras - Projeto LabSW.sln"-r win-x64 -p:PublishSingleFile=True -p:VersionPrefix=1.0.3 --self-contained false``` to create a single .exe file that is standalone.

