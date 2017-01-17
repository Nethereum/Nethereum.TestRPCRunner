@needsTestRPC
Feature: ContractMultiplication2
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