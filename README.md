# NotJustBubbleSort

## NOTE: This is a continuous development from repo https://github.com/CS5520FeinbergSpring2024/team-project-a6-group9 due to LFS data quota issues.

## Install Git LFS first!!!
To install, use homebrew `brew install git-lfs`

For the first time, init lfs globally, `git lfs install`

To check currently tracked files: `git lfs track`

To add new file type to lfs track: `git lfs track *.file_extension_you_want_to_track`

Tracked file types are stored in **.gitattributes**

Documentation: https://docs.github.com/en/repositories/working-with-files/managing-large-files/installing-git-large-file-storage

## Unity Editor Version: **2022.3.20f1**
- Type `git clone https://github.com/CS5520FeinbergSpring2024/team-project-a6-group9.git`
- Click **Add -> Add project from disk** in Unity, the necessary libraries will be downloaded automatically

## Project UI
Buttons - Asset LazyDay


## For each scene/level - adjust the following
For easy replacement from previous UI theme: make a copy of **BubbleSort1.unity** or **BubbleSort2.Unity (contains timer control)** and follow the steps:

- Replace the correct validator to NodeManager
- `(int, int) GetNextSwap(GameObject[] nodes)` returns a pair of index for the next valid swap, used for hints and auto-complete. Return (-1, -1) indicates no swap is available. Implement `GetNextSwap()` in your `XXXSortValidator` which implements `ISwapValidator`

- Adjust the following scene indices:
    - Pause - Window - Content - Restart Button: adjust to current scene index
    - Game Over - Window - Content - Restart Button: adjust to current scene index
    - Game Over Time (if exist): same as above
    - You Win - Window - Content - Next Button: adjust to next scene index

- To find scene indices, go to **File - Build Settings**, you will see the index for each scene. For example, **MainMenu** has index 0, **BubbleSort1** has index 2.

## WebGL-Specific Build Instructions
Unity WebGL builds does not inherently support WebGL, so external JSON wrappers will be needed. `FirebaseWebGL` is installed in `/Assets` for such purposes. *.jslib is the javascript method calls that handle JSON data, ensure you expose methods to javascript using the `[DllImport("__Internal")]` and `public static extern` signature for the functions that you want to expose to javascript.

To build:
 - write any firebase method calls as you would in `FirebaseDataManager.cs`
 - Build to WebGL
 - Add the following section in the build folder **index.html**:
    ```
    <script src="https://www.gstatic.com/firebasejs/7.19.0/firebase-app.js"></script>
    <script src="https://www.gstatic.com/firebasejs/7.19.0/firebase-database.js"></script>

    <script>
        const firebaseConfig = {
            apiKey: "XXXXX",
            authDomain: "XXXXX",
            databaseURL: "https://XXXX.firebaseio.com",
            projectId: "XXXX",
            storageBucket: "XXXX",
            messagingSenderId: "XXXX",
            appId: "XXXX",
            measurementId: "XXXX"
        };

        const firebaseApp = firebase.initializeApp(firebaseConfig);
        window.database = firebaseApp.database();
    </script>
    ```
- In the same **index.html**, find the code chunk and add the following line:

    ADD `window.unityInstance = unityInstance;` TO `then((unityInstance) => {`
    ```
    then((unityInstance) => {
    window.unityInstance = unityInstance;
    })
    ```

    The code chunk should look like:
    ```
    script.onload = () => {
    createUnityInstance(canvas, config, (progress) => {
        progressBarFull.style.width = 100 * progress + "%";
            }).then((unityInstance) => {
            window.unityInstance = unityInstance;
            loadingBar.style.display = "none";
            fullscreenButton.onclick = () => {
                unityInstance.SetFullscreen(1);
            };
            }).catch((message) => {
            alert(message);
            });
        };
    ```