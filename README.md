# GPS-AR-Switch 
This includes the Unity Setup for locationbased AR, with the ADDITIONAL feature of a switch between the larger scale navigation with GPS and the more detailed tracking via ARFoundation. This is made for theater pieces in public spaces.

# Before Downloading Package
Before you can successfully download the package you have to create a new empty Unity Project (with the Base 3D Template and built in Renderpipeline). I used Unity Editor Version 2021.3.ff LTS. Also make sure that you set your project up for ARFoundation. Meaning you have to import (via Unity Package Manager) the following External Packages:
  - ARFoundation 4.2.9
  - ARCore XR Plugin
  - ARFoundation Extensions v. 1.40.0 via Github: https://github.com/google-ar/arcore-unity-extensions

Then import this package.

Open the DemoScene to see how the elements correlate. 

1) You can and should customize the map, using Open Street Maps, for the area you want people to walk around in. On https://www.openstreetmap.org/ select an area and export it as an .osm file. In order to import it to your unity project convert it to an .obj file, following the instructions on: https://osm2world.org/

2) You need to manually set the GPS coodinates of your chosen Point of Reference in the PhoneManager.cs

3) It's crucial that you align the map in the editor, so the Point of Reference GameObject points to the above chosen Point of Reference in the real world AND in the unity
map. NOTE: The PointofReference Object needs to be at 0,0,0. To align move the map instead.

# Use case
New in this package is the inclusion of ARFoundation AR Tracking and the SWITCH between the GPS Camera and the ARCore Camera. You can navigate people using the GPS Camera in larger surroundings, like streets or a forest. Once they have arrived a Point Of Interest (POI) you can let them switch to the ARCore Camera, in order to have detailed and anchored view on the content you want them to see or hear or interact with. Once they leave the surroundings of the POI again, you switch back to the GPS navigation.

This package is a further development of "Locationbased AR", you cannot work with both.

This is still in development, thank you for being patient! Updates will follow soon.

# note
This package is the second of three packages. The third package is "Networked locationbased AR" and will be released in the beginning of 2024. 

This publication is made possible by the Recherchefoerderung of Fonds Darstellende Kuenste under the name of "Vernetzte Fiktionen" 
Gefoerdert vom Fonds Darstellende Kuenste aus Mitteln der Beauftragten der Bundesregierung fuer Kultur und Medien.

I want to thank Friedrich Kirschner, with whom I developed all of the code during the course of the last years. <3
