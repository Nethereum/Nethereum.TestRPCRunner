﻿using System;
using System.Threading.Tasks;
using Nethereum.Hex.HexTypes;
using TechTalk.SpecFlow;
using Nethereum.TestRPCRunner;
using Nethereum.Web3;
using Xunit;

namespace SpecsSample
{
    [Binding]
    public class ContractMultiplicationSteps
    {
        //NOTE: We are using [assembly: CollectionBehavior(DisableTestParallelization = true)]
        // so we can avoid clashing on different instances of test rpc
        // and run the tests sequentially
        // another option would be to execute testrpc and assign port numbers using a safe thread Interlocked.Increment.
        [BeforeFeature("needsTestRPC")]
        public static void StartTestRPC()
        {
            var testrpcRunner = new TestRPCEmbeddedRunner();
            testrpcRunner.RedirectOuputToDebugWindow = true;
            testrpcRunner.StartTestRPC();
            FeatureContext.Current.Add("testRpc", testrpcRunner);
        }

        [AfterFeature("needsTestRPC")]
        public static void StopTestRPC()
        {
            var testRpc = FeatureContext.Current["testRpc"] as TestRPCEmbeddedRunner;
            if (testRpc != null)
            {
                testRpc.StopTestRPC();
                testRpc.Dispose();
            }
        }

        [Given(@"I have deployed a multiplication contract with multipler of (.*)")]
        public async Task GivenIHaveDeployedAMultiplicationContractWithMultiplerOf(int multiplier)
        {
            var contractByteCode =
                       "0x6060604052604060405190810160405280600a81526020017f4d756c7469706c6965720000000000000000000000000000000000000000000081526020015060016000509080519060200190828054600181600116156101000203166002900490600052602060002090601f016020900481019282601f1061008c57805160ff19168380011785556100bd565b828001600101855582156100bd579182015b828111156100bc57825182600050559160200191906001019061009e565b5b5090506100e891906100ca565b808211156100e457600081815060009055506001016100ca565b5090565b5050604051602080610303833981016040528080519060200190919050505b806000600050819055505b506101e2806101216000396000f360606040526000357c01000000000000000000000000000000000000000000000000000000009004806340490a901461004f57806375d0c0dc14610072578063c6888fa1146100ed5761004d565b005b61005c6004805050610119565b6040518082815260200191505060405180910390f35b61007f6004805050610141565b60405180806020018281038252838181518152602001915080519060200190808383829060006004602084601f0104600f02600301f150905090810190601f1680156100df5780820380516001836020036101000a031916815260200191505b509250505060405180910390f35b610103600480803590602001909190505061012b565b6040518082815260200191505060405180910390f35b60006000600050549050610128565b90565b60006000600050548202905061013c565b919050565b60016000508054600181600116156101000203166002900480601f0160208091040260200160405190810160405280929190818152602001828054600181600116156101000203166002900480156101da5780601f106101af576101008083540402835291602001916101da565b820191906000526020600020905b8154815290600101906020018083116101bd57829003601f168201915b50505050508156";

            var abi =
                @"[{""constant"":true,""inputs"":[],""name"":""getMultiplier"",""outputs"":[{""name"":""d"",""type"":""uint256""}],""type"":""function""},{""constant"":true,""inputs"":[],""name"":""contractName"",""outputs"":[{""name"":"""",""type"":""string""}],""type"":""function""},{""constant"":false,""inputs"":[{""name"":""a"",""type"":""uint256""}],""name"":""multiply"",""outputs"":[{""name"":""d"",""type"":""uint256""}],""type"":""function""},{""inputs"":[{""name"":""multiplier"",""type"":""uint256""}],""type"":""constructor""}]";

            var web3 = new Web3();
            var addressFrom =  (await web3.Eth.Accounts.SendRequestAsync())[0];
  
            var transactionHash = await
                web3.Eth.DeployContract.SendRequestAsync(abi, contractByteCode, addressFrom,
                    new HexBigInteger(900000), multiplier);

            var receipt = await web3.Eth.Transactions.GetTransactionReceipt.SendRequestAsync(transactionHash);

            ScenarioContext.Current.Add("multiplicationContract",  web3.Eth.GetContract(abi, receipt.ContractAddress));
        }
        
        [When(@"I call multiply using (.*)")]
        public async Task WhenICallMultiplyUsing(int multiplier)
        {
            var contract = ScenarioContext.Current["multiplicationContract"] as Nethereum.Contracts.Contract;
            var multiplyFunction = contract.GetFunction("multiply");
            var callResult = await multiplyFunction.CallAsync<int>(multiplier);
            ScenarioContext.Current.Add("multiplicationResult", callResult);
        }
        
        [Then(@"the multiplication result should be (.*)")]
        public void ThenTheResultShouldBe(int multiplicationResult)
        {
          Assert.Equal(multiplicationResult, ScenarioContext.Current["multiplicationResult"]);
        }
    }
}
