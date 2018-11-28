### Deploying to Azure Web App from Local Git Repository

These instructions assume that you have a Web App instance on Azure. If you do not have a Web App, please follow the [Quick Start](https://docs.microsoft.com/en-us/azure/app-service/) for your desired language and select Local Git as the Deployment Option.

Perform the following steps to deploy your app to Azure:

1. Set the App for Local Deployment. You can check if it is already set in the Portal by browsing to `[Your Web App] > Properties > Git URL`. If the Git URL is present then your Web App is set up for local deployment. If the URL is not there, go to `[Your Web App] > Deployment Options` and select Disconnect to remove the current deployment. Then click on Setup and choose Local Git as your source. When you return to Properties the Git URL should be present.

1. Set the Publishing Profile. If you do not know the deployment username and password for the Web App, they can be set in the portal at `[Your Web App] > Deployment Credentials`. The username will automatically be reflected in the Git URL from Step 1.

1.  Deploy your local source code to your Azure Web App. First, set your Web App as a remote git repository with the URL generated in Step 1. The following commands must be run in the root of the local copy of this repository.

    ```bash
    git remote add azurepub https://<username>@<app_name>.scm.azurewebsites.net/<app_name>.git
    ```

    Then push your current branch to the master branch of the remote azure repository. Enter your deployment credential password from Step 2 if prompted. This step will take a few minutes to complete and should show the output of the server performing the build.

    ```bash
    git push azurepub <local_branch>:master
    ```

1.  Browse to the Swagger UI page for your new Web App to test the functionality.

    ```
    http://<app name>.azurewebsites.net/swagger
    ```
