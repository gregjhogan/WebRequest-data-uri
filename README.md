# WebRequest-data-uri
[WebClient](https://msdn.microsoft.com/en-us/library/system.net.webclient.aspx) and [HttpClient](https://msdn.microsoft.com/en-us/library/system.net.http.httpclient.aspx) do not natively support requests to data URIs.  Attempting to make a request to a data URI throws the exception `The URI prefix is not recognized.`  This library adds support.

## How To Use
Copy the source code into your project and register the data scheme by calling
```cs
DataWebRequestFactory.Register();
```

Then web requests to data URIs like the below examples succeed
```
data:text/plain,HelloWorld!
```

```
data:application/json;charset=utf-8;base64,DQp7DQogICAgImdyZWV0aW5nIjogIkhlbGxvIFdvcmxkISINCn0NCg==
```

There are some examples [here](DataUri.Tests/WebClientTests.cs)

There is also a [PowerShell 5 implementation](DataUriSupport.ps1)

## Why?
Sometimes people write libraries that require retrieving information from a URL. This allows integration without the requirement of a web server hosting the data.  My initial goal was to be able to dynamically build and pass an Azure ARM template to [New-AzureRmResourceGroupDeployment](https://msdn.microsoft.com/en-us/library/mt679003.aspx) using `-TemplateUri`, but unfortunately this didn't work because the data URI is passed up to Azure as a [linked template](https://azure.microsoft.com/en-us/documentation/articles/resource-group-linked-templates/) :(
