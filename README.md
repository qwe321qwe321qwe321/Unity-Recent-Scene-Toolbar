# Unity-RecentScene-Toolbar
Add Recent Scene button to quickly access recent scenes.
![](./img~/demo.gif)

# How is it done
* Hook up the `EditorSceneManager.activeSceneChangedInEditMode` to record the scenes to a scriptable object `RecentSceneList.asset`.

* `RecentSceneList.asset` will be generated if it doesn't exist.

* The `.gitignore` in `Editor` folder is used to prevent git tracking `RecentSceneList.asset` which always change when you switch scene.

* The toolbar UI solution is by https://github.com/marijnz/unity-toolbar-extender.
