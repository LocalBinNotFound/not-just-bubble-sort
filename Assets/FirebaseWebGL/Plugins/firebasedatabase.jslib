mergeInto(LibraryManager.library, {

    GetJSON: function(path, callback, fallback) {
        var parsedPath = UTF8ToString(path);
        var parsedCallback = UTF8ToString(callback);
        var parsedFallback = UTF8ToString(fallback);

        try {

            window.database.ref(parsedPath).once('value').then(function(snapshot) {
                unityInstance.Module.SendMessage(parsedCallback, JSON.stringify(snapshot.val()));
            });

        } catch (error) {
            unityInstance.Module.SendMessage(parsedFallback, JSON.stringify(error, Object.getOwnPropertyNames(error)));
        }
    },

    PostJSON: function(path, value, callback, fallback) {
        var parsedPath = UTF8ToString(path);
        var parsedValue = UTF8ToString(value);
        var parsedCallback = UTF8ToString(callback);
        var parsedFallback = UTF8ToString(fallback);

        try {

            window.database.ref(parsedPath).set(JSON.parse(parsedValue)).then(function(unused) {
                unityInstance.Module.SendMessage(parsedCallback, "Success: " + parsedValue + " was posted to " + parsedPath);
            });

        } catch (error) {
            unityInstance.Module.SendMessage(parsedFallback, JSON.stringify(error, Object.getOwnPropertyNames(error)));
        }
    },

    PushJSON: function(path, value, callback, fallback) {
        var parsedPath = UTF8ToString(path);
        var parsedValue = UTF8ToString(value);
        var parsedCallback = UTF8ToString(callback);
        var parsedFallback = UTF8ToString(fallback);

        try {

            window.database.ref(parsedPath).push().set(JSON.parse(parsedValue)).then(function(unused) {
                unityInstance.Module.SendMessage(parsedCallback, "Success: " + parsedValue + " was pushed to " + parsedPath);
            });

        } catch (error) {
            unityInstance.Module.SendMessage(arsedFallback, JSON.stringify(error, Object.getOwnPropertyNames(error)));
        }
    },

    UpdateJSON: function(path, value, callback, fallback) {
        var parsedPath = UTF8ToString(path);
        var parsedValue = UTF8ToString(value);
        var parsedCallback = UTF8ToString(callback);
        var parsedFallback = UTF8ToString(fallback);

        try {

            window.database.ref(parsedPath).update(JSON.parse(parsedValue)).then(function(unused) {
                unityInstance.Module.SendMessage(parsedCallback, "Success: " + parsedValue + " was updated in " + parsedPath);
            });

        } catch (error) {
            unityInstance.Module.SendMessage(parsedFallback, JSON.stringify(error, Object.getOwnPropertyNames(error)));
        }
    },

    DeleteJSON: function(path, callback, fallback) {
        var parsedPath = UTF8ToString(path);
        var parsedCallback = UTF8ToString(callback);
        var parsedFallback = UTF8ToString(fallback);

        try {

            window.database.ref(parsedPath).remove().then(function(unused) {
                unityInstance.Module.SendMessage(parsedCallback, "Success: " + parsedPath + " was deleted");
            });

        } catch (error) {
            unityInstance.Module.SendMessage(parsedFallback, JSON.stringify(error, Object.getOwnPropertyNames(error)));
        }
    },

    RegisterOrLogin: function(username, totalLevels) {
        var parsedUsername = UTF8ToString(username);
        var userPath = 'users/' + parsedUsername;


        window.database.ref(userPath).once('value').then(function(snapshot) {
            if (snapshot.exists()) {
                unityInstance.Module.SendMessage("UserDataObject", 'UpdateUserData', JSON.stringify(snapshot.val()));
            } else {
                var newUser = {
                    username: parsedUsername,
                    items: { 
                        hints: 3, 
                        autoCompletes: 1, 
                        coins: 100 },
                    levelMenu: {}
                };
                for (var i = 1; i <= totalLevels; i++) {
                    newUser.levelMenu['Level_' + i] = { starsEarned: 0 };
                }
                window.database.ref(userPath).set(newUser).then(function() {
                    unityInstance.Module.SendMessage("UserDataObject", 'UpdateUserData', JSON.stringify(newUser));
                });
            }
        }).catch(function(error) {
            console.error('Error handling user data:', error);
        });
    },

});