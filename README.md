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
We couldn't get the .apk to work/install outside of testing/developing environment. We could run the app using Android Emulator (Pixel 2 with 4GB of RAM).

## Building the Project
The .sln file is located inside ```TrabalhoLab.WPF```.
You will need to build TrabalhoLab.Domain and after that you need to add a new dependency in all the modules ```TrabalhoLab.WPF TrabalhoLab.Xamarin TrabalhoLab.Xamarin.Android```. Browse to the ```TrabalhoLab.Domain``` folder where the .dll were created and select it.
Now you can build solution normally and using  ```dotnet publish "Lista de Compras - Projeto LabSW.sln"-r win-x64 -p:PublishSingleFile=True -p:VersionPrefix=1.0.3 --self-contained false``` to create a single .exe file that is standalone.
### Warning
To build the android version of the app it is recommended to clone into ```C:/``` otherwise if there are non-ASCII chars or 255+ chars in your path it will error out
