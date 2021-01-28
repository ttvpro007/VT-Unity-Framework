using System;
using System.Collections;
using System.Collections.Generic;

//https://stackoverflow.com/questions/1569127/c-implementation-of-the-sieve-of-atkin
namespace VT.Math
{
    //implements the non-paged Sieve of Eratosthenes (full modulo 210 version with preculling)...
    public class SieveOfEratosthenes : IEnumerable<ulong>
    {
        private ushort[] buf = null;
        private long cnt = 0;
        private long opcnt = 0;
        private static byte[] basePRMS = { 2, 3, 5, 7, 11, 13, 17, 19 };
        private static byte[] modPRMS = { 23, 29, 31, 37, 41, 43, 47, 53, 59, 61, 67, 71, 73, 79, 83, 89, //positions + 23
                                      97, 101, 103, 107, 109, 113, 121, 127, 131, 137, 139, 143, 149, 151, 157, 163,
                                      167, 169, 173, 179, 181 ,187 ,191 ,193, 197, 199, 209, 211, 221, 223, 227, 229 };
        private static byte[] gapsPRMS = { 6, 2, 6, 4, 2, 4, 6, 6, 2, 6, 4, 2, 6, 4, 6, 8,
                                       4, 2, 4, 2, 4, 8, 6, 4, 6, 2, 4, 6, 2, 6, 6, 4,
                                       2, 4, 6, 2, 6, 4, 2, 4, 2, 10, 2, 10, 2, 4, 2, 4 };
        private static ulong[] modLUT;
        private static byte[] cntLUT;
        //initialize the private LUT's...
        static SieveOfEratosthenes()
        {
            modLUT = new ulong[210];
            for (int i = 0, m = 0; i < modLUT.Length; ++i)
            {
                if ((i & 1) != 0 || (i + 23) % 3 == 0 || (i + 23) % 5 == 0 || (i + 23) % 7 == 0) modLUT[i] = 0;
                else modLUT[i] = 1UL << (m++);
            }
            cntLUT = new byte[65536];
            for (int i = 0; i < cntLUT.Length; ++i)
            {
                var c = 0;
                for (int j = i ^ 0xFFFF; j > 0; j >>= 1) c += j & 1; //reverse logic; 0 is prime; 1 is composite
                cntLUT[i] = (byte)c;
            }
        }
        //initialization and all the work producing the prime bit array done in the constructor...
        public SieveOfEratosthenes(ulong range)
        {
            this.opcnt = 0;
            if (range < 23)
            {
                if (range > 1)
                {
                    for (int i = 0; i < modPRMS.Length; ++i) if (modPRMS[i] <= range) this.cnt++; else break;
                }
                this.buf = new ushort[0];
            }
            else
            {
                this.cnt = 8;
                var nrng = range - 23; var lmtw = nrng / 210; var lmtwt3 = lmtw * 3;
                //initialize sufficient wheels to prime
                this.buf = new ushort[lmtwt3 + 3]; //initial state of all zero's is all potential prime.

                //initialize array to account for preculling the primes of 11, 13, 17, and 19;
                //(2, 3, 5, and 7 already eliminated by the bit packing to residues).
                for (int pn = modPRMS.Length - 4; pn < modPRMS.Length; ++pn)
                {
                    uint p = modPRMS[pn] - 210u; ulong pt3 = p * 3;
                    ulong s = p * p - 23;
                    ulong xrng = System.Math.Min(9699709, nrng); // only do for the repeating master pattern size
                    ulong nwrds = (ulong)System.Math.Min(138567, this.buf.Length);
                    for (int j = 0; s <= xrng && j < modPRMS.Length; s += p * gapsPRMS[(pn + j++) % 48])
                    {
                        var sm = modLUT[s % 210];
                        var si = (sm < (1UL << 16)) ? 0UL : ((sm < (1UL << 32)) ? 1UL : 2UL);
                        var cd = s / 210 * 3 + si; var cm = (ushort)(sm >> (int)(si << 4));
                        for (ulong c = cd; c < nwrds; c += pt3)
                        { //tight culling loop for size of master pattern
                            this.buf[c] |= cm; // ++this.opcnt; //reverse logic; mark composites with ones.
                        }
                    }
                }
                //Now copy the master pattern so it repeats across the main buffer, allow for overflow...
                for (long i = 138567; i < this.buf.Length; i += 138567)
                    if (i + 138567 <= this.buf.Length)
                        Array.Copy(this.buf, 0, this.buf, i, 138567);
                    else Array.Copy(this.buf, 0, this.buf, i, this.buf.Length - i);

                //Eliminate all composites which are factors of base primes, only for those on the wheel:
                for (uint i = 0, w = 0, wi = 0, pd = 0, pn = 0, msk = 1; w < this.buf.Length; ++i)
                {
                    uint p = pd + modPRMS[pn];
                    ulong sqr = (ulong)p * (ulong)p;
                    if (sqr > range) break;
                    if ((this.buf[w] & msk) == 0)
                    { //found base prime, mark its composites...
                        ulong s = sqr - 23; ulong pt3 = p * 3;
                        for (int j = 0; s <= nrng && j < modPRMS.Length; s += p * gapsPRMS[(pn + j++) % 48])
                        {
                            var sm = modLUT[s % 210];
                            var si = (sm < (1UL << 16)) ? 0UL : ((sm < (1UL << 32)) ? 1UL : 2UL);
                            var cd = s / 210 * 3 + si; var cm = (ushort)(sm >> (int)(si << 4));
                            for (ulong c = cd; c < (ulong)this.buf.Length; c += pt3)
                            { //tight culling loop
                                this.buf[c] |= cm; // ++this.opcnt; //reverse logic; mark composites with ones.
                            }
                        }
                    }
                    ++pn;
                    if (msk >= 0x8000) { msk = 1; ++w; ++wi; if (wi == 3) { wi = 0; pn = 0; pd += 210; } }
                    else msk <<= 1;
                }

                //clear any overflow primes in the excess space in the last wheel/word:
                var ndx = nrng % 210; //clear any primes beyond the range
                for (; modLUT[ndx] == 0; --ndx) ;
                var cmsk = (~(modLUT[ndx] - 1)) << 1; //force all bits above to be composite ones.
                this.buf[lmtwt3++] |= (ushort)cmsk;
                this.buf[lmtwt3++] |= (ushort)(cmsk >> 16);
                this.buf[lmtwt3] |= (ushort)(cmsk >> 32);
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

        //returns the number of cull operations used to sieve the prime bit array...
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
            yield return 2; yield return 3; yield return 5; yield return 7;
            yield return 11; yield return 13; yield return 17; yield return 19;
            ulong pd = 0;
            for (uint i = 0, w = 0, wi = 0, pn = 0, msk = 1; w < this.buf.Length; ++i)
            {
                if ((this.buf[w] & msk) == 0) //found a prime bit...
                    yield return pd + modPRMS[pn];
                ++pn;
                if (msk >= 0x8000) { msk = 1; ++w; ++wi; if (wi == 3) { wi = 0; pn = 0; pd += 210; } }
                else msk <<= 1;
            }
        }

        //required for the above enumeration...
        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }
    }
    ///EDIT_ADD2:
    ///I here add the code for a SoE using a similiar constant modulo/bit position technique for the innermost loops as for the SoA
    ///above according to the pseudo code further down the answer as linked above.The code is quite a bit less complex than the above SoA in spite
    ///of having high wheel factorization and pre-culling applied such that the total number of cull operations are actually less than the
    ///combined toggle/cull operations for the SoA up to a sieving rang of about two billion.The code as follows:
    ///
    ///EDIT_FINAL Corrected code below and comments related to it END_EDIT_FINAL
    ///
    ///This code actually runs a few percent faster than the above SoA as it should as there are slightly less operations and the main bottleneck
    ///for this large array size for a range of a billion is memory access time of something like 40 to over 100 CPU clock cycles depending on CPU
    ///and memory specifications; this means that code optimizations (other than reducing the total number of operations) are ineffective as most
    ///of the time is spend waiting on memory access. At any rate, using a huge memory buffer isn't the most efficient way to sieve large ranges,
    ///with a factor of up to about eight times improvement for the SoE using page segmentation with the same maximum wheel factorization (which
    ///also paves the way for multi-processing).
    ///
    ///It is in implementing page segmentation and multi-processing that the SoA is really deficient for ranges much above four billion as compared
    ///to the SoE as any gains due to the reduced asymptotic complexity of the SoA rapidly get eaten up by page processing overhead factors related
    ///to the prime square free processing and calculating the much larger number of page start addresses; alternatively, one overcomes this by
    ///storing markers in RAM memory at a huge cost in memory consumption and further inefficiencies in accessing these marker store structures.
    ///END_EDIT_ADD2
    ///
    ///In short, the SoA isn't really a practical sieve as compared to the the fully wheel factorized SoE since just as the gain in asymptotic
    ///complexity starts to bring it close in performance to the fully optimized SoE, it starts to lose efficiency due to the details of practical
    ///implementation as to relative memory access time and page segmentation complexities as well as generally being more complex and difficult to
    ///write. In my opinion it is more of an interesting intellectual concept and mental exercise than a practical sieve as compared to the SoE.
    ///
    ///Some day I will adapt these techniques to a multi-threaded page segmented Sieve of Eratosthenes to be about as fast in C# as Atkin and
    ///Bernstein's "primegen" implementation of the SoA in 'C' and will blow it out of the water for large ranges above about four billion even
    ///single threaded, with an extra boost in speed of up to about four when multi-threading on my i7 (eight cores including Hyper Threading).
}