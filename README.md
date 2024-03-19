# Unity-Recent-Scene-Toolbar
Add Recent Scene button to open the recent scenes easily. (Additive-load supported)

The maximum number is 10 by default. (By the const value in `RecentSceneList.cs`)

![](./img~/demo.gif)

# Installation
Unity Package Manager -> Add package from git URL
```
https://github.com/qwe321qwe321qwe321/Unity-Recent-Scene-Toolbar.git
```

**Since package isn't able to create scriptable object, the `RecentSceneList.asset` will be created in `Assets/Recent-Scene-Toolbar/Editor` by default. It can be anywhere you like in the project. And you should add it to `.gitignore` manually to prevent tracking**

# How is it done
* Hook up the `EditorSceneManager.activeSceneChangedInEditMode` to record the scenes to a scriptable object `RecentSceneList.asset`.

  * `RecentSceneList.asset` will be generated if it doesn't exist.

  * The `.gitignore` in `Editor` folder is used to prevent git tracking `RecentSceneList.asset` which always change when you switch scene.

## Third parties
* [marijnz/unity-toolbar-extender](https://github.com/marijnz/unity-toolbar-extender)
  * It is proven to work up to (at least) Unity 2021.2.
* [nukadelic/UnityEditorIcons](https://github.com/nukadelic/UnityEditorIcons)
