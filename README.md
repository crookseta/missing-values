﻿# MissingValues numeric library for .Net 7+

MissingValues is a C# numeric library for C# that introduces support for large integers and higher precision floating-point numbers. It supports .Net 7 Generic Math.

## Features

### 256-Bit and 512-Bit Integers

The library implements 256-bit and 512-bit integers, for both signed (`Int256`/`Int512`) and unsigned (`UInt256`/`UInt512`) integer arithmetic operations.

Here is a chart comparing the existing binary integers to the MissingValues integers:

| Name    	| Size     	| Max Value                                                                                                                                                                                                      	| Min Value                                                                                                                                                                                                      	|
|---------	|----------	|----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------	|----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------	|
| `sbyte` 	| 8 bits   	| 127                                                                                                                                                                                                            	| -128                                                                                                                                                                                                           	|
| `byte`  	| 8 bits   	| 255                                                                                                                                                                                                            	| 0                                                                                                                                                                                                              	|
| `short`  	| 16 bits  	| 32,767                                                                                                                                                                                                         	| -32,768                                                                                                                                                                                                        	|
| `ushort`	| 16 bits  	| 65,535                                                                                                                                                                                                         	| 0                                                                                                                                                                                                              	|
| `int`		| 32 bits  	| 2,147,483,647                                                                                                                                                                                                  	| -2,147,483,648                                                                                                                                                                                                 	|
| `uint`	| 32 bits  	| 4,294,967,295                                                                                                                                                                                                  	| 0                                                                                                                                                                                                              	|
| `long`	| 64 bits  	| 9,223,372,036,854,775,807                                                                                                                                                                                      	| -9,223,372,036,854,775,808                                                                                                                                                                                     	|
| `ulong`	| 64 bits  	| 18,446,744,073,709,551,615                                                                                                                                                                                     	| 0                                                                                                                                                                                                              	|
| `Int128`	| 128 bits 	| 170,141,183,460,469,231,731,687,303,715,884,105,727                                                                                                                                                            	| −170,141,183,460,469,231,731,687,303,715,884,105,728                                                                                                                                                           	|
| `UInt128`	| 128 bits 	| 340,282,366,920,938,463,463,374,607,431,768,211,455                                                                                                                                                            	| 0                                                                                                                                                                                                              	|
| `Int256`	| 256 bits 	| 57,896,044,618,658,097,711,785,492,504,343,953,926,634,992,332,820,282,019,728,792,003,956,564,819,967                                                                                                         	| -57,896,044,618,658,097,711,785,492,504,343,953,926,634,992,332,820,282,019,728,792,003,956,564,819,968                                                                                                        	|
| `UInt256`	| 256 bits 	| 115,792,089,237,316,195,423,570,985,008,687,907,853,269,984,665,640,564,039,457,584,007,913,129,639,935                                                                                                        	| 0                                                                                                                                                                                                              	|
| `Int512`	| 512 bits 	| 6,703,903,964,971,298,549,787,012,499,102,923,063,739,682,910,296,196,688,861,780,721,860,882,015,036,773,488,400,937,149,083,451,713,845,015,929,093,243,025,426,876,941,405,973,284,973,216,824,503,042,047  	| -6,703,903,964,971,298,549,787,012,499,102,923,063,739,682,910,296,196,688,861,780,721,860,882,015,036,773,488,400,937,149,083,451,713,845,015,929,093,243,025,426,876,941,405,973,284,973,216,824,503,042,048 	|
| `UInt512`	| 512 bits 	| 13,407,807,929,942,597,099,574,024,998,205,846,127,479,365,820,592,393,377,723,561,443,721,764,030,073,546,976,801,874,298,166,903,427,690,031,858,186,486,050,853,753,882,811,946,569,946,433,649,006,084,095 	| 0                                                                                                                                                                                                              	|

**Example Usage**

```csharp
using MissingValues; // namespace

// You can use the constructor:
Int256 num1 = new Int256(0x8000_0000_0000_0000, 0x0000_0000_0000_0000, 0x0000_0000_0000_0000, 0x0000_0000_0000_0001);
// You can use Parse():
Int256 num2 = Int256.Parse("10000000000000000000000000000000000000000000000000000000000000000000000000000");
// Or you can convert any other number type:
Int256 num3 = long.MaxValue;

// sum = -57896044618658097711785492504343953926634992332820282019728792003956564819967 + 10000000000000000000000000000000000000000000000000000000000000000000000000000 + 9223372036854775807
Int256 sum = num1 + num2 + num3;

Console.WriteLine($"Sum: {sum}");

// Prints: -47896044618658097711785492504343953926634992332820282019719568631919710044160
```
### Quadruple-Precision and Octuple-Precision Floating-Point Number

The library introduces the `Quad` and `Octo` struct, representing a quadruple-precision floating-point and a octuple-precision floating-point number respectively. 
Quadruple-precision offers higher precision than standard `double` or `float` types, making it suitable for applications requiring extensive precision in numerical calculations.

Here is a chart comparing the existing IEEE floating point numbers to `Quad`:

| Name   	| Size 		| Significand Digits 	| Decimal Digits 	| Max Exponent 	| Min Exponent 	| Max Value 	| Min Value  	|
|--------	|--------	|--------------------	|----------------	|--------------	|--------------	|-----------	|------------	|
| Half   	| 16 bits  	| 11                 	| 3.31           	| 15           	| -14          	| ~65504     	| ~-65500     	|
| Single 	| 32 bits  	| 24                 	| 7.22           	| 127          	| -126         	| ~3.40e38   	| ~-3.40e38   	|
| Double 	| 64 bits  	| 53                 	| 15.95          	| 1023         	| -1022        	| ~1.80e308  	| ~-1.79e308  	|
| Quad   	| 128 bits 	| 113                	| 34.02          	| 16383        	| -16382       	| ~1.19e4932 	| ~-1.18e4932 	|
| Octo   	| 256 bits 	| 237                	| 71.34          	| 262143        | −262142       | ~1.61e78913 	| ~-1.61e78913 	|

**Example Usage**
```csharp
using MissingValues; // namespace

// You can use the constructor:
Quad num1 = new Quad(sign: false, exp: 0x4004, sig: new UInt128(0x0000_9400_0000_0000, 0x0000_0000_0000_0000));
// You can use Parse():
Quad num2 = Quad.Parse("2e24");
// Or you can convert any other number type:
Quad num3 = 12.25d;

// sum = 50,5 + 2000000000000000000000000 + 12,25
Quad sum = num1 + num2 + num3;

Console.WriteLine($"Sum: {sum}");

// Prints 2000000000000000000000062,75
```