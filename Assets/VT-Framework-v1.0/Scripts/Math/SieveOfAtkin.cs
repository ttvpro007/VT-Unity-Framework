using System.Collections;
using System.Collections.Generic;

//https://stackoverflow.com/questions/1569127/c-implementation-of-the-sieve-of-atkin
namespace VT.Math
{
    //Sieve of Atkin based on full non page segmented modulo 60 implementation...
    //implements the non-paged Sieve of Atkin (full modulo 60 version)...
    public class SieveOfAtkin : IEnumerable<ulong>
    {
        private ushort[] buf = null;
        private long cnt = 0;
        private long opcnt = 0;
        private static byte[] modPRMS = { 7, 11, 13, 17, 19, 23, 29, 31, 37, 41, 43, 47, 49, 53, 59, 61 };
        private static ushort[] modLUT;
        private static byte[] cntLUT;
        //initialize the private LUT's...
        static SieveOfAtkin()
        {
            modLUT = new ushort[60];
            for (int i = 0, m = 0; i < modLUT.Length; ++i)
            {
                if ((i & 1) != 0 || (i + 7) % 3 == 0 || (i + 7) % 5 == 0) modLUT[i] = 0;
                else modLUT[i] = (ushort)(1 << (m++));
            }
            cntLUT = new byte[65536];
            for (int i = 0; i < cntLUT.Length; ++i)
            {
                var c = 0;
                for (int j = i; j > 0; j >>= 1) c += j & 1;
                cntLUT[i] = (byte)c;
            }
        }
        //initialization and all the work producing the prime bit array done in the constructor...
        public SieveOfAtkin(ulong range)
        {
            this.opcnt = 0;
            if (range < 7)
            {
                if (range > 1)
                {
                    cnt = 1;
                    if (range > 2) this.cnt += (long)(range - 1) / 2;
                }
                this.buf = new ushort[0];
            }
            else
            {
                this.cnt = 3;
                var nrng = range - 7; var lmtw = nrng / 60;
                //initialize sufficient wheels to non-prime
                this.buf = new ushort[lmtw + 1];

                //Put in candidate primes:
                //for the 4 * x ^ 2 + y ^ 2 quadratic solution toggles - all x odd y...
                ulong n = 6; // equivalent to 13 - 7 = 6...
                for (uint x = 1, y = 3; n <= nrng; n += (x << 3) + 4, ++x, y = 1)
                {
                    var cb = n; if (x <= 1) n -= 8; //cancel the effect of skipping the first one...
                    for (uint i = 0; i < 15 && cb <= range; cb += (y << 2) + 4, y += 2, ++i)
                    {
                        var cbd = cb / 60; var cm = modLUT[cb % 60];
                        if (cm != 0)
                            for (uint c = (uint)cbd, my = y + 15; c < buf.Length; c += my, my += 30)
                            {
                                buf[c] ^= cm; // ++this.opcnt;
                            }
                    }
                }
                //for the 3 * x ^ 2 + y ^ 2 quadratic solution toggles - x odd y even...
                n = 0; // equivalent to 7 - 7 = 0...
                for (uint x = 1, y = 2; n <= nrng; n += ((x + x + x) << 2) + 12, x += 2, y = 2)
                {
                    var cb = n;
                    for (var i = 0; i < 15 && cb <= range; cb += (y << 2) + 4, y += 2, ++i)
                    {
                        var cbd = cb / 60; var cm = modLUT[cb % 60];
                        if (cm != 0)
                            for (uint c = (uint)cbd, my = y + 15; c < buf.Length; c += my, my += 30)
                            {
                                buf[c] ^= cm; // ++this.opcnt;
                            }
                    }
                }
                //for the 3 * x ^ 2 - y ^ 2 quadratic solution toggles all x and opposite y = x - 1...
                n = 4; // equivalent to 11 - 7 = 4...
                for (uint x = 2, y = x - 1; n <= nrng; n += (ulong)(x << 2) + 4, y = x, ++x)
                {
                    var cb = n; int i = 0;
                    for (; y > 1 && i < 15 && cb <= nrng; cb += (ulong)(y << 2) - 4, y -= 2, ++i)
                    {
                        var cbd = cb / 60; var cm = modLUT[cb % 60];
                        if (cm != 0)
                        {
                            uint c = (uint)cbd, my = y;
                            for (; my >= 30 && c < buf.Length; c += my - 15, my -= 30)
                            {
                                buf[c] ^= cm; // ++this.opcnt;
                            }
                            if (my > 0 && c < buf.Length) { buf[c] ^= cm; /* ++this.opcnt; */ }
                        }
                    }
                    if (y == 1 && i < 15)
                    {
                        var cbd = cb / 60; var cm = modLUT[cb % 60];
                        if ((cm & 0x4822) != 0 && cbd < (ulong)buf.Length) { buf[cbd] ^= cm; /* ++this.opcnt; */ }
                    }
                }

                //Eliminate squares of base primes, only for those on the wheel:
                for (uint i = 0, w = 0, pd = 0, pn = 0, msk = 1; w < this.buf.Length; ++i)
                {
                    uint p = pd + modPRMS[pn];
                    ulong sqr = (ulong)p * (ulong)p; //to handle ranges above UInt32.MaxValue
                    if (sqr > range) break;
                    if ((this.buf[w] & msk) != 0)
                    { //found base prime, square free it...
                        ulong s = sqr - 7;
                        for (int j = 0; s <= nrng && j < modPRMS.Length; s = sqr * modPRMS[j] - 7, ++j)
                        {
                            var cd = s / 60; var cm = (ushort)(modLUT[s % 60] ^ 0xFFFF);
                            //may need ulong loop index for ranges larger than two billion
                            //but buf length only good to about 2^31 * 60 = 120 million anyway,
                            //even with large array setting and half that with 32-bit...
                            for (ulong c = cd; c < (ulong)this.buf.Length; c += sqr)
                            {
                                this.buf[c] &= cm; // ++this.opcnt;
                            }
                        }
                    }
                    if (msk >= 0x8000) { msk = 1; pn = 0; ++w; pd += 60; }
                    else { msk <<= 1; ++pn; }
                }

                //clear any overflow primes in the excess space in the last wheel/word:
                var ndx = nrng % 60; //clear any primes beyond the range
                for (; modLUT[ndx] == 0; --ndx) ;
                this.buf[lmtw] &= (ushort)((modLUT[ndx] << 1) - 1);
            }
        }

        //uses a fast pop count Look Up Table to return the total number of primes...
        public long Count
        {
            get
            {
                long cnt = this.cnt;
                for (int i = 0; i < this.buf.Length; ++i) cnt += cntLUT[this.buf[i]];
                return cnt;
            }
        }

        //returns the number of toggle/cull operations used to sieve the prime bit array...
        public long Ops
        {
            get
            {
                return this.opcnt;
            }
        }

        //generate the enumeration of primes...
        public IEnumerator<ulong> GetEnumerator()
        {
            yield return 2; yield return 3; yield return 5;
            ulong pd = 0;
            for (uint i = 0, w = 0, pn = 0, msk = 1; w < this.buf.Length; ++i)
            {
                if ((this.buf[w] & msk) != 0) //found a prime bit...
                    yield return pd + modPRMS[pn]; //add it to the list
                if (msk >= 0x8000) { msk = 1; pn = 0; ++w; pd += 60; }
                else { msk <<= 1; ++pn; }
            }
        }

        //required for the above enumeration...
        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }
    }
    ///This code runs about twice as fast as Aaron's code (about 2.7 seconds using 64-bit or 32-bit mode on an i7-2700K (3.5 GHz) with the buffer
    ///about 16.5 Megabytes and about 0.258 billion combined toggle/prime square free cull operations (which can be shown by uncommenting the
    ///"++this.opcnt" statements) for a sieve range of one billion, as compared to 5.4/6.2 seconds (32-bit/64-bit) for his code without the count
    ///time and almost twice the memory use using about 0.359 billion combined toggle/cull operations for sieving up to one billion.
    ///
    ///Although it is faster than his most optimized naive odds-only implementation of the non-paged Sieve of Eratosthenes(SoE), that does not
    ///make the Sieve of Atkin faster than the Sieve of Eratosthenes, as if one applies similar techniques as used in the above SoA implementation
    ///to the SoE plus uses maximal wheel factorization, the SoE will about the same speed as this.
    ///
    ///Analysis: Although the number of operations for the fully optimized SoE are about the same as the number of operations for the SoA for a
    ///sieve range of one billion, the main bottleneck for these non-paged implementations is memory access once the sieve buffer size exceeds the
    ///CPU cache sizes (32 KiloBytes L1 cache at one clock cycle access, 256 Kilobytes L2 cache at about four clock cycles access time and 8
    ///Megabytes L3 cache at about 20 clock cycles access time for my i7), after which memory access can exceed a hundred clock cycles.
    ///
    ///Now both have a factor of about eight improvement in memory access speeds when one adapts the algorithms to page segmentation so one can
    ///sieve ranges that would not otherwise fit into available memory. However, the SoE continues to gain over the SoA as the sieve range starts
    ///to get very large due to difficulties in implementing the "primes square free" part of the algorithm due to the huge strides in culling
    ///scans that quickly grow to many hundreds of times the size of the page buffers. As well, and perhaps more serious, it gets very memory and
    ///or computationally intensive to compute the new start point for each value of 'x' as to the value of 'y' at the lowest representation of
    ///each page buffer for a further quite large loss in efficiency of the paged SoA comparaed to the SoE as the range grows.
    ///
    ///EDIT_ADD: The odds - only SoE as used by Aaron Murgatroyd uses about 1.026 billion cull operations for a sieve range of one billion so
    ///about four times as many operations as the SoA and thus should run about four times slower, but the SoA even as implemented here has a more
    ///complex inner loop and especially due to a much higher proportion of the odds - only SoE culls have a much shorter stride in the culling
    ///scans than the strides of the SoA the naive odds - only SoE has much better average memory access times in spite of the sieve buffer
    ///greatly exceeding the CPU cache sizes(better use of cache associativity).This explains why the above SoA is only about twice as fast as the
    ///odds - only SoE even though it would theoretically seem to be doing only one quarter of the work.
    ///
    ///If one were to use a similar algorithm using constant modulo inner loops as for the above SoA and implemented the same 2 / 3 / 5 wheel
    ///factorization, the SoE would reduce the number of cull operations to about 0.405 billion operations so only about 50 % more operations than
    ///the SoA and would theoretically run just slightly slower than the SoA, but may run at about the same speed due to the cull strides still
    ///being a little smaller than for the SoA on the average for this "naive" large memory buffer use.Increasing the wheel factorization to the
    ///2 / 3 / 5 / 7 wheel means the SoE cull operations are reduced to about 0.314 for a cull range of one billion and may make that version of
    ///the SoE run about the same speed for this algorithm.
    ///              
    ///
    ///Further use of wheel factorization can be made by pre - culling the sieve array(copying in a pattern) for the
    ///2 / 3 / 5 / 7 / 11 / 13 / 17 / 19 prime factors at almost no cost in execution time to reduce the total number of cull operations to about
    ///0.251 billion for a sieve range of one billion and the SoE will run faster or about the same speed than even this optimized version of the
    ///SoA, even for these large memory buffer versions, with the SoE still having much less code complexity than the above.
    ///
    ///Thus, it can be seen that the number of operations for the SoE can be greatly reduced from a naive or even odds - only or 2 / 3 / 5 wheel
    ///factorization version such that the number of operations are about the same as for the SoA while at the same time the time per operation may
    ///actually be less due to both less complex inner loops and more efficient memory access.END_EDIT_ADD
}