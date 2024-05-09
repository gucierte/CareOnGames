mergeInto(LibraryManager.library, {

  CheckXRSupport: function () {
    requestXRStatus();
  },

  GetPlataformInfo: function () {
    requestXRStatus();
  },

  GetPlataformInfo: function () {
    let agent = navigator.userAgent;
    let device = "";

    unityInstance.SendMessage('XRManager', 'OnPlataformInfo', String(agent));
  },

});