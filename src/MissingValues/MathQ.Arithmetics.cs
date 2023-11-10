using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Quic;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace MissingValues
{
		/*
		 * Parts of the code for basic math operators were based of Berkeley SoftFloat Release 3e
		 * 
		 * The following applies to most of the code below:

			Copyright 2011, 2012, 2013, 2014, 2015, 2016, 2017, 2018 The Regents of the
			University of California.  All rights reserved.
			
			Redistribution and use in source and binary forms, with or without
			modification, are permitted provided that the following conditions are met:
			
			 1. Redistributions of source code must retain the above copyright notice,
			    this list of conditions, and the following disclaimer.
			
			 2. Redistributions in binary form must reproduce the above copyright
			    notice, this list of conditions, and the following disclaimer in the
			    documentation and/or other materials provided with the distribution.
			
			 3. Neither the name of the University nor the names of its contributors
			    may be used to endorse or promote products derived from this software
			    without specific prior written permission.
			
			THIS SOFTWARE IS PROVIDED BY THE REGENTS AND CONTRIBUTORS "AS IS", AND ANY
			EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE IMPLIED
			WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE, ARE
			DISCLAIMED.  IN NO EVENT SHALL THE REGENTS OR CONTRIBUTORS BE LIABLE FOR ANY
			DIRECT, INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY, OR CONSEQUENTIAL DAMAGES
			(INCLUDING, BUT NOT LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR SERVICES;
			LOSS OF USE, DATA, OR PROFITS; OR BUSINESS INTERRUPTION) HOWEVER CAUSED AND
			ON ANY THEORY OF LIABILITY, WHETHER IN CONTRACT, STRICT LIABILITY, OR TORT
			(INCLUDING NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE OF
			THIS SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.

		 */

	internal static partial class MathQ
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]	
		internal static Quad Add(Quad left, Quad right)
		{
			// Source: berkeley-softfloat-3/source/f128_add.c
			bool signA;
			bool signB;

			signA = Quad.IsNegative(left);

			signB = Quad.IsNegative(right);

			if (signA == signB)
			{
				return Quad.UInt128BitsToQuad(BitHelper.AddQuadBits(
					 Quad.QuadToUInt128Bits(left), Quad.QuadToUInt128Bits(right), signA));
			}
			else
			{
				return Quad.UInt128BitsToQuad(BitHelper.SubQuadBits(
					Quad.QuadToUInt128Bits(left), Quad.QuadToUInt128Bits(right), signA));
			}
		}
		[MethodImpl(MethodImplOptions.AggressiveInlining)]	
		internal static Quad Sub(Quad left, Quad right)
		{
			// Source: berkeley-softfloat-3/source/f128_sub.c
			bool signA;
			bool signB;

			signA = Quad.IsNegative(left);

			signB = Quad.IsNegative(right);

			if (signA == signB)
			{
				return Quad.UInt128BitsToQuad(BitHelper.SubQuadBits(
					Quad.QuadToUInt128Bits(left), Quad.QuadToUInt128Bits(right), signA));
			}
			else
			{
				return Quad.UInt128BitsToQuad(BitHelper.AddQuadBits(
					Quad.QuadToUInt128Bits(left), Quad.QuadToUInt128Bits(right), signA));
			}
		}
		[MethodImpl(MethodImplOptions.AggressiveInlining)]	
		internal static Quad Mul(Quad left, Quad right)
		{
			// Source: berkeley-softfloat-3/source/f128_mul.c
			bool signA, signB, signZ;
			ushort expA, expB, expZ;
			UInt128 sigA, sigB, sigZ;
			UInt256 sig256;

			signA = Quad.IsNegative(left);
			expA = left.BiasedExponent;
			sigA = left.Significand;

			signB = Quad.IsNegative(right);
			expB = right.BiasedExponent;
			sigB = right.Significand;
			signZ = signA ^ signB;

			if (expA == 0x7FFF)
			{
				if ((sigA != UInt128.Zero) || ((expB == 0x7FFF) && (sigB != UInt128.Zero)))
				{
					return Quad.NaN;
				}

				bool magBits = (sigB | expB) != UInt128.Zero;
				if (!magBits)
				{
					return Quad.NaN;
				}

				return signZ ? Quad.NegativeInfinity : Quad.PositiveInfinity;
			}
			if (expB == 0x7FFF)
			{
				if (sigB != UInt128.Zero)
				{
					return Quad.NaN;
				}
				bool magBits = (sigA | expA) != UInt128.Zero;
				if (!magBits)
				{
					return Quad.NaN;
				}

				return signZ ? Quad.NegativeInfinity : Quad.PositiveInfinity;
			}

			if (expA == 0)
			{
				if (sigA == UInt128.Zero)
				{
					return signZ ? Quad.NegativeZero : Quad.Zero;
				}
				var normExpSig = BitHelper.NormalizeSubnormalF128Sig(sigA);
				expA = normExpSig.Exp;
				sigA = normExpSig.Sig;
			}
			if (expB == 0)
			{
				if (sigB == UInt128.Zero)
				{
					return signZ ? Quad.NegativeZero : Quad.Zero;
				}
				var normExpSig = BitHelper.NormalizeSubnormalF128Sig(sigB);
				expB = normExpSig.Exp;
				sigB = normExpSig.Sig;
			}

			expZ = (ushort)(expA + expB - 0x4000);
			sigA |= new UInt128(0x0001_0000_0000_0000, 0);
			sigB <<= 16;
			UInt128 sig256Hi = BitHelper.BigMul(sigA, sigB, out UInt128 sig256Lo);
			sig256 = new UInt256(sig256Hi, sig256Lo);
			ulong sigZExtra = Convert.ToUInt64(sig256 != UInt256.Zero);
			sigZ = sig256.Upper + sigA;
			if (0x0002_0000_0000_0000 <= (ulong)(sigZ >> 64))
			{
				++expZ;
				sigZ = BitHelper.ShortShiftRightJamExtra(sigZ, sigZExtra, 1, out sigZExtra);
			}

			return Quad.UInt128BitsToQuad(BitHelper.RoundPackToQuad(signZ, expZ, sigZ, sigZExtra));
		}
		[MethodImpl(MethodImplOptions.AggressiveInlining)]	
		internal static Quad Div(Quad left, Quad right)
		{
			// Source: berkeley-softfloat-3/source/f128_div.c
			bool signA = Quad.IsNegative(left);
			var expA = left.BiasedExponent;
			var sigA = left.Significand;
			bool signB = Quad.IsNegative(right);
			var expB = right.BiasedExponent;
			var sigB = right.Significand;
			bool signZ = signA ^ signB;

			if (expA == 0x7FFF)
			{
				if (sigA != UInt128.Zero)
				{
					return Quad.NaN;
				}
				if (expB == 0x7FFF)
				{
					return Quad.NaN;
				}
				return signZ ? Quad.NegativeInfinity : Quad.PositiveInfinity;
			}
			if (expB == 0x7FFF)
			{
				if (sigB != UInt128.Zero)
				{
					return Quad.NaN;
				}

				return signZ ? Quad.NegativeZero : Quad.Zero;
			}

			if (expB == 0)
			{
				if (sigB == UInt128.Zero)
				{
					if ((expA | sigA) == UInt128.Zero)
					{
						return Quad.NaN;
					}
					return signZ ? Quad.NegativeInfinity : Quad.PositiveInfinity;
				}
				(expB, sigB) = BitHelper.NormalizeSubnormalF128Sig(sigB);
			}
			if (expA == 0)
			{
				if (sigA == UInt128.Zero)
				{
					return signZ ? Quad.NegativeZero : Quad.Zero;
				}

				(expA, sigA) = BitHelper.NormalizeSubnormalF128Sig(sigA);
			}

			short expZ = ((short)(expA - expB + (Quad.ExponentBias - 1)));
			sigA |= new UInt128(0x0001_0000_0000_0000, 0);
			sigB |= new UInt128(0x0001_0000_0000_0000, 0);
			UInt128 rem = sigA;
			if (sigA < sigB)
			{
				--expZ;
				rem = sigA + sigA;
			}

			uint recip32 = BitHelper.ReciprocalApproximate((uint)(sigB >> 81));
			
			uint q;
			Span<uint> qs = stackalloc uint[3];
			UInt128 term;
			for(int ix = 3;;)
			{
				ulong q64 = (ulong)(uint)(rem >> 83) * recip32;
				q = (uint)((q64 + 0x8000_0000) >> 32);
				if (--ix < 0)
				{
					break;
				}
				rem <<= 29;
				term = sigB * q;
				rem -= term;
				if (((ulong)(rem >> 64) & 0x8000_0000_0000_0000) != 0)
				{
					--q;
					rem += sigB;
				}
				qs[ix] = q;
			}

			if (((q + 1) & 7) < 2)
			{
				rem <<= 29;
				term = sigB * q;
				rem -= term;
				if (((ulong)(rem >> 64) & 0x8000_0000_0000_0000) != 0)
				{
					--q;
					rem += sigB;
				}
				else if (sigB <= rem)
				{
					++q;
					rem -= sigB;
				}
				if (rem != UInt128.Zero)
				{
					q |= 1;
				}
			}

			ulong sigZExtra = (q << 60);
			term = new UInt128(0, qs[1]) << 54;
			UInt128 sigZ = new UInt128((ulong)qs[2] << 19, ((ulong)qs[0] << 25) + (q >> 4)) + term;
			return Quad.UInt128BitsToQuad(BitHelper.RoundPackToQuad(signZ, expZ, sigZ, sigZExtra));
		}
	}
}
