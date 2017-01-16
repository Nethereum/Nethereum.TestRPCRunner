# Nethereum TestRPCRunner
[TestRpc](https://github.com/ethereumjs/testrpc) embedded in Nethereum to simplify smart contract and Ethereum integration testing. 

The Nethereum TestRpc runner, it embeds the portable (packed) version of Test Rpc and allows to launch / close on demand any Ethereum Test Rpc server managed by .Net.
It has been tested in Windows, Linux and Osx.

## Installation and different versions
Latest version of Node is recommended.

There are 2 versions / projects. A generic .net 4.5 and a .Net standard 1.3, both sharing the same code and functionality. 
The .Net 4.5 has been created for simpler integration with Visual Studio and the .Net standard for portability and .net core support. 

## Usage

Just create a new instance of the launcher and use the Arguments property to specify any options for TestRpc.  For more info on the different type of options check the TestRpc [Readme](https://github.com/ethereumjs/testrpc) in Github.  

```csharp
  var launcher = new TestRPCEmbeddedRunner();
  launcher.RedirectOuputToDebugWindow = true;
  launcher.Arguments = "--port 8546";
  launcher.StartTestRPC();
```
Finally execute  ```StopTestRPC()``` to stop the server and remove any temporary files.

To ensure cleaning all resources call "Dispose()". 

### Unit testing

### BDD / ATD feature tests

