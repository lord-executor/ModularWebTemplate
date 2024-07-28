using WebTemplate;

// After launching the application, you can access
// * https://localhost:7169/swagger/
// * https://localhost:7169/v1/status
var server = new AppServer(new GitVersionInfo());
server.Start(args);
