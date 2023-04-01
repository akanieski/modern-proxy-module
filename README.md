# Modern Proxy Module
A .NET proxy module to plug into .NET Framework applications for support of common `HTTP_PROXY` and `NO_PROXY` environment variables. 

## Use Case
Some enterprises require that applications route their http traffic through a forward proxy (aka web proxy). Some enterprises also 
require support for basic authentication for those proxy environments. .NET Framework does support authenticated proxies, but it does 
not allow for explicit configuration of credentials in an `app.config`. Why might you need to configure in an `app.config` instead of 
just updating the code? Sometimes vendors will provide a .NET Framework application and not provide out of the box support for 
authenticated proxies. 

## Setup
First you will need to build the library. You can do so in Visual Studio 2019+. 

Next you can grab the `ModernAuthModule.dll` from your `bin\debug` folder and copy it to your applications installation directory, 
alongside the application `.exe`.

Next you will need to update the application executable's `.config` file. For example, if your application is `SomeApp.exe` then you 
would update your `SomeApp.exe.config` file (creating one if it doesn't exist) as follows:

```
<?xml version="1.0" encoding="utf-8" ?>
<configuration>
    <startup> 
        <supportedRuntime version="v4.0" sku=".NETFramework,Version=v4.8" />
    </startup>
	<system.net>
		<defaultProxy>
			<module type="DotNetAuthModule.CustomProxy, DotNetAuthModule" />
		</defaultProxy>
	</system.net>
</configuration>
```

Once this is updated, you can set one of three configurable environment variables:
- `HTTP_PROXY` - Configure the proxy server you would like to connect to. For example: `http://proxy.company.com`. You can also 
specify credentials inline with the url like so `http://user:pass@proxy.company.com`
- `NO_PROXY` - [Optional] Configure a comma seperated list of `host` or `host:port` suffixes that you do not want to have bypassed
- `HTTP_PROXY_LOG_PATH` - [Optional] Configure a directory where you would like to log traffic that has been proxied.
