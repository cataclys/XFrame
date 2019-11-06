
var HandleIO = {
    WindowAlert : function(message)
    {
        window.alert(Pointer_stringify(message));
    },
    SyncFiles : function()
    {
		console.log("On function...");
        FS.syncfs(false,function (err) {
            // handle callback
			console.log("Callback handled.");
        });
    }
};

mergeInto(LibraryManager.library, HandleIO);