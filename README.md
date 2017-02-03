# Nethereum TestRPCRunner
[TestRpc](https://github.com/ethereumjs/testrpc) embedded in Nethereum to simplify smart contract and Ethereum integration testing. 

The Nethereum TestRpc runner, it embeds the portable (packed) version of Test Rpc and allows to launch / close on demand any Ethereum Test Rpc server managed by .Net.
It has been tested in Windows, Linux and Osx.

## Installation and different versions
Latest version of Node is recommended.

There are 2 versions / projects. A generic .net 4.5 and a .Net standard 1.3, both sharing the same code and functionality. 
The .Net 4.5 has been created for simpler integration with Visual Studio and the .Net standard for portability and .net core support. 

| Package       | Nuget         | 
| ------------- |:-------------:|
| Nethereum.TestRpcRunner    | [![NuGet version](https://badge.fury.io/nu/nethereum.testrpcrunner.svg)](https://badge.fury.io/nu/nethereum.testrpcrunner)| 
| Nethereum.TestRpcRunner.Net45| [![NuGet version](https://badge.fury.io/nu/nethereum.testrpcrunner.net45.svg)](https://badge.fury.io/nu/nethereum.testrpcrunner.net45)|

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
There is a sample included, which demonstrates how to integrate TestRpc with your tests

```csharp

  [Fact]
        public async void ShouldDeployAContractWithConstructor()
        {
            using (var testrpcRunner = new TestRPCEmbeddedRunner())
            {
                try
                {
                    testrpcRunner.RedirectOuputToDebugWindow = true;
                    testrpcRunner.StartTestRPC();
                    .........
                } finally {
                    testrpcRunner.StopTestRPC();  
                }

```

### BDD / ATD feature tests

There is a sample included which demonstrates how to create Gherkin style written features, enable the integration of executable specifications with smart contracts.

```Gherkin
@needsTestRPC
Feature: ContractMultiplication
	In order to avoid silly mistakes
	As a ethereum user
	I want to multiply a number

Scenario: Multiplication by 7
Given I have deployed a multiplication contract with multipler of 7
	When I call multiply using 7
	Then the multiplication result should be 49


Scenario Outline: Multiplication
	Given I have deployed a multiplication contract with multipler of <initialMultiplier>
	When I call multiply using <multiplier>
	Then the multiplication result should be <multiplicationResult>

	Examples:
	| initialMultiplier | multiplier | multiplicationResult |
	| 7                 | 7          | 49                   |
	| 7                 | 8          | 56                   |
	| 3                 | 3          | 9                    |

``` 