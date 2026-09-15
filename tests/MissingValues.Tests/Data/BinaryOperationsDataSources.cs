namespace MissingValues.Tests.Data;

public class BinaryOperationsDataSources
{
    public static IEnumerable<Func<(UInt256, byte[])>> UInt256GetBytesTest()
    {
        yield return () => (UInt256.Zero, new byte[32]);
        yield return () =>
        {
            var buffer = new byte[32];
			
            for (int i = 0; i < 32; i++)
                buffer[i] = 0xFF;
			
            return (new UInt256(ulong.MaxValue, ulong.MaxValue, ulong.MaxValue, ulong.MaxValue), buffer);
        };
    }
    public static IEnumerable<Func<(Int256, byte[])>> Int256GetBytesTest()
    {
        yield return () => (Int256.Zero, new byte[32]);
        yield return () =>
        {
            var buffer = new byte[32];
			
            for (int i = 0; i < 32; i++)
                buffer[i] = 0xFF;
			
            return (new Int256(ulong.MaxValue, ulong.MaxValue, ulong.MaxValue, ulong.MaxValue), buffer);
        };
    }
    public static IEnumerable<Func<(UInt512, byte[])>> UInt512GetBytesTest()
    {
        yield return () => (UInt512.Zero, new byte[64]);
        yield return () =>
        {
            var buffer = new byte[64];
			
            for (int i = 0; i < 64; i++)
                buffer[i] = 0xFF;
			
            return (new UInt512(ulong.MaxValue, ulong.MaxValue, ulong.MaxValue, ulong.MaxValue, ulong.MaxValue, ulong.MaxValue, ulong.MaxValue, ulong.MaxValue), buffer);
        };
    }
    public static IEnumerable<Func<(Int512, byte[])>> Int512GetBytesTest()
    {
        yield return () => (Int512.Zero, new byte[64]);
        yield return () =>
        {
            var buffer = new byte[64];
			
            for (int i = 0; i < 64; i++)
                buffer[i] = 0xFF;
			
            return (new Int512(ulong.MaxValue, ulong.MaxValue, ulong.MaxValue, ulong.MaxValue, ulong.MaxValue, ulong.MaxValue, ulong.MaxValue, ulong.MaxValue), buffer);
        };
    }
    public static IEnumerable<Func<(Quad, byte[])>> QuadGetBytesTest()
    {
        yield return () => (Quad.Zero, new byte[16]);
        yield return () =>
        {
            var buffer = new byte[16];
			
            for (int i = 0; i < 16; i++)
                buffer[i] = 0xFF;
			
            return (new Quad(ulong.MaxValue, ulong.MaxValue), buffer);
        };
    }
    public static IEnumerable<Func<(Octo, byte[])>> OctoGetBytesTest()
    {
        yield return () => (Octo.Zero, new byte[32]);
        yield return () =>
        {
            var buffer = new byte[32];
			
            for (int i = 0; i < 32; i++)
                buffer[i] = 0xFF;
			
            return (new Octo(ulong.MaxValue, ulong.MaxValue, ulong.MaxValue, ulong.MaxValue), buffer);
        };
    }
    
    public static IEnumerable<Func<(UInt256, byte[])>> UInt256WriteBigEndianTest()
    {
        yield return () => (new UInt256(0, 0, 0, 0), new byte[32]);
        yield return () =>
        {
            var buffer = new byte[32];
			
            for (int i = 0; i < 31; i++)
                buffer[i] = 0;

            buffer[31] = 1;
			
            return (new UInt256(0, 0, 0, 1), buffer);
        };
        yield return () =>
        {
            var buffer = new byte[32];
			
            for (int i = 0; i < 32; i++)
                buffer[i] = 0xFF;
			
            return (UInt256.MaxValue, buffer);
        };
    }
    public static IEnumerable<Func<(Int256, byte[])>> Int256WriteBigEndianTest()
    {
        yield return () => (new Int256(0, 0, 0, 0), new byte[32]);
        yield return () =>
        {
            var buffer = new byte[32];
			
            for (int i = 0; i < 31; i++)
                buffer[i] = 0;

            buffer[31] = 1;
			
            return (new Int256(0, 0, 0, 1), buffer);
        };
        yield return () =>
        {
            var buffer = new byte[32];
			
            for (int i = 0; i < 32; i++)
                buffer[i] = 0xFF;
			
            return (new Int256(0xFFFF_FFFF_FFFF_FFFF, 0xFFFF_FFFF_FFFF_FFFF, 0xFFFF_FFFF_FFFF_FFFF, 0xFFFF_FFFF_FFFF_FFFF), buffer);
        };
    }
    public static IEnumerable<Func<(UInt512, byte[])>> UInt512WriteBigEndianTest()
    {
        yield return () => (new UInt512(0, 0, 0, 0, 0, 0, 0, 0), new byte[64]);
        yield return () =>
        {
            var buffer = new byte[64];
			
            for (int i = 0; i < 63; i++)
                buffer[i] = 0;

            buffer[63] = 1;
			
            return (new UInt512(0, 0, 0, 0, 0, 0, 0, 1), buffer);
        };
        yield return () =>
        {
            var buffer = new byte[64];
			
            for (int i = 0; i < 64; i++)
                buffer[i] = 0xFF;
			
            return (UInt512.MaxValue, buffer);
        };
    }
    public static IEnumerable<Func<(Int512, byte[])>> Int512WriteBigEndianTest()
    {
        yield return () => (new Int512(0, 0, 0, 0, 0, 0, 0, 0), new byte[64]);
        yield return () =>
        {
            var buffer = new byte[64];
			
            for (int i = 0; i < 63; i++)
                buffer[i] = 0;

            buffer[63] = 1;
			
            return (new Int512(0, 0, 0, 0, 0, 0, 0, 1), buffer);
        };
        yield return () =>
        {
            var buffer = new byte[64];
			
            for (int i = 0; i < 64; i++)
                buffer[i] = 0xFF;
			
            return (new Int512(0xFFFF_FFFF_FFFF_FFFF, 0xFFFF_FFFF_FFFF_FFFF, 0xFFFF_FFFF_FFFF_FFFF, 0xFFFF_FFFF_FFFF_FFFF, 0xFFFF_FFFF_FFFF_FFFF, 0xFFFF_FFFF_FFFF_FFFF, 0xFFFF_FFFF_FFFF_FFFF, 0xFFFF_FFFF_FFFF_FFFF), buffer);
        };
    }
    public static IEnumerable<Func<(Quad, byte[])>> QuadWriteBigEndianTest()
    {
        yield return () => (Values.CreateFloat<Quad>(0, 0), new byte[16]);
        yield return () =>
        {
            var buffer = new byte[16];
			
            for (int i = 0; i < 15; i++)
                buffer[i] = 0;

            buffer[15] = 1;
			
            return (Values.CreateFloat<Quad>(0, 1), buffer);
        };
        yield return () =>
        {
            var buffer = new byte[16];
			
            for (int i = 0; i < 16; i++)
                buffer[i] = 0xFF;
			
            return (Values.CreateFloat<Quad>(0xFFFF_FFFF_FFFF_FFFF, 0xFFFF_FFFF_FFFF_FFFF), buffer);
        };
    }
    public static IEnumerable<Func<(Octo, byte[])>> OctoWriteBigEndianTest()
    {
        yield return () => (Values.CreateFloat<Octo>(0, 0, 0, 0), new byte[32]);
        yield return () =>
        {
            var buffer = new byte[32];
			
            for (int i = 0; i < 31; i++)
                buffer[i] = 0;

            buffer[31] = 1;
			
            return (Values.CreateFloat<Octo>(0, 0, 0, 1), buffer);
        };
        yield return () =>
        {
            var buffer = new byte[32];
			
            for (int i = 0; i < 32; i++)
                buffer[i] = 0xFF;
			
            return (Values.CreateFloat<Octo>(0xFFFF_FFFF_FFFF_FFFF, 0xFFFF_FFFF_FFFF_FFFF, 0xFFFF_FFFF_FFFF_FFFF, 0xFFFF_FFFF_FFFF_FFFF), buffer);
        };
    }
    
    public static IEnumerable<Func<(UInt256, byte[])>> UInt256WriteLittleEndianTest()
    {
        yield return () => (new UInt256(0, 0, 0, 0), new byte[32]);
        yield return () =>
        {
            var buffer = new byte[32];
			
            buffer[0] = 1;
            for (int i = 1; i < 32; i++)
                buffer[i] = 0;
			
            return (new UInt256(0, 0, 0, 1), buffer);
        };
        yield return () =>
        {
            var buffer = new byte[32];
			
            for (int i = 0; i < 32; i++)
                buffer[i] = 0xFF;
			
            return (UInt256.MaxValue, buffer);
        };
    }
    public static IEnumerable<Func<(Int256, byte[])>> Int256WriteLittleEndianTest()
    {
        yield return () => (new Int256(0, 0, 0, 0), new byte[32]);
        yield return () =>
        {
            var buffer = new byte[32];
			
            buffer[0] = 1;
            for (int i = 1; i < 32; i++)
                buffer[i] = 0;
			
            return (new Int256(0, 0, 0, 1), buffer);
        };
        yield return () =>
        {
            var buffer = new byte[32];
			
            for (int i = 0; i < 32; i++)
                buffer[i] = 0xFF;
			
            return (new Int256(0xFFFF_FFFF_FFFF_FFFF, 0xFFFF_FFFF_FFFF_FFFF, 0xFFFF_FFFF_FFFF_FFFF, 0xFFFF_FFFF_FFFF_FFFF), buffer);
        };
    }
    public static IEnumerable<Func<(UInt512, byte[])>> UInt512WriteLittleEndianTest()
    {
        yield return () => (new UInt512(0, 0, 0, 0, 0, 0, 0, 0), new byte[64]);
        yield return () =>
        {
            var buffer = new byte[64];
			
            buffer[0] = 1;
            for (int i = 1; i < 64; i++)
                buffer[i] = 0;
			
            return (new UInt512(0, 0, 0, 0, 0, 0, 0, 1), buffer);
        };
        yield return () =>
        {
            var buffer = new byte[64];
			
            for (int i = 0; i < 64; i++)
                buffer[i] = 0xFF;
			
            return (UInt512.MaxValue, buffer);
        };
    }
    public static IEnumerable<Func<(Int512, byte[])>> Int512WriteLittleEndianTest()
    {
        yield return () => (new Int512(0, 0, 0, 0, 0, 0, 0, 0), new byte[64]);
        yield return () =>
        {
            var buffer = new byte[64];
			
            buffer[0] = 1;
            for (int i = 1; i < 64; i++)
                buffer[i] = 0;
			
            return (new Int512(0, 0, 0, 0, 0, 0, 0, 1), buffer);
        };
        yield return () =>
        {
            var buffer = new byte[64];
			
            for (int i = 0; i < 64; i++)
                buffer[i] = 0xFF;
			
            return (new Int512(0xFFFF_FFFF_FFFF_FFFF, 0xFFFF_FFFF_FFFF_FFFF, 0xFFFF_FFFF_FFFF_FFFF, 0xFFFF_FFFF_FFFF_FFFF, 0xFFFF_FFFF_FFFF_FFFF, 0xFFFF_FFFF_FFFF_FFFF, 0xFFFF_FFFF_FFFF_FFFF, 0xFFFF_FFFF_FFFF_FFFF), buffer);
        };
    }
    public static IEnumerable<Func<(Quad, byte[])>> QuadWriteLittleEndianTest()
    {
        yield return () => (Values.CreateFloat<Quad>(0, 0), new byte[16]);
        yield return () =>
        {
            var buffer = new byte[16];
			
            buffer[0] = 1;
            for (int i = 1; i < 15; i++)
                buffer[i] = 0;
			
            return (Values.CreateFloat<Quad>(0, 1), buffer);
        };
        yield return () =>
        {
            var buffer = new byte[16];
			
            for (int i = 0; i < 16; i++)
                buffer[i] = 0xFF;
			
            return (Values.CreateFloat<Quad>(0xFFFF_FFFF_FFFF_FFFF, 0xFFFF_FFFF_FFFF_FFFF), buffer);
        };
    }
    public static IEnumerable<Func<(Octo, byte[])>> OctoWriteLittleEndianTest()
    {
        yield return () => (Values.CreateFloat<Octo>(0, 0, 0, 0), new byte[32]);
        yield return () =>
        {
            var buffer = new byte[32];
			
            buffer[0] = 1;
            for (int i = 1; i < 32; i++)
                buffer[i] = 0;
			
            return (Values.CreateFloat<Octo>(0, 0, 0, 1), buffer);
        };
        yield return () =>
        {
            var buffer = new byte[32];
			
            for (int i = 0; i < 32; i++)
                buffer[i] = 0xFF;
			
            return (Values.CreateFloat<Octo>(0xFFFF_FFFF_FFFF_FFFF, 0xFFFF_FFFF_FFFF_FFFF, 0xFFFF_FFFF_FFFF_FFFF, 0xFFFF_FFFF_FFFF_FFFF), buffer);
        };
    }

    public static IEnumerable<Func<(byte[], int, UInt256)>> ToUInt256Test()
    {
        yield return () =>
        {
            byte[] array = new byte[32];
            Array.Fill(array, byte.MaxValue);
            return (array, 0, new UInt256(ulong.MaxValue, ulong.MaxValue, ulong.MaxValue, ulong.MaxValue));
        };
        yield return () =>
        {
            byte[] array = new byte[33];
            Array.Fill(array, byte.MaxValue, 1, 32);
            return (array, 1, new UInt256(ulong.MaxValue, ulong.MaxValue, ulong.MaxValue, ulong.MaxValue));
        };
        yield return () =>
        {
            byte[] array = new byte[64];
            Array.Fill(array, byte.MaxValue, 32, 32);
            return (array, 0, UInt256.Zero);
        };
        yield return () =>
        {
            byte[] array = new byte[64];
            Array.Fill(array, byte.MaxValue, 32, 32);
            return (array, 32, new UInt256(ulong.MaxValue, ulong.MaxValue, ulong.MaxValue, ulong.MaxValue));
        };
    }
    public static IEnumerable<Func<(byte[], int, Int256)>> ToInt256Test()
    {
        yield return () =>
        {
            byte[] array = new byte[32];
            Array.Fill(array, byte.MaxValue);
            return (array, 0, new Int256(ulong.MaxValue, ulong.MaxValue, ulong.MaxValue, ulong.MaxValue));
        };
        yield return () =>
        {
            byte[] array = new byte[33];
            Array.Fill(array, byte.MaxValue, 1, 32);
            return (array, 1, new Int256(ulong.MaxValue, ulong.MaxValue, ulong.MaxValue, ulong.MaxValue));
        };
        yield return () =>
        {
            byte[] array = new byte[64];
            Array.Fill(array, byte.MaxValue, 32, 32);
            return (array, 0, Int256.Zero);
        };
        yield return () =>
        {
            byte[] array = new byte[64];
            Array.Fill(array, byte.MaxValue, 32, 32);
            return (array, 32, new Int256(ulong.MaxValue, ulong.MaxValue, ulong.MaxValue, ulong.MaxValue));
        };
    }
    public static IEnumerable<Func<(byte[], int, UInt512)>> ToUInt512Test()
    {
        yield return () =>
        {
            byte[] array = new byte[64];
            Array.Fill(array, byte.MaxValue);
            return (array, 0, new UInt512(ulong.MaxValue, ulong.MaxValue, ulong.MaxValue, ulong.MaxValue, ulong.MaxValue, ulong.MaxValue, ulong.MaxValue, ulong.MaxValue));
        };
        yield return () =>
        {
            byte[] array = new byte[65];
            Array.Fill(array, byte.MaxValue, 1, 64);
            return (array, 1, new UInt512(ulong.MaxValue, ulong.MaxValue, ulong.MaxValue, ulong.MaxValue, ulong.MaxValue, ulong.MaxValue, ulong.MaxValue, ulong.MaxValue));
        };
        yield return () =>
        {
            byte[] array = new byte[128];
            Array.Fill(array, byte.MaxValue, 64, 64);
            return (array, 0, UInt512.Zero);
        };
        yield return () =>
        {
            byte[] array = new byte[128];
            Array.Fill(array, byte.MaxValue, 64, 64);
            return (array, 64, new UInt512(ulong.MaxValue, ulong.MaxValue, ulong.MaxValue, ulong.MaxValue, ulong.MaxValue, ulong.MaxValue, ulong.MaxValue, ulong.MaxValue));
        };
    }
    public static IEnumerable<Func<(byte[], int, Int512)>> ToInt512Test()
    {
        yield return () =>
        {
            byte[] array = new byte[64];
            Array.Fill(array, byte.MaxValue);
            return (array, 0, new Int512(ulong.MaxValue, ulong.MaxValue, ulong.MaxValue, ulong.MaxValue, ulong.MaxValue, ulong.MaxValue, ulong.MaxValue, ulong.MaxValue));
        };
        yield return () =>
        {
            byte[] array = new byte[65];
            Array.Fill(array, byte.MaxValue, 1, 64);
            return (array, 1, new Int512(ulong.MaxValue, ulong.MaxValue, ulong.MaxValue, ulong.MaxValue, ulong.MaxValue, ulong.MaxValue, ulong.MaxValue, ulong.MaxValue));
        };
        yield return () =>
        {
            byte[] array = new byte[128];
            Array.Fill(array, byte.MaxValue, 64, 64);
            return (array, 0, Int512.Zero);
        };
        yield return () =>
        {
            byte[] array = new byte[128];
            Array.Fill(array, byte.MaxValue, 64, 64);
            return (array, 64, new Int512(ulong.MaxValue, ulong.MaxValue, ulong.MaxValue, ulong.MaxValue, ulong.MaxValue, ulong.MaxValue, ulong.MaxValue, ulong.MaxValue));
        };
    }
    public static IEnumerable<Func<(byte[], int, Quad)>> ToQuadTest()
    {
        yield return () =>
        {
            byte[] array = new byte[16];
            Array.Fill(array, byte.MaxValue);
            return (array, 0, new Quad(ulong.MaxValue, ulong.MaxValue));
        };
        yield return () =>
        {
            byte[] array = new byte[17];
            Array.Fill(array, byte.MaxValue, 1, 16);
            return (array, 1, new Quad(ulong.MaxValue, ulong.MaxValue));
        };
        yield return () =>
        {
            byte[] array = new byte[32];
            Array.Fill(array, byte.MaxValue, 16, 16);
            return (array, 0, Quad.Zero);
        };
        yield return () =>
        {
            byte[] array = new byte[32];
            Array.Fill(array, byte.MaxValue, 16, 16);
            return (array, 16, new Quad(ulong.MaxValue, ulong.MaxValue));
        };
    }
    public static IEnumerable<Func<(byte[], int, Octo)>> ToOctoTest()
    {
        yield return () =>
        {
            byte[] array = new byte[32];
            Array.Fill(array, byte.MaxValue);
            return (array, 0, new Octo(ulong.MaxValue, ulong.MaxValue, ulong.MaxValue, ulong.MaxValue));
        };
        yield return () =>
        {
            byte[] array = new byte[33];
            Array.Fill(array, byte.MaxValue, 1, 32);
            return (array, 1, new Octo(ulong.MaxValue, ulong.MaxValue, ulong.MaxValue, ulong.MaxValue));
        };
        yield return () =>
        {
            byte[] array = new byte[64];
            Array.Fill(array, byte.MaxValue, 32, 32);
            return (array, 0, Octo.Zero);
        };
        yield return () =>
        {
            byte[] array = new byte[64];
            Array.Fill(array, byte.MaxValue, 32, 32);
            return (array, 32, new Octo(ulong.MaxValue, ulong.MaxValue, ulong.MaxValue, ulong.MaxValue));
        };
    }

    public static IEnumerable<Func<(byte[], UInt256)>> UInt256ReadBigEndianTest()
    {
        yield return () =>
        {
            byte[] array = new byte[32];
            Array.Fill(array, byte.MaxValue);
            return (array, UInt256.MaxValue);
        };
        yield return () =>
        {
            byte[] array = new byte[35];
            for (int i = 0; i < 35; i++)
                array[i] = byte.MaxValue;
            return (array, UInt256.MaxValue);
        };
        yield return () =>
        {
            byte[] array = new byte[32];
            array[0] = 0x80;
            return (array, new UInt256(1UL << 63, 0, 0, 0));
        };
    }
    public static IEnumerable<Func<(byte[], Int256)>> Int256ReadBigEndianTest()
    {
        yield return () =>
        {
            byte[] array = new byte[32];
            Array.Fill(array, byte.MaxValue);
            return (array, new Int256(0xFFFF_FFFF_FFFF_FFFF, 0xFFFF_FFFF_FFFF_FFFF, 0xFFFF_FFFF_FFFF_FFFF, 0xFFFF_FFFF_FFFF_FFFF));
        };
        yield return () =>
        {
            byte[] array = new byte[35];
            for (int i = 0; i < 35; i++)
                array[i] = byte.MaxValue;
            return (array, new Int256(0xFFFF_FFFF_FFFF_FFFF, 0xFFFF_FFFF_FFFF_FFFF, 0xFFFF_FFFF_FFFF_FFFF, 0xFFFF_FFFF_FFFF_FFFF));
        };
        yield return () =>
        {
            byte[] array = new byte[32];
            array[0] = 0x80;
            return (array, new Int256(1UL << 63, 0, 0, 0));
        };
    }
    public static IEnumerable<Func<(byte[], UInt512)>> UInt512ReadBigEndianTest()
    {
        yield return () =>
        {
            byte[] array = new byte[64];
            Array.Fill(array, byte.MaxValue);
            return (array, UInt512.MaxValue);
        };
        yield return () =>
        {
            byte[] array = new byte[67];
            for (int i = 0; i < 67; i++)
                array[i] = byte.MaxValue;
            return (array, UInt512.MaxValue);
        };
        yield return () =>
        {
            byte[] array = new byte[64];
            array[0] = 0x80;
            return (array, new UInt512(1UL << 63, 0, 0, 0, 0, 0, 0, 0));
        };
    }
    public static IEnumerable<Func<(byte[], Int512)>> Int512ReadBigEndianTest()
    {
        yield return () =>
        {
            byte[] array = new byte[64];
            Array.Fill(array, byte.MaxValue);
            return (array, new Int512(0xFFFF_FFFF_FFFF_FFFF, 0xFFFF_FFFF_FFFF_FFFF, 0xFFFF_FFFF_FFFF_FFFF, 0xFFFF_FFFF_FFFF_FFFF, 0xFFFF_FFFF_FFFF_FFFF, 0xFFFF_FFFF_FFFF_FFFF, 0xFFFF_FFFF_FFFF_FFFF, 0xFFFF_FFFF_FFFF_FFFF));
        };
        yield return () =>
        {
            byte[] array = new byte[67];
            for (int i = 0; i < 67; i++)
                array[i] = byte.MaxValue;
            return (array, new Int512(0xFFFF_FFFF_FFFF_FFFF, 0xFFFF_FFFF_FFFF_FFFF, 0xFFFF_FFFF_FFFF_FFFF, 0xFFFF_FFFF_FFFF_FFFF, 0xFFFF_FFFF_FFFF_FFFF, 0xFFFF_FFFF_FFFF_FFFF, 0xFFFF_FFFF_FFFF_FFFF, 0xFFFF_FFFF_FFFF_FFFF));
        };
        yield return () =>
        {
            byte[] array = new byte[64];
            array[0] = 0x80;
            return (array, new Int512(1UL << 63, 0, 0, 0, 0, 0, 0, 0));
        };
    }
    public static IEnumerable<Func<(byte[], Quad)>> QuadReadBigEndianTest()
    {
        yield return () =>
        {
            byte[] array = new byte[16];
            Array.Fill(array, byte.MaxValue);
            return (array, Values.CreateFloat<Quad>(0xFFFF_FFFF_FFFF_FFFF, 0xFFFF_FFFF_FFFF_FFFF));
        };
        yield return () =>
        {
            byte[] array = new byte[17];
            for (int i = 0; i < 17; i++)
                array[i] = byte.MaxValue;
            return (array, Values.CreateFloat<Quad>(0xFFFF_FFFF_FFFF_FFFF, 0xFFFF_FFFF_FFFF_FFFF));
        };
        yield return () =>
        {
            byte[] array = new byte[16];
            array[0] = 0x80;
            return (array, Values.CreateFloat<Quad>(1UL << 63, 0));
        };
    }
    public static IEnumerable<Func<(byte[], Octo)>> OctoReadBigEndianTest()
    {
        yield return () =>
        {
            byte[] array = new byte[32];
            Array.Fill(array, byte.MaxValue);
            return (array, Values.CreateFloat<Octo>(0xFFFF_FFFF_FFFF_FFFF, 0xFFFF_FFFF_FFFF_FFFF, 0xFFFF_FFFF_FFFF_FFFF, 0xFFFF_FFFF_FFFF_FFFF));
        };
        yield return () =>
        {
            byte[] array = new byte[35];
            for (int i = 0; i < 35; i++)
                array[i] = byte.MaxValue;
            return (array, Values.CreateFloat<Octo>(0xFFFF_FFFF_FFFF_FFFF, 0xFFFF_FFFF_FFFF_FFFF, 0xFFFF_FFFF_FFFF_FFFF, 0xFFFF_FFFF_FFFF_FFFF));
        };
        yield return () =>
        {
            byte[] array = new byte[32];
            array[0] = 0x80;
            return (array, Values.CreateFloat<Octo>(1UL << 63, 0, 0, 0));
        };
    }

    public static IEnumerable<Func<(byte[], UInt256)>> UInt256ReadLittleEndianTest()
    {
        yield return () =>
        {
            byte[] array = new byte[32];
            Array.Fill(array, byte.MaxValue);
            return (array, UInt256.MaxValue);
        };
        yield return () =>
        {
            byte[] array = new byte[35];
            for (int i = 0; i < 32; i++)
                array[i] = byte.MaxValue;
            return (array, UInt256.MaxValue);
        };
        yield return () =>
        {
            byte[] array = new byte[32];
            array[31] = 0x80;
            return (array, new UInt256(1UL << 63, 0, 0, 0));
        };
    }
    public static IEnumerable<Func<(byte[], Int256)>> Int256ReadLittleEndianTest()
    {
        yield return () =>
        {
            byte[] array = new byte[32];
            Array.Fill(array, byte.MaxValue);
            return (array, new Int256(0xFFFF_FFFF_FFFF_FFFF, 0xFFFF_FFFF_FFFF_FFFF, 0xFFFF_FFFF_FFFF_FFFF, 0xFFFF_FFFF_FFFF_FFFF));
        };
        yield return () =>
        {
            byte[] array = new byte[35];
            for (int i = 0; i < 35; i++)
                array[i] = byte.MaxValue;
            return (array, new Int256(0xFFFF_FFFF_FFFF_FFFF, 0xFFFF_FFFF_FFFF_FFFF, 0xFFFF_FFFF_FFFF_FFFF, 0xFFFF_FFFF_FFFF_FFFF));
        };
        yield return () =>
        {
            byte[] array = new byte[32];
            array[31] = 0x80;
            return (array, new Int256(1UL << 63, 0, 0, 0));
        };
    }
    public static IEnumerable<Func<(byte[], UInt512)>> UInt512ReadLittleEndianTest()
    {
        yield return () =>
        {
            byte[] array = new byte[64];
            Array.Fill(array, byte.MaxValue);
            return (array, UInt512.MaxValue);
        };
        yield return () =>
        {
            byte[] array = new byte[67];
            for (int i = 0; i < 64; i++)
                array[i] = byte.MaxValue;
            return (array, UInt512.MaxValue);
        };
        yield return () =>
        {
            byte[] array = new byte[64];
            array[63] = 0x80;
            return (array, new UInt512(1UL << 63, 0, 0, 0, 0, 0, 0, 0));
        };
    }
    public static IEnumerable<Func<(byte[], Int512)>> Int512ReadLittleEndianTest()
    {
        yield return () =>
        {
            byte[] array = new byte[64];
            Array.Fill(array, byte.MaxValue);
            return (array, new Int512(0xFFFF_FFFF_FFFF_FFFF, 0xFFFF_FFFF_FFFF_FFFF, 0xFFFF_FFFF_FFFF_FFFF, 0xFFFF_FFFF_FFFF_FFFF, 0xFFFF_FFFF_FFFF_FFFF, 0xFFFF_FFFF_FFFF_FFFF, 0xFFFF_FFFF_FFFF_FFFF, 0xFFFF_FFFF_FFFF_FFFF));
        };
        yield return () =>
        {
            byte[] array = new byte[67];
            for (int i = 0; i < 67; i++)
                array[i] = byte.MaxValue;
            return (array, new Int512(0xFFFF_FFFF_FFFF_FFFF, 0xFFFF_FFFF_FFFF_FFFF, 0xFFFF_FFFF_FFFF_FFFF, 0xFFFF_FFFF_FFFF_FFFF, 0xFFFF_FFFF_FFFF_FFFF, 0xFFFF_FFFF_FFFF_FFFF, 0xFFFF_FFFF_FFFF_FFFF, 0xFFFF_FFFF_FFFF_FFFF));
        };
        yield return () =>
        {
            byte[] array = new byte[64];
            array[63] = 0x80;
            return (array, new Int512(1UL << 63, 0, 0, 0, 0, 0, 0, 0));
        };
    }
    public static IEnumerable<Func<(byte[], Quad)>> QuadReadLittleEndianTest()
    {
        yield return () =>
        {
            byte[] array = new byte[16];
            Array.Fill(array, byte.MaxValue);
            return (array, Values.CreateFloat<Quad>(0xFFFF_FFFF_FFFF_FFFF, 0xFFFF_FFFF_FFFF_FFFF));
        };
        yield return () =>
        {
            byte[] array = new byte[17];
            for (int i = 0; i < 16; i++)
                array[i] = byte.MaxValue;
            return (array, Values.CreateFloat<Quad>(0xFFFF_FFFF_FFFF_FFFF, 0xFFFF_FFFF_FFFF_FFFF));
        };
        yield return () =>
        {
            byte[] array = new byte[16];
            array[15] = 0x80;
            return (array, Values.CreateFloat<Quad>(1UL << 63, 0));
        };
    }
    public static IEnumerable<Func<(byte[], Octo)>> OctoReadLittleEndianTest()
    {
        yield return () =>
        {
            byte[] array = new byte[32];
            Array.Fill(array, byte.MaxValue);
            return (array, Values.CreateFloat<Octo>(0xFFFF_FFFF_FFFF_FFFF, 0xFFFF_FFFF_FFFF_FFFF, 0xFFFF_FFFF_FFFF_FFFF, 0xFFFF_FFFF_FFFF_FFFF));
        };
        yield return () =>
        {
            byte[] array = new byte[35];
            for (int i = 0; i < 32; i++)
                array[i] = byte.MaxValue;
            return (array, Values.CreateFloat<Octo>(0xFFFF_FFFF_FFFF_FFFF, 0xFFFF_FFFF_FFFF_FFFF, 0xFFFF_FFFF_FFFF_FFFF, 0xFFFF_FFFF_FFFF_FFFF));
        };
        yield return () =>
        {
            byte[] array = new byte[32];
            array[31] = 0x80;
            return (array, Values.CreateFloat<Octo>(1UL << 63, 0, 0, 0));
        };
    }
    
    public static IEnumerable<Func<(UInt256, byte[], bool)>> UInt256TryWriteBytesTest()
    {
        yield return () => (UInt256.Zero, new byte[32], true);
        yield return () => (UInt256.Zero, new byte[31], false);
        yield return () =>
        {
            var buffer = new byte[32];
			
            for (int i = 0; i < 32; i++)
                buffer[i] = 0xFF;
			
            return (new UInt256(ulong.MaxValue, ulong.MaxValue, ulong.MaxValue, ulong.MaxValue), buffer, true);
        };
    }
    public static IEnumerable<Func<(Int256, byte[], bool)>> Int256TryWriteBytesTest()
    {
        yield return () => (Int256.Zero, new byte[32], true);
        yield return () => (Int256.Zero, new byte[31], false);
        yield return () =>
        {
            var buffer = new byte[32];
			
            for (int i = 0; i < 32; i++)
                buffer[i] = 0xFF;
			
            return (new Int256(ulong.MaxValue, ulong.MaxValue, ulong.MaxValue, ulong.MaxValue), buffer, true);
        };
    }
    public static IEnumerable<Func<(UInt512, byte[], bool)>> UInt512TryWriteBytesTest()
    {
        yield return () => (UInt512.Zero, new byte[64], true);
        yield return () => (UInt512.Zero, new byte[63], false);
        yield return () =>
        {
            var buffer = new byte[64];
			
            for (int i = 0; i < 64; i++)
                buffer[i] = 0xFF;
			
            return (new UInt512(ulong.MaxValue, ulong.MaxValue, ulong.MaxValue, ulong.MaxValue, ulong.MaxValue, ulong.MaxValue, ulong.MaxValue, ulong.MaxValue), buffer, true);
        };
    }
    public static IEnumerable<Func<(Int512, byte[], bool)>> Int512TryWriteBytesTest()
    {
        yield return () => (Int512.Zero, new byte[64], true);
        yield return () => (Int512.Zero, new byte[63], false);
        yield return () =>
        {
            var buffer = new byte[64];
			
            for (int i = 0; i < 64; i++)
                buffer[i] = 0xFF;
			
            return (new Int512(ulong.MaxValue, ulong.MaxValue, ulong.MaxValue, ulong.MaxValue, ulong.MaxValue, ulong.MaxValue, ulong.MaxValue, ulong.MaxValue), buffer, true);
        };
    }
    public static IEnumerable<Func<(Quad, byte[], bool)>> QuadTryWriteBytesTest()
    {
        yield return () => (Quad.Zero, new byte[16], true);
        yield return () => (Quad.Zero, new byte[15], false);
        yield return () =>
        {
            var buffer = new byte[16];
			
            for (int i = 0; i < 16; i++)
                buffer[i] = 0xFF;
			
            return (new Quad(ulong.MaxValue, ulong.MaxValue), buffer, true);
        };
    }
    public static IEnumerable<Func<(Octo, byte[], bool)>> OctoTryWriteBytesTest()
    {
        yield return () => (Octo.Zero, new byte[32], true);
        yield return () => (Octo.Zero, new byte[31], false);
        yield return () =>
        {
            var buffer = new byte[32];
			
            for (int i = 0; i < 32; i++)
                buffer[i] = 0xFF;
			
            return (new Octo(ulong.MaxValue, ulong.MaxValue, ulong.MaxValue, ulong.MaxValue), buffer, true);
        };
    }
    
    public static IEnumerable<Func<(UInt256, byte[], bool)>> UInt256TryWriteBigEndianTest()
    {
        yield return () => (new UInt256(0, 0, 0, 0), new byte[32], true);
        yield return () => (new UInt256(0, 0, 0, 0), new byte[31], false);
        yield return () =>
        {
            var buffer = new byte[32];
			
            for (int i = 0; i < 31; i++)
                buffer[i] = 0;

            buffer[31] = 1;
			
            return (new UInt256(0, 0, 0, 1), buffer, true);
        };
        yield return () =>
        {
            var buffer = new byte[32];
			
            for (int i = 0; i < 32; i++)
                buffer[i] = 0xFF;
			
            return (UInt256.MaxValue, buffer, true);
        };
    }
    public static IEnumerable<Func<(Int256, byte[], bool)>> Int256TryWriteBigEndianTest()
    {
        yield return () => (new Int256(0, 0, 0, 0), new byte[32], true);
        yield return () => (new Int256(0, 0, 0, 0), new byte[31], false);
        yield return () =>
        {
            var buffer = new byte[32];
			
            for (int i = 0; i < 31; i++)
                buffer[i] = 0;

            buffer[31] = 1;
			
            return (new Int256(0, 0, 0, 1), buffer, true);
        };
        yield return () =>
        {
            var buffer = new byte[32];
			
            for (int i = 0; i < 32; i++)
                buffer[i] = 0xFF;
			
            return (new Int256(0xFFFF_FFFF_FFFF_FFFF, 0xFFFF_FFFF_FFFF_FFFF, 0xFFFF_FFFF_FFFF_FFFF, 0xFFFF_FFFF_FFFF_FFFF), buffer, true);
        };
    }
    public static IEnumerable<Func<(UInt512, byte[], bool)>> UInt512TryWriteBigEndianTest()
    {
        yield return () => (new UInt512(0, 0, 0, 0, 0, 0, 0, 0), new byte[64], true);
        yield return () => (new UInt512(0, 0, 0, 0, 0, 0, 0, 0), new byte[63], false);
        yield return () =>
        {
            var buffer = new byte[64];
			
            for (int i = 0; i < 63; i++)
                buffer[i] = 0;

            buffer[63] = 1;
			
            return (new UInt512(0, 0, 0, 0, 0, 0, 0, 1), buffer, true);
        };
        yield return () =>
        {
            var buffer = new byte[64];
			
            for (int i = 0; i < 64; i++)
                buffer[i] = 0xFF;
			
            return (UInt512.MaxValue, buffer, true);
        };
    }
    public static IEnumerable<Func<(Int512, byte[], bool)>> Int512TryWriteBigEndianTest()
    {
        yield return () => (new Int512(0, 0, 0, 0, 0, 0, 0, 0), new byte[64], true);
        yield return () => (new Int512(0, 0, 0, 0, 0, 0, 0, 0), new byte[63], false);
        yield return () =>
        {
            var buffer = new byte[64];
			
            for (int i = 0; i < 63; i++)
                buffer[i] = 0;

            buffer[63] = 1;
			
            return (new Int512(0, 0, 0, 0, 0, 0, 0, 1), buffer, true);
        };
        yield return () =>
        {
            var buffer = new byte[64];
			
            for (int i = 0; i < 64; i++)
                buffer[i] = 0xFF;
			
            return (new Int512(0xFFFF_FFFF_FFFF_FFFF, 0xFFFF_FFFF_FFFF_FFFF, 0xFFFF_FFFF_FFFF_FFFF, 0xFFFF_FFFF_FFFF_FFFF, 0xFFFF_FFFF_FFFF_FFFF, 0xFFFF_FFFF_FFFF_FFFF, 0xFFFF_FFFF_FFFF_FFFF, 0xFFFF_FFFF_FFFF_FFFF), buffer, true);
        };
    }
    public static IEnumerable<Func<(Quad, byte[], bool)>> QuadTryWriteBigEndianTest()
    {
        yield return () => (Values.CreateFloat<Quad>(0, 0), new byte[16], true);
        yield return () => (Values.CreateFloat<Quad>(0, 0), new byte[15], false);
        yield return () =>
        {
            var buffer = new byte[16];
			
            for (int i = 0; i < 15; i++)
                buffer[i] = 0;

            buffer[15] = 1;
			
            return (Values.CreateFloat<Quad>(0, 1), buffer, true);
        };
        yield return () =>
        {
            var buffer = new byte[16];
			
            for (int i = 0; i < 16; i++)
                buffer[i] = 0xFF;
			
            return (Values.CreateFloat<Quad>(0xFFFF_FFFF_FFFF_FFFF, 0xFFFF_FFFF_FFFF_FFFF), buffer, true);
        };
    }
    public static IEnumerable<Func<(Octo, byte[], bool)>> OctoTryWriteBigEndianTest()
    {
        yield return () => (Values.CreateFloat<Octo>(0, 0, 0, 0), new byte[32], true);
        yield return () => (Values.CreateFloat<Octo>(0, 0, 0, 0), new byte[31], false);
        yield return () =>
        {
            var buffer = new byte[32];
			
            for (int i = 0; i < 31; i++)
                buffer[i] = 0;

            buffer[31] = 1;
			
            return (Values.CreateFloat<Octo>(0, 0, 0, 1), buffer, true);
        };
        yield return () =>
        {
            var buffer = new byte[32];
			
            for (int i = 0; i < 32; i++)
                buffer[i] = 0xFF;
			
            return (Values.CreateFloat<Octo>(0xFFFF_FFFF_FFFF_FFFF, 0xFFFF_FFFF_FFFF_FFFF, 0xFFFF_FFFF_FFFF_FFFF, 0xFFFF_FFFF_FFFF_FFFF), buffer, true);
        };
    }
    
    public static IEnumerable<Func<(UInt256, byte[], bool)>> UInt256TryWriteLittleEndianTest()
    {
        yield return () => (new UInt256(0, 0, 0, 0), new byte[32], true);
        yield return () => (new UInt256(0, 0, 0, 0), new byte[31], false);
        yield return () =>
        {
            var buffer = new byte[32];
			
            buffer[0] = 1;
            for (int i = 1; i < 32; i++)
                buffer[i] = 0;
			
            return (new UInt256(0, 0, 0, 1), buffer, true);
        };
        yield return () =>
        {
            var buffer = new byte[32];
			
            for (int i = 0; i < 32; i++)
                buffer[i] = 0xFF;
			
            return (UInt256.MaxValue, buffer, true);
        };
    }
    public static IEnumerable<Func<(Int256, byte[], bool)>> Int256TryWriteLittleEndianTest()
    {
        yield return () => (new Int256(0, 0, 0, 0), new byte[32], true);
        yield return () => (new Int256(0, 0, 0, 0), new byte[31], false);
        yield return () =>
        {
            var buffer = new byte[32];
			
            buffer[0] = 1;
            for (int i = 1; i < 32; i++)
                buffer[i] = 0;
			
            return (new Int256(0, 0, 0, 1), buffer, true);
        };
        yield return () =>
        {
            var buffer = new byte[32];
			
            for (int i = 0; i < 32; i++)
                buffer[i] = 0xFF;
			
            return (new Int256(0xFFFF_FFFF_FFFF_FFFF, 0xFFFF_FFFF_FFFF_FFFF, 0xFFFF_FFFF_FFFF_FFFF, 0xFFFF_FFFF_FFFF_FFFF), buffer, true);
        };
    }
    public static IEnumerable<Func<(UInt512, byte[], bool)>> UInt512TryWriteLittleEndianTest()
    {
        yield return () => (new UInt512(0, 0, 0, 0, 0, 0, 0, 0), new byte[64], true);
        yield return () => (new UInt512(0, 0, 0, 0, 0, 0, 0, 0), new byte[63], false);
        yield return () =>
        {
            var buffer = new byte[64];
			
            buffer[0] = 1;
            for (int i = 1; i < 64; i++)
                buffer[i] = 0;
			
            return (new UInt512(0, 0, 0, 0, 0, 0, 0, 1), buffer, true);
        };
        yield return () =>
        {
            var buffer = new byte[64];
			
            for (int i = 0; i < 64; i++)
                buffer[i] = 0xFF;
			
            return (UInt512.MaxValue, buffer, true);
        };
    }
    public static IEnumerable<Func<(Int512, byte[], bool)>> Int512TryWriteLittleEndianTest()
    {
        yield return () => (new Int512(0, 0, 0, 0, 0, 0, 0, 0), new byte[64], true);
        yield return () => (new Int512(0, 0, 0, 0, 0, 0, 0, 0), new byte[63], false);
        yield return () =>
        {
            var buffer = new byte[64];
			
            buffer[0] = 1;
            for (int i = 1; i < 64; i++)
                buffer[i] = 0;
			
            return (new Int512(0, 0, 0, 0, 0, 0, 0, 1), buffer, true);
        };
        yield return () =>
        {
            var buffer = new byte[64];
			
            for (int i = 0; i < 64; i++)
                buffer[i] = 0xFF;
			
            return (new Int512(0xFFFF_FFFF_FFFF_FFFF, 0xFFFF_FFFF_FFFF_FFFF, 0xFFFF_FFFF_FFFF_FFFF, 0xFFFF_FFFF_FFFF_FFFF, 0xFFFF_FFFF_FFFF_FFFF, 0xFFFF_FFFF_FFFF_FFFF, 0xFFFF_FFFF_FFFF_FFFF, 0xFFFF_FFFF_FFFF_FFFF), buffer, true);
        };
    }
    public static IEnumerable<Func<(Quad, byte[], bool)>> QuadTryWriteLittleEndianTest()
    {
        yield return () => (Values.CreateFloat<Quad>(0, 0), new byte[16], true);
        yield return () => (Values.CreateFloat<Quad>(0, 0), new byte[15], false);
        yield return () =>
        {
            var buffer = new byte[16];
			
            buffer[0] = 1;
            for (int i = 1; i < 15; i++)
                buffer[i] = 0;
			
            return (Values.CreateFloat<Quad>(0, 1), buffer, true);
        };
        yield return () =>
        {
            var buffer = new byte[16];
			
            for (int i = 0; i < 16; i++)
                buffer[i] = 0xFF;
			
            return (Values.CreateFloat<Quad>(0xFFFF_FFFF_FFFF_FFFF, 0xFFFF_FFFF_FFFF_FFFF), buffer, true);
        };
    }
    public static IEnumerable<Func<(Octo, byte[], bool)>> OctoTryWriteLittleEndianTest()
    {
        yield return () => (Values.CreateFloat<Octo>(0, 0, 0, 0), new byte[32], true);
        yield return () => (Values.CreateFloat<Octo>(0, 0, 0, 0), new byte[31], false);
        yield return () =>
        {
            var buffer = new byte[32];
			
            buffer[0] = 1;
            for (int i = 1; i < 32; i++)
                buffer[i] = 0;
			
            return (Values.CreateFloat<Octo>(0, 0, 0, 1), buffer, true);
        };
        yield return () =>
        {
            var buffer = new byte[32];
			
            for (int i = 0; i < 32; i++)
                buffer[i] = 0xFF;
			
            return (Values.CreateFloat<Octo>(0xFFFF_FFFF_FFFF_FFFF, 0xFFFF_FFFF_FFFF_FFFF, 0xFFFF_FFFF_FFFF_FFFF, 0xFFFF_FFFF_FFFF_FFFF), buffer, true);
        };
    }
    
    public static IEnumerable<Func<(byte[], UInt256, bool)>> UInt256TryReadBigEndianTest()
    {
        yield return () =>
        {
            byte[] array = new byte[32];
            Array.Fill(array, byte.MaxValue);
            return (array, UInt256.MaxValue, true);
        };
        yield return () =>
        {
            byte[] array = new byte[31];
            Array.Fill(array, byte.MaxValue);
            return (array, default, false);
        };
        yield return () =>
        {
            byte[] array = new byte[35];
            for (int i = 0; i < 35; i++)
                array[i] = byte.MaxValue;
            return (array, UInt256.MaxValue, true);
        };
        yield return () =>
        {
            byte[] array = new byte[32];
            array[0] = 0x80;
            return (array, new UInt256(1UL << 63, 0, 0, 0), true);
        };
    }
    public static IEnumerable<Func<(byte[], Int256, bool)>> Int256TryReadBigEndianTest()
    {
        yield return () =>
        {
            byte[] array = new byte[32];
            Array.Fill(array, byte.MaxValue);
            return (array, new Int256(0xFFFF_FFFF_FFFF_FFFF, 0xFFFF_FFFF_FFFF_FFFF, 0xFFFF_FFFF_FFFF_FFFF, 0xFFFF_FFFF_FFFF_FFFF), true);
        };
        yield return () =>
        {
            byte[] array = new byte[31];
            Array.Fill(array, byte.MaxValue);
            return (array, default, false);
        };
        yield return () =>
        {
            byte[] array = new byte[35];
            for (int i = 0; i < 35; i++)
                array[i] = byte.MaxValue;
            return (array, new Int256(0xFFFF_FFFF_FFFF_FFFF, 0xFFFF_FFFF_FFFF_FFFF, 0xFFFF_FFFF_FFFF_FFFF, 0xFFFF_FFFF_FFFF_FFFF), true);
        };
        yield return () =>
        {
            byte[] array = new byte[32];
            array[0] = 0x80;
            return (array, new Int256(1UL << 63, 0, 0, 0), true);
        };
    }
    public static IEnumerable<Func<(byte[], UInt512, bool)>> UInt512TryReadBigEndianTest()
    {
        yield return () =>
        {
            byte[] array = new byte[64];
            Array.Fill(array, byte.MaxValue);
            return (array, UInt512.MaxValue, true);
        };
        yield return () =>
        {
            byte[] array = new byte[63];
            Array.Fill(array, byte.MaxValue);
            return (array, default, false);
        };
        yield return () =>
        {
            byte[] array = new byte[67];
            for (int i = 0; i < 67; i++)
                array[i] = byte.MaxValue;
            return (array, UInt512.MaxValue, true);
        };
        yield return () =>
        {
            byte[] array = new byte[64];
            array[0] = 0x80;
            return (array, new UInt512(1UL << 63, 0, 0, 0, 0, 0, 0, 0), true);
        };
    }
    public static IEnumerable<Func<(byte[], Int512, bool)>> Int512TryReadBigEndianTest()
    {
        yield return () =>
        {
            byte[] array = new byte[64];
            Array.Fill(array, byte.MaxValue);
            return (array, new Int512(0xFFFF_FFFF_FFFF_FFFF, 0xFFFF_FFFF_FFFF_FFFF, 0xFFFF_FFFF_FFFF_FFFF, 0xFFFF_FFFF_FFFF_FFFF, 0xFFFF_FFFF_FFFF_FFFF, 0xFFFF_FFFF_FFFF_FFFF, 0xFFFF_FFFF_FFFF_FFFF, 0xFFFF_FFFF_FFFF_FFFF), true);
        };
        yield return () =>
        {
            byte[] array = new byte[63];
            Array.Fill(array, byte.MaxValue);
            return (array, default, false);
        };
        yield return () =>
        {
            byte[] array = new byte[67];
            for (int i = 0; i < 67; i++)
                array[i] = byte.MaxValue;
            return (array, new Int512(0xFFFF_FFFF_FFFF_FFFF, 0xFFFF_FFFF_FFFF_FFFF, 0xFFFF_FFFF_FFFF_FFFF, 0xFFFF_FFFF_FFFF_FFFF, 0xFFFF_FFFF_FFFF_FFFF, 0xFFFF_FFFF_FFFF_FFFF, 0xFFFF_FFFF_FFFF_FFFF, 0xFFFF_FFFF_FFFF_FFFF), true);
        };
        yield return () =>
        {
            byte[] array = new byte[64];
            array[0] = 0x80;
            return (array, new Int512(1UL << 63, 0, 0, 0, 0, 0, 0, 0), true);
        };
    }
    public static IEnumerable<Func<(byte[], Quad, bool)>> QuadTryReadBigEndianTest()
    {
        yield return () =>
        {
            byte[] array = new byte[16];
            Array.Fill(array, byte.MaxValue);
            return (array, Values.CreateFloat<Quad>(0xFFFF_FFFF_FFFF_FFFF, 0xFFFF_FFFF_FFFF_FFFF), true);
        };
        yield return () =>
        {
            byte[] array = new byte[15];
            Array.Fill(array, byte.MaxValue);
            return (array, default, false);
        };
        yield return () =>
        {
            byte[] array = new byte[17];
            for (int i = 0; i < 17; i++)
                array[i] = byte.MaxValue;
            return (array, Values.CreateFloat<Quad>(0xFFFF_FFFF_FFFF_FFFF, 0xFFFF_FFFF_FFFF_FFFF), true);
        };
        yield return () =>
        {
            byte[] array = new byte[16];
            array[0] = 0x80;
            return (array, Values.CreateFloat<Quad>(1UL << 63, 0), true);
        };
    }
    public static IEnumerable<Func<(byte[], Octo, bool)>> OctoTryReadBigEndianTest()
    {
        yield return () =>
        {
            byte[] array = new byte[32];
            Array.Fill(array, byte.MaxValue);
            return (array, Values.CreateFloat<Octo>(0xFFFF_FFFF_FFFF_FFFF, 0xFFFF_FFFF_FFFF_FFFF, 0xFFFF_FFFF_FFFF_FFFF, 0xFFFF_FFFF_FFFF_FFFF), true);
        };
        yield return () =>
        {
            byte[] array = new byte[31];
            Array.Fill(array, byte.MaxValue);
            return (array, default, false);
        };
        yield return () =>
        {
            byte[] array = new byte[35];
            for (int i = 0; i < 35; i++)
                array[i] = byte.MaxValue;
            return (array, Values.CreateFloat<Octo>(0xFFFF_FFFF_FFFF_FFFF, 0xFFFF_FFFF_FFFF_FFFF, 0xFFFF_FFFF_FFFF_FFFF, 0xFFFF_FFFF_FFFF_FFFF), true);
        };
        yield return () =>
        {
            byte[] array = new byte[32];
            array[0] = 0x80;
            return (array, Values.CreateFloat<Octo>(1UL << 63, 0, 0, 0), true);
        };
    }
    
    public static IEnumerable<Func<(byte[], UInt256, bool)>> UInt256TryReadLittleEndianTest()
    {
        yield return () =>
        {
            byte[] array = new byte[32];
            Array.Fill(array, byte.MaxValue);
            return (array, UInt256.MaxValue, true);
        };
        yield return () =>
        {
            byte[] array = new byte[31];
            Array.Fill(array, byte.MaxValue);
            return (array, default, false);
        };
        yield return () =>
        {
            byte[] array = new byte[35];
            for (int i = 0; i < 32; i++)
                array[i] = byte.MaxValue;
            return (array, UInt256.MaxValue, true);
        };
        yield return () =>
        {
            byte[] array = new byte[32];
            array[31] = 0x80;
            return (array, new UInt256(1UL << 63, 0, 0, 0), true);
        };
    }
    public static IEnumerable<Func<(byte[], Int256, bool)>> Int256TryReadLittleEndianTest()
    {
        yield return () =>
        {
            byte[] array = new byte[32];
            Array.Fill(array, byte.MaxValue);
            return (array, new Int256(0xFFFF_FFFF_FFFF_FFFF, 0xFFFF_FFFF_FFFF_FFFF, 0xFFFF_FFFF_FFFF_FFFF, 0xFFFF_FFFF_FFFF_FFFF), true);
        };
        yield return () =>
        {
            byte[] array = new byte[31];
            Array.Fill(array, byte.MaxValue);
            return (array, default, false);
        };
        yield return () =>
        {
            byte[] array = new byte[35];
            for (int i = 0; i < 35; i++)
                array[i] = byte.MaxValue;
            return (array, new Int256(0xFFFF_FFFF_FFFF_FFFF, 0xFFFF_FFFF_FFFF_FFFF, 0xFFFF_FFFF_FFFF_FFFF, 0xFFFF_FFFF_FFFF_FFFF), true);
        };
        yield return () =>
        {
            byte[] array = new byte[32];
            array[31] = 0x80;
            return (array, new Int256(1UL << 63, 0, 0, 0), true);
        };
    }
    public static IEnumerable<Func<(byte[], UInt512, bool)>> UInt512TryReadLittleEndianTest()
    {
        yield return () =>
        {
            byte[] array = new byte[64];
            Array.Fill(array, byte.MaxValue);
            return (array, UInt512.MaxValue, true);
        };
        yield return () =>
        {
            byte[] array = new byte[63];
            Array.Fill(array, byte.MaxValue);
            return (array, default, false);
        };
        yield return () =>
        {
            byte[] array = new byte[67];
            for (int i = 0; i < 64; i++)
                array[i] = byte.MaxValue;
            return (array, UInt512.MaxValue, true);
        };
        yield return () =>
        {
            byte[] array = new byte[64];
            array[63] = 0x80;
            return (array, new UInt512(1UL << 63, 0, 0, 0, 0, 0, 0, 0), true);
        };
    }
    public static IEnumerable<Func<(byte[], Int512, bool)>> Int512TryReadLittleEndianTest()
    {
        yield return () =>
        {
            byte[] array = new byte[64];
            Array.Fill(array, byte.MaxValue);
            return (array, new Int512(0xFFFF_FFFF_FFFF_FFFF, 0xFFFF_FFFF_FFFF_FFFF, 0xFFFF_FFFF_FFFF_FFFF, 0xFFFF_FFFF_FFFF_FFFF, 0xFFFF_FFFF_FFFF_FFFF, 0xFFFF_FFFF_FFFF_FFFF, 0xFFFF_FFFF_FFFF_FFFF, 0xFFFF_FFFF_FFFF_FFFF), true);
        };
        yield return () =>
        {
            byte[] array = new byte[63];
            Array.Fill(array, byte.MaxValue);
            return (array, default, false);
        };
        yield return () =>
        {
            byte[] array = new byte[67];
            for (int i = 0; i < 67; i++)
                array[i] = byte.MaxValue;
            return (array, new Int512(0xFFFF_FFFF_FFFF_FFFF, 0xFFFF_FFFF_FFFF_FFFF, 0xFFFF_FFFF_FFFF_FFFF, 0xFFFF_FFFF_FFFF_FFFF, 0xFFFF_FFFF_FFFF_FFFF, 0xFFFF_FFFF_FFFF_FFFF, 0xFFFF_FFFF_FFFF_FFFF, 0xFFFF_FFFF_FFFF_FFFF), true);
        };
        yield return () =>
        {
            byte[] array = new byte[64];
            array[63] = 0x80;
            return (array, new Int512(1UL << 63, 0, 0, 0, 0, 0, 0, 0), true);
        };
    }
    public static IEnumerable<Func<(byte[], Quad, bool)>> QuadTryReadLittleEndianTest()
    {
        yield return () =>
        {
            byte[] array = new byte[16];
            Array.Fill(array, byte.MaxValue);
            return (array, Values.CreateFloat<Quad>(0xFFFF_FFFF_FFFF_FFFF, 0xFFFF_FFFF_FFFF_FFFF), true);
        };
        yield return () =>
        {
            byte[] array = new byte[15];
            Array.Fill(array, byte.MaxValue);
            return (array, default, false);
        };
        yield return () =>
        {
            byte[] array = new byte[17];
            for (int i = 0; i < 16; i++)
                array[i] = byte.MaxValue;
            return (array, Values.CreateFloat<Quad>(0xFFFF_FFFF_FFFF_FFFF, 0xFFFF_FFFF_FFFF_FFFF), true);
        };
        yield return () =>
        {
            byte[] array = new byte[16];
            array[15] = 0x80;
            return (array, Values.CreateFloat<Quad>(1UL << 63, 0), true);
        };
    }
    public static IEnumerable<Func<(byte[], Octo, bool)>> OctoTryReadLittleEndianTest()
    {
        yield return () =>
        {
            byte[] array = new byte[32];
            Array.Fill(array, byte.MaxValue);
            return (array, Values.CreateFloat<Octo>(0xFFFF_FFFF_FFFF_FFFF, 0xFFFF_FFFF_FFFF_FFFF, 0xFFFF_FFFF_FFFF_FFFF, 0xFFFF_FFFF_FFFF_FFFF), true);
        };
        yield return () =>
        {
            byte[] array = new byte[31];
            Array.Fill(array, byte.MaxValue);
            return (array, default, false);
        };
        yield return () =>
        {
            byte[] array = new byte[35];
            for (int i = 0; i < 32; i++)
                array[i] = byte.MaxValue;
            return (array, Values.CreateFloat<Octo>(0xFFFF_FFFF_FFFF_FFFF, 0xFFFF_FFFF_FFFF_FFFF, 0xFFFF_FFFF_FFFF_FFFF, 0xFFFF_FFFF_FFFF_FFFF), true);
        };
        yield return () =>
        {
            byte[] array = new byte[32];
            array[31] = 0x80;
            return (array, Values.CreateFloat<Octo>(1UL << 63, 0, 0, 0), true);
        };
    }
}