# CSGOAutoPlaces
Tool for map creators to automatically augment placenames into CS:GO nav-file.

## How it works
The tool searches all solids with appropriately named visgroups and does an AABB-collision check with navmesh areas; painting them with the Place name they collide with. Placenames for all areas that do not hit with any solid with a place name; will be unset.

## Configuring Hammer
1. Open "Run Map..." dialog.
   1. Make sure to enable expert mode
1. Add new configuration
   1. Click Edit
   1. Click New
   1. Enter name for the new configuration. For example `[AutoPlaces]`
 1. Set up newly created configuration
    1. Click New
    1. Click Cmds
       1. Choose Executable and navigate to the executable of CSGOAutoPlaces
    1. Set parameters to: `-vmf=$path/$file.vmf -nav=$gamedir/maps/$file.nav -strategy=aabb`
1. Done

## How to use
Before using the tool, there needs to be the nav file generated. Compile the map and run the game to generate the navmap.  To regenerate navmap; execute `nav_generate` in the developer console.

Placenames are assigned through VisGroups with naming convention of `ap_<Place>`. For example: `ap_CTSpawn`.  You can find the full list of compatible placenames by typing `nav_use_place` into the developer console. Or you can look them up from [Here](https://developer.valvesoftware.com/wiki/Standard_Place_Names_for_CS:GO)  
  
Create placenames by assigning appropriate VisGroup for brushes on the floor on. 
**For maximal compatibility, assign only one placename per brush.**  
