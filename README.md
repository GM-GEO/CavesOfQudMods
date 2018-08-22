# Caves Of Qud Mods
A sample of mods that can be used with Caves of Qud

## Important Information for Modding
The **modding directory** is located in ```C:\Users\{User}\AppData\LocalLow\Freehold Games\CavesOfQud```

The **game asset directory** is located in ```..\Caves Of Qud_Windows_142.1\Windows\CoQ_Data\StreamingAssets\Base```

The **compiled DLL directory** is located in ``` ..\Caves Of Qud_Windows_142.1\Windows\CoQ_Data\Managed```

### Third Eye
The Third Eye mod is the first mod I've created for Caves of Qud. It does a standard implementation of an increased stat via a mutation.

1. Create ThirdEye folder in the modding directory
2. Add a Mutations.xml file, including the definition for the Third Eye

   The class attribute of the mutation tag will need to be named the same as the CS file to ensure proper linkage.
   
3. Create PREFIX_ThirdEye.cs in the ThirdEye folder
4. Do basic implementation of desired results (+2 Ego)
5. Start Caves of Qud, go into the settings, and change the UI to use the new Overlay UI
6. In the bottom right corner of the screen, enable the ThirdEye mod
