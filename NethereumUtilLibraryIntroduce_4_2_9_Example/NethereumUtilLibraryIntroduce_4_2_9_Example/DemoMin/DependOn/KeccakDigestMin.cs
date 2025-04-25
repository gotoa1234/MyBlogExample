namespace NethereumUtilLibraryIntroduce_4_2_9_Example.DemoMin.DependOn
{
    // 3-2. KeccakDigest
    /// <summary>
    /// Keccak 雜湊函數的核心實現類別
    /// Keccak 是 SHA-3 雜湊函數家族的基礎演算法，此實現特別用於以太坊中的 Keccak-256
    /// </summary>
    public class KeccakDigestMin
    {
        /// <summary>
        /// Keccak 輪常數陣列，用於每輪的 Iota 步驟
        /// </summary>
        private static readonly ulong[] KeccakRoundConstants = KeccakInitializeRoundConstants();

        /// <summary>
        /// Keccak 位元偏移量陣列，用於 Rho 步驟中的位元旋轉操作
        /// </summary>
        private static readonly int[] KeccakRhoOffsets = KeccakInitializeRhoOffsets();

        /// <summary>
        /// Keccak 狀態陣列的長度（位元組數），固定為 200 位元組 (1600 位元)
        /// </summary>
        private static readonly int STATE_LENGTH = 200;

        /// <summary>
        /// Keccak 的內部狀態，以 64 位元無符號整數陣列表示（25 個 64 位元值 = 1600 位元）
        /// </summary>
        private ulong[] state = new ulong[STATE_LENGTH / 8];

        /// <summary>
        /// 資料佇列，用於存儲待處理的輸入資料
        /// </summary>
        protected byte[] dataQueue = new byte[192];

        /// <summary>
        /// 速率值，定義了每個吸收階段可處理的位元數
        /// </summary>
        protected int rate;

        /// <summary>
        /// 目前在資料佇列中的位元數
        /// </summary>
        protected int bitsInQueue;

        /// <summary>
        /// 雜湊輸出的固定長度（位元數）
        /// </summary>
        protected int fixedOutputLength;

        /// <summary>
        /// 標記是否處於擠壓階段
        /// </summary>
        protected bool squeezing;

        /// <summary>
        /// 擠壓階段可用的位元數
        /// </summary>
        protected int bitsAvailableForSqueezing;

        /// <summary>
        /// 初始化 Keccak 輪常數
        /// 這些常數用於 Keccak 排列函數的 Iota 步驟
        /// </summary>
        /// <returns>包含 24 個輪常數的陣列</returns>
        private static ulong[] KeccakInitializeRoundConstants()
        {
            ulong[] array = new ulong[24];
            byte b = 1;
            for (int i = 0; i < 24; i++)
            {
                array[i] = 0uL;
                for (int j = 0; j < 7; j++)
                {
                    int num = (1 << j) - 1;
                    if (((uint)b & (true ? 1u : 0u)) != 0)
                    {
                        array[i] ^= (ulong)(1L << num);
                    }

                    bool flag = (b & 0x80) != 0;
                    b <<= 1;
                    if (flag)
                    {
                        b = (byte)(b ^ 0x71u);
                    }
                }
            }

            return array;
        }

        /// <summary>
        /// 初始化 Keccak Rho 偏移量
        /// 這些偏移量用於 Keccak 排列函數的 Rho 步驟中的旋轉操作
        /// </summary>
        /// <returns>包含 25 個偏移量的陣列</returns>
        private static int[] KeccakInitializeRhoOffsets()
        {
            int[] array = new int[25];
            int num = (array[0] = 0);
            int num2 = 1;
            int num3 = 0;
            for (int i = 1; i < 25; i++)
            {
                num = (num + i) & 0x3F;
                array[num2 % 5 + 5 * (num3 % 5)] = num;
                int num4 = num3 % 5;
                int num5 = (2 * num2 + 3 * num3) % 5;
                num2 = num4;
                num3 = num5;
            }

            return array;
        }

        /// <summary>
        /// 以指定位元長度建構 Keccak 摘要器
        /// </summary>
        /// <param name="bitLength">雜湊結果的位元長度，必須是 128、224、256、288、384 或 512 之一</param>
        public KeccakDigestMin(int bitLength)
        {
            Init(bitLength);
        }



        /// <summary>
        /// 獲取摘要大小（位元組數）
        /// </summary>
        /// <returns>雜湊結果的位元組數</returns>
        public virtual int GetDigestSize()
        {
            return fixedOutputLength >> 3;  // 將位元數轉換為位元組數（除以 8）
        }


        /// <summary>
        /// 更新摘要器狀態，吸收位元組陣列的指定部分
        /// </summary>
        /// <param name="input">輸入位元組陣列</param>
        /// <param name="inOff">起始偏移量</param>
        /// <param name="len">長度</param>
        public virtual void BlockUpdate(byte[] input, int inOff, int len)
        {
            Absorb(input, inOff, len);
        }

        /// <summary>
        /// 完成雜湊計算並將結果寫入輸出陣列
        /// </summary>
        /// <param name="output">輸出位元組陣列</param>
        /// <param name="outOff">輸出偏移量</param>
        /// <returns>寫入的位元組數</returns>
        public virtual int DoFinal(byte[] output, int outOff)
        {
            Squeeze(output, outOff, fixedOutputLength >> 3);
            Reset();
            return GetDigestSize();
        }

        /// <summary>
        /// 重置摘要器狀態
        /// </summary>
        public virtual void Reset()
        {
            Init(fixedOutputLength);
        }

        /// <summary>
        /// 初始化摘要器，設置正確的位元長度
        /// </summary>
        /// <param name="bitLength">雜湊結果的位元長度</param>
        private void Init(int bitLength)
        {
            switch (bitLength)
            {
                case 128:
                case 224:
                case 256:
                case 288:
                case 384:
                case 512:
                    InitSponge(1600 - (bitLength << 1));
                    break;
                default:
                    throw new ArgumentException("must be one of 128, 224, 256, 288, 384, or 512.", "bitLength");
            }
        }

        /// <summary>
        /// 初始化海綿結構，設置速率和容量
        /// </summary>
        /// <param name="rate">海綿結構的速率（位元）</param>
        private void InitSponge(int rate)
        {
            // 檢查速率值是否有效 (必須是 64 的倍數且在有效範圍內)
            if (rate <= 0 || rate >= 1600 || ((uint)rate & 0x3Fu) != 0)
            {
                throw new InvalidOperationException("invalid rate value");
            }

            this.rate = rate;
            Array.Clear(state, 0, state.Length);  // 清空狀態陣列
            ArraysMin.Fill(dataQueue, 0);            // 清空資料佇列
            bitsInQueue = 0;
            squeezing = false;
            bitsAvailableForSqueezing = 0;
            fixedOutputLength = 1600 - rate >> 1;  // 容量的一半作為輸出長度
        }

        /// <summary>
        /// 吸收階段 - 將輸入資料加入狀態
        /// </summary>
        /// <param name="data">輸入資料</param>
        /// <param name="off">偏移量</param>
        /// <param name="len">長度</param>
        protected void Absorb(byte[] data, int off, int len)
        {
            // 檢查佇列中的位元是否全都是完整位元組 (8 的倍數)
            if (((uint)bitsInQueue & 7u) != 0)
            {
                throw new InvalidOperationException("attempt to absorb with odd length queue");
            }

            // 檢查是否已經處於擠壓階段
            if (squeezing)
            {
                throw new InvalidOperationException("attempt to absorb while squeezing");
            }

            int num = bitsInQueue >> 3;  // 佇列中的位元組數
            int num2 = rate >> 3;        // 速率對應的位元組數
            int num3 = 0;                // 已處理的輸入位元組數

            while (num3 < len)
            {
                // 如果佇列為空且有足夠的輸入資料，直接處理整個區塊
                if (num == 0 && num3 <= len - num2)
                {
                    do
                    {
                        KeccakAbsorb(data, off + num3);
                        num3 += num2;
                    }
                    while (num3 <= len - num2);
                    continue;
                }

                // 否則，將資料添加到佇列中
                int num4 = Math.Min(num2 - num, len - num3);
                Array.Copy(data, off + num3, dataQueue, num, num4);
                num += num4;
                num3 += num4;

                // 如果佇列已滿，處理該區塊
                if (num == num2)
                {
                    KeccakAbsorb(dataQueue, 0);
                    num = 0;
                }
            }

            bitsInQueue = num << 3;  // 更新佇列中的位元數
        }

        
        /// <summary>
        /// 填充並切換到擠壓階段
        /// </summary>
        private void PadAndSwitchToSqueezingPhase()
        {
            // 添加填充位元 (1000...001)
            dataQueue[bitsInQueue >> 3] |= (byte)(1 << (bitsInQueue & 7));

            // 如果佇列已滿，處理該區塊
            if (++bitsInQueue == rate)
            {
                KeccakAbsorb(dataQueue, 0);
                bitsInQueue = 0;
            }

            // 處理剩餘資料
            int num = bitsInQueue >> 6;       // 完整的 64 位元塊數
            int num2 = bitsInQueue & 0x3F;    // 剩餘位元數
            int num3 = 0;                     // 已處理的位元組數

            // 處理完整的 64 位元塊
            for (int i = 0; i < num; i++)
            {
                state[i] ^= PackMin.LE_To_UInt64(dataQueue, num3);
                num3 += 8;
            }

            // 處理剩餘不足 64 位元的部分
            if (num2 > 0)
            {
                ulong num4 = (ulong)((1L << num2) - 1);
                state[num] ^= PackMin.LE_To_UInt64(dataQueue, num3) & num4;
            }

            // 添加最後的填充位元
            state[rate - 1 >> 6] ^= 9223372036854775808uL;  // 2^63

            // 執行 Keccak 排列函數
            KeccakPermutation();
            KeccakExtract();

            // 切換到擠壓階段
            bitsAvailableForSqueezing = rate;
            bitsInQueue = 0;
            squeezing = true;
        }

        /// <summary>
        /// 擠壓階段 - 提取雜湊結果
        /// </summary>
        /// <param name="output">輸出陣列</param>
        /// <param name="off">偏移量</param>
        /// <param name="len">要提取的位元組數</param>
        protected void Squeeze(byte[] output, int off, int len)
        {
            // 如果尚未進入擠壓階段，先執行填充
            if (!squeezing)
            {
                PadAndSwitchToSqueezingPhase();
            }

            long num = (long)len << 3;  // 轉換為位元數
            int num3;

            // 逐步擠出所需的位元數
            for (long num2 = 0L; num2 < num; num2 += num3)
            {
                // 如果沒有可用位元，執行 Keccak 排列函數
                if (bitsAvailableForSqueezing == 0)
                {
                    KeccakPermutation();
                    KeccakExtract();
                    bitsAvailableForSqueezing = rate;
                }

                // 確定本次可提取的位元數
                num3 = (int)Math.Min(bitsAvailableForSqueezing, num - num2);

                // 複製到輸出陣列
                Array.Copy(dataQueue, rate - bitsAvailableForSqueezing >> 3,
                          output, off + (int)(num2 >> 3), num3 >> 3);

                // 更新可用位元數
                bitsAvailableForSqueezing -= num3;
            }
        }

        /// <summary>
        /// 將資料吸收到 Keccak 狀態中並執行一次排列函數
        /// </summary>
        /// <param name="data">輸入資料</param>
        /// <param name="off">偏移量</param>
        private void KeccakAbsorb(byte[] data, int off)
        {
            int num = rate >> 6;  // 速率對應的 64 位元塊數

            // 將資料異或進狀態
            for (int i = 0; i < num; i++)
            {
                state[i] ^= PackMin.LE_To_UInt64(data, off);
                off += 8;
            }

            // 執行 Keccak 排列函數
            KeccakPermutation();
        }

        /// <summary>
        /// 從 Keccak 狀態中提取資料
        /// </summary>
        private void KeccakExtract()
        {
            PackMin.UInt64_To_LE(state, 0, rate >> 6, dataQueue, 0);
        }

        /// <summary>
        /// Keccak 排列函數 - 執行完整的 24 輪轉換
        /// </summary>
        private void KeccakPermutation()
        {
            for (int i = 0; i < 24; i++)
            {
                Theta(state);  // θ (Theta) 步驟
                Rho(state);    // ρ (Rho) 步驟
                Pi(state);     // π (Pi) 步驟
                Chi(state);    // χ (Chi) 步驟
                Iota(state, i); // ι (Iota) 步驟
            }
        }

        /// <summary>
        /// 執行位元左旋轉操作
        /// </summary>
        /// <param name="v">要旋轉的值</param>
        /// <param name="r">旋轉的位元數</param>
        /// <returns>旋轉後的值</returns>
        private static ulong leftRotate(ulong v, int r)
        {
            return (v << r) | (v >> -r);
        }

        /// <summary>
        /// Theta (θ) 步驟 - 行與列的混合擴散
        /// </summary>
        /// <param name="A">狀態陣列</param>
        private static void Theta(ulong[] A)
        {
            // 計算每列的奇偶校驗
            ulong num = A[0] ^ A[5] ^ A[10] ^ A[15] ^ A[20];
            ulong num2 = A[1] ^ A[6] ^ A[11] ^ A[16] ^ A[21];
            ulong num3 = A[2] ^ A[7] ^ A[12] ^ A[17] ^ A[22];
            ulong num4 = A[3] ^ A[8] ^ A[13] ^ A[18] ^ A[23];
            ulong num5 = A[4] ^ A[9] ^ A[14] ^ A[19] ^ A[24];

            // 計算並應用 Theta 效果
            ulong num6 = leftRotate(num2, 1) ^ num5;
            A[0] ^= num6;
            A[5] ^= num6;
            A[10] ^= num6;
            A[15] ^= num6;
            A[20] ^= num6;

            num6 = leftRotate(num3, 1) ^ num;
            A[1] ^= num6;
            A[6] ^= num6;
            A[11] ^= num6;
            A[16] ^= num6;
            A[21] ^= num6;

            num6 = leftRotate(num4, 1) ^ num2;
            A[2] ^= num6;
            A[7] ^= num6;
            A[12] ^= num6;
            A[17] ^= num6;
            A[22] ^= num6;

            num6 = leftRotate(num5, 1) ^ num3;
            A[3] ^= num6;
            A[8] ^= num6;
            A[13] ^= num6;
            A[18] ^= num6;
            A[23] ^= num6;

            num6 = leftRotate(num, 1) ^ num4;
            A[4] ^= num6;
            A[9] ^= num6;
            A[14] ^= num6;
            A[19] ^= num6;
            A[24] ^= num6;
        }

        /// <summary>
        /// Rho (ρ) 步驟 - 位元旋轉操作
        /// </summary>
        /// <param name="A">狀態陣列</param>
        private static void Rho(ulong[] A)
        {
            // 對除 A[0] 外的所有狀態位元進行旋轉
            for (int i = 1; i < 25; i++)
            {
                A[i] = leftRotate(A[i], KeccakRhoOffsets[i]);
            }
        }

        /// <summary>
        /// Pi (π) 步驟 - 位置置換
        /// </summary>
        /// <param name="A">狀態陣列</param>
        private static void Pi(ulong[] A)
        {
            // 保存 A[1] 的值用於循環置換
            ulong num = A[1];

            // 執行位置置換
            A[1] = A[6];
            A[6] = A[9];
            A[9] = A[22];
            A[22] = A[14];
            A[14] = A[20];
            A[20] = A[2];
            A[2] = A[12];
            A[12] = A[13];
            A[13] = A[19];
            A[19] = A[23];
            A[23] = A[15];
            A[15] = A[4];
            A[4] = A[24];
            A[24] = A[21];
            A[21] = A[8];
            A[8] = A[16];
            A[16] = A[5];
            A[5] = A[3];
            A[3] = A[18];
            A[18] = A[17];
            A[17] = A[11];
            A[11] = A[7];
            A[7] = A[10];

            // 完成循環置換
            A[10] = num;
        }

        /// <summary>
        /// Chi (χ) 步驟 - 非線性變換
        /// </summary>
        /// <param name="A">狀態陣列</param>
        private static void Chi(ulong[] A)
        {
            // 每 5 個元素為一組，執行非線性變換
            for (int i = 0; i < 25; i += 5)
            {
                // 暫存每組的原始值
                ulong num = A[i] ^ (~A[1 + i] & A[2 + i]);
                ulong num2 = A[1 + i] ^ (~A[2 + i] & A[3 + i]);
                ulong num3 = A[2 + i] ^ (~A[3 + i] & A[4 + i]);
                ulong num4 = A[3 + i] ^ (~A[4 + i] & A[i]);
                ulong num5 = A[4 + i] ^ (~A[i] & A[1 + i]);

                // 更新狀態
                A[i] = num;
                A[1 + i] = num2;
                A[2 + i] = num3;
                A[3 + i] = num4;
                A[4 + i] = num5;
            }
        }

        /// <summary>
        /// Iota (ι) 步驟 - 將輪常數添加到狀態
        /// </summary>
        /// <param name="A">狀態陣列</param>
        /// <param name="indexRound">當前輪數</param>
        private static void Iota(ulong[] A, int indexRound)
        {
            // 將對應輪的常數異或進狀態的首位元素
            A[0] ^= KeccakRoundConstants[indexRound];
        }
    }
}
