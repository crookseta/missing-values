using System;
using System.Collections.Generic;
using System.Linq;
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

	public static partial class MathQ
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		internal static LongDouble Add(LongDouble left, LongDouble right)
		{
			return left + right;
		}
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		internal static LongDouble Sub(LongDouble left, LongDouble right)
		{
			return left + right;
		}
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		internal static LongDouble Mul(LongDouble left, LongDouble right)
		{
			return left + right;
		}
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		internal static LongDouble Div(LongDouble left, LongDouble right)
		{
			return left + right;
		}
	}
}
